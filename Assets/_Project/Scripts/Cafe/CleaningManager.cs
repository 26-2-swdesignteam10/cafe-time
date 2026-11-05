using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 청소 요청 (SFR-066): 퇴식구가 가득 차 쟁반을 두고 간 테이블 + 무작위 돌발 요청(DAY가 지날수록 자주).
    // 제한 시간 안에 청소하지 않으면 청소 미처리 감점
    public class CleaningManager : MonoBehaviour
    {
        [SerializeField] CafeTable[] tables;

        public static CleaningManager Instance { get; private set; }

        GameBalance balance;
        float nextRandom;

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
            nextRandom = Interval();
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        float Interval()
        {
            float scale = Mathf.Pow(balance.CleaningIntervalPerDay, SaveSystem.Current.day - 1);
            return balance.CleaningInterval * scale * Random.Range(0.7f, 1.3f);
        }

        // 퇴식구가 가득 차 손님이 테이블에 쟁반을 두고 감 (SFR-062)
        public void TrayLeft(CafeTable table, Tray tray)
        {
            table.LeaveTray(tray);
            table.RequestCleaning(balance.CleaningTimeLimit);
            DayResult.Today.traysLeft++;
        }

        void Update()
        {
            float dt = GameClock.Delta;
            if (GameClock.Instance != null && !GameClock.Instance.IsClosed)
            {
                nextRandom -= dt;
                if (nextRandom <= 0)
                {
                    nextRandom = Interval();
                    var free = tables.Where(t => t.IsFree).ToList();
                    if (free.Count > 0) free[Random.Range(0, free.Count)].RequestCleaning(balance.CleaningTimeLimit);
                }
            }

            foreach (var table in tables)
            {
                table.TryMoveTray(); // 퇴식구에 자리가 나면 남은 쟁반을 옮김
                if (!table.CleaningRequested || table.CleaningMissed) continue;
                table.CleaningLeft -= dt;
                if (table.CleaningLeft > 0) continue;
                table.CleaningMissed = true; // 감점은 한 번만, 요청은 청소할 때까지 남음
                DayResult.Today.cleaningMissed++;
            }
        }
    }
}
