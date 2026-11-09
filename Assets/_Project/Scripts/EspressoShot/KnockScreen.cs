using System;
using System.Collections;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.EspressoShot
{
    // SFR-036: 찌꺼기 털기 창. 사용한 포터필터를 넉박스에 대면 열린다.
    //  1) 바스켓이 아래를 향한 포터필터를 막대에 빠르게 내려치면 찌꺼기 덩어리가 대부분 떨어진다
    //  2) 오른쪽 브러시를 끌어 바스켓 안을 문지르면 남은 가루가 사라지고, 다 털면 창이 닫히며 빈 필터가 된다
    // 중간에 닫으면 포터필터는 찌꺼기가 남은 채 넉박스 자리에 있고, 다시 넉박스에 대면 처음부터 한다.
    public class KnockScreen : Popup, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        enum Phase { Knock, Falling, Brush, Done }

        [Header("포터필터")]
        public Image portafilter;
        public Sprite knockSprite;        // 뒤집힌 옆모습, 찌꺼기 덩어리 있음
        public Sprite knockEmptySprite;   // 덩어리가 빠진 모습
        public Sprite brushSprite;        // 바로 놓고 위에서 본 모습
        public Vector2 knockStart = new Vector2(-120, 200);
        public Vector2 knockSize = new Vector2(560, 220);
        public Vector2 brushPosition = Vector2.zero;
        public Vector2 brushViewSize = new Vector2(440, 300);
        [Tooltip("내려칠 때 포터필터 중심이 멈추는 높이 (바스켓 테두리가 막대 윗면에 닿는 곳)")]
        public float contactY = -8f;
        [Tooltip("바스켓 가운데 아래 끝 (포터필터 중심 기준). 덩어리가 여기서 떨어진다")]
        public Vector2 basketBottom = new Vector2(120, -100);
        [Tooltip("이 속도(캔버스 단위/초) 이상으로 내려치면 털린다")]
        public float strongSpeed = 1400f;

        [Header("찌꺼기")]
        public RectTransform puck;
        public float puckFallTo = -230f;
        public Image residue;             // brushSprite 위에 겹치는 남은 가루
        [Tooltip("남은 가루 위 타원 (포터필터 중심 기준 중심, 반지름)")]
        public Vector2 basketCenter = new Vector2(30, 20);
        public Vector2 basketRadius = new Vector2(150, 92);
        [Tooltip("다 털어 내려면 바스켓 안에서 브러시를 움직여야 하는 거리")]
        public float brushDistance = 1400f;

        [Header("브러시")]
        public RectTransform brush;
        public Vector2 brushHome = new Vector2(560, -250);
        [Tooltip("브러시 솔 끝 (브러시 중심 기준)")]
        public Vector2 brushTip = new Vector2(-80, 0);

        [Header("안내 글자")]
        public Image hint;
        public Sprite hintKnock, hintHarder, hintBrush;

        Phase phase;
        Action onCleaned;
        RectTransform pfRect;
        Canvas rootCanvas;
        bool draggingPortafilter, draggingBrush;
        float speed;       // 아래 방향 속도 (부드럽게 평균)
        float brushed;

        protected override void Awake()
        {
            base.Awake();
            pfRect = portafilter.rectTransform;
            rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
        }

        // 넉박스(PortafilterSlot)에서 호출. 다 털면 onCleaned
        public void Open(Action onCleaned)
        {
            this.onCleaned = onCleaned;
            Open();      // 처음 열 때 Awake 가 여기서 실행된다
            ResetView();
        }

        public override void Close()
        {
            StopAllCoroutines();
            draggingPortafilter = draggingBrush = false;
            base.Close();
        }

        void ResetView()
        {
            phase = Phase.Knock;
            portafilter.sprite = knockSprite;
            pfRect.sizeDelta = knockSize;
            pfRect.anchoredPosition = knockStart;
            puck.gameObject.SetActive(false);
            residue.gameObject.SetActive(false);
            brush.gameObject.SetActive(false);
            brush.anchoredPosition = brushHome;
            SetHint(hintKnock);
            brushed = 0f;
        }

        void SetHint(Sprite sprite)
        {
            if (hint == null || sprite == null) return;
            hint.sprite = sprite;
            hint.rectTransform.sizeDelta = sprite.rect.size;
        }

        bool Pressed(PointerEventData e, Component target) =>
            e.pointerPressRaycast.gameObject != null && e.pointerPressRaycast.gameObject.transform.IsChildOf(target.transform);

        public void OnBeginDrag(PointerEventData e)
        {
            draggingPortafilter = phase == Phase.Knock && Pressed(e, portafilter);
            draggingBrush = phase == Phase.Brush && Pressed(e, brush);
            speed = 0f;
        }

        public void OnDrag(PointerEventData e)
        {
            Vector2 delta = e.delta / rootCanvas.scaleFactor;
            if (draggingPortafilter) DragPortafilter(delta);
            else if (draggingBrush) DragBrush(delta);
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (draggingPortafilter && phase == Phase.Knock) StartCoroutine(MoveTo(pfRect, knockStart, 0.15f));
            if (draggingBrush && phase == Phase.Brush) StartCoroutine(MoveTo(brush, brushHome, 0.15f));
            draggingPortafilter = draggingBrush = false;
        }

        // ---------- 1) 내려치기 ----------
        void DragPortafilter(Vector2 delta)
        {
            float dt = Mathf.Max(Time.unscaledDeltaTime, 0.001f);
            speed = Mathf.Lerp(speed, -delta.y / dt, 0.5f);

            float y = Mathf.Min(pfRect.anchoredPosition.y + delta.y, knockStart.y + 40f);
            if (y > contactY)
            {
                pfRect.anchoredPosition = new Vector2(knockStart.x, y);
                return;
            }
            pfRect.anchoredPosition = new Vector2(knockStart.x, contactY);
            draggingPortafilter = false;
            StartCoroutine(speed >= strongSpeed ? StrongHit() : WeakHit());
        }

        IEnumerator WeakHit()
        {
            SetHint(hintHarder);
            yield return Bounce(12f);
            yield return MoveTo(pfRect, knockStart, 0.2f);
        }

        IEnumerator StrongHit()
        {
            phase = Phase.Falling;
            portafilter.sprite = knockEmptySprite;
            puck.gameObject.SetActive(true);
            Vector2 from = new Vector2(knockStart.x, contactY) + basketBottom;
            puck.anchoredPosition = from;
            var puckImage = puck.GetComponent<Image>();
            StartCoroutine(Bounce(18f));

            // 덩어리가 박스 안으로 떨어지며 사라짐
            for (float t = 0; t < 0.45f; t += Time.unscaledDeltaTime)
            {
                float k = t / 0.45f;
                puck.anchoredPosition = new Vector2(from.x, Mathf.Lerp(from.y, puckFallTo, k * k));
                puckImage.color = new Color(1, 1, 1, 1f - Mathf.Clamp01((k - 0.6f) / 0.4f));
                yield return null;
            }
            puck.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.2f);

            // 바로 놓고 브러시 단계로
            phase = Phase.Brush;
            portafilter.sprite = brushSprite;
            pfRect.sizeDelta = brushViewSize;
            pfRect.anchoredPosition = brushPosition;
            residue.gameObject.SetActive(true);
            residue.color = Color.white;
            brush.gameObject.SetActive(true);
            SetHint(hintBrush);
        }

        // ---------- 2) 브러시 ----------
        void DragBrush(Vector2 delta)
        {
            brush.anchoredPosition += delta;
            Vector2 tip = brush.anchoredPosition + brushTip - (pfRect.anchoredPosition + basketCenter);
            Vector2 n = new Vector2(tip.x / basketRadius.x, tip.y / basketRadius.y);
            if (n.sqrMagnitude > 1f) return; // 바스켓 밖

            brushed += delta.magnitude;
            float left = 1f - Mathf.Clamp01(brushed / brushDistance);
            residue.color = new Color(1, 1, 1, left);
            if (left <= 0f) StartCoroutine(Finish());
        }

        IEnumerator Finish()
        {
            phase = Phase.Done;
            draggingBrush = false;
            residue.gameObject.SetActive(false);
            yield return MoveTo(brush, brushHome, 0.2f);
            yield return new WaitForSecondsRealtime(0.25f);
            var done = onCleaned;
            onCleaned = null;
            Close();
            done?.Invoke();
        }

        // ---------- 연출 ----------
        IEnumerator Bounce(float height)
        {
            Vector2 start = pfRect.anchoredPosition;
            for (float t = 0; t < 0.18f; t += Time.unscaledDeltaTime)
            {
                pfRect.anchoredPosition = start + Vector2.up * (height * Mathf.Sin(t / 0.18f * Mathf.PI));
                yield return null;
            }
            pfRect.anchoredPosition = start;
        }

        static IEnumerator MoveTo(RectTransform rt, Vector2 to, float seconds)
        {
            Vector2 from = rt.anchoredPosition;
            for (float t = 0; t < seconds; t += Time.unscaledDeltaTime)
            {
                rt.anchoredPosition = Vector2.Lerp(from, to, t / seconds);
                yield return null;
            }
            rt.anchoredPosition = to;
        }
    }
}
