using System.Collections.Generic;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 맵의 픽업대 카운터 (SFR-056, ADR-0053): 픽업대 트레이에 놓은 음료와, 그 주문 손님이 진열대에서 집어 온 베이커리를
    // 카운터 위에 그림으로 늘어놓는다. 베이커리는 그 주문에 음료를 하나라도 놓으면 자동으로 같이 놓인다.
    // 손님이 받아 가면(주문이 사라지면) 카운터에서 없어진다
    public class PickupCounterView : MonoBehaviour
    {
        [SerializeField] PickupScreen pickup;
        [Tooltip("물건을 늘어놓는 구간: 카운터 기준점(아래 가운데)에서 왼쪽 끝 x, 오른쪽 끝 x. 왼쪽 끝의 호출 벨은 피한다")]
        [SerializeField] Vector2 rangeX = new Vector2(-1.55f, 2.15f);
        [Tooltip("물건 아래 끝 높이 (상판 윗면, 카운터 기준점 기준)")]
        [SerializeField] float surfaceY = 0.5f;
        [Tooltip("그림 높이 (아이콘 그림 전체, 투명 여백 포함)")]
        [SerializeField] float drinkHeight = 0.6f, dessertHeight = 0.5f;
        [Tooltip("물건 사이 최대 간격. 많으면 구간 안에 맞춰 좁힌다")]
        [SerializeField] float maxSpacing = 0.45f;
        [Tooltip("주문 사이에 더 띄우는 간격")]
        [SerializeField] float orderGap = 0.15f;

        readonly List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        Transform layer;

        void Awake() => layer = WorldItemSprite.CreateLayer(transform, "PickupItems");

        void Start()
        {
            pickup.Changed += Refresh;
            OrderBoard.Instance.Changed += Refresh;
            Refresh();
        }

        void OnDestroy()
        {
            if (pickup != null) pickup.Changed -= Refresh;
            if (OrderBoard.Instance != null) OrderBoard.Instance.Changed -= Refresh;
        }

        void Refresh()
        {
            // 주문이 들어온 순서대로, 주문마다 음료 → 베이커리
            var items = new List<(Sprite sprite, float height, bool newOrder)>();
            foreach (var order in OrderBoard.Instance.Orders)
            {
                var placed = pickup.Placed(order);
                if (placed.Count == 0) continue;
                bool first = true;
                foreach (var item in placed)
                {
                    items.Add((ItemIcons.Of(item), item is DessertItem ? dessertHeight : drinkHeight, first && items.Count > 0));
                    first = false;
                }
                if (order.Party == null) continue;
                foreach (var line in order.Party.CollectedBakery)
                    for (int i = 0; i < line.Quantity; i++)
                        items.Add((ItemIcons.ForMenu(line.Menu, Temperature.None), dessertHeight, false));
            }

            while (renderers.Count < items.Count) renderers.Add(WorldItemSprite.Create(layer, renderers.Count));
            float width = rangeX.y - rangeX.x;
            int gaps = 0;
            foreach (var item in items) if (item.newOrder) gaps++;
            float spacing = items.Count > 1 ? Mathf.Min(maxSpacing, (width - gaps * orderGap) / (items.Count - 1)) : 0f;
            float used = spacing * Mathf.Max(0, items.Count - 1) + gaps * orderGap;
            float x = (rangeX.x + rangeX.y) / 2f - used / 2f;
            for (int i = 0; i < renderers.Count; i++)
            {
                if (i >= items.Count)
                {
                    renderers[i].enabled = false;
                    continue;
                }
                if (items[i].newOrder) x += orderGap;
                WorldItemSprite.Place(renderers[i], items[i].sprite, x, surfaceY, items[i].height);
                x += spacing;
            }
        }
    }
}
