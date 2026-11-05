using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 퇴식구 화면 (SFR-059~062): 쟁반 위 물건을 하나씩 끌어 쓰레기통 / 설거지통으로 분류
    public class TrayReturnScreen : StationScreen
    {
        [Tooltip("쟁반 칸마다 쟁반 그림 (GameBalance.trayReturnSlots 와 같은 수)")]
        [SerializeField] GameObject[] trays;
        [Tooltip("쟁반 칸마다 물건을 놓을 컨테이너")]
        [SerializeField] Transform[] itemContainers;
        [SerializeField] DragItem itemTemplate;
        [SerializeField] DropZone trashBin;
        [SerializeField] DropZone dishBin;

        readonly List<DragItem> items = new List<DragItem>();

        static TrayReturn Return => TrayReturn.Instance;

        protected override void Awake()
        {
            base.Awake();
            Return.Changed += RefreshIfOpen;
            itemTemplate.gameObject.SetActive(false);
            trashBin.Dropped += e => Sort(e, false);
            dishBin.Dropped += e => Sort(e, true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Return != null) Return.Changed -= RefreshIfOpen;
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
                for (int i = 0; i < tray.Items.Count; i++)
                {
                    var view = Instantiate(itemTemplate, itemContainers[slot]);
                    view.GetComponent<Image>().sprite = ItemIcons.Of(tray.Items[i]);
                    view.Payload = new Vector2Int(slot, i);
                    view.gameObject.SetActive(true);
                    items.Add(view);
                }
            }
        }
    }
}
