using System.Collections.Generic;
using CafeTime.UI;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 주문 내역 창 (SFR-013): HUD 의 주문 내역 버튼으로 연다. 대기 중인 주문을 영수증으로 나란히 보여 준다.
    // 영수증에는 픽업대 트레이에 놓인 개수도 표시한다 (SFR-056, ADR-0018)
    public class OrdersScreen : Popup
    {
        [SerializeField] Transform ticketContainer;
        [SerializeField] TicketView ticketTemplate;
        [SerializeField] GameObject emptyLabel;
        [SerializeField] PickupScreen pickup;

        readonly List<TicketView> tickets = new List<TicketView>();

        protected override void Awake()
        {
            base.Awake();
            ticketTemplate.gameObject.SetActive(false);
            OrderBoard.Instance.Changed += RefreshIfOpen;
        }

        void OnDestroy()
        {
            if (OrderBoard.Instance != null) OrderBoard.Instance.Changed -= RefreshIfOpen;
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        void RefreshIfOpen()
        {
            if (isActiveAndEnabled) Refresh();
        }

        void Refresh()
        {
            var orders = OrderBoard.Instance.Orders;
            while (tickets.Count < orders.Count) tickets.Add(Instantiate(ticketTemplate, ticketContainer));
            for (int i = 0; i < tickets.Count; i++)
            {
                if (i < orders.Count) tickets[i].Setup(orders[i], pickup.PlacedCount(orders[i]));
                else tickets[i].gameObject.SetActive(false);
            }
            emptyLabel.SetActive(orders.Count == 0);
        }
    }
}
