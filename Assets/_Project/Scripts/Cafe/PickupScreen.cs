using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 픽업대 (SFR-056): 대기 중인 주문마다 트레이가 하나씩 생긴다. 손 칸에서 고른 완성 음료·디저트를
    // 트레이를 눌러(또는 끌어다) 놓고, 진동벨 창에서 주문 번호를 눌러 호출하면 손님이 와서 가져간다 (ADR-0018, ADR-0040).
    // 주문과 다른 메뉴도 놓을 수 있고, 덜 채운 채로 호출할 수도 있다 (만족도 하락)
    public class PickupScreen : Popup
    {
        [SerializeField] Transform trayContainer;
        [SerializeField] PickupTrayView trayTemplate;
        [SerializeField] GameObject emptyLabel;
        [SerializeField] UnityEngine.UI.Button bellButton;
        [SerializeField] BellCallScreen bellScreen;

        public enum CallResult { Called, NoSuchOrder, EmptyTray, AlreadyCalled }

        static readonly List<HeldItem> None = new List<HeldItem>();

        // 주문별 트레이에 놓인 물건. 창을 닫아도 남는다
        readonly Dictionary<Order, List<HeldItem>> trays = new Dictionary<Order, List<HeldItem>>();
        readonly List<PickupTrayView> views = new List<PickupTrayView>();
        readonly HashSet<Order> called = new HashSet<Order>(); // 호출해서 손님이 받으러 오는 중 (트레이 고정)

        protected override void Awake()
        {
            base.Awake();
            trayTemplate.gameObject.SetActive(false);
            bellButton.onClick.AddListener(bellScreen.Open);
            OrderBoard.Instance.Changed += RefreshIfOpen;
            OrderBoard.Instance.Removed += Discard;
        }

        void OnDestroy()
        {
            if (OrderBoard.Instance == null) return;
            OrderBoard.Instance.Changed -= RefreshIfOpen;
            OrderBoard.Instance.Removed -= Discard;
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        // 주문 내역 창의 영수증에 표시할, 이 주문 트레이에 놓인 물건 수
        public int PlacedCount(Order order) => trays.TryGetValue(order, out var items) ? items.Count : 0;

        public bool IsCalled(Order order) => called.Contains(order);

        // 트레이를 누름: 손 칸에서 고른 물건을 놓는다. 고른 칸이 없으면 완성품을 든 첫 칸(왼쪽부터)
        public void Click(Order order)
        {
            var hands = HandInventory.Instance;
            int hand = hands.Selected;
            if (hand < 0) hand = hands.Find(item => item.Servable);
            if (hand >= 0) PlaceFromHand(order, hand);
        }

        // 완성 음료·디저트만, 주문 개수까지만 놓을 수 있다
        public void PlaceFromHand(Order order, int hand)
        {
            var item = HandInventory.Instance[hand];
            if (item == null || !item.Servable || IsCalled(order) || PlacedCount(order) >= order.ItemCount) return;
            if (!trays.TryGetValue(order, out var items)) trays[order] = items = new List<HeldItem>();
            items.Add(HandInventory.Instance.Take(hand));
            Refresh();
        }

        // 트레이에 놓인 물건을 눌러 손으로 되돌림
        public void TakeBack(Order order, HeldItem item)
        {
            if (IsCalled(order) || !trays.TryGetValue(order, out var items) || !items.Contains(item)) return;
            if (!HandInventory.Instance.TryAdd(item))
            {
                Toast.Show(ToastMessage.HandsFull);
                return;
            }
            items.Remove(item);
            Refresh();
        }

        // 진동벨 호출 (ADR-0040): 번호의 트레이에 놓인 물건을 손님이 받으러 온다. 없는 번호·빈 트레이는 안내만.
        // 더 먼저 들어온 대기 주문이 있는데 이 주문을 먼저 호출하면 선입선출 위반 감점 (ADR-0022)
        public CallResult Call(int number)
        {
            var order = OrderBoard.Instance.Orders.FirstOrDefault(o => o.Number == number);
            if (order == null) return CallResult.NoSuchOrder;
            if (IsCalled(order)) return CallResult.AlreadyCalled;
            if (PlacedCount(order) == 0) return CallResult.EmptyTray;
            if (!CustomerManager.Instance.Call(order, trays[order])) return CallResult.NoSuchOrder; // 아직 자리에 앉지 않음 등
            var orders = OrderBoard.Instance.Orders; // 들어온 순서 (번호는 무작위라 순서와 무관, ADR-0043)
            if (orders.TakeWhile(o => o != order).Any(o => !IsCalled(o)))
            {
                DayResult.Today.ordersSkipped++;
                Toast.Show(ToastMessage.OrderSkipped);
            }
            called.Add(order);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayServe(); // 진동벨 호출 차임 (SFR-056)
            RefreshIfOpen();
            return CallResult.Called;
        }

        // 손님이 받아 가거나 떠나 주문이 사라지면 트레이가 없어진다 (떠난 손님 트레이의 음료는 버려짐)
        void Discard(Order order)
        {
            trays.Remove(order);
            called.Remove(order);
        }

        void RefreshIfOpen()
        {
            if (isActiveAndEnabled) Refresh();
        }

        void Refresh()
        {
            var orders = OrderBoard.Instance.Orders;
            while (views.Count < orders.Count) views.Add(Instantiate(trayTemplate, trayContainer));
            for (int i = 0; i < views.Count; i++)
            {
                if (i < orders.Count) views[i].Setup(orders[i], trays.TryGetValue(orders[i], out var items) ? items : None, IsCalled(orders[i]));
                else views[i].gameObject.SetActive(false);
            }
            emptyLabel.SetActive(orders.Count == 0);
        }
    }
}
