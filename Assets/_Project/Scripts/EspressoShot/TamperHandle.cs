using UnityEngine;
using UnityEngine.EventSystems;

namespace CafeTime.EspressoShot
{
    // SFR-032: 탬핑. 탬퍼(손잡이)를 마우스로 아래로 드래그하면 완료.
    // 탬퍼 오브젝트에 붙이세요. (Image Raycast Target ON)
    // 포터필터를 탬핑 스테이션 슬롯에 올려둔 뒤, 탬퍼를 아래로 끌어내리면 돼요.
    public class TamperHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public PortafilterSlot tampSlot;     // 탬핑 스테이션 슬롯 (type = Tamp)
        public float travel = 60f;           // 아래로 내려가야 하는 거리 (캔버스 단위)

        RectTransform rect;
        Canvas canvas;
        float startY;
        bool done;

        void Awake()
        {
            rect = (RectTransform)transform;
            canvas = GetComponentInParent<Canvas>().rootCanvas;
            startY = rect.anchoredPosition.y;
        }

        public void OnBeginDrag(PointerEventData e) { done = false; }

        public void OnDrag(PointerEventData e)
        {
            float y = rect.anchoredPosition.y + e.delta.y / canvas.scaleFactor;
            y = Mathf.Clamp(y, startY - travel, startY);   // 위아래로만, 범위 안에서
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);

            if (!done && y <= startY - travel + 0.5f)
            {
                done = true;
                Portafilter pf = tampSlot.Current;
                // 원두가루가 담긴 포터필터가 아니면 작동 안 함
                if (pf != null && pf.State == PortafilterState.Ground)
                    pf.SetState(PortafilterState.Tamped);
            }
        }

        public void OnEndDrag(PointerEventData e)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, startY);   // 탬퍼는 원위치
        }
    }
}
