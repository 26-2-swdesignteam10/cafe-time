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

        [Header("메뉴 (SFR-026, 판매가 임시)")]
        [SerializeField] List<MenuDefinition> menus = DefaultMenus();

        [Header("영업 시간 (SFR-014)")]
        [Tooltip("게임 시간 1시간이 실제 몇 초인지 (명세: 2분)")]
        [SerializeField] float secondsPerGameHour = 120f;
        [SerializeField] int openHour = 9;
        [SerializeField] int entryCloseHour = 20;
        [SerializeField] int closeHour = 21;

        [Header("손님 (SFR-024, 025, 057, 058) — 임시값, 시간은 실제 초")]
        [Tooltip("DAY 1 손님 일행 입장 간격 (최소~최대)")]
        [SerializeField] Vector2 arrivalInterval = new Vector2(35f, 70f);
        [Tooltip("DAY가 하루 지날 때마다 입장 간격에 곱하는 값 (작을수록 빨리 붐빔)")]
        [SerializeField] float arrivalIntervalPerDay = 0.95f;
        [Tooltip("일행 1·2·3·4명이 될 가중치")]
        [SerializeField] int[] partySizeWeights = { 4, 3, 2, 1 };
        [Tooltip("일행이 디저트를 같이 주문할 확률")]
        [Range(0f, 1f)] [SerializeField] float dessertChance = 0.35f;
        [Tooltip("줄 맨 앞에서 주문을 기다리는 인내심")]
        [SerializeField] float orderPatience = 60f;
        [Tooltip("주문 후 음료를 기다리는 인내심")]
        [SerializeField] float servePatience = 180f;
        [Tooltip("음료를 받은 뒤 테이블에 머무는 시간")]
        [SerializeField] float stayDuration = 20f;
        [Tooltip("만족도 100일 때 팁을 줄 확률 (만족도에 비례)")]
        [Range(0f, 1f)] [SerializeField] float maxTipChance = 0.6f;
        [Tooltip("팁 금액 (일행 1명당)")]
        [SerializeField] int tipPerPerson = 500;
        [Tooltip("이 만족도 평균일 때 평판 변동 0 (SFR-072)")]
        [SerializeField] float neutralSatisfaction = 60f;
        [Tooltip("하루 평판 변동 최대치 (+/-)")]
        [SerializeField] float maxReputationChange = 0.5f;
        [Tooltip("이 양보다 적으면 HUD 재고를 경고색으로 (기본 단위, SFR-013)")]
        [SerializeField] int lowStockWarning = 5;

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
        public IReadOnlyList<MenuDefinition> Menus => menus;

        public float SecondsPerGameHour => secondsPerGameHour;
        public int OpenHour => openHour;
        public int EntryCloseHour => entryCloseHour;
        public int CloseHour => closeHour;
        public Vector2 ArrivalInterval => arrivalInterval;
        public float ArrivalIntervalPerDay => arrivalIntervalPerDay;
        public IReadOnlyList<int> PartySizeWeights => partySizeWeights;
        public float DessertChance => dessertChance;
        public float OrderPatience => orderPatience;
        public float ServePatience => servePatience;
        public float StayDuration => stayDuration;
        public float MaxTipChance => maxTipChance;
        public int TipPerPerson => tipPerPerson;
        public float NeutralSatisfaction => neutralSatisfaction;
        public float MaxReputationChange => maxReputationChange;
        public int LowStockWarning => lowStockWarning;

        public int GetRent(int day) => rentByDay[Mathf.Clamp(day - 1, 0, rentByDay.Length - 1)];

        public IngredientDefinition FindIngredient(string id) => ingredients.Find(i => i.Id == id);

        public MenuDefinition FindMenu(string id) => menus.Find(m => m.Id == id);

        // ───────── 에셋을 처음 만들 때의 기본값 (docs/GAME_DATA.md 의 임시 값) ─────────

        static int[] DefaultRent()
        {
            var rent = new int[TotalDays];
            for (int i = 0; i < rent.Length; i++) rent[i] = 60000;
            return rent;
        }

        static List<MenuDefinition> DefaultMenus()
        {
            const MenuCategory coffee = MenuCategory.Coffee;
            const MenuCategory beverage = MenuCategory.Beverage;
            const MenuCategory smoothie = MenuCategory.Smoothie;
            const MenuCategory dessert = MenuCategory.Dessert;
            return new List<MenuDefinition>
            {
                new("americano", "아메리카노", coffee, 4000, true, true),
                new("cafe_latte", "카페라떼", coffee, 4500, true, true),
                new("vanilla_latte", "바닐라라떼", coffee, 5000, true, true),
                new("caramel_macchiato", "카라멜마키아토", coffee, 5500, true, true),
                new("hazelnut_latte", "헤이즐넛라떼", coffee, 5000, true, true),
                new("cafe_mocha", "카페모카", coffee, 5500, true, true),
                new("condensed_latte", "연유라떼", coffee, 5500, true, true),
                new("strawberry_latte", "딸기라떼", beverage, 5500, false, true),
                new("grapefruit_ade", "자몽에이드", beverage, 5500, false, true),
                new("yuja_ade", "유자에이드", beverage, 5500, false, true),
                new("strawberry_ade", "딸기에이드", beverage, 5500, false, true),
                new("blue_lemon_ade", "블루레몬에이드", beverage, 5500, false, true),
                new("yuja_tea", "유자차", beverage, 5000, true, false),
                new("grapefruit_tea", "자몽차", beverage, 5000, true, false),
                new("chamomile", "캐모마일", beverage, 4500, true, true),
                new("earl_grey", "얼그레이", beverage, 4500, true, true),
                new("strawberry_smoothie", "딸기 스무디", smoothie, 6000, false, true),
                new("mango_smoothie", "망고 스무디", smoothie, 6000, false, true),
                new("hallabong_smoothie", "한라봉 스무디", smoothie, 6000, false, true),
                new("melon_smoothie", "멜론 스무디", smoothie, 6000, false, true),
                new("cookie_shake", "쿠키앤크림 쉐이크", smoothie, 6000, false, true),
                new("strawberry_cake", "딸기 케이크", dessert, 6500, false, false, "dessert_strawberry_cake"),
                new("croissant", "크루아상", dessert, 4000, false, false, "dessert_croissant"),
                new("cookie", "쿠키", dessert, 3000, false, false, "dessert_cookie"),
            };
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
