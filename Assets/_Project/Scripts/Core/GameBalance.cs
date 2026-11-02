using System.Collections.Generic;
using UnityEngine;

namespace CafeTime.Core
{
    // 여러 씬이 함께 쓰는 밸런스 수치 (ADR-0009). 값은 docs/GAME_DATA.md 와 함께 관리한다.
    // 에셋 위치: Assets/_Project/Resources/GameBalance.asset (CafeTime > Create Game Balance 로 생성)
    [CreateAssetMenu(fileName = "GameBalance", menuName = "CafeTime/Game Balance")]
    public class GameBalance : ScriptableObject
    {
        public const int TotalDays = 15; // SFR-005

        [Header("진행")]
        [SerializeField] int startingMoney = 150000;
        [Tooltip("DAY 1 ~ 15 임대료 (SFR-071)")]
        [SerializeField] int[] rentByDay = DefaultRent();
        [Range(1f, 5f)]
        [SerializeField] float initialReputation = 3f;

        [Header("재료 (SFR-010)")]
        [SerializeField] List<IngredientDefinition> ingredients = DefaultIngredients();

        static GameBalance instance;

        public static GameBalance Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GameBalance>("GameBalance");
                    if (instance == null)
                        Debug.LogError("Resources/GameBalance 에셋이 없습니다. CafeTime > Create Game Balance 를 실행하세요.");
                }
                return instance;
            }
        }

        public int StartingMoney => startingMoney;
        public float InitialReputation => initialReputation;
        public IReadOnlyList<IngredientDefinition> Ingredients => ingredients;

        public int GetRent(int day) => rentByDay[Mathf.Clamp(day - 1, 0, rentByDay.Length - 1)];

        public IngredientDefinition FindIngredient(string id) => ingredients.Find(i => i.Id == id);

        // ───────── 에셋을 처음 만들 때의 기본값 (docs/GAME_DATA.md 의 임시 값) ─────────

        static int[] DefaultRent()
        {
            var rent = new int[TotalDays];
            for (int i = 0; i < rent.Length; i++) rent[i] = 60000;
            return rent;
        }

        static List<IngredientDefinition> DefaultIngredients()
        {
            const IngredientCategory coffee = IngredientCategory.Coffee;
            const IngredientCategory liquid = IngredientCategory.MilkLiquid;
            const IngredientCategory syrup = IngredientCategory.SyrupCheong;
            const IngredientCategory tea = IngredientCategory.Tea;
            const IngredientCategory smoothie = IngredientCategory.Smoothie;
            const IngredientCategory dessert = IngredientCategory.Dessert;
            return new List<IngredientDefinition>
            {
                new("bean", "원두", coffee, "봉", 20, "회", 12000),
                new("milk", "우유", liquid, "팩", 1000, "g", 2500),
                new("soda", "탄산수", liquid, "병", 1000, "g", 1500),
                new("condensed_milk", "연유", liquid, "통", 500, "g", 4000),
                new("syrup_plain", "일반 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_vanilla", "바닐라 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_caramel", "카라멜 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_hazelnut", "헤이즐넛 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_chocolate", "초코 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_blue", "블루 시럽", syrup, "병", 1000, "g", 8000),
                new("cheong_strawberry", "딸기청", syrup, "병", 1000, "g", 10000),
                new("cheong_grapefruit", "자몽청", syrup, "병", 1000, "g", 10000),
                new("cheong_yuja", "유자청", syrup, "병", 1000, "g", 10000),
                new("tea_chamomile", "캐모마일 티백", tea, "박스", 10, "개", 5000),
                new("tea_earlgrey", "얼그레이 티백", tea, "박스", 10, "개", 5000),
                new("smoothie_strawberry", "딸기 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_mango", "망고 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_hallabong", "한라봉 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_melon", "멜론 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_cookie", "쿠키앤크림", smoothie, "팩", 1000, "g", 9000),
                new("dessert_strawberry_cake", "딸기 케이크", dessert, "개", 1, "개", 2500),
                new("dessert_croissant", "크루아상", dessert, "개", 1, "개", 1500),
                new("dessert_cookie", "쿠키", dessert, "개", 1, "개", 800),
            };
        }
    }
}
