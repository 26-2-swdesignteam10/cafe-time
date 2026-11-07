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

        [Header("테스트")]
        [Tooltip("켜면 영업 시작 때 모든 재료·디저트 재고를 가득 채운다. 플레이 테스트용이므로 출시 전에 끈다")]
        [SerializeField] bool testFullStock = true;

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

        [Header("레시피 (SFR-051) — 정량 임시값")]
        [SerializeField] List<RecipeDefinition> recipes = DefaultRecipes();
        [Tooltip("재료별 오차 비율이 이 값 이하면 '완벽'")]
        [SerializeField] float perfectTolerance = 0.1f;
        [Tooltip("이 값 이하면 '좋음', 넘으면 '나쁨'")]
        [SerializeField] float goodTolerance = 0.25f;

        [Header("커피잔·접시 (SFR-028, 064) — 임시값. 매일 영업 시작 때 모두 선반에 있다")]
        [SerializeField] int ceramicCups = 8;
        [SerializeField] int plasticCups = 10;
        [SerializeField] int plates = 6;

        [Header("에스프레소 머신 (SFR-031, 035)")]
        [SerializeField] float grindSeconds = 5f;
        [SerializeField] float extractSeconds = 5f;

        [Header("제빙기 (SFR-038, 039) — 임시값")]
        [SerializeField] int iceMax = 24;
        [Tooltip("얼음 1개를 만드는 시간 (실제 초)")]
        [SerializeField] float iceSecondsPerCube = 3f;
        [Tooltip("스쿱 1회에 담는 얼음 개수")]
        [SerializeField] int icePerScoop = 4;

        [Header("정수기 (SFR-043) — 임시값")]
        [SerializeField] float waterSeconds = 3f;
        [SerializeField] float waterAmount = 200f;

        [Header("음료·쉐이크 제조실 (SFR-046, 047, 053, 054) — 임시값")]
        [Tooltip("시럽·청·연유 클릭 1회 투입량 (g)")]
        [SerializeField] float syrupPerClick = 10f;
        [Tooltip("스무디 원액 클릭 1회 투입량 (g)")]
        [SerializeField] float smoothiePerClick = 50f;
        [Tooltip("우유·탄산수를 누르고 있을 때 1초에 부어지는 양 (g)")]
        [SerializeField] float pourPerSecond = 50f;
        [Tooltip("저울에 표시되는 샷 1개 무게 (g)")]
        [SerializeField] float shotWeight = 30f;
        [Tooltip("저울에 표시되는 얼음 스쿱 1회 무게 (g)")]
        [SerializeField] float iceScoopWeight = 100f;
        [SerializeField] float teabagWeight = 2f;
        [SerializeField] float blendSeconds = 5f;

        [Header("퇴식구·설거지 (SFR-060, 064, 065) — 임시값")]
        [SerializeField] int trayReturnSlots = 4;
        [Range(0f, 1f)] [SerializeField] float napkinChance = 0.5f;
        [Tooltip("얼룩을 다 지우는 데 필요한 수세미 이동 거리 (화면 px)")]
        [SerializeField] float scrubDistance = 1200f;
        [SerializeField] float dryingSeconds = 15f;

        [Header("청소 요청 (SFR-066) — 임시값")]
        [SerializeField] float cleaningTimeLimit = 90f;
        [Tooltip("DAY 1 무작위 청소 요청 간격 (실제 초)")]
        [SerializeField] float cleaningInterval = 300f;
        [Tooltip("DAY가 지날 때마다 간격에 곱하는 값")]
        [SerializeField] float cleaningIntervalPerDay = 0.92f;

        [Header("점수 (SFR-068) — 임시값")]
        [SerializeField] int pointsPerOrder = 100;
        [Tooltip("손님 만족도(0~100) 1점당 점수")]
        [SerializeField] float pointsPerSatisfaction = 1f;
        [SerializeField] int penaltyLost = 100;
        [SerializeField] int penaltyDiscarded = 20;
        [SerializeField] int penaltyTrayLeft = 50;
        [SerializeField] int penaltyCleaning = 50;

        [Header("엔딩 (SFR-073) — 임시값. 최종 수익 = 최종 보유금 - 시작 자금")]
        [SerializeField] int ending03Profit = 150000;
        [SerializeField] float ending03Reputation = 3f;
        [SerializeField] int ending04Profit = 600000;
        [SerializeField] float ending04Reputation = 4f;

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
        public bool TestFullStock => testFullStock;
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

        public IReadOnlyList<RecipeDefinition> Recipes => recipes;
        public float PerfectTolerance => perfectTolerance;
        public float GoodTolerance => goodTolerance;
        public int CeramicCups => ceramicCups;
        public int PlasticCups => plasticCups;
        public int Plates => plates;
        public float GrindSeconds => grindSeconds;
        public float ExtractSeconds => extractSeconds;
        public int IceMax => iceMax;
        public float IceSecondsPerCube => iceSecondsPerCube;
        public int IcePerScoop => icePerScoop;
        public float WaterSeconds => waterSeconds;
        public float WaterAmount => waterAmount;
        public float SyrupPerClick => syrupPerClick;
        public float SmoothiePerClick => smoothiePerClick;
        public float PourPerSecond => pourPerSecond;
        public float ShotWeight => shotWeight;
        public float IceScoopWeight => iceScoopWeight;
        public float TeabagWeight => teabagWeight;
        public float BlendSeconds => blendSeconds;
        public int TrayReturnSlots => trayReturnSlots;
        public float NapkinChance => napkinChance;
        public float ScrubDistance => scrubDistance;
        public float DryingSeconds => dryingSeconds;
        public float CleaningTimeLimit => cleaningTimeLimit;
        public float CleaningInterval => cleaningInterval;
        public float CleaningIntervalPerDay => cleaningIntervalPerDay;
        public int PointsPerOrder => pointsPerOrder;
        public float PointsPerSatisfaction => pointsPerSatisfaction;
        public int PenaltyLost => penaltyLost;
        public int PenaltyDiscarded => penaltyDiscarded;
        public int PenaltyTrayLeft => penaltyTrayLeft;
        public int PenaltyCleaning => penaltyCleaning;
        public int Ending03Profit => ending03Profit;
        public float Ending03Reputation => ending03Reputation;
        public int Ending04Profit => ending04Profit;
        public float Ending04Reputation => ending04Reputation;

        // 재료 클릭 1회 투입량 (SFR-046, 053). 티백은 1개
        public float AmountPerClick(IngredientDefinition ingredient) =>
            ingredient.Category == IngredientCategory.Smoothie ? smoothiePerClick :
            ingredient.Category == IngredientCategory.Tea ? 1f : syrupPerClick;

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

        // 메뉴별 레시피 (SFR-051, 정량 임시). HOT = 도자기 잔, ICE = 플라스틱 컵 + 얼음 1스쿱
        static List<RecipeDefinition> DefaultRecipes()
        {
            const Temperature hot = Temperature.Hot;
            const Temperature ice = Temperature.Ice;
            static RecipeAmount A(string key, float amount) => new RecipeAmount(key, amount);
            var shot = A(RecipeKeys.Shot, 1);
            var cube = A(RecipeKeys.Ice, 1);
            var hotWater = A(RecipeKeys.HotWater, 200);
            var coldWater = A(RecipeKeys.ColdWater, 200);
            var milk = A("milk", 200);
            var soda = A("soda", 200);
            var list = new List<RecipeDefinition>
            {
                new("americano", hot, shot, hotWater),
                new("americano", ice, shot, coldWater, cube),
            };
            // 라떼류: 샷 + (시럽) + 우유
            foreach (var (id, syrup, grams) in new[]
                     {
                         ("cafe_latte", "", 0f), ("vanilla_latte", "syrup_vanilla", 20f), ("caramel_macchiato", "syrup_caramel", 20f),
                         ("hazelnut_latte", "syrup_hazelnut", 20f), ("cafe_mocha", "syrup_chocolate", 30f), ("condensed_latte", "condensed_milk", 30f),
                     })
            {
                var parts = new List<RecipeAmount> { shot };
                if (syrup != "") parts.Add(A(syrup, grams));
                parts.Add(milk);
                list.Add(new RecipeDefinition(id, hot, parts.ToArray()));
                parts.Add(cube);
                list.Add(new RecipeDefinition(id, ice, parts.ToArray()));
            }
            list.Add(new("strawberry_latte", ice, A("cheong_strawberry", 40), milk, cube));
            list.Add(new("grapefruit_ade", ice, A("cheong_grapefruit", 40), soda, cube));
            list.Add(new("yuja_ade", ice, A("cheong_yuja", 40), soda, cube));
            list.Add(new("strawberry_ade", ice, A("cheong_strawberry", 40), soda, cube));
            list.Add(new("blue_lemon_ade", ice, A("syrup_blue", 30), A("syrup_plain", 10), soda, cube));
            list.Add(new("yuja_tea", hot, A("cheong_yuja", 40), hotWater));
            list.Add(new("grapefruit_tea", hot, A("cheong_grapefruit", 40), hotWater));
            foreach (var (id, bag) in new[] { ("chamomile", "tea_chamomile"), ("earl_grey", "tea_earlgrey") })
            {
                list.Add(new RecipeDefinition(id, hot, A(bag, 1), hotWater));
                list.Add(new RecipeDefinition(id, ice, A(bag, 1), coldWater, cube));
            }
            // 스무디·쉐이크 (믹서기): 원액 + 우유 + 얼음 1스쿱
            foreach (var (id, fruit) in new[]
                     {
                         ("strawberry_smoothie", "smoothie_strawberry"), ("mango_smoothie", "smoothie_mango"),
                         ("hallabong_smoothie", "smoothie_hallabong"), ("melon_smoothie", "smoothie_melon"),
                     })
                list.Add(new RecipeDefinition(id, ice, A(fruit, 150), A("milk", 100), cube));
            list.Add(new("cookie_shake", ice, A("smoothie_cookie", 150), A("milk", 150), cube));
            return list;
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
