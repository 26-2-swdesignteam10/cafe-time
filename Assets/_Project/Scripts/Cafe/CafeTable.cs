using System.Collections.Generic;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 테이블 1개 (SFR-024). 일행 1개가 함께 앉는다.
    // 청소 요청(SFR-066)이 있거나 쟁반이 남아 있으면 손님이 앉을 수 없다.
    // 서빙받은 음료·베이커리는 앉은 자리 앞 테이블 위에 놓이고, 머무는 동안 한 입씩 먹고 마신다 (ADR-0053)
    public class CafeTable : MonoBehaviour
    {
        [Tooltip("앉는 자리 (왼쪽 의자, 오른쪽 의자, 뒤쪽 2자리 순)")]
        [SerializeField] Transform[] seats;
        [Tooltip("테이블로 걸어올 때 지나는 가로 통로의 y")]
        [SerializeField] float laneY;
        [Tooltip("인내심 게이지 위치")]
        [SerializeField] Transform gaugePoint;
        [SerializeField] GameObject cleanIcon;    // 청소 요청 아이콘
        [SerializeField] GameObject trayOnTable;  // 두고 간 쟁반 그림
        [SerializeField] TableCleanSpot cleanSpot; // 행주를 들고 E (요청이 있을 때만 켜짐)

        [Header("식사 (ADR-0053)")]
        [Tooltip("자리별 음식 놓는 곳 (테이블 기준점 기준, 아래 끝): 왼쪽 의자, 오른쪽 의자, 뒤 왼쪽, 뒤 오른쪽")]
        [SerializeField] Vector2[] mealSpots = { new Vector2(-0.5f, 0.85f), new Vector2(0.5f, 0.85f), new Vector2(-0.4f, 1.1f), new Vector2(0.4f, 1.1f) };
        [Tooltip("한 자리에 물건이 여럿이면 옆으로 벌리는 간격")]
        [SerializeField] float mealSpread = 0.28f;
        [Tooltip("그림 높이 (아이콘 그림 전체, 투명 여백 포함)")]
        [SerializeField] float mealDrinkHeight = 0.6f, mealDessertHeight = 0.5f;
        [Tooltip("머무는 동안 먹는 횟수. 한 입마다 베이커리가 조금씩 줄고 음료를 들어 마신다")]
        [SerializeField] int mealBites = 5;
        [SerializeField] float sipLift = 0.12f, sipSeconds = 0.35f;

        class MealItem
        {
            public SpriteRenderer Renderer;
            public bool Dessert;
            public Vector3 Home;
            public Vector3 Scale;
        }

        readonly List<MealItem> meal = new List<MealItem>();
        Transform mealLayer;
        int bitesShown;
        float sipLeft;

        public CustomerParty Party { get; set; }
        public Tray LeftTray { get; private set; }
        public bool CleaningRequested { get; private set; }
        public float CleaningLeft { get; set; }       // 청소 제한 시간 남은 초
        public bool CleaningMissed { get; set; }      // 제한 시간이 지나 감점을 받았는지

        public bool IsFree => Party == null && !CleaningRequested && LeftTray == null;
        public float LaneY => laneY;
        public Transform GaugePoint => gaugePoint;

        public Vector3 Seat(int index) => seats[Mathf.Clamp(index, 0, seats.Length - 1)].position;

        // 자리에 앉아 바라보는 방향: 왼쪽 의자는 오른쪽(테이블), 오른쪽 의자는 왼쪽, 뒤쪽 자리는 정면 (SFR-024)
        public Vector2 SeatFacing(int index) => index == 0 ? Vector2.right : index == 1 ? Vector2.left : Vector2.down;

        void Awake() => Refresh();

        // 자리 i 의 손님이 받은 음료·베이커리를 그 자리 앞에 놓는다. sprites[i] = (그림, 베이커리인지)
        public void ShowMeal(IReadOnlyList<List<(Sprite sprite, bool dessert)>> bySeat)
        {
            ClearMeal();
            if (mealLayer == null) mealLayer = WorldItemSprite.CreateLayer(transform, "Meal");
            int order = 0;
            for (int seat = 0; seat < bySeat.Count; seat++)
            {
                var spot = mealSpots[Mathf.Clamp(seat, 0, mealSpots.Length - 1)];
                var items = bySeat[seat];
                for (int i = 0; i < items.Count; i++)
                {
                    var sr = WorldItemSprite.Create(mealLayer, order++);
                    float x = spot.x + (i - (items.Count - 1) / 2f) * mealSpread;
                    WorldItemSprite.Place(sr, items[i].sprite, x, spot.y, items[i].dessert ? mealDessertHeight : mealDrinkHeight);
                    meal.Add(new MealItem { Renderer = sr, Dessert = items[i].dessert, Home = sr.transform.localPosition, Scale = sr.transform.localScale });
                }
            }
            bitesShown = 0;
            sipLeft = 0;
        }

        public void ClearMeal()
        {
            foreach (var item in meal) Destroy(item.Renderer.gameObject);
            meal.Clear();
        }

        // 머문 비율(0~1)만큼 먹는다. 새로 한 입 먹었으면 true (손님이 몸을 들썩인다)
        public bool Eat(float progress)
        {
            int bites = Mathf.Clamp(Mathf.FloorToInt(progress * mealBites), 0, mealBites);
            if (bites <= bitesShown || meal.Count == 0) return false;
            bitesShown = bites;
            float left = 1f - (float)bites / mealBites; // 베이커리 남은 양 (마지막 한 입에 다 먹음)
            foreach (var item in meal)
            {
                if (!item.Dessert) continue;
                // 아래 끝을 붙인 채 줄어든다
                float k = Mathf.Max(left, 0f);
                item.Renderer.transform.localScale = item.Scale * k;
                var sprite = item.Renderer.sprite;
                float bottom = item.Home.y + sprite.bounds.min.y * item.Scale.y;
                item.Renderer.transform.localPosition = new Vector3(item.Home.x, bottom - sprite.bounds.min.y * item.Scale.y * k, 0);
                item.Renderer.enabled = k > 0f;
            }
            sipLeft = sipSeconds;
            return true;
        }

        void Update()
        {
            if (sipLeft <= 0f) return;
            sipLeft = Mathf.Max(0f, sipLeft - GameClock.Delta);
            float lift = sipLift * Mathf.Sin((1f - sipLeft / sipSeconds) * Mathf.PI); // 음료를 들었다 내려놓음
            foreach (var item in meal)
                if (!item.Dessert) item.Renderer.transform.localPosition = item.Home + Vector3.up * lift;
        }

        public void RequestCleaning(float timeLimit)
        {
            CleaningRequested = true;
            CleaningLeft = timeLimit;
            CleaningMissed = false;
            Refresh();
        }

        public void LeaveTray(Tray tray)
        {
            LeftTray = tray;
            Refresh();
        }

        // 청소 완료: 남은 쟁반은 퇴식구로 (가득 차 있으면 테이블에 남음)
        public void Clean()
        {
            CleaningRequested = false;
            TryMoveTray();
            Refresh();
        }

        public void TryMoveTray()
        {
            if (LeftTray == null || CleaningRequested || !TrayReturn.Instance.TryReturn(LeftTray)) return;
            LeftTray = null;
            Refresh();
        }

        void Refresh()
        {
            if (cleanIcon != null) cleanIcon.SetActive(CleaningRequested);
            if (trayOnTable != null) trayOnTable.SetActive(LeftTray != null);
            if (cleanSpot != null) cleanSpot.enabled = CleaningRequested;
        }
    }
}
