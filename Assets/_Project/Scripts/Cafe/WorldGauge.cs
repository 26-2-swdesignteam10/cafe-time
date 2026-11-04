using UnityEngine;

namespace CafeTime.Cafe
{
    // 맵 위 인내심 게이지 (SFR-025 줄 맨 앞 손님 머리 위, SFR-057 테이블 위). 일행마다 코드로 하나씩 만든다
    public class WorldGauge : MonoBehaviour
    {
        static readonly Color Full = new Color32(0x6F, 0xB3, 0x4A, 0xFF);
        static readonly Color Empty = new Color32(0xC0, 0x39, 0x2B, 0xFF);

        SpriteRenderer fill;
        Transform fillPivot; // 채움 이미지의 왼쪽 끝. 이 크기를 줄여 게이지를 줄인다

        // 0~1
        public void Set(float value)
        {
            value = Mathf.Clamp01(value);
            fillPivot.localScale = new Vector3(value, 1, 1);
            fill.color = Color.Lerp(Empty, Full, value);
        }

        public void Show(bool on) => gameObject.SetActive(on);

        public static WorldGauge Create(Transform parent, Sprite frameSprite, Sprite fillSprite)
        {
            var root = new GameObject("PatienceGauge");
            root.transform.SetParent(parent, false);
            var frame = root.AddComponent<SpriteRenderer>();
            frame.sprite = frameSprite;
            frame.sortingOrder = 200;

            float half = fillSprite != null ? fillSprite.bounds.extents.x : 0.47f;
            var pivot = new GameObject("FillPivot").transform;
            pivot.SetParent(root.transform, false);
            pivot.localPosition = new Vector3(-half, 0, 0);
            var fillRenderer = new GameObject("Fill").AddComponent<SpriteRenderer>();
            fillRenderer.transform.SetParent(pivot, false);
            fillRenderer.transform.localPosition = new Vector3(half, 0, 0);
            fillRenderer.sprite = fillSprite;
            fillRenderer.sortingOrder = 201;

            var gauge = root.AddComponent<WorldGauge>();
            gauge.fill = fillRenderer;
            gauge.fillPivot = pivot;
            gauge.Set(1f);
            return gauge;
        }
    }
}
