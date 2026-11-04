using System.Collections.Generic;
using System.Linq;
using CafeTime.UI;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 픽업대 (SFR-056): 손의 완성 음료·디저트를 끌어다 놓고, 위쪽 주문표를 눌러 그 주문에 배정한다.
    // 주문한 개수가 모두 모이면 손님이 가져가고 서빙 완료. 다른 메뉴를 배정해도 놓을 수 있다(만족도 하락)
    public class PickupScreen : Popup
    {
        [SerializeField] PickupSlotView[] slots;

        PickupSlotView selected;

        protected override void Awake()
        {
            base.Awake();
            OrderBoard.Instance.TicketClicked += Assign;
            OrderBoard.Instance.Removed += Unassign;
        }

        void OnDestroy()
        {
            if (OrderBoard.Instance == null) return;
            OrderBoard.Instance.TicketClicked -= Assign;
            OrderBoard.Instance.Removed -= Unassign;
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        public void PlaceFromHand(PickupSlotView slot, int hand)
        {
            if (slot.Item != null || HandInventory.Instance[hand] == null) return;
            slot.Item = HandInventory.Instance.Take(hand);
            selected = slot; // 놓자마자 주문표를 누르면 배정되도록
            Refresh();
        }

        // 미배정 물건: 선택 / 이미 선택된 물건을 다시 누르면 손으로 되돌림
        public void Click(PickupSlotView slot)
        {
            if (slot.Item == null) return;
            if (slot == selected && slot.Assigned == null && HandInventory.Instance.TryAdd(slot.Item))
            {
                slot.Item = null;
                selected = null;
            }
            else selected = slot.Assigned == null ? slot : null;
            Refresh();
        }

        void Assign(Order order)
        {
            if (!isActiveAndEnabled || selected == null || selected.Item == null || selected.Assigned != null) return;
            if (slots.Count(s => s.Assigned == order) >= order.ItemCount) return; // 이미 다 모임
            selected.Assigned = order;
            selected = null;

            var assigned = slots.Where(s => s.Assigned == order).ToList();
            if (assigned.Count >= order.ItemCount)
            {
                // 주문한 메뉴가 모두 모임 → 손님이 가져감
                var items = new List<HeldItem>();
                foreach (var s in assigned)
                {
                    items.Add(s.Item);
                    s.Item = null;
                    s.Assigned = null;
                }
                CustomerManager.Instance.Serve(order, items);
            }
            Refresh();
        }

        // 손님이 떠나 주문이 사라지면 놓인 물건은 미배정으로 남는다
        void Unassign(Order order)
        {
            foreach (var s in slots.Where(s => s.Assigned == order)) s.Assigned = null;
            if (isActiveAndEnabled) Refresh();
        }

        void Refresh()
        {
            foreach (var slot in slots) slot.Refresh(slot == selected);
        }
    }
}
