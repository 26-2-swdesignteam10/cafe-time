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
    // 예약 주문 (SFR-009 DAY 14 → 15, ADR-0051): 예약 DAY 영업 중에 대화창으로 다음 날 주문 예약을 수락/거절하고,
    //  수락했으면 주문 DAY 의 요청 시각에 단체 손님이 와서 선택 없이 주문이 등록된다. 그 뒤 서빙·마감은 위와 같다
    public class GroupOrderManager : MonoBehaviour
    {
        [SerializeField] EventDialog dialog;
        [Tooltip("대화창에 표시할 단체 손님 대표 그림")]
        [SerializeField] Sprite representative;

        public static GroupOrderManager Instance { get; private set; }

        const string Speaker = "단체 손님";
        public const string Accept = "accept";
        public const string Decline = "decline";

        GroupOrderEvent ev;          // 오늘 들어오는 단체 주문 (당일 요청 또는 수락한 예약)
        GroupOrderEvent reservation; // 오늘 영업 중에 받는 다음 날 이후 주문 예약
        bool reserveAsked;
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
            var balance = GameBalance.Instance;
            var save = SaveSystem.Current;
            ev = balance.GroupOrderFor(save.day);
            if (ev != null && ev.reserveDay > 0 && save.GetChoice(ev.choiceId) != Accept) ev = null; // 예약을 받지 않았으면 오지 않음
            reservation = balance.GroupReservationFor(save.day);
        }

        void Update()
        {
            var clock = GameClock.Instance;
            if (clock == null || EventDialog.IsShowing) return; // 다른 이벤트 대화 중이면 끝난 뒤에
            if (reservation != null && !reserveAsked && !clock.IsEntryClosed && clock.Minutes >= reservation.reserveHour * 60f)
            {
                AskReservation();
                return; // 같은 프레임에 대화창을 겹쳐 열지 않는다
            }
            if (ev == null) return;
            if (!requested && !clock.IsEntryClosed && clock.Minutes >= ev.requestHour * 60f)
            {
                if (ev.reserveDay > 0) ArriveReserved();
                else Request();
            }
            if (order != null && clock.Minutes >= pending.DeadlineMinutes) Expire();
        }

        // ───────── 요청·선택 (SFR-008, 009) ─────────

        void Request()
        {
            requested = true;
            pending = GroupOrder.From(ev, GameBalance.Instance);
            if (pending == null) return;
            string menuText = MenuText(pending);
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
            RegisterOrder();
            return $"감사합니다! {pending.DeadlineText}까지 픽업대로 찾으러 올게요.";
        }

        void RegisterOrder()
        {
            order = OrderBoard.Instance.Register(pending.Lines.Select(l => new OrderLine(l.Menu, l.Temperature, l.Quantity)).ToList(),
                pending.Total, null, pending);
        }

        static string MenuText(GroupOrder group) => string.Join(" ", OrderText.Sentences(group.Lines)).Replace(" 주세요.", "");

        // ───────── 예약 (SFR-009 DAY 14, ADR-0051) ─────────

        // 다음 날 주문을 미리 알려 주므로 오픈 준비 때 재료를 사 둘 수 있다
        void AskReservation()
        {
            reserveAsked = true;
            var preview = GroupOrder.From(reservation, GameBalance.Instance);
            if (preview == null) return;
            string when = DayText(reservation.day);
            var lines = new List<string>
            {
                "사장님, 안녕하세요! 저희 회사가 바로 옆인데, 여기 커피가 맛있다고 소문이 났어요.",
                $"{when} 회사 행사가 있어서 음료를 단체로 예약하고 싶은데요. {MenuText(preview)}, 모두 {preview.CupCount}잔이요.",
                $"{when} {GameClock.HourText(reservation.requestHour)}에 주문 넣고 {preview.DeadlineText}까지 찾으러 올게요. ₩{preview.Total:N0}에 ₩{preview.Bonus:N0}을 더 얹어 드릴게요. 예약 가능할까요?",
            };
            dialog.Show(Speaker, representative, lines, accept =>
            {
                SaveSystem.Current.SetChoice(reservation.choiceId, accept ? Accept : Decline); // 하루 정산 때 같이 저장 (SFR-071)
                return accept
                    ? $"감사합니다! 그럼 {when} 뵐게요. 재료 넉넉히 준비해 주세요!"
                    : "아, 어려우시군요. 다른 곳을 알아볼게요. 수고하세요!";
            });
        }

        static string DayText(int day)
        {
            int diff = day - SaveSystem.Current.day;
            return diff == 1 ? "내일" : diff == 2 ? "모레" : $"DAY {day}에";
        }

        // 예약한 단체 손님이 와서 바로 주문한다 (선택 없음)
        void ArriveReserved()
        {
            requested = true;
            pending = GroupOrder.From(ev, GameBalance.Instance);
            if (pending == null) return;
            RegisterOrder();
            var lines = new List<string>
            {
                "사장님! 예약했던 단체 주문 하러 왔어요.",
                $"{MenuText(pending)}, 모두 {pending.CupCount}잔이요. {pending.DeadlineText}까지 픽업대로 찾으러 올게요. 잘 부탁드려요!",
            };
            dialog.Show(Speaker, representative, lines, null);
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
