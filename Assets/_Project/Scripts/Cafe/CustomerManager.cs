using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 일행의 입장·줄·주문·서빙·퇴장을 관리 (SFR-024, 025, 027, 056, 057, 058)
    // 동선: 문 → 가운데 세로 통로(aisleX) → 포스기 앞 가로 줄(queueLaneY) / 테이블 줄의 가로 통로(CafeTable.LaneY)
    public class CustomerManager : MonoBehaviour
    {
        [Header("외형 (Art/Sprites/Customers)")]
        [SerializeField] CustomerLook[] looks;
        [SerializeField] Sprite gaugeFrame;
        [SerializeField] Sprite gaugeFill;

        [Header("동선 (맵 좌표, 1칸 = 1)")]
        [SerializeField] Vector2 doorOutside = new Vector2(13f, 0.3f);
        [SerializeField] float bottomLaneY = 1.25f;  // 문 바로 안쪽 가로 통로
        [SerializeField] float aisleX = 11.5f;       // 가운데 세로 통로
        [SerializeField] float queueLaneY = 7.15f;   // 포스기 앞 줄
        [SerializeField] float queueStartX = 3f;     // 줄 맨 앞 (포스기 바로 앞)
        [SerializeField] float queueSpacing = 1.8f;  // 일행 사이 간격
        [SerializeField] float headHeight = 1.7f;    // 머리 위 게이지 높이
        [SerializeField] CafeTable[] tables;

        public static CustomerManager Instance { get; private set; }

        readonly List<CustomerParty> parties = new List<CustomerParty>();
        readonly List<CustomerParty> queue = new List<CustomerParty>();
        GameBalance balance;
        float nextArrival;
        bool closed;

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

        void Start() => nextArrival = 3f; // 영업 시작 직후 첫 손님

        void Update()
        {
            float dt = GameClock.Delta;
            if (!closed && !GameClock.Instance.IsEntryClosed)
            {
                nextArrival -= dt;
                if (nextArrival <= 0)
                {
                    TrySpawn();
                    nextArrival = NextInterval();
                }
            }
            foreach (var party in parties.ToArray()) Tick(party, dt);
        }

        float NextInterval()
        {
            // DAY가 지날수록 입장 빈도 증가 (SFR-024)
            float scale = Mathf.Pow(balance.ArrivalIntervalPerDay, Save.day - 1);
            return Random.Range(balance.ArrivalInterval.x, balance.ArrivalInterval.y) * scale;
        }

        // ───────── 입장 ─────────

        void TrySpawn()
        {
            var free = tables.Where(t => t.IsFree).ToList();
            if (free.Count == 0) return; // 만석이면 들어오지 않고 돌아감 (감점 없음)

            int size = WeightedPartySize();
            var party = new GameObject("Party").AddComponent<CustomerParty>();
            party.transform.SetParent(transform, false);
            party.Table = free[Random.Range(0, free.Count)];
            party.Table.Party = party;
            party.Wanted = CustomerParty.RandomWants(size, balance);
            party.Gauge = WorldGauge.Create(party.transform, gaugeFrame, gaugeFill);
            party.Gauge.Show(false);
            for (int i = 0; i < size; i++)
            {
                var look = looks[Random.Range(0, looks.Length)];
                party.Members.Add(Customer.Create(party.transform, look, (Vector3)doorOutside + Vector3.down * 0.4f * i));
            }
            parties.Add(party);
            Result.visitors += size;

            party.QueueIndex = queue.Count;
            queue.Add(party);
            party.Current = CustomerParty.State.ToQueue;
            WalkToQueue(party, fromDoor: true);
        }

        int WeightedPartySize()
        {
            var weights = balance.PartySizeWeights;
            int total = weights.Sum();
            int roll = Random.Range(0, total);
            for (int i = 0; i < weights.Count; i++)
            {
                if (roll < weights[i]) return i + 1;
                roll -= weights[i];
            }
            return 1;
        }

        // ───────── 줄 (SFR-025) ─────────

        // 줄 칸 위치. 가로 줄이 차면 가운데 통로를 따라 아래로 이어 선다
        Vector3 QueueSlot(int index)
        {
            float x = queueStartX + index * queueSpacing;
            if (x <= aisleX - queueSpacing * 0.5f) return new Vector3(x, queueLaneY);
            int lastOnLane = Mathf.FloorToInt((aisleX - queueSpacing * 0.5f - queueStartX) / queueSpacing);
            return new Vector3(aisleX, queueLaneY - (index - lastOnLane) * queueSpacing * 0.8f);
        }

        bool OnLane(Vector3 p) => Mathf.Abs(p.y - queueLaneY) < 0.3f;

        void WalkToQueue(CustomerParty party, bool fromDoor)
        {
            var slot = QueueSlot(party.QueueIndex);
            for (int i = 0; i < party.Members.Count; i++)
            {
                var member = party.Members[i];
                // 일행은 대표 뒤에 조금씩 겹쳐 선다
                var offset = OnLane(slot) ? new Vector3(0.3f * i, -0.12f * i) : new Vector3(0.25f * i, -0.25f * i);
                var points = new List<Vector3>();
                if (fromDoor)
                {
                    points.Add(new Vector3(doorOutside.x, bottomLaneY));
                    points.Add(new Vector3(aisleX, bottomLaneY));
                }
                else if (!OnLane(member.transform.position) && OnLane(slot))
                {
                    points.Add(new Vector3(aisleX, queueLaneY)); // 통로에서 가로 줄로 올라감
                }
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
        public Order TakeOrder(CustomerParty party, List<OrderLine> lines, int total)
        {
            if (party == null || party.Current != CustomerParty.State.Queued) return null;
            Save.money += total; // 결제 금액만큼 보유 자금 증가
            Result.revenue += total;
            var order = OrderBoard.Instance.Register(lines, total, party);
            party.Order = order;

            LeaveQueue(party);
            party.Gauge.Show(false);
            party.Current = CustomerParty.State.ToTable;
            var table = party.Table;
            bool frontRow = table.LaneY > queueLaneY - 1.5f; // 포스기 앞 줄과 붙은 테이블 줄
            for (int i = 0; i < party.Members.Count; i++)
            {
                var member = party.Members[i];
                var seat = table.Seat(i);
                var points = new List<Vector3>();
                if (!frontRow)
                {
                    points.Add(new Vector3(aisleX, queueLaneY));
                    points.Add(new Vector3(aisleX, table.LaneY));
                }
                else points.Add(new Vector3(member.transform.position.x, table.LaneY));
                points.Add(new Vector3(seat.x, table.LaneY));
                points.Add(seat);
                member.Walk(points);
            }
            return order;
        }

        // ───────── 서빙 (SFR-056, 058) ─────────

        public void Serve(Order order, IReadOnlyList<HeldItem> items)
        {
            var party = order.Party;
            OrderBoard.Instance.Remove(order);
            if (party == null || party.Current == CustomerParty.State.Leaving) return;

            float satisfaction = party.Satisfaction(items, balance.ServePatience);
            Result.ordersCompleted++;
            Result.satisfactionSum += satisfaction;
            Result.drinksSold += items.Count(i => !i.Menu.IsDessert);

            // 만족도가 높을수록 높은 확률로 팁, 보유 자금에 바로 더함
            if (Random.value < balance.MaxTipChance * satisfaction / 100f)
            {
                int tip = balance.TipPerPerson * party.Members.Count;
                Save.money += tip;
                Result.tips += tip;
            }

            party.Order = null;
            party.Gauge.Show(false);
            party.StayLeft = balance.StayDuration;
            party.Current = CustomerParty.State.Drinking;
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
            bool fromTable = party.Current == CustomerParty.State.ToTable || party.Current == CustomerParty.State.Waiting ||
                             party.Current == CustomerParty.State.Drinking;
            if (party.QueueIndex >= 0) LeaveQueue(party);
            if (party.Table.Party == party) party.Table.Party = null;
            party.Gauge.Show(false);
            party.Current = CustomerParty.State.Leaving;
            // 쟁반 반납(퇴식구, SFR-057·060)은 퇴식구 구현 시 추가
            foreach (var member in party.Members)
            {
                var p = member.transform.position;
                // 지금 있는 가로 통로(테이블 줄 또는 포스기 앞 줄)를 따라 가운데 통로로 간 뒤 문으로
                float laneY = fromTable ? party.Table.LaneY : OnLane(p) ? queueLaneY : p.y;
                var points = new List<Vector3> { new Vector3(p.x, laneY), new Vector3(aisleX, laneY) };
                points.Add(new Vector3(aisleX, bottomLaneY));
                points.Add(new Vector3(doorOutside.x, bottomLaneY));
                points.Add(doorOutside);
                member.Walk(points);
            }
        }

        // 21:00 영업 종료: 음료를 못 받은 주문은 이탈과 같이 처리(환불·감점), 나머지 손님은 그냥 퇴장 (SFR-014)
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
                case CustomerParty.State.ToQueue:
                    if (party.Arrived)
                    {
                        party.Current = CustomerParty.State.Queued;
                        foreach (var m in party.Members) m.Face(Vector2.up);
                        party.MaxPatience = party.Patience = balance.OrderPatience;
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
                        party.MaxPatience = party.Patience = balance.ServePatience;
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

                case CustomerParty.State.Drinking:
                    // 테이블로 걸어가는 중에 서빙받은 손님도 도착하면 앉는다
                    foreach (var m in party.Members) if (!m.IsMoving) m.Sit();
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
