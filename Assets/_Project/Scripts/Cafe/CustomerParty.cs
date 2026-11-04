using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 일행 (1~4명). 입장 → 줄 서기 → 주문 → 테이블에서 대기 → 음료 받고 마시기 → 퇴장 (SFR-024, 025, 057)
    public class CustomerParty : MonoBehaviour
    {
        public enum State { ToQueue, Queued, ToTable, Waiting, Drinking, Leaving }

        public State Current { get; set; }
        public List<Customer> Members { get; } = new List<Customer>();
        public List<OrderLine> Wanted { get; set; }   // 손님이 실제로 원하는 메뉴
        public CafeTable Table { get; set; }
        public Order Order { get; set; }
        public int QueueIndex { get; set; } = -1;
        public float Patience { get; set; }           // 남은 인내심 (실제 초)
        public float MaxPatience { get; set; }
        public float WaitedSeconds { get; set; }      // 주문 후 서빙까지 걸린 시간 (만족도)
        public float StayLeft { get; set; }
        public WorldGauge Gauge { get; set; }

        public Customer Leader => Members[0];
        public bool Arrived => Members.All(m => !m.IsMoving);

        public string Sentence => OrderText.Sentence(Wanted);

        // 받은 물건으로 만족도 계산 (0~100, SFR-068 계산식은 추후 결정 → 임시)
        //  - 원한 메뉴(메뉴+HOT/ICE)와 다른 물건 1개마다 크게 하락
        //  - 음료 등급: 좋음 -10, 나쁨 -30, 실패 -60
        //  - 대기 시간: 인내심을 다 쓸수록 최대 -30
        public float Satisfaction(IReadOnlyList<HeldItem> received, float servePatience)
        {
            var remaining = Wanted.SelectMany(l => Enumerable.Repeat((l.Menu, l.Temperature), l.Quantity)).ToList();
            int total = Mathf.Max(1, remaining.Count);
            float score = 100f;
            foreach (var item in received)
            {
                int index = remaining.FindIndex(w => w.Menu == item.Menu && w.Temperature == item.Temperature);
                if (index >= 0) remaining.RemoveAt(index);
                else score -= 100f / total; // 주문과 다른 음료
                if (item.Grade == DrinkGrade.Good) score -= 10f / total;
                else if (item.Grade == DrinkGrade.Bad) score -= 30f / total;
                else if (item.Grade == DrinkGrade.Failed) score -= 60f / total;
            }
            score -= 30f * Mathf.Clamp01(WaitedSeconds / servePatience);
            return Mathf.Clamp(score, 0f, 100f);
        }

        // 일행 인원수만큼 원하는 음료(+가끔 디저트)를 정한다
        public static List<OrderLine> RandomWants(int size, GameBalance balance)
        {
            var drinks = balance.Menus.Where(m => !m.IsDessert).ToList();
            var desserts = balance.Menus.Where(m => m.IsDessert).ToList();
            var lines = new List<OrderLine>();

            void Add(MenuDefinition menu, Temperature temperature)
            {
                var line = lines.Find(l => l.Matches(menu, temperature));
                if (line != null) line.Quantity++;
                else lines.Add(new OrderLine(menu, temperature, 1));
            }

            for (int i = 0; i < size; i++)
            {
                var menu = drinks[Random.Range(0, drinks.Count)];
                var temperature = menu.OnlyTemperature != Temperature.None ? menu.OnlyTemperature
                    : Random.value < 0.5f ? Temperature.Hot : Temperature.Ice;
                Add(menu, temperature);
            }
            if (desserts.Count > 0 && Random.value < balance.DessertChance)
            {
                int count = Random.Range(1, size + 1);
                for (int i = 0; i < count; i++) Add(desserts[Random.Range(0, desserts.Count)], Temperature.None);
            }
            // 음료 먼저, 디저트 나중 (대사 순서)
            return lines.OrderBy(l => l.Menu.IsDessert).ToList();
        }
    }
}
