using UnityEngine;

namespace CafeTime.Cafe
{
    // 맵 위 게이지. 인내심 (SFR-025 줄 맨 앞 손님 머리 위, SFR-057 테이블 위)과 기기 작업 진행 (SFR-022). 코드로 하나씩 만든다
    // 틀·채움 모두 9-slice 캡슐이라, 채움은 비율로 늘이지 않고 너비(size)를 줄여 양끝이 둥근 모양을 유지한다
    public class WorldGauge : MonoBehaviour
    {
        static readonly Color Full = new Color32(0x6F, 0xB3, 0x4A, 0xFF);
        static readonly Color Empty = new Color32(0xC0, 0x39, 0x2B, 0xFF);
        static readonly Color Work = new Color32(0xFF, 0xA0, 0x32, 0xFF); // 회색조 채움에 곱하는 캐러멜색

        SpriteRenderer fill;
        float width;       // 꽉 찼을 때 채움 너비 (월드 단위)
        bool warnColors;   // 인내심이면 남은 양에 따라 초록→빨강

        // 0~1
        public void Set(float value)
        {
            value = Mathf.Clamp01(value);
            fill.enabled = value > 0f;
            fill.size = new Vector2(width * value, fill.size.y);
            fill.transform.localPosition = new Vector3((value - 1f) * width / 2f, 0, 0); // 왼쪽 끝 고정
            if (warnColors) fill.color = Color.Lerp(Empty, Full, value);
        }

        public void Show(bool on) => gameObject.SetActive(on);

        public static WorldGauge Create(Transform parent, Sprite frameSprite, Sprite fillSprite, bool warnColors = true)
        {
            var root = new GameObject(warnColors ? "PatienceGauge" : "WorkGauge");
            root.transform.SetParent(parent, false);
            var frame = root.AddComponent<SpriteRenderer>();
            frame.sprite = frameSprite;
            frame.sortingOrder = 200;

            var fillRenderer = new GameObject("Fill").AddComponent<SpriteRenderer>();
            fillRenderer.transform.SetParent(root.transform, false);
            fillRenderer.sprite = fillSprite;
            fillRenderer.drawMode = SpriteDrawMode.Sliced;
            fillRenderer.size = fillSprite != null ? (Vector2)fillSprite.bounds.size : new Vector2(0.94f, 0.125f);
            fillRenderer.color = Work;
            fillRenderer.sortingOrder = 201;

            var gauge = root.AddComponent<WorldGauge>();
            gauge.fill = fillRenderer;
            gauge.width = fillRenderer.size.x;
            gauge.warnColors = warnColors;
            gauge.Set(1f);
            return gauge;
        }
    }
}
