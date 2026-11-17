using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CafeTime.Cafe
{
    // 끌기 이벤트를 화면 스크립트로 넘겨 주는 부품 (재료 작업 창의 청 숟가락·티백 끌기, ADR-0028).
    // 손 칸 물건 끌기(DragItem)와 달리 손 인벤토리를 거치지 않는다. 쉐이크 제조실 원액 국자·우유 팩(ADR-0029)은 마우스 올림도 받는다
    public class PointerDragRelay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action Entered;
        public event Action Exited;
        public event Action<PointerEventData> Began;
        public event Action<PointerEventData> Dragging;
        public event Action<PointerEventData> Ended;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) Began?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) Dragging?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) Ended?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData) => Entered?.Invoke();

        public void OnPointerExit(PointerEventData eventData) => Exited?.Invoke();
    }
}
