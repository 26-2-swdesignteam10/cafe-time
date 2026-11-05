using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CafeTime.Cafe
{
    // 끄는 방향·거리만 쓰는 도구 (탬퍼를 아래로, 수세미를 좌우로). 끄는 동안 도구 그림이 조금 따라 움직이고 놓으면 제자리
    public class DragMotion : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Vector2 followRange = new Vector2(60, 60); // 그림이 따라 움직이는 최대 거리 (px)

        public event Action<Vector2> Moved; // 이번 이동량 (화면 px)
        public event Action Released;

        Vector3 home;
        Vector2 offset;

        void Awake() => home = transform.localPosition;

        public void OnBeginDrag(PointerEventData eventData) => offset = Vector2.zero;

        public void OnDrag(PointerEventData eventData)
        {
            offset += eventData.delta;
            transform.localPosition = home + new Vector3(Mathf.Clamp(offset.x, -followRange.x, followRange.x),
                Mathf.Clamp(offset.y, -followRange.y, followRange.y), 0);
            Moved?.Invoke(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.localPosition = home;
            Released?.Invoke();
        }
    }
}
