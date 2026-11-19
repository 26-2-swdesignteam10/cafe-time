using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 영업 중 단체 주문 이벤트 (SFR-009 DAY 4, ADR-0042)
    //  1. 정해진 시각에 단체 손님 대표가 대화창으로 주문을 요청 → 수락/거절 (SFR-008 선택지, 결과는 SaveData.choices)
    //  2. 수락하면 포스기 입력 없이 주문 내역·픽업대에 단체 주문이 등록된다
    //  3. 마감 시각 전에 픽업대에서 서빙하면 맞춘 음료 값 + (전부 맞으면) 보너스를 받는다
    //  4. 마감 시각까지 서빙하지 못하면 취소되고 손님 이탈 1회로 감점된다
    // 같은 구조로 DAY 15 단체 주문 예약(DAY 14 선택)도 붙일 수 있다
    public class GroupOrderManager : MonoBehaviour
    {
        [SerializeField] EventDialog dialog;
        [Tooltip("대화창에 표시할 단체 손님 대표 그림")]
        [SerializeField] Sprite representative;

        public static GroupOrderManager Instance { get; private set; }

        const string Speaker = "단체 손님";
        public const string Accept = "accept";
        public const string Decline = "decline";

        GroupOrderEvent ev;
        GroupOrder pending; // 대화 중 또는 수락 후 서빙 전
        Order order;
        bool requested;

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Start()
        {
            ev = GameBalance.Instance.GroupOrderFor(SaveSystem.Current.day);
        }

        void Update()
        {
            var clock = GameClock.Instance;
            if (ev == null || clock == null) return;
            if (!requested && !clock.IsEntryClosed && clock.Minutes >= ev.requestHour * 60f) Request();
            if (order != null && clock.Minutes >= pending.DeadlineMinutes) Expire();
        }

        // ───────── 요청·선택 (SFR-008, 009) ─────────

        void Request()
        {
            requested = true;
            pending = GroupOrder.From(ev, GameBalance.Instance);
            if (pending == null) return;
            var menu = OrderText.Sentences(pending.Lines);
            string menuText = string.Join(" ", menu).Replace(" 주세요.", "");
            var lines = new List<string>
            {
                "안녕하세요! 근처 회사에서 왔는데요, 회의에 쓸 음료를 한꺼번에 주문하고 싶어서요.",
                $"{menuText}, 모두 {pending.CupCount}잔이요.",
                $"{pending.DeadlineText}까지 준비해 주시면 ₩{pending.Total:N0}에 ₩{pending.Bonus:N0}을 더 얹어 드릴게요. 가능할까요?",
            };
            dialog.Show(Speaker, representative, lines, Choose);
        }

        string Choose(bool accept)
        {
            SaveSystem.Current.SetChoice(ev.choiceId, accept ? Accept : Decline); // 하루 정산 때 같이 저장 (SFR-071)
            if (!accept)
            {
                pending = null;
                return "아, 오늘은 어려우시군요. 다음에 또 들를게요!";
            }
            order = OrderBoard.Instance.Register(pending.Lines.Select(l => new OrderLine(l.Menu, l.Temperature, l.Quantity)).ToList(),
                pending.Total, null, pending);
            return $"감사합니다! {pending.DeadlineText}까지 픽업대로 찾으러 올게요.";
        }

        // ───────── 서빙·마감 ─────────

        // 픽업대 서빙 버튼 (PickupScreen). 덜 채우거나 다른 음료를 놓아도 서빙할 수 있고, 맞춘 만큼만 받는다
        public void Serve(Order served, IReadOnlyList<HeldItem> items)
        {
            if (served != order) return;
            OrderBoard.Instance.Remove(order);
            int earned = pending.Earned(items, out _);
            var save = SaveSystem.Current;
            var result = DayResult.Today;
            save.money += earned;
            result.revenue += earned;
            result.ordersCompleted++;
            result.satisfactionSum += CustomerParty.MatchScore(pending.Lines, items);
            result.drinksSold += items.Count;
            // 포장해 가므로 매장 컵은 돌아오지 않는다 → 일회용 포장 컵을 썼다고 보고 매장 컵 수는 그대로 둔다 (ADR-0042)
            foreach (var item in items)
                if (item is Cup cup) CupShelf.Instance.Return(cup.Type == CupType.Ceramic ? TrayItemKind.DirtyCeramic : TrayItemKind.DirtyPlastic);
            order = null;
            pending = null;
        }

        // 마감 시각이 지났거나 영업이 끝났는데 서빙하지 못함: 주문 취소, 손님 이탈 감점 (SFR-057, 068)
        public void Expire()
        {
            if (order == null) return;
            OrderBoard.Instance.Remove(order); // 픽업대 트레이에 놓아 둔 음료는 버려진다
            DayResult.Today.customersLost++;
            Toast.Show(ToastMessage.GroupOrderExpired);
            order = null;
            pending = null;
        }
    }
}
