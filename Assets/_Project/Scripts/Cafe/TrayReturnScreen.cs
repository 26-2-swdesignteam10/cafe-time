using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 퇴식구 화면 (SFR-059~062): 정면에서 본 수납장 위 쟁반의 물건을 하나씩 치운다.
    // 쓰레기는 쓰레기통으로 끌어 버리고, 잔·접시는 손 칸으로 끌거나 눌러서 가져간다 → 설거지실 설거지 대기 바구니에 넣는다 (ADR-0052).
    // 쟁반 위 물건은 한 줄 5개씩 앞줄부터 놓고, 줄이 늘수록 뒤로(위로) 쌓인다
    public class TrayReturnScreen : StationScreen
    {
        [Tooltip("쟁반 칸마다 쟁반 그림 (GameBalance.trayReturnSlots 와 같은 수)")]
        [SerializeField] GameObject[] trays;
        [Tooltip("쟁반 칸마다 물건을 놓을 컨테이너 (아래 가운데가 쟁반 윗면)")]
        [SerializeField] RectTransform[] itemContainers;
        [SerializeField] DragItem itemTemplate;
        [SerializeField] DropZone trashBin;
        [Tooltip("잔·접시를 눌러 가져올 때 손 칸으로 날아가는 시간")]
        [SerializeField] float flySeconds = 0.4f;

        [Header("쟁반 위 물건 배치")]
        [SerializeField] float itemSize = 96f;
        [SerializeField] float itemGap = 62f;      // 같은 줄 물건 사이 (가운데 간격)
        [SerializeField] int itemsPerRow = 5;
        [SerializeField] float rowStep = 40f;      // 뒷줄이 올라가는 높이
        [SerializeField] float rowScale = 0.94f;   // 뒷줄일수록 조금 작게

        readonly List<DragItem> items = new List<DragItem>();

        static TrayReturn Return => TrayReturn.Instance;

        protected override void Awake()
        {
            base.Awake();
            Return.Changed += RefreshIfOpen;
            HandInventory.Instance.DroppedOnHand += OnDroppedOnHand;
            itemTemplate.gameObject.SetActive(false);
            trashBin.Dropped += e =>
            {
                if (DragItem.From(e)?.DroppedPayload is Vector2Int at) Return.Trash(at.x, at.y); // 잔·접시면 쟁반 위에 남는다
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Return != null) Return.Changed -= RefreshIfOpen;
            if (HandInventory.Instance != null) HandInventory.Instance.DroppedOnHand -= OnDroppedOnHand;
        }

        // 쟁반 위 잔·접시를 손 칸에 끌어 놓음 (쓰레기면 그대로 돌아감)
        void OnDroppedOnHand(int hand, PointerEventData e)
        {
            if (!isActiveAndEnabled) return;
            if (DragItem.From(e)?.DroppedPayload is Vector2Int at) Return.TakeDish(at.x, at.y);
        }

        // 잔·접시를 누르면 손 칸으로 날아간다
        void TakeByClick(Vector2Int at, Image source)
        {
            var hands = HandInventoryView.Snapshot();
            if (Return.TakeDish(at.x, at.y)) HandInventoryView.FlyToNewSlot(hands, source, flySeconds);
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
                    var image = view.GetComponent<Image>();
                    image.sprite = ItemIcons.Of(tray.Items[i]);
                    var at = new Vector2Int(slot, i);
                    view.Payload = at;
                    if (!Tray.IsTrash(tray.Items[i])) view.gameObject.AddComponent<ClickRelay>().Clicked += () => TakeByClick(at, image);
                    Place((RectTransform)view.transform, i, count);
                    view.transform.SetAsFirstSibling(); // 뒷줄(나중 물건)을 먼저 그려 앞줄이 위에 오게
                    view.gameObject.SetActive(true);
                    items.Add(view);
                }
            }
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
    }
}
