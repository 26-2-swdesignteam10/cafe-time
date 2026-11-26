using UnityEngine;
using UnityEngine.Rendering;

namespace CafeTime.Cafe
{
    // 맵 위에 물건 아이콘(손 칸·픽업대 창과 같은 그림)을 원하는 크기로 세워 그린다 (픽업대 카운터, 테이블 식사, ADR-0053)
    public static class WorldItemSprite
    {
        // 물건을 담는 층. 가구 기준점 바로 앞(-0.01)에 두고 SortingGroup 으로 묶어,
        // 가구와 가구 뒤에 선(앉은) 사람보다 앞, 가구 앞에 선 사람보다 뒤에 그려지게 한다
        public static Transform CreateLayer(Transform parent, string name)
        {
            var layer = new GameObject(name).transform;
            layer.SetParent(parent, false);
            layer.localPosition = new Vector3(0, -0.01f, 0);
            layer.gameObject.AddComponent<SortingGroup>();
            return layer;
        }

        public static SpriteRenderer Create(Transform layer, int order)
        {
            var sr = new GameObject("Item").AddComponent<SpriteRenderer>();
            sr.transform.SetParent(layer, false);
            sr.sortingOrder = order;
            return sr;
        }

        // 그림 아래 끝이 (x, bottom), 그림 높이가 height 가 되게 놓는다 (layer 기준 좌표). 그림이 없으면 숨김
        public static void Place(SpriteRenderer sr, Sprite sprite, float x, float bottom, float height)
        {
            sr.sprite = sprite;
            sr.enabled = sprite != null;
            if (sprite == null) return;
            float scale = height / Mathf.Max(0.0001f, sprite.bounds.size.y);
            sr.transform.localScale = new Vector3(scale, scale, 1);
            sr.transform.localPosition = new Vector3(x, bottom - sprite.bounds.min.y * scale, 0);
        }
    }
}
