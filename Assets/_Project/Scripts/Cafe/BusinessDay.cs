using System.Collections;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 하루 영업의 시작과 끝 (SFR-014): 20:00 입장 마감 안내, 21:00 영업 종료 → 평판 변동 계산 → 하루 정산
    public class BusinessDay : MonoBehaviour
    {
        [SerializeField] GameClock clock;
        [SerializeField] CustomerManager customers;
        [SerializeField] GameObject closedBanner; // "영업 종료"
        [SerializeField] float bannerSeconds = 3f;

        void Awake()
        {
            // Cafe 씬을 바로 Play 해도 동작하도록 (ADR-0011 미리보기 데이터)
            SaveSystem.EnsureCurrentForPreview();
            if (DayResult.Today == null) DayResult.StartNewDay();
            Pantry.StartDay();
            closedBanner.SetActive(false);
        }

        void Start()
        {
            clock.EntryClosed += () => Toast.Show(ToastMessage.EntryClosed);
            clock.Closed += () => StartCoroutine(Close());
        }

        IEnumerator Close()
        {
            customers.CloseAll();
            closedBanner.SetActive(true);
            yield return new WaitForSecondsRealtime(bannerSeconds);
            DayResult.Today.reputationChange = ReputationChange(DayResult.Today, GameBalance.Instance);
            SceneLoader.Load(SceneNames.Score); // 득점 결과 → 하루 정산 (SFR-067, 071)
        }

        // 그날 손님 만족도 평균으로 평판 변동 (SFR-072, 기준은 임시값). 떠난 손님은 만족도 0으로 친다
        static float ReputationChange(DayResult result, GameBalance balance)
        {
            int count = result.ordersCompleted + result.customersLost;
            if (count == 0) return 0f;
            float average = result.satisfactionSum / count;
            float neutral = balance.NeutralSatisfaction;
            float t = average >= neutral ? (average - neutral) / (100f - neutral) : (average - neutral) / neutral;
            return Mathf.Round(t * balance.MaxReputationChange * 10f) / 10f; // 0.1 단위
        }
    }
}
