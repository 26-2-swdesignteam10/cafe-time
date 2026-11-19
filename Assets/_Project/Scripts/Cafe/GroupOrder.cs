using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 영업 중 단체 주문 1건 (SFR-009 DAY 4, ADR-0042). GameBalance 의 GroupOrderEvent 를 실제 메뉴로 바꾼 것
    public class GroupOrder
    {
        public GroupOrderEvent Event { get; }
        public List<OrderLine> Lines { get; }
        public int Total { get; }   // 주문 금액 (정가 합계)
        public int Bonus { get; }   // 모두 정확히 맞췄을 때 더 받는 금액
        public float DeadlineMinutes => Event.deadlineHour * 60f;
        public string DeadlineText => GameClock.HourText(Event.deadlineHour);
        public int CupCount => Lines.Sum(l => l.Quantity);

        GroupOrder(GroupOrderEvent ev, List<OrderLine> lines)
        {
            Event = ev;
            Lines = lines;
            Total = lines.Sum(l => l.Price);
            Bonus = Mathf.RoundToInt(Total * ev.bonusRate / 100f) * 100; // 100원 단위
        }

        // 설정의 메뉴 id 가 없으면 그 줄은 건너뛴다. 남는 줄이 없으면 null
        public static GroupOrder From(GroupOrderEvent ev, GameBalance balance)
        {
            if (ev == null || ev.lines == null) return null;
            var lines = new List<OrderLine>();
            foreach (var entry in ev.lines)
            {
                var menu = balance.FindMenu(entry.menuId);
                if (menu == null || menu.IsDessert || entry.quantity <= 0)
                {
                    Debug.LogWarning($"단체 주문 메뉴 '{entry.menuId}' 를 쓸 수 없어 건너뜁니다.");
                    continue;
                }
                var temperature = menu.OnlyTemperature != Temperature.None ? menu.OnlyTemperature : entry.temperature;
                lines.Add(new OrderLine(menu, temperature, entry.quantity));
            }
            return lines.Count > 0 ? new GroupOrder(ev, lines) : null;
        }

        // 서빙한 물건으로 받을 금액: 주문과 맞고 실패하지 않은 음료만 값을 받고, 전부 맞으면 보너스 (ADR-0042)
        public int Earned(IReadOnlyList<HeldItem> served, out bool perfect)
        {
            var remaining = Lines.SelectMany(l => Enumerable.Repeat(l, l.Quantity)).ToList();
            int earned = 0, wrong = 0;
            foreach (var item in served)
            {
                int index = remaining.FindIndex(l => l.Matches(item.Menu, item.Temperature));
                if (index < 0 || item.Grade == DrinkGrade.Failed)
                {
                    wrong++;
                    continue;
                }
                earned += remaining[index].Menu.Price;
                remaining.RemoveAt(index);
            }
            perfect = wrong == 0 && remaining.Count == 0;
            return perfect ? earned + Bonus : earned;
        }
    }
}
