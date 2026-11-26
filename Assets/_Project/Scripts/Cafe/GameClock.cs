using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 영업 시간 (SFR-014): 게임 시간 09:00 ~, 실제 35초 = 게임 1시간 (GameBalance, ADR-0021).
    // 20:00 입장 마감, 21:00 마감 시각. 마감 시각이 지나도 시계는 계속 가고(초과 시간 = 감점), 하루는 BusinessDay 가 끝낸다 (ADR-0055)
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
        public event Action Overtime;    // 21:00 마감 시각을 넘김 (ADR-0055)

        float minutes; // 00:00 부터의 게임 시간(분)
        GameBalance balance;

        public bool IsEntryClosed { get; private set; }
        public bool IsOvertime { get; private set; }  // 마감 시각을 넘겨 영업 중
        public bool IsClosed { get; private set; }    // 하루 영업이 끝남 (EndDay). 시계가 멈춘다
        // 마감 시각을 넘긴 게임 시간(분). 넘기지 않았으면 0
        public float OvertimeMinutes => Mathf.Max(0f, minutes - balance.CloseHour * 60f);
        public float Hour => minutes / 60f; // 시간대 연출용 (DaylightCycle)
        public string TimeText => $"{(int)minutes / 60:00}:{(int)minutes % 60 / 10 * 10:00}"; // 10분 단위 표시
        public float Minutes => minutes; // 00:00 부터의 게임 시간(분)

        // 게임 시각(시, 소수 가능) → "16:00"
        public static string HourText(float hour)
        {
            int total = Mathf.RoundToInt(hour * 60f);
            return $"{total / 60:00}:{total % 60:00}";
        }

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
            if (!IsOvertime && minutes >= balance.CloseHour * 60f)
            {
                IsOvertime = true;
                Overtime?.Invoke();
            }
        }

        // 하루 영업 종료: 시계를 멈춘다 (BusinessDay)
        public void EndDay() => IsClosed = true;
    }
}
