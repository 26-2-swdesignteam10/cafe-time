using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.EspressoShot
{
    // SFR-032: 탬핑. 탬퍼(손잡이)를 아래로 끌어 가루에 댄 뒤, 그대로 누르고 있으면(GameBalance.TampSeconds) 완료.
    // 탬퍼 오브젝트에 붙이세요. (Image Raycast Target ON)
    // 중간에 놓으면 처음부터 다시 누른다.
    public class TamperHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public PortafilterSlot tampSlot;     // 탬핑 스테이션 슬롯 (type = Tamp)
        public float travel = 60f;           // 가루 윗면에 닿기까지 내려가야 하는 거리 (캔버스 단위)
        public float pressDepth = 8f;        // 누르는 동안 가루가 눌려 더 내려가는 거리
        public Image progress;               // 탬퍼 위 진행 게이지의 채움 (GaugeBar). 누르는 동안만 틀을 켠다

        RectTransform rect;
        Canvas canvas;
        float startY;
        bool dragging, done;
        float pressed;   // 가루에 닿은 채 누른 시간

        void Awake()
        {
            rect = (RectTransform)transform;
            canvas = GetComponentInParent<Canvas>().rootCanvas;
            startY = rect.anchoredPosition.y;
            ShowProgress(-1f);
        }

        public void OnBeginDrag(PointerEventData e)
        {
            dragging = true;
            done = false;
            pressed = 0f;
        }

        public void OnDrag(PointerEventData e)
        {
            float y = rect.anchoredPosition.y + e.delta.y / canvas.scaleFactor;
            y = Mathf.Clamp(y, Contact - Squeeze, startY);   // 위아래로만, 범위 안에서
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
        }

        void Update()
        {
            if (!dragging || done) return;
            Portafilter pf = GroundPortafilter();
            bool touching = pf != null && rect.anchoredPosition.y <= Contact + 0.5f;
            if (!touching)
            {
                // 가루에서 떼면 처음부터
                if (pressed > 0f) { pressed = 0f; pf?.HideFill(); ShowProgress(-1f); }
                return;
            }

            float seconds = GameBalance.Instance.TampSeconds;
            pressed += Time.deltaTime; // 일시정지 중에는 멈춤
            float k = Mathf.Clamp01(pressed / seconds);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Contact - Squeeze);
            pf.ShowFill(PortafilterState.Tamped, k); // 눌린 가루가 차오르듯 보임
            ShowProgress(k);
            if (k >= 1f)
            {
                done = true;
                ShowProgress(-1f);
                pf.HideFill();
                pf.SetState(PortafilterState.Tamped);
            }
        }

        public void OnEndDrag(PointerEventData e)
        {
            dragging = false;
            if (!done) GroundPortafilter()?.HideFill();
            pressed = 0f;
            ShowProgress(-1f);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, startY);   // 탬퍼는 원위치
        }

        // 0~1 이면 게이지를 보이고 채움, 음수면 숨김
        void ShowProgress(float k) => GaugeBar.Show(progress, k);

        float Contact => startY - travel;

        // 누르는 동안 pressDepth 만큼 천천히 더 내려간다
        float Squeeze => pressDepth * Mathf.Clamp01(pressed / Mathf.Max(GameBalance.Instance.TampSeconds, 0.01f));

        // 원두가루가 담긴 포터필터가 아니면 작동 안 함
        Portafilter GroundPortafilter()
        {
            Portafilter pf = tampSlot.Current;
            return pf != null && pf.State == PortafilterState.Ground ? pf : null;
        }
    }
}
