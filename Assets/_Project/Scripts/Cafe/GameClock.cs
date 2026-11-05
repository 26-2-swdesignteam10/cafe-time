using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 영업 시간 (SFR-014): 게임 시간 09:00 ~ 21:00, 실제 2분 = 게임 1시간 (GameBalance)
    // 기기 화면이 열려 있어도 시간은 흐르고, 일시정지 메뉴(SFR-023)가 열린 동안에만 멈춘다 → Paused
    public class GameClock : MonoBehaviour
    {
        [Tooltip("테스트용 시간 배속. 손님 인내심·머무는 시간도 같이 빨라진다 (출시 때는 1)")]
        [SerializeField] float testSpeed = 1f;

        public static GameClock Instance { get; private set; }

        // 일시정지 메뉴가 열려 있는 동안 true (SFR-023 구현 시 사용)
        public static bool Paused { get; set; }

        // 영업 시간 기준 경과 시간(실제 초). 손님 타이머는 Time.deltaTime 대신 이 값을 쓴다
        public static float Delta => Instance == null || Paused ? 0f : Time.deltaTime * Instance.testSpeed;

        public event Action EntryClosed; // 20:00 손님 입장 마감
        public event Action Closed;      // 21:00 영업 종료

        float minutes; // 00:00 부터의 게임 시간(분)
        GameBalance balance;

        public bool IsEntryClosed { get; private set; }
        public bool IsClosed { get; private set; }
        public string TimeText => $"{(int)minutes / 60:00}:{(int)minutes % 60 / 10 * 10:00}"; // 10분 단위 표시

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
            minutes = balance.OpenHour * 60f;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
            Paused = false;
        }

        void Update()
        {
            if (IsClosed) return;
            minutes += Delta * 60f / balance.SecondsPerGameHour;

            if (!IsEntryClosed && minutes >= balance.EntryCloseHour * 60f)
            {
                IsEntryClosed = true;
                EntryClosed?.Invoke();
            }
            if (minutes >= balance.CloseHour * 60f)
            {
                minutes = balance.CloseHour * 60f;
                IsClosed = true;
                Closed?.Invoke();
            }
        }
    }
}
