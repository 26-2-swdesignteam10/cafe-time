using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CafeTime.Cafe
{
    // 물건을 끌어다 놓을 수 있는 자리. 손 칸(HandSlotView)이나 DragItem 을 놓으면 Dropped 이벤트
    public class DropZone : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public event Action<PointerEventData> Dropped;
        public event Action Clicked;

        public void OnDrop(PointerEventData eventData) => Dropped?.Invoke(eventData);

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) Clicked?.Invoke();
        }

        // 놓은 것이 손 칸이면 그 손 번호, 아니면 -1
        public static int HandOf(PointerEventData eventData)
        {
            var slot = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<HandSlotView>() : null;
            return slot != null ? slot.Hand : -1;
        }
    }
}
