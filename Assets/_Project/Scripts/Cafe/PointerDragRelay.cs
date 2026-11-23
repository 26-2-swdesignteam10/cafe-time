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

        // 끄는 그림 안의 한 점(정규화 좌표, 0~1)의 화면 좌표. 숟가락·국자는 손잡이 끝이 마우스에 붙어 있어,
        // 놓을 자리 판정에 머리(내용물이 담긴 쪽) 위치도 함께 쓴다
        public static Vector2 ScreenPoint(RectTransform rect, Vector2 normalized, Camera cam)
        {
            var corners = new Vector3[4];
            rect.GetWorldCorners(corners); // 0 = 왼쪽 아래, 2 = 오른쪽 위
            var world = new Vector3(Mathf.Lerp(corners[0].x, corners[2].x, normalized.x), Mathf.Lerp(corners[0].y, corners[2].y, normalized.y), corners[0].z);
            return RectTransformUtility.WorldToScreenPoint(cam, world);
        }

        // 마우스 위치 또는 그림의 머리 점이 target 안에 있는가
        public static bool Over(RectTransform target, PointerEventData e, RectTransform dragged, Vector2 head) =>
            RectTransformUtility.RectangleContainsScreenPoint(target, e.position, e.pressEventCamera)
            || RectTransformUtility.RectangleContainsScreenPoint(target, ScreenPoint(dragged, head, e.pressEventCamera), e.pressEventCamera);
    }
}
