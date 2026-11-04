using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 화면 하단 손 칸 1개 (SFR-021). 끌어서 픽업대에 놓기(SFR-056), 우클릭으로 '버리기' 메뉴
    public class HandSlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] int hand;
        [SerializeField] Image icon;
        [SerializeField] HandInventoryView owner;

        Image dragGhost;

        public int Hand => hand;

        public void Refresh(HeldItem item)
        {
            icon.sprite = ItemIcons.Of(item);
            icon.enabled = icon.sprite != null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && HandInventory.Instance[hand] != null)
                owner.OpenMenu(this);
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
    }
}
