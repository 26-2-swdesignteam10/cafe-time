using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 오늘 고용한 알바생을 맵에 내보내고 일감을 나눈다 (ADR-0057). 같은 테이블·퇴식구·싱크대를 두 명이 맡지 않게 맡은 일을 표시해 둔다.
    // 동선은 손님처럼 길찾기 없이 정해 둔 통로를 따른다: 주방 가로 통로 → 주방 출입구 → 포스기 줄 왼쪽 끝(hub) → 홀
    public class StaffManager : MonoBehaviour
    {
        // 맵 위 장소와 그곳에서 hub 까지 가는 꺾은 점들
        public class Place
        {
            public Vector3 Point;
            public List<Vector3> ToHub;
            public Vector2 Facing = Vector2.down; // 도착해서 바라볼 방향
        }

        public static StaffManager Instance { get; private set; }

        [SerializeField] StaffLook[] looks;
        [SerializeField] CafeTable[] tables;

        [Header("동선 (맵 좌표, CustomerManager 동선과 같은 통로)")]
        [SerializeField] Vector2 hub = new Vector2(2.5f, 7.25f);  // 주방 출입구 바로 아래, 포스기 줄 왼쪽 끝
        [SerializeField] float kitchenDoorX = 2f;                  // 주방 출입구 (x1~2 칸)
        [SerializeField] float kitchenLaneY = 10.2f;               // 주방 안 가로 통로
        [SerializeField] Vector2 dishSpot = new Vector2(1.75f, 11f);
        [SerializeField] Vector2 trayReturnSpot = new Vector2(2.5f, 4.2f);
        [SerializeField] Vector2 idleSpot = new Vector2(3.4f, 10.2f);
        [SerializeField] float idleSpacing = 1.3f;
        [SerializeField] float hallRouteX = 9.5f;
        [SerializeField] float hallLaneY = 3.7f;
        [SerializeField] float hallAisleX = 11.1f;

        readonly HashSet<CafeTable> claimedTables = new HashSet<CafeTable>();
        bool traysClaimed;
        bool dishesClaimed;

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Start()
        {
            var save = SaveSystem.Current;
            var balance = GameBalance.Instance;
            if (save == null) return;
            for (int i = 0; i < save.hiredStaff.Count; i++)
            {
                var definition = balance.FindStaff(save.hiredStaff[i]);
                var look = looks.FirstOrDefault(l => l.id == save.hiredStaff[i]);
                if (definition == null || look == null) continue;
                var idle = Kitchen(idleSpot + Vector2.right * idleSpacing * i, Vector2.down);
                var body = Customer.Create(transform, look.look, idle.Point);
                body.name = $"Staff {definition.Id}";
                body.Face(Vector2.down);
                body.gameObject.AddComponent<StaffWorker>().Init(definition, body, this, idle);
            }
        }

        // ───────── 장소 ─────────

        Place Kitchen(Vector2 point, Vector2 facing) => new Place
        {
            Point = point,
            Facing = facing,
            ToHub = new List<Vector3>
            {
                new Vector3(point.x, kitchenLaneY), new Vector3(kitchenDoorX, kitchenLaneY),
                new Vector3(kitchenDoorX, hub.y), hub,
            },
        };

        public Place DishPlace => Kitchen(dishSpot, Vector2.up);

        public Place TrayReturnPlace => new Place { Point = trayReturnSpot, Facing = Vector2.left, ToHub = new List<Vector3> { hub } };

        // 테이블 뒤 가로 통로에서 닦는다
        public Place TablePlace(CafeTable table) => new Place
        {
            Point = new Vector3(table.transform.position.x, table.LaneY),
            Facing = Vector2.down,
            ToHub = new List<Vector3>
            {
                new Vector3(hallAisleX, table.LaneY), new Vector3(hallAisleX, hallLaneY),
                new Vector3(hallRouteX, hallLaneY), new Vector3(hallRouteX, hub.y), hub,
            },
        };

        // from → to 경로. 두 장소가 hub 까지 함께 지나는 뒷부분은 건너뛰고, 처음 만나는 갈림점에서 꺾는다
        public static List<Vector3> Route(Place from, Place to)
        {
            var a = new List<Vector3>(from.ToHub);
            var b = new List<Vector3>(to.ToHub);
            Vector3? junction = null;
            while (a.Count > 0 && b.Count > 0 && (a[a.Count - 1] - b[b.Count - 1]).sqrMagnitude < 0.0001f)
            {
                junction = a[a.Count - 1];
                a.RemoveAt(a.Count - 1);
                b.RemoveAt(b.Count - 1);
            }
            var path = a;
            if (junction != null) path.Add(junction.Value);
            b.Reverse();
            path.AddRange(b);
            path.Add(to.Point);
            return path;
        }

        // ───────── 일감 맡기 ─────────

        // 청소 요청이 있고 아직 아무도 맡지 않은 테이블
        public bool TryClaimTable(out CafeTable table)
        {
            table = tables.FirstOrDefault(t => t.CleaningRequested && !claimedTables.Contains(t));
            if (table == null) return false;
            claimedTables.Add(table);
            return true;
        }

        public bool HasTableWork => tables.Any(t => t.CleaningRequested && !claimedTables.Contains(t));

        public void ReleaseTable(CafeTable table) => claimedTables.Remove(table);

        public bool TryClaimTrays()
        {
            if (traysClaimed || TrayReturn.Instance == null || TrayReturn.Instance.Slots.All(s => s == null)) return false;
            return traysClaimed = true;
        }

        public void ReleaseTrays() => traysClaimed = false;

        public bool TryClaimDishes()
        {
            if (dishesClaimed || DishStation.Instance == null || DishStation.Instance.InSink == null) return false;
            return dishesClaimed = true;
        }

        public void ReleaseDishes() => dishesClaimed = false;
    }
}
