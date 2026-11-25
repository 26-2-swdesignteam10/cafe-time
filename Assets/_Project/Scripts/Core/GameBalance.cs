using System.Collections.Generic;
using System.Linq;
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
        [Tooltip("새 게임 DAY 1 시작 재고, 기본 단위 (docs/GAME_DATA.md 3장)")]
        [SerializeField] List<IngredientStock> startingStock = DefaultStartingStock();

        [Header("메뉴 (SFR-026, 판매가 임시)")]
        [SerializeField] List<MenuDefinition> menus = DefaultMenus();

        [Header("영업 시간 (SFR-014)")]
        [Tooltip("게임 시간 1시간이 실제 몇 초인지 (명세 2분 → ADR-0021 35초, 하루 약 7분)")]
        [SerializeField] float secondsPerGameHour = 35f;
        [SerializeField] int openHour = 9;
        [Tooltip("손님 입장 마감 시각 (17.5 = 17:30)")]
        [SerializeField] float entryCloseHour = 17.5f; // ADR-0044
        [SerializeField] int closeHour = 18;           // ADR-0044

        [Header("손님 (SFR-024, 025, 057, 058) — 임시값, 시간은 실제 초")]
        [Tooltip("DAY 1 ~ 15 손님 난이도: 입장 간격, 일행 인원 가중치, 인내심 (ADR-0020)")]
        [SerializeField] List<DayDifficulty> difficultyByDay = DefaultDifficulty();
        [Tooltip("일행이 디저트를 같이 주문할 확률")]
        [Range(0f, 1f)] [SerializeField] float dessertChance = 0.35f;
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

        [Header("단골손님 (SFR-009 DAY 9, ADR-0049) — 임시값")]
        [Tooltip("이름 있는 단골. firstDay 부터 매일 한 번 혼자 방문해 늘 마시는 메뉴를 주문한다")]
        [SerializeField] List<RegularCustomer> regulars = DefaultRegulars();
        [Tooltip("이 횟수만큼 주문한 뒤부터는 메뉴를 말하지 않고 '늘 마시던 걸로' 주문한다")]
        [SerializeField] int regularVisitsToRemember = 2;
        [Tooltip("단골이 이 만족도 이상이면 반드시 주는 팁")]
        [SerializeField] int regularTip = 2000;
        [Range(0f, 100f)] [SerializeField] float regularTipMinSatisfaction = 70f;
        [Tooltip("단골 방문 게임 시각 범위 (시). 매일 이 사이에서 무작위")]
        [SerializeField] Vector2 regularVisitHours = new Vector2(9.5f, 16.5f);

        [Header("단체 주문 이벤트 (SFR-009, ADR-0042) — 임시값")]
        [Tooltip("DAY별 영업 중 단체 주문. 해당 DAY가 없으면 이벤트 없음")]
        [SerializeField] List<GroupOrderEvent> groupOrders = DefaultGroupOrders();

        [Header("인플루언서 방문 이벤트 (SFR-009, ADR-0048) — 임시값")]
        [Tooltip("DAY별 영업 중 인플루언서 방문. 해당 DAY가 없으면 이벤트 없음")]
        [SerializeField] List<InfluencerEvent> influencerEvents = DefaultInfluencerEvents();

        [Header("점심시간 러시 이벤트 (SFR-009, ADR-0050) — 임시값")]
        [Tooltip("DAY별 점심시간 러시. 해당 DAY가 없으면 이벤트 없음")]
        [SerializeField] List<LunchRushEvent> lunchRushes = DefaultLunchRushes();

        [Header("레시피 (SFR-051) — 정량 임시값")]
        [SerializeField] List<RecipeDefinition> recipes = DefaultRecipes();
        [Tooltip("재료별 오차 비율이 이 값 이하면 '완벽'")]
        [SerializeField] float perfectTolerance = 0.1f;
        [Tooltip("이 값 이하면 '좋음', 넘으면 '나쁨'")]
        [SerializeField] float goodTolerance = 0.25f;

        [Header("커피잔·접시 (SFR-028, 064) — 임시값. 매일 영업 시작 때 모두 선반에 있다")]
        [SerializeField] int ceramicCups = 16; // 2026-11-18 사용자 요청 8 → 16
        [SerializeField] int plasticCups = 20; // 2026-11-18 사용자 요청 10 → 20 (10개씩 두 더미)
        [SerializeField] int plates = 6;

        [Header("에스프레소 머신 (SFR-031, 032, 035)")]
        [SerializeField] float grindSeconds = 7f;
        [Tooltip("탬퍼를 가루에 대고 누르고 있어야 하는 시간 (SFR-032)")]
        [SerializeField] float tampSeconds = 1f;
        [SerializeField] float extractSeconds = 7f;

        [Header("제빙기 (SFR-038, 039) — 임시값")]
        [SerializeField] int iceMax = 24;
        [Tooltip("얼음 1개를 만드는 시간 (실제 초)")]
        [SerializeField] float iceSecondsPerCube = 3f;
        [Tooltip("스쿱 1회에 담는 얼음 개수")]
        [SerializeField] int icePerScoop = 4;

        [Header("정수기 (SFR-043) — 임시값")]
        [Tooltip("물 받는 시간 (2026-11-17 사용자 요청 3초 → 6초)")]
        [SerializeField] float waterSeconds = 6f;
        [SerializeField] float waterAmount = 200f;

        [Header("음료·쉐이크 제조실 (SFR-046, 047, 053, 054) — 임시값")]
        [Tooltip("시럽·청·연유 클릭 1회 투입량 (g)")]
        [SerializeField] float syrupPerClick = 10f;
        [Tooltip("스무디 원액 클릭 1회 투입량 (g)")]
        [SerializeField] float smoothiePerClick = 50f;
        [Tooltip("우유·탄산수를 누르고 있을 때 1초에 부어지는 양 (g)")]
        [SerializeField] float pourPerSecond = 50f;

        [Header("우유 스티밍 (ADR-0046) — 임시값")]
        [SerializeField] float pitcherCapacity = 400f;       // 피처에 담을 수 있는 우유 (g)
        [SerializeField] float coldMilkTemperature = 5f;     // 냉장 우유 온도 (℃)
        [SerializeField] float steamDegreesPerSecond = 15f;  // 스팀 중 초당 오르는 온도
        [SerializeField] float maxMilkTemperature = 90f;
        [SerializeField] float steamMinTemperature = 60f;    // 적정 구간. 벗어난 우유를 넣은 잔은 등급 한 단계 하락
        [SerializeField] float steamMaxTemperature = 70f;
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
        [Tooltip("선입선출을 어기고 나중 주문을 먼저 서빙했을 때 1회 감점 (ADR-0022)")]
        [SerializeField] int penaltyOrderSkipped = 30;

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

        void OnEnable() => EnsureCatalog();

        // 기존 밸런스 에셋을 코드의 메뉴 구성에 맞춘다 (ADR-0038). 기존 가격·레시피 정량·재료 아이콘·재고 설정은 보존하고,
        // 없어진 메뉴·온도의 레시피는 지우고, 새 메뉴·레시피·재료·시작 재고만 추가한다. 메뉴 순서와 HOT/ICE 가능 여부는 코드 기준 (SFR-010, 026, 051)
        public void EnsureCatalog()
        {
            var merged = new List<MenuDefinition>();
            foreach (var menu in DefaultMenus())
            {
                var old = menus.Find(m => m.Id == menu.Id);
                merged.Add(old != null ? menu.WithPrice(old.Price) : menu);
            }
            menus = merged;

            recipes.RemoveAll(r => FindMenu(r.MenuId) is not { } m || !m.Allows(r.Temperature));
            foreach (var recipe in DefaultRecipes())
            {
                int index = recipes.FindIndex(r => r.MenuId == recipe.MenuId && r.Temperature == recipe.Temperature);
                if (index < 0) recipes.Add(recipe);
                // 재료 구성이 코드와 다르면(재료가 바뀌었거나 에셋이 깨진 경우) 코드 기본값으로. 같은 구성이면 정량은 보존. 샷 누락 복구(ADR-0043)도 여기에 포함된다
                else if (!SameKeys(recipes[index], recipe)) recipes[index] = recipe;
            }

            var orderedIngredients = new List<IngredientDefinition>();
            foreach (var ingredient in DefaultIngredients())
                orderedIngredients.Add(ingredients.Find(i => i.Id == ingredient.Id) ?? ingredient);
            ingredients = orderedIngredients; // 코드에서 뺀 재료(예: 2026-11-18 일반 시럽)는 지운다

            var defaultStock = DefaultStartingStock();
            startingStock.RemoveAll(s => !defaultStock.Exists(d => d.id == s.id));
            foreach (var stock in defaultStock)
                if (!startingStock.Exists(i => i.id == stock.id)) startingStock.Add(stock);

            // 새로 생긴 단체 주문(예: DAY 15 예약, ADR-0051)만 추가하고, 이미 있는 줄의 설정은 보존한다
            groupOrders ??= new List<GroupOrderEvent>();
            foreach (var ev in DefaultGroupOrders())
                if (!groupOrders.Exists(g => g.choiceId == ev.choiceId)) groupOrders.Add(ev);
        }

        static bool SameKeys(RecipeDefinition a, RecipeDefinition b) =>
            a.Amounts.Count == b.Amounts.Count && a.Amounts.All(x => b.Amounts.Any(y => y.Key == x.Key));

        public int StartingMoney => startingMoney;
        public IReadOnlyList<IngredientStock> StartingStock => startingStock;
        public float InitialReputation => initialReputation;
        public bool TestFullStock => testFullStock;
        public IReadOnlyList<IngredientDefinition> Ingredients => ingredients;
        public IReadOnlyList<MenuDefinition> Menus => menus;

        public float SecondsPerGameHour => secondsPerGameHour;
        public int OpenHour => openHour;
        public float EntryCloseHour => entryCloseHour;
        public int CloseHour => closeHour;
        public DayDifficulty Difficulty(int day) => difficultyByDay[Mathf.Clamp(day - 1, 0, difficultyByDay.Count - 1)];
        public float DessertChance => dessertChance;
        public float StayDuration => stayDuration;
        public float MaxTipChance => maxTipChance;
        public int TipPerPerson => tipPerPerson;
        public float NeutralSatisfaction => neutralSatisfaction;
        public float MaxReputationChange => maxReputationChange;
        // 그 DAY의 단체 주문 이벤트. 없으면 null (SFR-009)
        public GroupOrderEvent GroupOrderFor(int day) => groupOrders?.Find(e => e.day == day);
        // 그 DAY 영업 중에 받는 단체 주문 예약 (다음 날 이후 주문). 없으면 null (SFR-009 DAY 14, ADR-0051)
        public GroupOrderEvent GroupReservationFor(int day) => groupOrders?.Find(e => e.reserveDay > 0 && e.reserveDay == day);
        public IReadOnlyList<RegularCustomer> Regulars => regulars;
        public int RegularVisitsToRemember => regularVisitsToRemember;
        public int RegularTip => regularTip;
        public float RegularTipMinSatisfaction => regularTipMinSatisfaction;
        public Vector2 RegularVisitHours => regularVisitHours;
        public InfluencerEvent InfluencerFor(int day) => influencerEvents?.Find(e => e.day == day);
        public LunchRushEvent LunchRushFor(int day) => lunchRushes?.Find(e => e.day == day);

        public IReadOnlyList<RecipeDefinition> Recipes => recipes;
        public float PerfectTolerance => perfectTolerance;
        public float GoodTolerance => goodTolerance;
        public int CeramicCups => ceramicCups;
        public int PlasticCups => plasticCups;
        public int Plates => plates;
        public float GrindSeconds => grindSeconds;
        public float TampSeconds => tampSeconds;
        public float ExtractSeconds => extractSeconds;
        public int IceMax => iceMax;
        public float IceSecondsPerCube => iceSecondsPerCube;
        public int IcePerScoop => icePerScoop;
        public float WaterSeconds => waterSeconds;
        public float WaterAmount => waterAmount;
        public float SyrupPerClick => syrupPerClick;
        public float SmoothiePerClick => smoothiePerClick;
        public float PourPerSecond => pourPerSecond;
        public float PitcherCapacity => pitcherCapacity;
        public float ColdMilkTemperature => coldMilkTemperature;
        public float SteamDegreesPerSecond => steamDegreesPerSecond;
        public float MaxMilkTemperature => maxMilkTemperature;
        public float SteamMinTemperature => steamMinTemperature;
        public float SteamMaxTemperature => steamMaxTemperature;
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
        public int PenaltyOrderSkipped => penaltyOrderSkipped;
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

        // 첫날 재료를 사지 않아도 모든 메뉴를 여러 번 만들어 볼 수 있는 양 (2026-11-12 사용자 요청, 약 ₩120,000어치)
        static List<IngredientStock> DefaultStartingStock()
        {
            var list = new List<IngredientStock>();
            void Add(int amount, params string[] ids)
            {
                foreach (var id in ids) list.Add(new IngredientStock { id = id, amount = amount });
            }
            Add(40, "bean");                 // 2봉
            Add(5000, "milk");               // 5팩
            Add(2000, "soda");               // 2병
            Add(300, "condensed_milk");
            Add(300, "syrup_vanilla", "syrup_caramel", "syrup_hazelnut", "syrup_chocolate", "syrup_blue");
            Add(300, "cheong_strawberry", "cheong_grapefruit", "cheong_yuja", "cheong_lemon");
            Add(10, "tea_chamomile", "tea_earlgrey"); // 각 1박스
            Add(600, "cold_brew");           // 콜드브루 6잔
            Add(200, "matcha");              // 말차라떼 10잔
            Add(100, "cinnamon");            // 카푸치노 10잔
            Add(450, "vanilla_ice_cream");   // 밀크쉐이크 3잔
            Add(450, "smoothie_strawberry", "smoothie_mango", "smoothie_hallabong", "smoothie_melon", "smoothie_cookie"); // 각 3잔
            Add(5, "dessert_strawberry_cake", "dessert_croissant", "dessert_cookie",
                "dessert_sandwich", "dessert_bagel", "dessert_waffle", "dessert_honey_bread", "dessert_croque_monsieur");
            return list;
        }

        // 초반은 1~2명 일행이 드문드문, 후반은 큰 일행이 자주 오고 덜 기다린다 (2026-11-12 사용자 요청, docs/GAME_DATA.md 7장)
        static List<DayDifficulty> DefaultDifficulty() => new List<DayDifficulty>
        {
            new DayDifficulty(45, 75, new[] { 7, 3, 0, 0 }, 30, 360), // DAY 1
            new DayDifficulty(42, 72, new[] { 6, 4, 0, 0 }, 30, 350),
            new DayDifficulty(40, 68, new[] { 5, 4, 1, 0 }, 28, 340),
            new DayDifficulty(38, 64, new[] { 5, 4, 1, 0 }, 28, 330),
            new DayDifficulty(36, 60, new[] { 4, 4, 2, 0 }, 26, 320), // DAY 5
            new DayDifficulty(34, 57, new[] { 4, 4, 2, 0 }, 26, 300),
            new DayDifficulty(32, 54, new[] { 4, 3, 2, 1 }, 25, 290),
            new DayDifficulty(30, 51, new[] { 4, 3, 2, 1 }, 24, 280),
            new DayDifficulty(28, 48, new[] { 3, 3, 2, 1 }, 23, 270),
            new DayDifficulty(27, 45, new[] { 3, 3, 2, 1 }, 22, 260), // DAY 10
            new DayDifficulty(25, 42, new[] { 3, 3, 2, 2 }, 21, 250),
            new DayDifficulty(24, 40, new[] { 3, 3, 2, 2 }, 20, 240),
            new DayDifficulty(22, 37, new[] { 2, 3, 3, 2 }, 19, 230),
            new DayDifficulty(21, 35, new[] { 2, 3, 3, 2 }, 18, 220),
            new DayDifficulty(20, 32, new[] { 2, 3, 3, 2 }, 18, 210), // DAY 15
        };

        // DAY 4 단체 주문 (SFR-009, docs/GAME_DATA.md 9장): 11:00 요청, 16:00 마감, ICE 6잔 ₩26,000 + 정확히 맞추면 30% 보너스
        static List<GroupOrderEvent> DefaultGroupOrders() => new List<GroupOrderEvent>
        {
            new GroupOrderEvent(4, 11f, 16f, 0.3f, "day4_group_order", new List<GroupOrderEntry>
            {
                new GroupOrderEntry("americano", Temperature.Ice, 3),
                new GroupOrderEntry("cafe_latte", Temperature.Ice, 2),
                new GroupOrderEntry("vanilla_latte", Temperature.Ice, 1),
            }),
            // DAY 14 14:00 에 예약 요청 → 수락하면 DAY 15 10:00 주문, 15:00 마감, ICE 10잔 ₩44,500 + 정확히 맞추면 40% 보너스 (ADR-0051)
            new GroupOrderEvent(15, 10f, 15f, 0.4f, "day14_group_reserve", new List<GroupOrderEntry>
            {
                new GroupOrderEntry("americano", Temperature.Ice, 4),
                new GroupOrderEntry("cafe_latte", Temperature.Ice, 3),
                new GroupOrderEntry("vanilla_latte", Temperature.Ice, 3),
            })
            {
                reserveDay = 14, reserveHour = 14f,
            },
        };

        // 단골손님 (SFR-009, docs/GAME_DATA.md 9장): DAY 7 부터 매일 방문 → 3번째 방문(DAY 9)부터 '늘 마시던 걸로'
        // 외형 번호: 0 회사원, 1 학생(인플루언서와 같은 그림이라 쓰지 않음), 2 할머니, 3 안경 쓴 청년
        static List<RegularCustomer> DefaultRegulars() => new List<RegularCustomer>
        {
            new RegularCustomer("kim", "김 과장", 0, "americano", Temperature.Hot, 7),
            new RegularCustomer("sunja", "순자 할머니", 2, "chamomile", Temperature.Hot, 7),
            new RegularCustomer("junho", "준호", 3, "vanilla_latte", Temperature.Ice, 7), // DAY 9 스토리의 아이스 바닐라라떼 단골
        };

        // DAY 6 인플루언서 방문 (SFR-009, docs/GAME_DATA.md 9장): 13:00 방문, 15:00 마감, 대표 메뉴 ICE 바닐라라떼 1잔
        // DAY 11 점심시간 러시 (SFR-009, docs/GAME_DATA.md 9장): 11:30~13:30 손님이 몰려와 대부분 ICE 아메리카노를 주문
        static List<LunchRushEvent> DefaultLunchRushes() => new List<LunchRushEvent>
        {
            new LunchRushEvent(11, 11.5f, 13.5f, 6f, 10f, new[] { 5, 4, 1, 0 }, "americano", Temperature.Ice, 1f, 0.5f),
        };

        static List<InfluencerEvent> DefaultInfluencerEvents() => new List<InfluencerEvent>
        {
            new InfluencerEvent(6, 13f, 15f, "vanilla_latte", Temperature.Ice, "day6_influencer")
            {
                greatReputation = 0.3f, greatVisitorRate = 1.15f,
                goodReputation = 0.1f, goodVisitorRate = 1.05f,
                badReputation = -0.3f, badVisitorRate = 0.9f,
            },
        };

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
                // 커피 (2026-11-17 사용자 요청 순서, ADR-0038)
                new("americano", "아메리카노", coffee, 4000, true, true),
                new("cafe_latte", "카페라떼", coffee, 4500, true, true),
                new("vanilla_latte", "바닐라라떼", coffee, 5000, true, true),
                new("hazelnut_latte", "헤이즐넛라떼", coffee, 5000, true, true),
                new("caramel_macchiato", "카라멜마키아또", coffee, 5500, true, true),
                new("cafe_mocha", "카페모카", coffee, 5500, true, true),
                new("cappuccino", "카푸치노", coffee, 4500, true, true),
                new("cold_brew", "콜드브루", coffee, 4500, true, true),
                new("cold_brew_latte", "콜드브루라떼", coffee, 5000, true, true),
                // 음료: 온도 고정 (차 HOT, 라떼·에이드 ICE)
                new("strawberry_latte", "딸기라떼", beverage, 5500, false, true),
                new("chocolate_latte", "초콜릿라떼", beverage, 5000, false, true),
                new("matcha_latte", "말차라떼", beverage, 5500, false, true),
                new("grapefruit_ade", "자몽에이드", beverage, 5500, false, true),
                new("blue_lemon_ade", "블루레몬에이드", beverage, 5500, false, true),
                new("yuja_tea", "유자차", beverage, 5000, true, false),
                new("grapefruit_tea", "자몽차", beverage, 5000, true, false),
                new("chamomile", "캐모마일", beverage, 4500, true, false),
                new("earl_grey", "얼그레이", beverage, 4500, true, false),
                new("strawberry_smoothie", "딸기 스무디", smoothie, 6000, false, true),
                new("mango_smoothie", "망고 스무디", smoothie, 6000, false, true),
                new("hallabong_smoothie", "한라봉 스무디", smoothie, 6000, false, true),
                new("melon_smoothie", "멜론 스무디", smoothie, 6000, false, true),
                new("cookie_shake", "쿠키앤크림 쉐이크", smoothie, 6000, false, true),
                new("milk_shake", "밀크쉐이크", smoothie, 6000, false, true),
                new("strawberry_cake", "딸기 케이크", dessert, 6500, false, false, "dessert_strawberry_cake"),
                new("croissant", "크루아상", dessert, 4000, false, false, "dessert_croissant"),
                new("cookie", "쿠키", dessert, 3000, false, false, "dessert_cookie"),
                new("sandwich", "샌드위치", dessert, 6000, false, false, "dessert_sandwich"),
                new("bagel", "베이글", dessert, 4000, false, false, "dessert_bagel"),
                new("waffle", "와플", dessert, 5000, false, false, "dessert_waffle"),
                new("honey_bread", "허니브레드", dessert, 6500, false, false, "dessert_honey_bread"),
                new("croque_monsieur", "크로크무슈", dessert, 6500, false, false, "dessert_croque_monsieur"),
            };
        }

        // 메뉴별 레시피 (SFR-051, 정량 임시). HOT = HOT 컵, ICE = ICE 컵 + 얼음 1스쿱
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
            var steamed = A(RecipeKeys.SteamedMilk, 200); // HOT 우유 메뉴는 스팀 우유 (ADR-0046)
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
                         ("hazelnut_latte", "syrup_hazelnut", 20f), ("cafe_mocha", "syrup_chocolate", 30f),
                     })
            {
                var parts = new List<RecipeAmount> { shot };
                if (syrup != "") parts.Add(A(syrup, grams));
                list.Add(new RecipeDefinition(id, hot, parts.Append(steamed).ToArray()));
                list.Add(new RecipeDefinition(id, ice, parts.Append(milk).Append(cube).ToArray()));
            }
            // 카푸치노: 우유를 덜 넣고 시나몬 (2026-11-17 사용자 선택)
            list.Add(new("cappuccino", hot, shot, A(RecipeKeys.SteamedMilk, 150), A("cinnamon", 10)));
            list.Add(new("cappuccino", ice, shot, A("milk", 150), A("cinnamon", 10), cube));
            // 콜드브루: HOT 은 뜨거운 물, ICE 는 차가운 물 + 얼음 (2026-11-18 사용자 요청으로 HOT 추가)
            list.Add(new("cold_brew", hot, A("cold_brew", 100), A(RecipeKeys.HotWater, 150)));
            list.Add(new("cold_brew", ice, A("cold_brew", 100), A(RecipeKeys.ColdWater, 150), cube));
            list.Add(new("cold_brew_latte", hot, A("cold_brew", 100), A(RecipeKeys.SteamedMilk, 150)));
            list.Add(new("cold_brew_latte", ice, A("cold_brew", 100), A("milk", 150), cube));
            list.Add(new("strawberry_latte", ice, A("cheong_strawberry", 40), milk, cube));
            list.Add(new("chocolate_latte", ice, A("syrup_chocolate", 30), milk, cube));
            list.Add(new("matcha_latte", ice, A("matcha", 20), milk, cube));
            list.Add(new("grapefruit_ade", ice, A("cheong_grapefruit", 40), soda, cube));
            list.Add(new("blue_lemon_ade", ice, A("syrup_blue", 20), A("cheong_lemon", 30), soda, cube)); // 2026-11-18 사용자 요청으로 레몬청
            list.Add(new("yuja_tea", hot, A("cheong_yuja", 40), hotWater));
            list.Add(new("grapefruit_tea", hot, A("cheong_grapefruit", 40), hotWater));
            foreach (var (id, bag) in new[] { ("chamomile", "tea_chamomile"), ("earl_grey", "tea_earlgrey") })
            {
                list.Add(new RecipeDefinition(id, hot, A(bag, 1), hotWater));
            }
            // 스무디·쉐이크 (믹서기): 원액 + 우유 + 얼음 1스쿱
            foreach (var (id, fruit) in new[]
                     {
                         ("strawberry_smoothie", "smoothie_strawberry"), ("mango_smoothie", "smoothie_mango"),
                         ("hallabong_smoothie", "smoothie_hallabong"), ("melon_smoothie", "smoothie_melon"),
                     })
                list.Add(new RecipeDefinition(id, ice, A(fruit, 150), A("milk", 100), cube));
            list.Add(new("cookie_shake", ice, A("smoothie_cookie", 150), A("milk", 150), cube));
            list.Add(new("milk_shake", ice, A("vanilla_ice_cream", 150), A("milk", 150), cube));
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
                new("cold_brew", "콜드브루 원액", coffee, "병", 1000, "g", 12000),
                new("matcha", "말차 파우더", coffee, "봉", 500, "g", 15000),
                new("cinnamon", "시나몬 파우더", coffee, "통", 200, "g", 6000),
                new("milk", "우유", liquid, "팩", 1000, "g", 2500),
                new("soda", "탄산수", liquid, "병", 1000, "g", 1500),
                new("condensed_milk", "연유", liquid, "통", 500, "g", 4000),
                new("syrup_vanilla", "바닐라 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_caramel", "카라멜 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_hazelnut", "헤이즐넛 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_chocolate", "초코 시럽", syrup, "병", 1000, "g", 8000),
                new("syrup_blue", "블루 시럽", syrup, "병", 1000, "g", 8000),
                new("cheong_strawberry", "딸기청", syrup, "병", 1000, "g", 10000),
                new("cheong_grapefruit", "자몽청", syrup, "병", 1000, "g", 10000),
                new("cheong_yuja", "유자청", syrup, "병", 1000, "g", 10000),
                new("cheong_lemon", "레몬청", syrup, "병", 1000, "g", 10000), // 블루레몬에이드 (2026-11-18 사용자 요청, 일반 시럽 대신)
                new("tea_chamomile", "캐모마일 티백", tea, "박스", 10, "개", 5000),
                new("tea_earlgrey", "얼그레이 티백", tea, "박스", 10, "개", 5000),
                new("smoothie_strawberry", "딸기 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_mango", "망고 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_hallabong", "한라봉 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_melon", "멜론 스무디", smoothie, "팩", 1000, "g", 9000),
                new("smoothie_cookie", "쿠키앤크림", smoothie, "팩", 1000, "g", 9000),
                new("vanilla_ice_cream", "바닐라 아이스크림", smoothie, "통", 1000, "g", 10000),
                new("dessert_strawberry_cake", "딸기 케이크", dessert, "개", 1, "개", 2500),
                new("dessert_croissant", "크루아상", dessert, "개", 1, "개", 1500),
                new("dessert_cookie", "쿠키", dessert, "개", 1, "개", 800),
                new("dessert_sandwich", "샌드위치", dessert, "개", 1, "개", 3000),
                new("dessert_bagel", "베이글", dessert, "개", 1, "개", 1800),
                new("dessert_waffle", "와플", dessert, "개", 1, "개", 2200),
                new("dessert_honey_bread", "허니브레드", dessert, "개", 1, "개", 2800),
                new("dessert_croque_monsieur", "크로크무슈", dessert, "개", 1, "개", 3200),
            };
        }
    }
}
