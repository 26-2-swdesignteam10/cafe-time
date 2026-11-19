using System.Collections.Generic;
using CafeTime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 큰 컵 그림 위에 내용물을 겹쳐 그린다 (ADR-0037).
    // 플라스틱 컵: 넣은 순서대로 재료 층을 아래부터 쌓고 (두께 = 양), 탄산수는 기포, 얼음은 위에 뜬 얼음 조각.
    // 머그잔: 옆이 안 보이므로 윗면 색만 섞어서 보여 주고, 우유 + 샷이면 라떼 아트.
    // 그림은 모두 512×640 컵 캔버스 기준 픽셀 좌표이며, 이 오브젝트는 컵 Image 와 같은 크기로 겹쳐 둔다
    [RequireComponent(typeof(RectTransform))]
    public class CupContentsView : MonoBehaviour
    {
        const float CanvasWidth = 512;

        [Header("플라스틱 컵")]
        [SerializeField] Sprite plasticMask;    // 액체가 보이는 안쪽 영역
        [SerializeField] Sprite plasticFront;   // 액체 위에 덮는 앞면 유리
        [SerializeField] Sprite liquidShade;    // 층 하나 (색을 곱해 씀)
        [SerializeField] Sprite liquidSurface;  // 맨 위 윗면 타원
        [SerializeField] Sprite bubblesTile;    // 탄산 기포 (Tiled)
        [SerializeField] Sprite iceFill;        // 컵 안을 가득 채운 얼음 (캔버스 크기)
        [Tooltip("캔버스 위에서 잰 y: 안쪽 바닥, 가득 찬 높이")]
        [SerializeField] float plasticBottomY = 560, plasticTopY = 300;
        [Tooltip("안쪽 너비: 바닥, 가득 찬 높이")]
        [SerializeField] float plasticBottomWidth = 220, plasticTopWidth = 300;
        [SerializeField] float plasticCenterX = 256;
        [SerializeField] float surfaceHeight = 40;
        [SerializeField] float minLayerHeight = 6;
        [Tooltip("이 무게(g)면 가득 찬 높이까지 찬다. 얼음은 높이에 넣지 않음")]
        [SerializeField] float fullWeight = 330;

        [Header("머그잔")]
        [SerializeField] Sprite ceramicSurface; // 윗면 (색을 곱해 씀)
        [SerializeField] Sprite ceramicFoam;    // 라떼 아트
        [Tooltip("윗면 색을 섞을 때 샷 무게에 곱하는 값 (샷이 진하게 보이도록)")]
        [SerializeField] float shotColorStrength = 4;
        [Tooltip("물처럼 투명한 내용물도 머그잔 윗면이 보이도록 하는 최소 불투명도")]
        [SerializeField] float ceramicMinAlpha = 0.6f;

        [Tooltip("잔에 담긴 티백: 머그잔, 플라스틱 컵 순서")]
        [SerializeField] Sprite[] teaBagSprites = new Sprite[2];

        [Header("재료 색")]
        [SerializeField] string[] colorKeys;
        [SerializeField] Color[] colors;
        [SerializeField] Color fallbackColor = new Color(0.55f, 0.36f, 0.22f, 0.9f);

        readonly List<Image> layers = new List<Image>();
        readonly List<Image> bubbles = new List<Image>();
        RectTransform plasticRoot;
        Image surface, ice, front, ceramicTop, foam, teaBag;

        struct Layer
        {
            public string Key;
            public float Weight;
            public Color Color;
        }

        void Awake() => Build();

        void Build()
        {
            if (plasticRoot != null) return;
            var mask = Stretched("Liquid", transform, plasticMask);
            mask.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            plasticRoot = mask.rectTransform;
            surface = Child("Surface", plasticRoot, liquidSurface);
            ice = Stretched("Ice", plasticRoot, iceFill);
            front = Stretched("Front", transform, plasticFront);
            ceramicTop = Stretched("CeramicSurface", transform, ceramicSurface);
            foam = Stretched("Foam", transform, ceramicFoam);
            teaBag = Stretched("TeaBag", transform, null);
            teaBag.preserveAspect = true;
        }

        // cup 이 null 이거나 비었으면 아무것도 안 그림. steeping: 티백을 담그는 중이라 아직 차 색이 안 우러남
        public void Show(Cup cup, bool steeping = false)
        {
            Build(); // 화면이 처음 열릴 때 Awake 보다 먼저 불릴 수 있다
            bool plastic = cup != null && !cup.IsEmpty && cup.Type == CupType.Plastic;
            bool ceramic = cup != null && !cup.IsEmpty && cup.Type == CupType.Ceramic;
            plasticRoot.gameObject.SetActive(plastic);
            front.enabled = plastic;
            ceramicTop.enabled = ceramic;
            foam.enabled = false;
            // 티백은 담그기가 끝난 뒤 잔에 걸쳐 남는다
            bool hasTeaBag = (plastic || ceramic) && !steeping && cup.Contents.HasTeabag(GameBalance.Instance);
            teaBag.enabled = hasTeaBag;
            if (hasTeaBag) teaBag.sprite = teaBagSprites[plastic ? 1 : 0];
            if (!plastic && !ceramic) return;

            var stack = Layers(cup, steeping);
            if (plastic) ShowPlastic(stack, (int)cup.Contents.Get(RecipeKeys.Ice));
            else ShowCeramic(cup, stack);
        }

        // 아래부터 쌓을 층 목록. 같은 재료를 나중에 더 넣어도 처음 넣은 자리의 층이 두꺼워진다
        List<Layer> Layers(Cup cup, bool steeping)
        {
            var balance = GameBalance.Instance;
            var contents = cup.Contents;
            string tea = null;
            foreach (var key in contents.Order)
                if (balance.FindIngredient(key) is { Category: IngredientCategory.Tea }) tea = key;

            var stack = new List<Layer>();
            foreach (var key in contents.Order)
            {
                if (key == RecipeKeys.Ice || balance.FindIngredient(key) is { Category: IngredientCategory.Tea }) continue;
                float weight = key == RecipeKeys.Shot ? contents.Get(key) * balance.ShotWeight : contents.Get(key);
                bool water = key == RecipeKeys.HotWater || key == RecipeKeys.ColdWater;
                var color = water && tea != null && !steeping ? ColorOf(tea) : ColorOf(key); // 우러난 차는 물 색을 바꾼다
                stack.Add(new Layer { Key = key, Weight = weight, Color = color });
            }
            if (cup.Blended && stack.Count > 1) // 갈아 부은 음료는 한 색으로
            {
                float total = 0;
                foreach (var layer in stack) total += layer.Weight;
                stack = new List<Layer> { new Layer { Key = stack[0].Key, Weight = total, Color = Mix(stack, 1) } };
            }
            return stack;
        }

        void ShowPlastic(List<Layer> stack, int iceScoops)
        {
            float scale = ((RectTransform)transform).rect.width / CanvasWidth;
            float fullHeight = plasticBottomY - plasticTopY;
            float y = plasticBottomY; // 지금까지 쌓은 높이 (캔버스 위에서 잰 y)
            int bubbleCount = 0;
            for (int i = 0; i < stack.Count; i++)
            {
                float height = Mathf.Max(minLayerHeight, stack[i].Weight / fullWeight * fullHeight);
                float top = Mathf.Max(plasticTopY, y - height);
                var layer = Pooled(layers, i, "Layer", liquidShade, Image.Type.Simple);
                layer.color = stack[i].Color;
                Place(layer.rectTransform, plasticCenterX, top, CanvasWidth, y - top, scale);
                if (stack[i].Key == "soda")
                {
                    var bubble = Pooled(bubbles, bubbleCount++, "Bubbles", bubblesTile, Image.Type.Tiled);
                    Place(bubble.rectTransform, plasticCenterX, top, CanvasWidth, y - top, scale);
                }
                y = top;
            }
            Hide(layers, stack.Count);
            Hide(bubbles, bubbleCount);

            bool liquid = stack.Count > 0;
            surface.enabled = liquid;
            if (liquid)
            {
                var top = stack[stack.Count - 1].Color;
                surface.color = new Color(Mathf.Lerp(top.r, 1, 0.25f), Mathf.Lerp(top.g, 1, 0.25f), Mathf.Lerp(top.b, 1, 0.25f), top.a);
                Place(surface.rectTransform, plasticCenterX, y - surfaceHeight / 2, WidthAt(y), surfaceHeight, scale);
                surface.transform.SetAsLastSibling();
            }

            // 얼음이 들어가면 컵 안을 얼음이 가득 채운다 (액체 층 위, 앞면 유리 아래)
            ice.enabled = iceScoops > 0;
            ice.transform.SetAsLastSibling();
        }

        // 붓는 중 (쉐이크 제조실 스무디): 한 색이 weight(g)만큼 차오른다
        public void ShowPouring(CupType type, Color color, float weight)
        {
            Build();
            bool plastic = type == CupType.Plastic;
            bool any = weight > 0.5f;
            plasticRoot.gameObject.SetActive(plastic && any);
            front.enabled = plastic;
            ceramicTop.enabled = !plastic && any;
            foam.enabled = teaBag.enabled = false;
            if (!any) return;
            if (plastic)
            {
                ShowPlastic(new List<Layer> { new Layer { Key = "", Weight = weight, Color = color } }, 0);
                return;
            }
            color.a = Mathf.Max(ceramicMinAlpha, Mathf.Clamp01(weight / fullWeight * 2f)); // 머그잔은 윗면이 점점 진해짐
            ceramicTop.color = color;
        }

        void ShowCeramic(Cup cup, List<Layer> stack)
        {
            if (stack.Count == 0) { ceramicTop.enabled = false; return; }
            var color = Mix(stack, shotColorStrength);
            color.a = Mathf.Max(color.a, ceramicMinAlpha);
            ceramicTop.color = color;
            var contents = cup.Contents;
            foam.enabled = !cup.Blended && contents.Get(RecipeKeys.Shot) > 0 && (contents.Get("milk") > 0 || contents.Get(RecipeKeys.SteamedMilk) > 0);
        }

        // 무게 × 불투명도로 색을 섞는다 (물처럼 투명한 재료는 색에 적게 들어감)
        static Color Mix(List<Layer> stack, float shotStrength)
        {
            Color sum = Color.clear;
            float total = 0;
            foreach (var layer in stack)
            {
                float w = layer.Weight * (layer.Key == RecipeKeys.Shot ? shotStrength : 1) * Mathf.Max(0.05f, layer.Color.a);
                sum += layer.Color * w;
                total += w;
            }
            var mixed = total > 0 ? sum / total : Color.clear;
            float alpha = 0;
            foreach (var layer in stack) alpha = Mathf.Max(alpha, layer.Color.a);
            mixed.a = alpha;
            return mixed;
        }

        Color ColorOf(string key)
        {
            int i = colorKeys != null ? System.Array.IndexOf(colorKeys, key) : -1;
            return i >= 0 && i < colors.Length ? colors[i] : fallbackColor;
        }

        float WidthAt(float y)
        {
            float t = Mathf.InverseLerp(plasticBottomY, plasticTopY, y);
            return Mathf.Lerp(plasticBottomWidth, plasticTopWidth, t);
        }

        // 캔버스 좌표 (가운데 x, 위 y, 너비, 높이) → 위치. Liquid 는 이 오브젝트와 같은 크기라 좌표가 같다
        static void Place(RectTransform rt, float centerX, float topY, float width, float height, float scale)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0.5f, 1);
            rt.sizeDelta = new Vector2(width, height) * scale;
            rt.anchoredPosition = new Vector2(centerX * scale, -topY * scale);
        }

        Image Pooled(List<Image> pool, int index, string name, Sprite sprite, Image.Type type)
        {
            while (pool.Count <= index)
            {
                var image = Child($"{name}{pool.Count + 1}", plasticRoot, sprite);
                image.type = type;
                image.pixelsPerUnitMultiplier = 2; // 타일을 캔버스 크기의 절반(UI 크기)으로
                pool.Add(image);
            }
            pool[index].enabled = true;
            return pool[index];
        }

        static void Hide(List<Image> pool, int shown)
        {
            for (int i = shown; i < pool.Count; i++) pool[i].enabled = false;
        }

        static Image Child(string name, Transform parent, Sprite sprite)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var image = go.AddComponent<Image>();
            image.sprite = sprite;
            image.raycastTarget = false;
            image.enabled = false;
            return image;
        }

        static Image Stretched(string name, Transform parent, Sprite sprite)
        {
            var image = Child(name, parent, sprite);
            var rt = image.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            image.enabled = true;
            return image;
        }
    }
}
