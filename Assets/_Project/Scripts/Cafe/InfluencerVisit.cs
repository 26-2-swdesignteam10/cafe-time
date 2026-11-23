using System.Collections.Generic;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 영업 중 인플루언서 방문 1건 (SFR-009 DAY 6, ADR-0048). GameBalance 의 InfluencerEvent 를 실제 메뉴로 바꾼 것
    public class InfluencerVisit
    {
        public InfluencerEvent Event { get; }
        public OrderLine Line { get; }   // 추천한 대표 메뉴 1잔
        public List<OrderLine> Lines => new List<OrderLine> { Line };
        public int Price => Line.Price;
        public float DeadlineMinutes => Event.deadlineHour * 60f;
        public string DeadlineText => GameClock.HourText(Event.deadlineHour);

        // 대사용 메뉴 이름: "아이스 바닐라라떼" (온도 고정 메뉴는 온도를 붙이지 않음)
        public string MenuText =>
            Line.Temperature == Temperature.None || Line.Menu.OnlyTemperature != Temperature.None
                ? Line.Menu.DisplayName
                : $"{(Line.Temperature == Temperature.Hot ? "따뜻한" : "아이스")} {Line.Menu.DisplayName}";

        InfluencerVisit(InfluencerEvent ev, OrderLine line)
        {
            Event = ev;
            Line = line;
        }

        // 설정의 메뉴 id 를 쓸 수 없으면 null (이벤트 없음)
        public static InfluencerVisit From(InfluencerEvent ev, GameBalance balance)
        {
            if (ev == null) return null;
            var menu = balance.FindMenu(ev.menuId);
            if (menu == null || menu.IsDessert)
            {
                Debug.LogWarning($"인플루언서 대표 메뉴 '{ev.menuId}' 를 쓸 수 없어 이벤트를 건너뜁니다.");
                return null;
            }
            var temperature = menu.OnlyTemperature != Temperature.None ? menu.OnlyTemperature : ev.temperature;
            return new InfluencerVisit(ev, new OrderLine(menu, temperature, 1));
        }

        // 서빙한 물건 중 대표 메뉴와 맞는 음료의 가장 좋은 등급으로 결과를 정한다. 맞는 음료가 없으면 Bad
        public InfluencerOutcome Judge(IReadOnlyList<HeldItem> served, out bool matched)
        {
            matched = false;
            var best = DrinkGrade.Failed;
            foreach (var item in served)
            {
                if (!Line.Matches(item.Menu, item.Temperature) || item.Grade == DrinkGrade.Failed) continue;
                matched = true;
                if (item.Grade < best) best = item.Grade; // Perfect < Good < Bad
            }
            if (!matched) return InfluencerOutcome.Bad;
            return best switch
            {
                DrinkGrade.Perfect => InfluencerOutcome.Great,
                DrinkGrade.Good => InfluencerOutcome.Good,
                _ => InfluencerOutcome.Bad,
            };
        }

        // 결과에 따른 평판 변동(그날 정산)과 손님 수 배율(다음 DAY 부터)
        public void Effect(InfluencerOutcome outcome, out float reputation, out float visitorRate)
        {
            switch (outcome)
            {
                case InfluencerOutcome.Great:
                    reputation = Event.greatReputation;
                    visitorRate = Event.greatVisitorRate;
                    break;
                case InfluencerOutcome.Good:
                    reputation = Event.goodReputation;
                    visitorRate = Event.goodVisitorRate;
                    break;
                default:
                    reputation = Event.badReputation;
                    visitorRate = Event.badVisitorRate;
                    break;
            }
        }
    }
}
