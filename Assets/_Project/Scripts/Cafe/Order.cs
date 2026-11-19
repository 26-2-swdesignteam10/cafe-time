using System.Collections.Generic;
using System.Linq;

namespace CafeTime.Cafe
{
    // 포스기로 결제해 등록한 주문 (SFR-027). Lines 는 플레이어가 입력한 내용이고,
    // 손님이 실제로 원한 메뉴는 Party.Wanted 에 있다 (잘못 입력해도 그대로 주문서에 반영, SFR-026)
    // 단체 주문(SFR-009, ADR-0042)은 Party 없이 Group 이 있고, Paid 는 서빙하면 받을 주문 금액이다
    public class Order
    {
        public int Number { get; }
        public IReadOnlyList<OrderLine> Lines { get; }
        public int Paid { get; }
        public CustomerParty Party { get; }
        public GroupOrder Group { get; }

        public bool IsGroup => Group != null;

        public Order(int number, List<OrderLine> lines, int paid, CustomerParty party, GroupOrder group = null)
        {
            Number = number;
            Lines = lines;
            Paid = paid;
            Party = party;
            Group = group;
        }

        // 베이커리는 손님이 이미 가져왔다. 영수증에는 포함하고 제조·픽업 목록에서는 제외 (SFR-056).
        public IReadOnlyList<OrderLine> PreparationLines => Lines.Where(l => !l.Menu.IsDessert).ToList();
        public int ItemCount => PreparationLines.Sum(l => l.Quantity);
    }
}
