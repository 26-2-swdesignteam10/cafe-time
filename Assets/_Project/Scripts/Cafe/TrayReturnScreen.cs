using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 퇴식구 화면 (SFR-059~062): 정면에서 본 수납장 위 쟁반의 물건을 하나씩 끌어 쓰레기통 / 설거지통으로 분류.
    // 쟁반 위 물건은 한 줄 5개씩 앞줄부터 놓고, 줄이 늘수록 뒤로(위로) 쌓인다.
    // 설거지통에는 설거지 대기 중인 잔이 4개씩 포개어 쌓이고(최대 4줄 = 16잔: 쟁반 4칸 × 일행 최대 4명), 접시는 따로 쌓인다
    public class TrayReturnScreen : StationScreen
    {
        [Tooltip("쟁반 칸마다 쟁반 그림 (GameBalance.trayReturnSlots 와 같은 수)")]
        [SerializeField] GameObject[] trays;
        [Tooltip("쟁반 칸마다 물건을 놓을 컨테이너 (아래 가운데가 쟁반 윗면)")]
        [SerializeField] RectTransform[] itemContainers;
        [SerializeField] DragItem itemTemplate;
        [SerializeField] DropZone trashBin;
        [SerializeField] DropZone dishBin;

        [Header("쟁반 위 물건 배치")]
        [SerializeField] float itemSize = 96f;
        [SerializeField] float itemGap = 62f;      // 같은 줄 물건 사이 (가운데 간격)
        [SerializeField] int itemsPerRow = 5;
        [SerializeField] float rowStep = 40f;      // 뒷줄이 올라가는 높이
        [SerializeField] float rowScale = 0.94f;   // 뒷줄일수록 조금 작게

        [Header("설거지통에 쌓인 잔·접시")]
        [Tooltip("잔 자리. 왼쪽 줄부터, 줄마다 아래에서 위로 (빌더가 4줄 × 4개로 만든다)")]
        [SerializeField] Image[] cupStack;
        [Tooltip("접시 더미 자리 (아래에서 위로)")]
        [SerializeField] Image[] platePile;
        [SerializeField] float dropHeight = 60f, dropSeconds = 0.18f;

        readonly List<DragItem> items = new List<DragItem>();
        Vector2[] cupHome, plateHome;
        int shownCups, shownPlates;
        bool opening;

        static TrayReturn Return => TrayReturn.Instance;
        static DishStation Dish => DishStation.Instance;

        protected override void Awake()
        {
            base.Awake();
            Return.Changed += RefreshIfOpen;
            Dish.Changed += RefreshIfOpen;
            itemTemplate.gameObject.SetActive(false);
            trashBin.Dropped += e => Sort(e, false);
            dishBin.Dropped += e => Sort(e, true);
            cupHome = cupStack.Select(i => i.rectTransform.anchoredPosition).ToArray();
            plateHome = platePile.Select(i => i.rectTransform.anchoredPosition).ToArray();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Return != null) Return.Changed -= RefreshIfOpen;
            if (Dish != null) Dish.Changed -= RefreshIfOpen;
        }

        // 창을 열 때는 쌓인 잔을 떨어뜨리지 않고 그대로 보여 준다
        public override void Open()
        {
            opening = true;
            base.Open();
            opening = false;
        }

        // 맞지 않는 곳에 놓으면 그대로 쟁반 위에 남는다
        static void Sort(UnityEngine.EventSystems.PointerEventData e, bool toDishBin)
        {
            if (DragItem.From(e)?.DroppedPayload is Vector2Int at) Return.Sort(at.x, at.y, toDishBin);
        }

        protected override void Refresh()
        {
            foreach (var item in items) Destroy(item.gameObject);
            items.Clear();
            for (int slot = 0; slot < trays.Length; slot++)
            {
                var tray = slot < Return.Slots.Length ? Return.Slots[slot] : null;
                trays[slot].SetActive(tray != null);
                if (tray == null) continue;
                int count = tray.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    var view = Instantiate(itemTemplate, itemContainers[slot]);
                    view.GetComponent<Image>().sprite = ItemIcons.Of(tray.Items[i]);
                    view.Payload = new Vector2Int(slot, i);
                    Place((RectTransform)view.transform, i, count);
                    view.transform.SetAsFirstSibling(); // 뒷줄(나중 물건)을 먼저 그려 앞줄이 위에 오게
                    view.gameObject.SetActive(true);
                    items.Add(view);
                }
            }
            RefreshDishBin();
        }

        // i 번째 물건 자리: 한 줄 itemsPerRow 개를 가운데 정렬, 줄이 늘면 위로 올라가며 조금 작아진다
        void Place(RectTransform rt, int i, int count)
        {
            int row = i / itemsPerRow, col = i % itemsPerRow;
            int inRow = Mathf.Min(itemsPerRow, count - row * itemsPerRow);
            float k = Mathf.Pow(rowScale, row);
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0f);
            rt.sizeDelta = Vector2.one * itemSize * k;
            rt.anchoredPosition = new Vector2((col - (inRow - 1) / 2f) * itemGap * k, row * rowStep);
        }

        // 설거지 대기 중인 잔(HOT 먼저, ICE 다음)과 접시를 쌓아 보여 준다. 새로 쌓인 것은 위에서 툭 떨어진다
        void RefreshDishBin()
        {
            var waiting = Dish.Waiting;
            var cups = waiting.Where(k => k == TrayItemKind.DirtyCeramic)
                .Concat(waiting.Where(k => k == TrayItemKind.DirtyPlastic)).Take(cupStack.Length).ToList();
            int plates = Mathf.Min(platePile.Length, waiting.Count(k => k == TrayItemKind.DirtyPlate));

            for (int i = 0; i < cupStack.Length; i++)
            {
                ShowIcon(cupStack[i], i < cups.Count ? ItemIcons.Of(cups[i]) : null);
                cupStack[i].rectTransform.anchoredPosition = cupHome[i];
            }
            for (int i = 0; i < platePile.Length; i++)
            {
                ShowIcon(platePile[i], i < plates ? ItemIcons.Of(TrayItemKind.DirtyPlate) : null);
                platePile[i].rectTransform.anchoredPosition = plateHome[i];
            }
            if (!opening && isActiveAndEnabled)
            {
                for (int i = shownCups; i < cups.Count; i++) StartCoroutine(Drop(cupStack[i].rectTransform, cupHome[i]));
                for (int i = shownPlates; i < plates; i++) StartCoroutine(Drop(platePile[i].rectTransform, plateHome[i]));
            }
            shownCups = cups.Count;
            shownPlates = plates;
        }

        IEnumerator Drop(RectTransform rt, Vector2 home)
        {
            for (float t = 0; t < dropSeconds; t += Time.unscaledDeltaTime)
            {
                float k = t / dropSeconds;
                rt.anchoredPosition = home + Vector2.up * (dropHeight * (1f - k * k));
                yield return null;
            }
            rt.anchoredPosition = home;
        }
    }
}
