using System.Collections;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 하루 영업의 시작과 끝 (SFR-014, ADR-0055): 20:00 입장 마감 안내 → 남은 손님을 다 보내면 영업 종료
    // → 평판 변동 계산 → 하루 정산. 21:00 마감 시각을 넘기면 넘긴 시간만큼 감점되고, 영업은 손님이 다 갈 때까지 계속된다
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
            closedBanner.SetActive(false);
        }

        void Start()
        {
            clock.EntryClosed += () => Toast.Show(ToastMessage.EntryClosed);
        }

        // 입장 마감 뒤 손님 일행·대기 주문이 모두 없어지면 하루를 끝낸다
        void Update()
        {
            if (clock.IsClosed || !clock.IsEntryClosed) return;
            if (customers.HasParties || OrderBoard.Instance.Orders.Count > 0) return;
            StartCoroutine(Close());
        }

        IEnumerator Close()
        {
            DayResult.Today.overtimeMinutes = Mathf.CeilToInt(clock.OvertimeMinutes);
            clock.EndDay();
            customers.CloseAll();
            if (GroupOrderManager.Instance != null) GroupOrderManager.Instance.Expire(); // 서빙 못 한 단체 주문 취소 (ADR-0042)
            if (InfluencerManager.Instance != null) InfluencerManager.Instance.Expire(showReaction: false); // 받지 못한 인플루언서 촬영 실패 (ADR-0048)
            closedBanner.SetActive(true);
            yield return new WaitForSecondsRealtime(bannerSeconds);
            var result = DayResult.Today;
            result.reputationChange = Mathf.Round((ReputationChange(result, GameBalance.Instance) + result.eventReputationChange) * 10f) / 10f;
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
