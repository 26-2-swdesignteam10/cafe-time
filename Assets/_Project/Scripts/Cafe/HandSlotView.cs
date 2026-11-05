using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 화면 하단 손 칸 1개 (SFR-021)
    //  - 끌어서 기기 화면·픽업대에 놓기, 기기 화면의 물건을 끌어와 놓기
    //  - 왼쪽 클릭: 선택 (선택한 뒤 컵 자리를 클릭해 놓기, SFR-034) / 오른쪽 클릭: '버리기' 메뉴
    public class HandSlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
    {
        [SerializeField] int hand;
        [SerializeField] Image frame;
        [SerializeField] Image icon;
        [SerializeField] HandInventoryView owner;

        static readonly Color Normal = Color.white;
        static readonly Color Selected = new Color32(0xFF, 0xD8, 0x8A, 0xFF);

        Image dragGhost;

        public int Hand => hand;

        public void Refresh(HeldItem item, bool selected)
        {
            icon.sprite = ItemIcons.Of(item);
            icon.enabled = icon.sprite != null;
            frame.color = selected ? Selected : Normal;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (HandInventory.Instance[hand] == null) return;
            if (eventData.button == PointerEventData.InputButton.Right) owner.OpenMenu(this);
            else if (eventData.button == PointerEventData.InputButton.Left && !eventData.dragging) HandInventory.Instance.Select(hand);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (HandInventory.Instance[hand] == null) return;
            // 커서를 따라다니는 아이콘. 놓을 곳을 가리지 않도록 레이캐스트 끔
            dragGhost = Instantiate(icon, owner.DragLayer);
            dragGhost.raycastTarget = false;
            dragGhost.transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragGhost != null) dragGhost.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragGhost != null) Destroy(dragGhost.gameObject);
            dragGhost = null;
        }

        public void OnDrop(PointerEventData eventData) => HandInventory.Instance.RaiseDropped(hand, eventData);
    }
}
