using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 픽업대 (SFR-056): 대기 중인 주문마다 트레이가 하나씩 생긴다. 손 칸에서 고른 완성 음료·디저트를
    // 트레이를 눌러(또는 끌어다) 놓고, 서빙 버튼을 누르면 손님이 가져간다 (ADR-0018).
    // 주문과 다른 메뉴도 놓을 수 있고, 덜 채운 채로 서빙할 수도 있다 (만족도 하락)
    public class PickupScreen : Popup
    {
        [SerializeField] Transform trayContainer;
        [SerializeField] PickupTrayView trayTemplate;
        [SerializeField] GameObject emptyLabel;

        static readonly List<HeldItem> None = new List<HeldItem>();

        // 주문별 트레이에 놓인 물건. 창을 닫아도 남는다
        readonly Dictionary<Order, List<HeldItem>> trays = new Dictionary<Order, List<HeldItem>>();
        readonly List<PickupTrayView> views = new List<PickupTrayView>();

        protected override void Awake()
        {
            base.Awake();
            trayTemplate.gameObject.SetActive(false);
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

        // 트레이를 누름: 손 칸에서 고른 물건을 놓는다. 고른 손이 없으면 완성품을 든 손(왼손 먼저)
        public void Click(Order order)
        {
            var hands = HandInventory.Instance;
            int hand = hands.Selected;
            if (hand < 0) hand = hands[HandInventory.Left] != null && hands[HandInventory.Left].Servable ? HandInventory.Left : HandInventory.Right;
            PlaceFromHand(order, hand);
        }

        // 완성 음료·디저트만, 주문 개수까지만 놓을 수 있다
        public void PlaceFromHand(Order order, int hand)
        {
            var item = HandInventory.Instance[hand];
            if (item == null || !item.Servable || PlacedCount(order) >= order.ItemCount) return;
            if (!trays.TryGetValue(order, out var items)) trays[order] = items = new List<HeldItem>();
            items.Add(HandInventory.Instance.Take(hand));
            Refresh();
        }

        // 트레이에 놓인 물건을 눌러 손으로 되돌림
        public void TakeBack(Order order, HeldItem item)
        {
            if (!trays.TryGetValue(order, out var items) || !items.Contains(item)) return;
            if (!HandInventory.Instance.TryAdd(item))
            {
                Toast.Show(ToastMessage.HandsFull);
                return;
            }
            items.Remove(item);
            Refresh();
        }

        // 서빙 버튼: 놓인 물건을 손님이 가져간다 (빈 트레이는 서빙 불가).
        // 더 먼저 들어온 대기 주문이 있는데 이 주문을 서빙하면 선입선출 위반 감점 (ADR-0022)
        public void Serve(Order order)
        {
            if (PlacedCount(order) == 0) return;
            if (OrderBoard.Instance.Orders.Any(o => o.Number < order.Number))
            {
                DayResult.Today.ordersSkipped++;
                Toast.Show(ToastMessage.OrderSkipped);
            }
            var items = trays[order];
            trays.Remove(order);
            CustomerManager.Instance.Serve(order, items);
            Refresh();
        }

        // 손님이 떠나 주문이 사라지면 트레이에 놓인 음료는 버려진다
        void Discard(Order order) => trays.Remove(order);

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
                if (i < orders.Count) views[i].Setup(orders[i], trays.TryGetValue(orders[i], out var items) ? items : None);
                else views[i].gameObject.SetActive(false);
            }
            emptyLabel.SetActive(orders.Count == 0);
        }
    }
}
