using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 일행 (1~4명). 입장 → 베이커리 직접 픽업 → 줄 서기 → 주문 → 테이블에서 대기 → 음료 받고 마시기 → 퇴장 (SFR-024, 025, 057)
    public class CustomerParty : MonoBehaviour
    {
        public enum State { ToShowcase, ToQueue, Queued, ToTable, Waiting, ToPickup, Drinking, Leaving }

        public State Current { get; set; }
        public List<Customer> Members { get; } = new List<Customer>();
        public List<OrderLine> Wanted { get; set; }   // 손님이 실제로 원하는 메뉴
        public CafeTable Table { get; set; }
        public Order Order { get; set; }
        public int QueueIndex { get; set; } = -1;
        public float Patience { get; set; }           // 남은 인내심 (실제 초)
        public float MaxPatience { get; set; }
        public float KioskLeft { get; set; }          // 키오스크 자동 주문까지 남은 시간 (포스기 3단계, ADR-0057)
        public float WaitedSeconds { get; set; }      // 주문 후 서빙까지 걸린 시간 (만족도)
        public float StayLeft { get; set; }
        public float StayTotal { get; set; }          // 서빙받은 뒤 머무는 전체 시간 (먹는 진행률, ADR-0053)
        public List<HeldItem> Served { get; set; }    // 받은 음료·디저트 (퇴식구 쟁반 내용, SFR-060)
        public WorldGauge Gauge { get; set; }
        public Customer Runner { get; set; }              // 진동벨이 울려 픽업대로 받으러 간 일행 1명 (ADR-0040)
        public List<HeldItem> CalledItems { get; set; }   // 호출할 때 트레이에 놓여 있던 물건

        public List<OrderLine> CollectedBakery { get; } = new List<OrderLine>(); // 직접 집은 베이커리 (SFR-026, 056)
        public List<OrderLine> EatenBakery { get; set; }  // 서빙 때 받은 베이커리. 테이블에 놓고 먹는다 (ADR-0053)
        public bool MealShown { get; set; }               // 받은 음식을 테이블에 놓았는지

        // 단골손님 (ADR-0049). null 이면 일반 손님. Visits = 오늘 전까지 주문한 횟수
        public RegularCustomer Regular { get; set; }
        public int RegularVisits { get; set; }
        public bool RegularRemembered { get; set; } // true 면 메뉴를 말하지 않고 '늘 마시던 걸로' 주문

        public bool LunchRush { get; set; } // 점심시간 러시에 들어온 손님 (ADR-0050). 음료를 받고 금방 떠난다

        public Customer Leader => Members[0];
        public bool Arrived => Members.All(m => !m.IsMoving);

        public List<string> Sentences
        {
            get
            {
                if (Regular != null) return RegularSentences();
                var sentences = OrderText.Sentences(Wanted.Where(l => !l.Menu.IsDessert).ToList());
                // 집어 온 베이커리 품목·수량을 대사로 알려 주고 플레이어가 직접 포스기에 입력한다 (SFR-026).
                foreach (var sentence in OrderText.Sentences(CollectedBakery))
                    sentences.Add(sentence.Replace(" 주세요.", " 결제해 주세요."));
                return sentences;
            }
        }

        // 단골 주문 대사: 이름을 밝히고, 기억해 줄 때가 되면 메뉴 없이 '늘 마시던 걸로' (SFR-009 DAY 9, ADR-0049)
        List<string> RegularSentences()
        {
            string name = Regular.displayName;
            if (RegularRemembered)
                return new List<string> { $"{name}{Copula(name)}! 사장님, 또 왔어요.", "늘 마시던 걸로 주세요." };
            string greeting = RegularVisits == 0
                ? $"안녕하세요, 근처에 사는 {name}{Copula(name)}."
                : $"{name}{Copula(name)}! 오늘도 들렀어요.";
            var sentences = new List<string> { greeting };
            sentences.AddRange(OrderText.Sentences(Wanted));
            return sentences;
        }

        // 받침이 있으면 "이에요", 없으면 "예요"
        static string Copula(string word)
        {
            if (string.IsNullOrEmpty(word)) return "예요";
            char last = word[word.Length - 1];
            bool batchim = last >= 0xAC00 && last <= 0xD7A3 && (last - 0xAC00) % 28 != 0;
            return batchim ? "이에요" : "예요";
        }

        // 받은 물건으로 만족도 계산 (0~100, SFR-068 계산식은 추후 결정 → 임시)
        //  - 원한 메뉴(메뉴+HOT/ICE)와 다른 물건 1개마다 크게 하락
        //  - 음료 등급: 좋음 -10, 나쁨 -30, 실패 -60
        //  - 대기 시간: 인내심을 다 쓸수록 최대 -30
        public float Satisfaction(IReadOnlyList<HeldItem> received, float servePatience)
        {
            float score = MatchScore(Wanted, received);
            score -= 30f * Mathf.Clamp01(WaitedSeconds / servePatience);
            return Mathf.Clamp(score, 0f, 100f);
        }

        // 원한 음료와 받은 물건을 비교한 점수 (0~100, 대기 시간 제외). 단체 주문도 같은 기준을 쓴다 (ADR-0042)
        public static float MatchScore(IEnumerable<OrderLine> wanted, IReadOnlyList<HeldItem> received)
        {
            var remaining = wanted.Where(l => !l.Menu.IsDessert).SelectMany(l => Enumerable.Repeat((l.Menu, l.Temperature), l.Quantity)).ToList();
            int total = Mathf.Max(1, remaining.Count);
            float score = 100f;
            int wrong = 0;
            foreach (var item in received)
            {
                int index = remaining.FindIndex(w => w.Menu == item.Menu && w.Temperature == item.Temperature);
                if (index >= 0) remaining.RemoveAt(index);
                else wrong++; // 주문과 다른 음료
                if (item.Grade == DrinkGrade.Good) score -= 10f / total;
                else if (item.Grade == DrinkGrade.Bad) score -= 30f / total;
                else if (item.Grade == DrinkGrade.Failed) score -= 60f / total;
            }
            // 다른 음료·못 받은 음료 1개당 −100/주문 개수. 다른 음료가 못 받은 자리를 대신하면 한 번만 깎는다 (ADR-0018 덜 채운 서빙)
            score -= 100f / total * Mathf.Max(wrong, remaining.Count);
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
