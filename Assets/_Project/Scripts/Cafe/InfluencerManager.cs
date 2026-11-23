using System.Collections.Generic;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 영업 중 인플루언서 방문 이벤트 (SFR-009 DAY 6, ADR-0048)
    //  1. 정해진 시각에 인플루언서가 대화창으로 대표 메뉴 추천과 촬영을 요청 → 수락/거절 (SFR-008 선택지, 결과는 SaveData.choices)
    //  2. 수락하면 포스기 입력 없이 대표 메뉴 1잔 주문이 주문 내역·픽업대에 등록된다
    //  3. 마감 시각 전에 픽업대에서 진동벨로 호출하면 받아 가며 음료 값을 내고, 맛(등급)에 대한 소감을 대화창으로 말한다
    //  4. 결과(완벽/좋음/나쁨·마감 넘김)에 따라 오늘 평판이 바뀌고, 다음 DAY 부터 손님 입장 빈도가 바뀐다 (영상 공개)
    //  거절하면 평판·손님 수 변화 없음
    public class InfluencerManager : MonoBehaviour
    {
        [SerializeField] EventDialog dialog;
        [Tooltip("대화창에 표시할 인플루언서 그림")]
        [SerializeField] Sprite portrait;
        [SerializeField] string speaker = "인플루언서";
        [Tooltip("여러 이벤트가 겹쳐도 손님 수 배율이 이 범위를 넘지 않는다")]
        [SerializeField] Vector2 visitorRateRange = new Vector2(0.5f, 2f);

        public static InfluencerManager Instance { get; private set; }

        public const string Accept = "accept";
        public const string Decline = "decline";
        const string ResultSuffix = "_result"; // 촬영 결과 선택 id: "<choiceId>_result" = great / good / bad / expired

        InfluencerEvent ev;
        InfluencerVisit visit; // 대화 중 또는 수락 후 받기 전
        Order order;
        bool requested;

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Start()
        {
            ev = GameBalance.Instance.InfluencerFor(SaveSystem.Current.day);
        }

        void Update()
        {
            var clock = GameClock.Instance;
            if (ev == null || clock == null || EventDialog.IsShowing) return; // 다른 이벤트 대화 중이면 끝난 뒤에 요청
            if (!requested && !clock.IsEntryClosed && clock.Minutes >= ev.requestHour * 60f) Request();
            if (order != null && clock.Minutes >= visit.DeadlineMinutes) Expire();
        }

        // ───────── 요청·선택 (SFR-008, 009) ─────────

        void Request()
        {
            requested = true;
            visit = InfluencerVisit.From(ev, GameBalance.Instance);
            if (visit == null) return;
            var lines = new List<string>
            {
                "안녕하세요! 동네 카페를 소개하는 영상을 올리고 있어요. 새로 생긴 카페가 있다고 해서 와 봤어요.",
                "사장님이 추천하시는 이 카페 대표 메뉴를 마셔 보면서 촬영하고 싶은데요.",
                $"대표 메뉴 {visit.MenuText} 한 잔, {visit.DeadlineText}까지 준비해 주시면 촬영해도 될까요? 맛있으면 구독자분들께 꼭 소개할게요!",
            };
            dialog.Show(speaker, portrait, lines, Choose);
        }

        string Choose(bool accept)
        {
            SaveSystem.Current.SetChoice(ev.choiceId, accept ? Accept : Decline); // 하루 정산 때 같이 저장 (SFR-071)
            if (!accept)
            {
                visit = null;
                return "아쉽네요. 오늘은 구경만 하고 갈게요!";
            }
            order = OrderBoard.Instance.Register(visit.Lines, visit.Price, null, influencer: visit);
            return $"감사합니다! {visit.DeadlineText}까지 픽업대 앞에서 카메라 세팅하고 기다릴게요.";
        }

        // ───────── 받아 가기·마감 ─────────

        // 픽업대 진동벨 호출 (PickupScreen). 대표 메뉴와 맞고 실패 등급이 아니면 음료 값을 낸다
        public void Serve(Order served, IReadOnlyList<HeldItem> items)
        {
            if (served != order) return;
            OrderBoard.Instance.Remove(order);
            var outcome = visit.Judge(items, out bool matched);
            var save = SaveSystem.Current;
            var result = DayResult.Today;
            if (matched)
            {
                save.money += visit.Price;
                result.revenue += visit.Price;
            }
            result.ordersCompleted++;
            result.satisfactionSum += CustomerParty.MatchScore(visit.Lines, items);
            result.drinksSold += items.Count;
            // 촬영 후 들고 나가므로 단체 주문처럼 매장 컵 수는 그대로 둔다 (ADR-0042)
            foreach (var item in items)
                if (item is Cup cup) CupShelf.Instance.Return(cup.Type == CupType.Ceramic ? TrayItemKind.DirtyCeramic : TrayItemKind.DirtyPlastic);
            Finish(outcome, showReaction: true);
        }

        // 마감 시각이 지났거나 영업이 끝났는데 받지 못함: 손님 이탈 감점 + 나쁜 결과 (SFR-057, 068)
        public void Expire(bool showReaction = true)
        {
            if (order == null) return;
            OrderBoard.Instance.Remove(order); // 픽업대 트레이에 놓아 둔 음료는 버려진다
            DayResult.Today.customersLost++;
            Finish(InfluencerOutcome.Expired, showReaction);
        }

        void Finish(InfluencerOutcome outcome, bool showReaction)
        {
            visit.Effect(outcome, out float reputation, out float rate);
            var save = SaveSystem.Current;
            DayResult.Today.eventReputationChange += reputation; // 오늘 정산 평판에 더함 (SFR-072)
            if (rate > 0f) save.visitorRate = Mathf.Clamp(save.visitorRate * rate, visitorRateRange.x, visitorRateRange.y);
            if (!string.IsNullOrEmpty(ev.choiceId)) save.SetChoice(ev.choiceId + ResultSuffix, outcome.ToString().ToLowerInvariant());
            if (showReaction) dialog.Show(speaker, portrait, Reaction(outcome), null);
            order = null;
            visit = null;
        }

        static List<string> Reaction(InfluencerOutcome outcome) => outcome switch
        {
            InfluencerOutcome.Great => new List<string>
            {
                "와, 이거 정말 맛있어요! 오늘 영상 제목은 '동네 숨은 맛집'으로 해야겠어요.",
                "구독자분들께 꼭 추천할게요. 앞으로 손님 많아지실 거예요!",
            },
            InfluencerOutcome.Good => new List<string>
            {
                "음, 맛있네요! 분위기 좋은 카페로 소개할게요.",
                "영상 올라가면 손님이 조금 늘 거예요. 감사합니다!",
            },
            InfluencerOutcome.Bad => new List<string>
            {
                "음... 기대했던 맛이랑은 좀 다르네요.",
                "영상에는 솔직하게 올릴게요. 다음엔 더 맛있게 부탁드려요.",
            },
            _ => new List<string>
            {
                "기다리다 촬영 시간이 다 지나 버렸네요...",
                "오늘 촬영은 여기까지 할게요. 영상에는 있는 그대로 올릴게요.",
            },
        };
    }
}
