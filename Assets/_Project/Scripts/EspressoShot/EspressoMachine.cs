using System;
using System.Collections.Generic;
using CafeTime.Cafe;
using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.EspressoShot
{
    // SFR-029: 샷 내리기 화면 열기/닫기 + 분쇄·추출 진행
    // "항상 켜져 있는" 오브젝트(영업 씬: Business, ShotTest: Canvas 아래 EspressoMachine)에 붙이세요.
    // screenPanel 만 껐다 켜는 구조라서, 닫아도 포터필터 위치/상태가 그대로 유지되고
    // 분쇄·추출 타이머도 이 오브젝트에서 돌기 때문에 화면을 닫아도 계속 진행돼요.
    // 영업 씬에서는 맵의 작업 상태 표시(SFR-022)를 위해 IWorkStatus 를 구현합니다.
    public class EspressoMachine : MonoBehaviour, IWorkStatus
    {
        public static EspressoMachine Instance { get; private set; }

        [Tooltip("샷 내리기 화면. Popup 이 붙어 있으면 영업 씬의 ESC·이동 잠금과 함께 동작")]
        public GameObject screenPanel;

        // 분쇄·추출처럼 시간이 걸리는 작업
        class Job
        {
            public float Total;
            public float Left;
            public Action<float> Tick;   // 남은 시간(초)
            public Action Finished;
        }

        readonly List<Job> jobs = new List<Job>();
        Popup screen;
        bool done;

        // 영업 씬에서는 일시정지(SFR-023) 때 멈추는 게임 시간, GameClock 이 없는 테스트 씬(ShotTest)에서는 실제 시간
        public static float Delta => GameClock.Instance != null ? GameClock.Delta : Time.deltaTime;

        void Awake()
        {
            Instance = this;
            SaveSystem.EnsureCurrentForPreview(); // 원두 재고(Pantry)를 읽기 위해. 이미 있으면 그대로
            if (screenPanel != null)
            {
                screen = screenPanel.GetComponent<Popup>();
                screenPanel.SetActive(false);
            }
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            // Popup 이 없는 테스트 씬에서만 직접 ESC 처리 (영업 씬은 CafeSceneInput 이 처리)
            if (screen == null && screenPanel != null && screenPanel.activeSelf &&
                Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                Close();

            float dt = Delta;
            for (int i = jobs.Count - 1; i >= 0; i--)
            {
                var job = jobs[i];
                job.Left -= dt;
                if (job.Left > 0f)
                {
                    job.Tick?.Invoke(job.Left);
                    continue;
                }
                jobs.RemoveAt(i);
                job.Tick?.Invoke(0f);
                job.Finished?.Invoke();
                done = true;
            }
        }

        // 맵에서 에스프레소 머신과 상호작용할 때 호출. 열면 완료 아이콘이 사라짐 (SFR-022)
        public void Open()
        {
            done = false;
            if (screen != null) screen.Open();
            else screenPanel.SetActive(true);
        }

        // X 버튼 OnClick 또는 ESC
        public void Close()
        {
            if (screen != null) screen.Close();
            else screenPanel.SetActive(false);
        }

        // 화면을 닫아도 계속 진행되는 타이머
        public void RunTimed(float seconds, Action<float> tick, Action finished)
        {
            jobs.Add(new Job { Total = seconds, Left = seconds, Tick = tick, Finished = finished });
        }

        // ---------- 추출 (그룹헤드 슬롯별 독립 진행, SFR-035) ----------
        public void StartExtraction(PortafilterSlot head)
        {
            if (head.Locked) return;
            Portafilter pf = head.Current;
            if (pf == null || pf.State != PortafilterState.Tamped)
            {
                Toast.Show(ToastMessage.AttachPortafilter);
                return;
            }
            ShotCupSlot cup = head.cup; // 컵 자리가 없는 테스트 씬에서는 컵 없이 추출
            if (cup != null && cup.Glass == null)
            {
                Toast.Show(ToastMessage.PlaceShotGlass);
                return;
            }
            if (cup != null && cup.Glass.HasShot)
            {
                Toast.Show(ToastMessage.ShotAlready);
                return;
            }

            head.Locked = true; // 추출 중에는 포터필터 분리·컵 회수 불가
            RunTimed(GameBalance.Instance.ExtractSeconds, left => head.TimeLeft = left, () =>
            {
                pf.SetState(PortafilterState.Used); // → '사용 완료(찌꺼기)'
                if (cup != null) cup.FillShot();    // → '샷이 든 컵'
                head.Locked = false;
            });
        }

        // ---------- 작업 상태 표시 (SFR-022) ----------
        public float Progress
        {
            get
            {
                float p = -1f;
                foreach (var job in jobs) p = Mathf.Max(p, 1f - job.Left / job.Total);
                return p;
            }
        }

        public bool Done => done;

        public void ClearDone() => done = false;
    }
}
