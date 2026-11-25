using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 일행의 입장·줄·주문·서빙·퇴장을 관리 (SFR-024, 025, 027, 056, 057, 058)
    // 동선: 문 → 입구 세로 통로(aisleX) → 포스기 앞 가로 줄(queueLaneY) → 진열대 오른쪽 통로(hallRouteX)
    //       → 화분 사이(hallLaneY) → 좌석 세로 통로(hallAisleX) → 테이블 줄의 가로 통로(CafeTable.LaneY) (ADR-0016)
    public class CustomerManager : MonoBehaviour
    {
        [Header("외형 (Art/Sprites/Customers)")]
        [SerializeField] CustomerLook[] looks;
        [SerializeField] Sprite gaugeFrame;
        [SerializeField] Sprite gaugeFill;

        [Header("동선 (맵 좌표, 1칸 = 1)")]
        [SerializeField] Vector2 doorOutside = new Vector2(2f, 0.3f);
        [SerializeField] float bottomLaneY = 1.25f;  // 문 바로 안쪽 가로 통로
        [SerializeField] float aisleX = 2.5f;        // 입구 세로 통로
        [SerializeField] float queueLaneY = 7.25f;   // 포스기 앞 줄
        [SerializeField] float queueStartX = 4.625f; // 줄 맨 앞 (포스기 바로 앞). 줄은 왼쪽으로 이어짐
        [SerializeField] float queueSpacing = 1.8f;  // 일행 사이 간격
        public static Vector2 DefaultBakeryPickupPoint => new Vector2(6f, 4f); // 아래쪽 빵 진열대 사이 통로 (SFR-056)
        [SerializeField] Vector2 bakeryPickupPoint = DefaultBakeryPickupPoint;
        [SerializeField] int maxQueue = 5;           // 줄이 입구까지 차면 더 들어오지 않음
        [SerializeField] float hallRouteX = 9.5f;    // 진열대 오른쪽 세로 통로
        [SerializeField] float hallLaneY = 3.7f;     // 화분 사이로 좌석 구역에 들어가는 가로 통로
        [SerializeField] float hallAisleX = 11.1f;   // 좌석 구역 세로 통로
        [SerializeField] float headHeight = 1.7f;    // 머리 위 게이지 높이
        [SerializeField] Vector2 pickupPoint = new Vector2(7.625f, 7.25f); // 진동벨 손님이 서는 픽업대 앞 (ADR-0040)
        [SerializeField] CafeTable[] tables;

        [Header("점심시간 러시 (SFR-009 DAY 11, ADR-0050)")]
        [Tooltip("러시 중 만석·줄이 가득 차 못 들어왔을 때 다시 입장을 시도하는 간격 (실제 초). 자리가 나는 대로 바로 채워진다")]
        [SerializeField] float rushRetrySeconds = 1f;
        [SerializeField] string rushSpeaker = "직장인 손님";

        public static CustomerManager Instance { get; private set; }

        readonly List<CustomerParty> parties = new List<CustomerParty>();
        readonly List<CustomerParty> queue = new List<CustomerParty>();
        GameBalance balance;
        float nextArrival;
        readonly List<(RegularCustomer regular, float minutes)> dueRegulars = new List<(RegularCustomer, float)>(); // 오늘 올 단골과 방문 시각 (ADR-0049)
        float visitorRate = 1f; // 인플루언서 방문 결과 (ADR-0048). 오늘 바뀐 값은 다음 DAY 부터 적용되도록 시작 때 한 번 읽는다
        bool closed;
        LunchRushEvent rush;   // 오늘의 점심시간 러시. 없으면 null (ADR-0050)
        MenuDefinition rushMenu;
        bool rushStarted;

        static DayResult Result => DayResult.Today;
        static SaveData Save => SaveSystem.Current;

        public bool HasParties => parties.Count > 0;

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Start()
        {
            nextArrival = 3f; // 영업 시작 직후 첫 손님
            visitorRate = Mathf.Max(0.1f, Save.visitorRate);
            ScheduleRegulars();
            SetUpLunchRush();
        }

        // 오늘 점심시간 러시가 있으면 메뉴를 확인해 둔다 (SFR-009 DAY 11, ADR-0050)
        void SetUpLunchRush()
        {
            rush = balance.LunchRushFor(Save.day);
            if (rush == null) return;
            rushMenu = balance.FindMenu(rush.menuId);
            if (rushMenu == null || rushMenu.IsDessert)
            {
                Debug.LogWarning($"점심 러시 메뉴 '{rush.menuId}' 를 쓸 수 없어 러시 손님은 평소처럼 주문합니다.");
                rushMenu = null;
            }
        }

        // 러시 중인지. 시작 안내 뒤부터 끝 시각 전까지
        bool InRush => rushStarted && GameClock.Instance.Minutes < rush.endHour * 60f;

        // 러시 시작 시각이 되면 손님이 대사로 알리고, 첫 러시 손님이 곧바로 들어온다 (ADR-0050)
        void UpdateLunchRush()
        {
            if (rush == null || rushStarted || EventDialog.IsShowing) return; // 다른 이벤트 대화 중이면 끝난 뒤에 시작
            var clock = GameClock.Instance;
            if (clock.Minutes < rush.startHour * 60f || clock.Minutes >= rush.endHour * 60f) return;
            rushStarted = true;
            nextArrival = Mathf.Min(nextArrival, rushRetrySeconds);
            // 씬을 다시 빌드하지 않도록 단체 주문·인플루언서가 쓰는 대화창을 찾아 쓴다. 없으면 안내 없이 러시만 시작
            var dialog = FindAnyObjectByType<EventDialog>(FindObjectsInactive.Include);
            if (dialog == null || looks.Length == 0) return;
            string menuText = rushMenu == null ? "커피"
                : rushMenu.OnlyTemperature != Temperature.None ? rushMenu.DisplayName
                : (RushTemperature == Temperature.Hot ? "따뜻한 " : "아이스 ") + rushMenu.DisplayName;
            dialog.Show(rushSpeaker, looks[0].portrait, new List<string>
            {
                "사장님, 점심시간이에요! 근처 회사 사람들이 다 같이 나왔어요.",
                $"다들 {menuText} 한 잔씩 사 들고 들어가려고요. 뒤에 줄이 엄청 길어요, 빨리 부탁드려요!",
            }, null);
        }

        Temperature RushTemperature => rushMenu.OnlyTemperature != Temperature.None ? rushMenu.OnlyTemperature : rush.temperature;

        // 오늘 방문할 단골과 시각을 정한다 (SFR-009 DAY 9, ADR-0049)
        void ScheduleRegulars()
        {
            if (balance.Regulars == null) return;
            var hours = balance.RegularVisitHours;
            foreach (var regular in balance.Regulars)
            {
                if (regular == null || Save.day < regular.firstDay || looks.Length == 0) continue;
                var menu = balance.FindMenu(regular.menuId);
                if (menu == null || menu.IsDessert)
                {
                    Debug.LogWarning($"단골 '{regular.displayName}' 의 메뉴 '{regular.menuId}' 를 쓸 수 없어 오늘은 오지 않습니다.");
                    continue;
                }
                dueRegulars.Add((regular, Random.Range(hours.x, hours.y) * 60f));
            }
        }

        void Update()
        {
            float dt = GameClock.Delta;
            if (!closed && !GameClock.Instance.IsEntryClosed)
            {
                // 방문 시각이 된 단골. 만석·줄이 가득 차면 자리가 날 때까지 기다렸다 들어온다 (ADR-0049)
                for (int i = dueRegulars.Count - 1; i >= 0; i--)
                    if (GameClock.Instance.Minutes >= dueRegulars[i].minutes && TrySpawnRegular(dueRegulars[i].regular))
                        dueRegulars.RemoveAt(i);
                UpdateLunchRush();
                nextArrival -= dt;
                if (nextArrival <= 0)
                {
                    bool rushing = InRush;
                    // 러시 중에는 자리가 없어 못 들어온 손님이 문밖에서 기다리다 자리가 나면 바로 들어온다 (ADR-0050)
                    nextArrival = TrySpawn() || !rushing ? NextInterval() : rushRetrySeconds;
                }
            }
            foreach (var party in parties.ToArray()) Tick(party, dt);
        }

        DayDifficulty Difficulty => balance.Difficulty(Save.day);

        float NextInterval()
        {
            // DAY가 지날수록 입장 빈도 증가 (SFR-024, DAY별 난이도 표 ADR-0020)
            var interval = InRush ? rush.arrivalInterval : Difficulty.arrivalInterval; // 점심 러시 중에는 몰려온다 (ADR-0050)
            return Random.Range(interval.x, interval.y) / visitorRate; // 배율이 클수록 손님이 자주 온다 (SFR-009 인플루언서, ADR-0048)
        }

        // ───────── 입장 ─────────

        // 만석이거나 줄이 가득 차면 들어오지 않음 (감점 없음)
        bool CanSpawn(out List<CafeTable> free)
        {
            free = tables.Where(t => t.IsFree).ToList();
            return free.Count > 0 && queue.Count + parties.Count(p => p.Current == CustomerParty.State.ToShowcase) < maxQueue;
        }

        // 들어오면 true
        bool TrySpawn()
        {
            if (!CanSpawn(out var free)) return false;
            bool rushing = InRush;
            var weights = rushing && rush.partySizeWeights is { Length: > 0 } ? rush.partySizeWeights : Difficulty.partySizeWeights;
            int size = WeightedPartySize(weights);
            var wants = rushing ? RushWants(size) : CustomerParty.RandomWants(size, balance);
            var party = Spawn(free, wants, Enumerable.Range(0, size).Select(_ => looks[Random.Range(0, looks.Length)]).ToList());
            party.LunchRush = rushing;
            return true;
        }

        // 러시 손님은 한 명씩 정해진 확률로 러시 메뉴(기본 ICE 아메리카노)를 고르고, 아니면 평소처럼 무작위 음료.
        // 점심을 먹고 커피만 사러 온 손님이라 베이커리는 고르지 않는다 (SFR-009 DAY 11, ADR-0050)
        List<OrderLine> RushWants(int size)
        {
            var lines = new List<OrderLine>();
            int rushCount = rushMenu == null ? 0 : Enumerable.Range(0, size).Count(_ => Random.value < rush.menuChance);
            if (rushCount > 0) lines.Add(new OrderLine(rushMenu, RushTemperature, rushCount));
            if (rushCount < size)
            {
                foreach (var line in CustomerParty.RandomWants(size - rushCount, balance).Where(l => !l.Menu.IsDessert))
                {
                    var same = lines.Find(l => l.Matches(line.Menu, line.Temperature));
                    if (same != null) same.Quantity += line.Quantity;
                    else lines.Add(line);
                }
            }
            return lines;
        }

        // 단골은 혼자 와서 늘 마시는 메뉴 한 잔을 주문한다. 들어오면 true (ADR-0049)
        bool TrySpawnRegular(RegularCustomer regular)
        {
            if (!CanSpawn(out var free)) return false;
            var menu = balance.FindMenu(regular.menuId);
            var temperature = menu.OnlyTemperature != Temperature.None ? menu.OnlyTemperature : regular.temperature;
            var look = looks[Mathf.Clamp(regular.lookIndex, 0, looks.Length - 1)];
            var party = Spawn(free, new List<OrderLine> { new OrderLine(menu, temperature, 1) }, new List<CustomerLook> { look });
            party.Regular = regular;
            party.RegularVisits = Save.GetRegularVisits(regular.id);
            party.RegularRemembered = party.RegularVisits >= balance.RegularVisitsToRemember;
            return true;
        }

        CustomerParty Spawn(List<CafeTable> free, List<OrderLine> wants, List<CustomerLook> memberLooks)
        {
            int size = memberLooks.Count;
            var party = new GameObject("Party").AddComponent<CustomerParty>();
            party.transform.SetParent(transform, false);
            party.Table = free[Random.Range(0, free.Count)];
            party.Table.Party = party;
            party.Wanted = wants;
            party.Gauge = WorldGauge.Create(party.transform, gaugeFrame, gaugeFill);
            party.Gauge.Show(false);
            for (int i = 0; i < size; i++)
                party.Members.Add(Customer.Create(party.transform, memberLooks[i], (Vector3)doorOutside + Vector3.down * 0.4f * i));
            parties.Add(party);
            Result.visitors += size;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayCustomerEnter();

            // 베이커리를 고른 손님은 먼저 진열장에 들른다 (SFR-026, 056).
            if (party.Wanted.Any(l => l.Menu.IsDessert))
            {
                party.Current = CustomerParty.State.ToShowcase;
                for (int i = 0; i < party.Members.Count; i++)
                    party.Members[i].Walk(new List<Vector3>
                    {
                        new Vector3(doorOutside.x, bottomLaneY), new Vector3(bakeryPickupPoint.x, bottomLaneY),
                        (Vector3)bakeryPickupPoint + Vector3.down * (0.25f * i),
                    });
            }
            else JoinQueue(party, fromDoor: true);
            return party;
        }

        void JoinQueue(CustomerParty party, bool fromDoor)
        {
            party.QueueIndex = queue.Count;
            queue.Add(party);
            party.Current = CustomerParty.State.ToQueue;
            WalkToQueue(party, fromDoor);
        }

        // 집는 순간 재고를 차감하므로 여러 일행이 같은 마지막 빵을 중복으로 가져가지 않는다.
        void CollectBakery(CustomerParty party)
        {
            foreach (var line in party.Wanted.Where(l => l.Menu.IsDessert).ToList())
            {
                int quantity = Mathf.Min(line.Quantity, Mathf.Max(0, Save.GetStock(line.Menu.StockId)));
                if (quantity > 0)
                {
                    Save.AddStock(line.Menu.StockId, -quantity);
                    party.CollectedBakery.Add(new OrderLine(line.Menu, Temperature.None, quantity));
                }
                line.Quantity = quantity;
            }
            party.Wanted.RemoveAll(l => l.Quantity <= 0);
        }

        int WeightedPartySize(int[] weights)
        {
            int total = weights.Sum();
            int roll = Random.Range(0, total);
            for (int i = 0; i < weights.Length; i++)
            {
                if (roll < weights[i]) return i + 1;
                roll -= weights[i];
            }
            return 1;
        }

        // ───────── 줄 (SFR-025) ─────────

        // 줄 칸 위치. 가로 줄이 차면 입구 세로 통로를 따라 아래로 이어 선다
        Vector3 QueueSlot(int index)
        {
            float x = queueStartX - index * queueSpacing;
            if (x >= aisleX + queueSpacing * 0.5f) return new Vector3(x, queueLaneY);
            int lastOnLane = Mathf.FloorToInt((queueStartX - aisleX - queueSpacing * 0.5f) / queueSpacing);
            return new Vector3(aisleX, queueLaneY - (index - lastOnLane) * queueSpacing * 0.8f);
        }

        bool OnLane(Vector3 p) => Mathf.Abs(p.y - queueLaneY) < 0.3f;

        // 큰 진열대 둘 사이의 세로 통로. 가구를 대각선으로 가로지르지 않도록 판별 (SFR-024, 056).
        bool InBakeryAisle(Vector3 p) => Mathf.Abs(p.x - bakeryPickupPoint.x) < 0.7f && p.y < queueLaneY - 0.3f;

        void WalkToQueue(CustomerParty party, bool fromDoor)
        {
            var slot = QueueSlot(party.QueueIndex);
            for (int i = 0; i < party.Members.Count; i++)
            {
                var member = party.Members[i];
                // 일행은 대표 뒤에 조금씩 겹쳐 선다
                var offset = OnLane(slot) ? new Vector3(-0.3f * i, -0.12f * i) : new Vector3(0, -0.25f * i);
                var points = new List<Vector3>();
                if (fromDoor)
                {
                    points.Add(new Vector3(doorOutside.x, bottomLaneY));
                    points.Add(new Vector3(aisleX, bottomLaneY));
                }
                else if (InBakeryAisle(member.transform.position))
                {
                    points.Add(new Vector3(bakeryPickupPoint.x, queueLaneY));
                    if (!OnLane(slot)) points.Add(new Vector3(aisleX, queueLaneY));
                }
                else if (!OnLane(member.transform.position) && OnLane(slot))
                {
                    points.Add(new Vector3(aisleX, queueLaneY)); // 통로에서 가로 줄로 올라감
                }
                if (!fromDoor && OnLane(member.transform.position) && !OnLane(slot))
                    points.Add(new Vector3(aisleX, queueLaneY));
                if (fromDoor && OnLane(slot)) points.Add(new Vector3(aisleX, queueLaneY));
                points.Add(slot + offset);
                member.Walk(points);
            }
        }

        void LeaveQueue(CustomerParty party)
        {
            queue.Remove(party);
            party.QueueIndex = -1;
            for (int i = 0; i < queue.Count; i++)
            {
                queue[i].QueueIndex = i;
                WalkToQueue(queue[i], fromDoor: false); // 앞으로 당겨 섬
            }
        }

        // 포스기에서 주문받을 수 있는 일행 (줄 맨 앞, 도착함)
        public CustomerParty FrontParty =>
            queue.Count > 0 && queue[0].Current == CustomerParty.State.Queued && queue[0].Arrived ? queue[0] : null;

        // ───────── 주문·결제 (SFR-027) ─────────

        // 포스기 결제. 그 사이 손님이 떠났으면 null
        public Order TakeOrder(CustomerParty party, List<OrderLine> lines)
        {
            if (party == null || party.Current != CustomerParty.State.Queued) return null;
            // 음료·베이커리 모두 포스기에 직접 입력한 품목·수량대로 결제한다 (SFR-026, 027).
            // 실물 베이커리 재고는 진열장에서 집을 때 이미 차감했으므로 여기서 다시 차감하지 않는다.
            int total = lines.Sum(l => l.Price);
            Save.money += total; // 결제 금액만큼 보유 자금 증가
            Result.revenue += total;
            var order = OrderBoard.Instance.Register(lines, total, party);
            party.Order = order;
            if (party.Regular != null) Save.AddRegularVisit(party.Regular.id); // 주문한 방문만 센다 (ADR-0049)

            LeaveQueue(party);
            party.Gauge.Show(false);
            party.Current = CustomerParty.State.ToTable;
            var table = party.Table;
            for (int i = 0; i < party.Members.Count; i++)
            {
                var member = party.Members[i];
                var seat = table.Seat(i);
                var points = new List<Vector3>
                {
                    new Vector3(hallRouteX, queueLaneY),
                    new Vector3(hallRouteX, hallLaneY),
                    new Vector3(hallAisleX, hallLaneY),
                    new Vector3(hallAisleX, table.LaneY),
                    new Vector3(seat.x, table.LaneY),
                    seat,
                };
                member.WalkToSeat(points, table.SeatFacing(i));
            }
            if (order.ItemCount == 0) Serve(order, new List<HeldItem>()); // 베이커리만 결제하면 제조 대기 없음
            return order;
        }

        // ───────── 서빙 (SFR-056, 058) ─────────

        public void Serve(Order order, IReadOnlyList<HeldItem> items)
        {
            var party = order.Party;
            OrderBoard.Instance.Remove(order);
            if (party == null || party.Current == CustomerParty.State.Leaving) return;

            float satisfaction = party.Satisfaction(items, Difficulty.servePatience);
            Result.ordersCompleted++;
            Result.satisfactionSum += satisfaction;
            Result.drinksSold += items.Count(i => !(i is DessertItem));
            party.Served = items.ToList();
            party.CollectedBakery.Clear(); // 베이커리는 셀프서비스 포장으로 제공, 설거지 접시를 만들지 않음

            // 만족도가 높을수록 높은 확률로 팁, 보유 자금에 바로 더함. 단골은 늘 마시던 음료를 잘 받으면 반드시 단골 팁 (ADR-0049)
            int tip = 0;
            if (party.Regular != null)
            {
                if (satisfaction >= balance.RegularTipMinSatisfaction) tip = balance.RegularTip;
            }
            else if (Random.value < balance.MaxTipChance * satisfaction / 100f) tip = balance.TipPerPerson * party.Members.Count;
            Save.money += tip;
            Result.tips += tip;

            party.Order = null;
            party.Gauge.Show(false);
            party.StayLeft = balance.StayDuration * (party.LunchRush ? rush.stayMultiplier : 1f); // 러시 손님은 금방 회사로 돌아감 (ADR-0050)
            party.Current = CustomerParty.State.Drinking;
        }

        // 진동벨 호출 (ADR-0040): 일행 중 1명이 픽업대로 걸어와 호출 때 트레이에 있던 물건을 받아 자리로 돌아간다.
        // 걸어오는 동안 기다림 게이지는 멈추고, 만족도는 호출한 시점까지의 대기 시간으로 계산한다. 도착하면 onArrive
        public bool Call(Order order, List<HeldItem> items)
        {
            var party = order.Party;
            if (party == null || party.Current != CustomerParty.State.Waiting) return false;
            party.Current = CustomerParty.State.ToPickup;
            party.CalledItems = items;
            party.Gauge.Show(false);
            int index = party.Members.Count - 1; // 마지막 일행이 받으러 감
            party.Runner = party.Members[index];
            party.Runner.Walk(SeatToPickup(party, index));
            return true;
        }

        List<Vector3> SeatToPickup(CustomerParty party, int index)
        {
            var table = party.Table;
            var seat = table.Seat(index);
            return new List<Vector3>
            {
                new Vector3(seat.x, table.LaneY),
                new Vector3(hallAisleX, table.LaneY),
                new Vector3(hallAisleX, hallLaneY),
                new Vector3(hallRouteX, hallLaneY),
                new Vector3(hallRouteX, queueLaneY),
                pickupPoint,
            };
        }

        // ───────── 퇴장 ─────────

        // 주문 후 음료를 못 받고 떠남: 결제 금액 환불 + 손님 이탈 감점 (SFR-057, 014)
        void Refund(CustomerParty party)
        {
            if (party.Order == null) return;
            Save.money -= party.Order.Paid;
            Result.revenue -= party.Order.Paid;
            OrderBoard.Instance.Remove(party.Order);
            party.Order = null;
        }

        void Leave(CustomerParty party, bool lost)
        {
            if (lost) Result.customersLost++;
            // 결제 전 이탈 또는 미서빙 주문 환불 시 집었던 베이커리를 진열장 재고로 되돌린다.
            foreach (var line in party.CollectedBakery) Save.AddStock(line.Menu.StockId, line.Quantity);
            party.CollectedBakery.Clear();
            if (party.QueueIndex >= 0) LeaveQueue(party);
            if (party.Table.Party == party) party.Table.Party = null;
            party.Gauge.Show(false);
            party.Current = CustomerParty.State.Leaving;
            // 다 마시고 떠나면 퇴식구에 쟁반 반납. 가득 차 있으면 테이블에 두고 감 → 청소 요청 (SFR-057, 060, 062)
            if (party.Served != null)
            {
                var tray = Tray.FromServed(party.Served, balance);
                if (!TrayReturn.Instance.TryReturn(tray)) CleaningManager.Instance.TrayLeft(party.Table, tray);
                party.Served = null;
            }
            foreach (var member in party.Members)
            {
                member.Walk(ExitPath(member.transform.position, party.Table.LaneY));
            }
        }

        // 지금 위치에서 문까지 들어온 길을 되짚는 경로
        List<Vector3> ExitPath(Vector3 p, float tableLaneY)
        {
            var points = new List<Vector3>();
            if (InBakeryAisle(p)) // 진열대 방문 중 영업 종료 시 들어온 아래쪽 길로 돌아감
            {
                points.Add(new Vector3(bakeryPickupPoint.x, bottomLaneY));
            }
            else if (p.x >= hallAisleX) // 좌석 구역
            {
                points.Add(new Vector3(p.x, tableLaneY));
                points.Add(new Vector3(hallAisleX, tableLaneY));
                points.Add(new Vector3(hallAisleX, hallLaneY));
                points.Add(new Vector3(hallRouteX, hallLaneY));
                points.Add(new Vector3(hallRouteX, queueLaneY));
                points.Add(new Vector3(aisleX, queueLaneY));
            }
            else if (p.x > aisleX + 0.5f) // 포스기 앞 줄 또는 진열대 오른쪽 통로
            {
                if (!OnLane(p))
                {
                    points.Add(new Vector3(hallRouteX, p.y));
                    points.Add(new Vector3(hallRouteX, queueLaneY));
                }
                points.Add(new Vector3(aisleX, queueLaneY));
            }
            points.Add(new Vector3(aisleX, bottomLaneY));
            points.Add(new Vector3(doorOutside.x, bottomLaneY));
            points.Add(doorOutside);
            return points;
        }

        // 18:00 영업 종료: 음료를 못 받은 주문은 이탈과 같이 처리(환불·감점), 나머지 손님은 그냥 퇴장 (SFR-014)
        public void CloseAll()
        {
            closed = true;
            foreach (var party in parties.ToArray())
            {
                if (party.Current == CustomerParty.State.Leaving) continue;
                bool ordered = party.Order != null;
                Refund(party);
                Leave(party, lost: ordered);
            }
        }

        // ───────── 매 프레임 ─────────

        void Tick(CustomerParty party, float dt)
        {
            switch (party.Current)
            {
                case CustomerParty.State.ToShowcase:
                    if (party.Arrived)
                    {
                        CollectBakery(party);
                        JoinQueue(party, fromDoor: false);
                    }
                    break;

                case CustomerParty.State.ToQueue:
                    if (party.Arrived)
                    {
                        party.Current = CustomerParty.State.Queued;
                        foreach (var m in party.Members) m.Face(Vector2.up);
                        party.MaxPatience = party.Patience = Difficulty.orderPatience;
                    }
                    break;

                case CustomerParty.State.Queued:
                    // 줄 맨 앞 손님만 인내심이 줄어듦 (SFR-025)
                    if (party.Arrived) foreach (var m in party.Members) m.Face(Vector2.up); // 앞으로 당겨 선 뒤 포스기 쪽을 봄
                    bool front = party.QueueIndex == 0 && party.Arrived;
                    party.Gauge.Show(front);
                    if (!front) break;
                    if (!PosStation.TakingOrder) party.Patience -= dt; // 주문받는 중에는 기다림 멈춤
                    party.Gauge.transform.position = party.Leader.transform.position + Vector3.up * headHeight;
                    party.Gauge.Set(party.Patience / party.MaxPatience);
                    if (party.Patience <= 0) Leave(party, lost: true); // 주문하지 않고 떠남
                    break;

                case CustomerParty.State.ToTable:
                    if (party.Arrived)
                    {
                        foreach (var m in party.Members) m.Sit();
                        party.Current = CustomerParty.State.Waiting;
                        party.MaxPatience = party.Patience = Difficulty.servePatience;
                        party.Gauge.transform.position = party.Table.GaugePoint.position;
                        party.Gauge.Show(true);
                    }
                    break;

                case CustomerParty.State.Waiting:
                    party.WaitedSeconds += dt;
                    party.Patience -= dt;
                    party.Gauge.Set(party.Patience / party.MaxPatience);
                    if (party.Patience <= 0)
                    {
                        Refund(party);
                        Leave(party, lost: true);
                    }
                    break;

                case CustomerParty.State.ToPickup:
                    if (party.Runner.IsMoving) break;
                    {
                        var runner = party.Runner;
                        int index = party.Members.IndexOf(runner);
                        var back = SeatToPickup(party, index);
                        back.Reverse();
                        back.RemoveAt(0); // 지금 서 있는 픽업대 앞
                        back.Add(party.Table.Seat(index));
                        Serve(party.Order, party.CalledItems); // 상태가 Drinking 이 되고, 돌아가 앉으면 마시기 시작
                        party.Runner = null;
                        party.CalledItems = null;
                        runner.WalkToSeat(back, party.Table.SeatFacing(index));
                    }
                    break;

                case CustomerParty.State.Drinking:
                    // 테이블로 걸어가는 중에 서빙받은 손님도 도착하면 앉는다
                    foreach (var m in party.Members) if (!m.IsMoving) m.Sit();
                    if (!party.Arrived) break;
                    party.StayLeft -= dt;
                    if (party.StayLeft <= 0) Leave(party, lost: false);
                    break;

                case CustomerParty.State.Leaving:
                    if (party.Arrived)
                    {
                        parties.Remove(party);
                        Destroy(party.gameObject);
                    }
                    break;
            }
        }
    }
}
