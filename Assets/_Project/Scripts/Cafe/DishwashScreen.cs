using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 설거지실 화면 (SFR-063~065): 싱크대의 잔을 수세미로 좌우로 문질러 얼룩을 지우면 건조대로.
    // 연출: 문지르면 비누 방울·거품이 생기고, 얼룩이 줄 때마다 잔을 뒤집어 닦는다.
    // 다 닦으면 물로 헹군 뒤 반짝이고, 거꾸로 뒤집히며 건조대 빈칸으로 날아간다. 다음 잔은 바구니 쪽에서 내려온다.
    // 연출은 화면에만 있고 설거지 규칙(DishStation)은 그대로다
    public class DishwashScreen : StationScreen
    {
        [SerializeField] TMP_Text waitingText;  // "3개"
        [SerializeField] Image[] crateItems;    // 바구니에 보이는 대기 잔 (앞에서부터)
        [SerializeField] Image sinkItem;        // 깨끗한 잔 그림
        [SerializeField] Image stain;           // 같은 자리에 겹친 더러운 잔 그림. 얼룩이 줄수록 투명해진다
        [SerializeField] Image suds;            // 잔 위 비누 거품. 문지를수록 진해진다
        [SerializeField] DragMotion sponge;
        [SerializeField] Image[] rack;          // 건조대 칸 (잔을 거꾸로 엎어 둔다)
        [SerializeField] GameObject dryingLabel; // "건조 중"

        [Header("연출")]
        [SerializeField] RectTransform fxLayer; // 방울·반짝임을 그리는 층 (싱크 전체)
        [SerializeField] Image bubbleTemplate;  // 복제해서 쓰는 비누 방울
        [SerializeField] Image[] sparkles;
        [SerializeField] GameObject rinse;      // 헹굼 물줄기
        [SerializeField] Image flyer;           // 다 닦은 잔이 건조대로 날아가는 그림
        [SerializeField] Image flyerSuds;
        [Tooltip("얼룩이 이 값 아래로 내려갈 때마다 잔을 한 번 뒤집어 닦는다")]
        [SerializeField] float[] flipAt = { 0.66f, 0.33f };
        [Tooltip("문지른 거리(px)마다 방울이 하나 생긴다")]
        [SerializeField] float bubbleEvery = 26f;
        [SerializeField] int maxBubbles = 40;
        [SerializeField] float flipSeconds = 0.3f, rinseSeconds = 0.45f, flySeconds = 0.55f, dropSeconds = 0.3f;
        [Tooltip("건조대로 날아갈 때 위로 솟는 높이")]
        [SerializeField] float flyArc = 160f;

        static DishStation Dish => DishStation.Instance;

        protected override IWorkStatus Work => Dish;

        readonly List<Image> bubbles = new List<Image>();
        Vector2 itemHome;
        float facing = 1f;          // 잔 그림 좌우 방향 (뒤집을 때마다 반대)
        int flipsWanted, flipsShown;
        Coroutine flipRunner;
        float bubbleDistance, wobble;
        bool finishing, hadItem;
        DishStation.Drying flying; // 날아가는 중인 건조대 잔 (도착할 때까지 칸을 비워 둔다)

        protected override void Awake()
        {
            base.Awake();
            itemHome = sinkItem.rectTransform.anchoredPosition;
            Dish.Changed += RefreshIfOpen;
            sponge.Moved += OnScrub;
            sponge.Released += ResetSponge;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Dish != null) Dish.Changed -= RefreshIfOpen;
        }

        public override void Open()
        {
            base.Open(); // 처음 열 때 Awake 가 여기서 실행된다
            ResetFx();
            Refresh();
        }

        // 닫혀 있던 동안 멈춘 연출을 지우고, 지금 얼룩 상태에 맞는 모습으로
        void ResetFx()
        {
            StopAllCoroutines();
            flipRunner = null;
            finishing = false;
            flying = null;
            flipsWanted = flipsShown = Dish.InSink != null ? flipAt.Count(t => Dish.Stain <= t) : 0;
            facing = flipsShown % 2 == 0 ? 1f : -1f;
            var rt = sinkItem.rectTransform;
            rt.anchoredPosition = itemHome;
            rt.localRotation = Quaternion.identity;
            rt.localScale = new Vector3(facing, 1, 1);
            flyer.gameObject.SetActive(false);
            rinse.SetActive(false);
            foreach (var s in sparkles) s.gameObject.SetActive(false);
            foreach (var b in bubbles) b.gameObject.SetActive(false);
            ResetSponge();
            hadItem = Dish.InSink != null;
        }

        static Sprite Clean(TrayItemKind kind) => ItemIcons.Get(
            kind == TrayItemKind.DirtyCeramic ? ItemSprite.CupCeramic : kind == TrayItemKind.DirtyPlastic ? ItemSprite.CupPlastic : ItemSprite.Plate);

        static Sprite Dirty(TrayItemKind kind) => ItemIcons.Get(
            kind == TrayItemKind.DirtyCeramic ? ItemSprite.DirtyCeramic : kind == TrayItemKind.DirtyPlastic ? ItemSprite.DirtyPlastic : ItemSprite.DirtyPlate);

        // 처음엔 거품이 없고, 얼룩이 40% 줄면 가득
        static float SudsAlpha(float stain) => Mathf.Clamp01((1f - stain) * 2.5f);

        protected override void Refresh()
        {
            waitingText.text = $"{Dish.Waiting.Count}개";
            for (int i = 0; i < crateItems.Length; i++)
                ShowIcon(crateItems[i], i < Dish.Waiting.Count ? Dirty(Dish.Waiting[i]) : null);

            var inSink = Dish.InSink;
            bool show = inSink != null && !finishing;
            ShowIcon(sinkItem, show ? Clean(inSink.Value) : null);
            ShowIcon(stain, show ? Dirty(inSink.Value) : null);
            stain.color = new Color(1, 1, 1, Mathf.Clamp01(Dish.Stain));
            suds.enabled = show;
            suds.color = new Color(1, 1, 1, SudsAlpha(Dish.Stain));
            // 비어 있던 싱크대에 잔이 올라오면 바구니 쪽에서 내려온다
            if (show && !hadItem && isActiveAndEnabled) StartCoroutine(DropIn());
            if (!finishing) hadItem = inSink != null;

            for (int i = 0; i < rack.Length; i++)
            {
                bool filled = i < Dish.Rack.Count && Dish.Rack[i] != flying;
                ShowIcon(rack[i], filled ? Clean(Dish.Rack[i].Kind) : null);
            }
            dryingLabel.SetActive(Dish.Rack.Count > 0);
        }

        // ───────── 문지르기 ─────────

        void OnScrub(Vector2 delta)
        {
            if (Dish.InSink == null || finishing) return; // 빈 싱크대·마무리 연출 중에는 수세미가 작동하지 않음
            var kind = Dish.InSink.Value;
            int before = Dish.Rack.Count;
            float distance = Mathf.Abs(delta.x);
            Dish.Scrub(distance);
            SquishSponge(distance);
            SpawnScrubBubbles(distance);

            if (Dish.Rack.Count > before)
            {
                StartCoroutine(Finish(kind, Dish.Rack[Dish.Rack.Count - 1]));
                return;
            }
            int due = flipAt.Count(t => Dish.Stain <= t);
            if (due > flipsWanted)
            {
                flipsWanted = due;
                if (flipRunner == null) flipRunner = StartCoroutine(RunFlips());
            }
        }

        // 수세미가 문지르는 방향으로 기울고 눌린다
        void SquishSponge(float distance)
        {
            wobble += distance;
            float s = Mathf.Sin(wobble / 35f);
            sponge.transform.localRotation = Quaternion.Euler(0, 0, s * 7f);
            sponge.transform.localScale = new Vector3(1f + 0.06f * Mathf.Abs(s), 1f - 0.08f * Mathf.Abs(s), 1);
        }

        void ResetSponge()
        {
            sponge.transform.localRotation = Quaternion.identity;
            sponge.transform.localScale = Vector3.one;
        }

        void SpawnScrubBubbles(float distance)
        {
            bubbleDistance += distance;
            Vector2 at = fxLayer.InverseTransformPoint(sponge.transform.position);
            while (bubbleDistance >= bubbleEvery)
            {
                bubbleDistance -= bubbleEvery;
                SpawnBubble(at + new Vector2(Random.Range(-60f, 60f), Random.Range(10f, 45f)), Random.Range(14f, 40f));
            }
        }

        void SpawnBubble(Vector2 at, float size)
        {
            var bubble = bubbles.FirstOrDefault(b => !b.gameObject.activeSelf);
            if (bubble == null)
            {
                if (bubbles.Count >= maxBubbles) return;
                bubble = Instantiate(bubbleTemplate, fxLayer);
                bubbles.Add(bubble);
            }
            bubble.gameObject.SetActive(true);
            StartCoroutine(Float(bubble, at, size));
        }

        // 비누 방울: 커지며 흔들흔들 떠오르다가 톡 터진다
        IEnumerator Float(Image bubble, Vector2 from, float size)
        {
            var rt = bubble.rectTransform;
            rt.sizeDelta = new Vector2(size, size);
            float life = Random.Range(0.7f, 1.3f), rise = Random.Range(60f, 150f), phase = Random.Range(0f, 6f);
            for (float t = 0; t < life; t += Time.unscaledDeltaTime)
            {
                float k = t / life;
                rt.anchoredPosition = from + new Vector2(Mathf.Sin(phase + k * 9f) * 10f, rise * k);
                float pop = Mathf.Clamp01((k - 0.85f) / 0.15f);
                rt.localScale = Vector3.one * (Mathf.Lerp(0.4f, 1f, Mathf.Clamp01(k * 4f)) + pop * 0.35f);
                bubble.color = new Color(1, 1, 1, 1f - pop);
                yield return null;
            }
            bubble.gameObject.SetActive(false);
        }

        // ───────── 뒤집기 ─────────

        IEnumerator RunFlips()
        {
            while (flipsShown < flipsWanted)
            {
                flipsShown++;
                yield return Flip();
            }
            flipRunner = null;
        }

        // 잔을 살짝 들어 올려 반대쪽으로 뒤집는다 (좌우 반전 + 기울기)
        IEnumerator Flip()
        {
            var rt = sinkItem.rectTransform;
            float from = facing, to = -facing;
            facing = to;
            for (float t = 0; t < flipSeconds; t += Time.unscaledDeltaTime)
            {
                float k = t / flipSeconds;
                float arc = Mathf.Sin(k * Mathf.PI);
                rt.localScale = new Vector3(Mathf.Lerp(from, to, Mathf.SmoothStep(0, 1, k)), 1f + 0.08f * arc, 1);
                rt.anchoredPosition = itemHome + Vector2.up * (40f * arc);
                rt.localRotation = Quaternion.Euler(0, 0, 14f * arc * from);
                yield return null;
            }
            rt.localScale = new Vector3(facing, 1, 1);
            rt.anchoredPosition = itemHome;
            rt.localRotation = Quaternion.identity;
            Vector2 at = fxLayer.InverseTransformPoint(rt.position);
            for (int i = 0; i < 4; i++) SpawnBubble(at + new Vector2(Random.Range(-70f, 70f), Random.Range(-10f, 40f)), Random.Range(12f, 28f));
        }

        // ───────── 마무리: 헹굼 → 반짝 → 건조대로 ─────────

        IEnumerator Finish(TrayItemKind kind, DishStation.Drying drying)
        {
            if (flipRunner != null) StopCoroutine(flipRunner);
            flipRunner = null;
            finishing = true;
            flying = drying;
            Refresh(); // 싱크대 잔과 도착할 칸을 숨긴다

            var item = sinkItem.rectTransform;
            var fly = flyer.rectTransform;
            var parent = (RectTransform)fly.parent;
            flyer.sprite = Clean(kind);
            flyer.enabled = true;
            fly.sizeDelta = item.sizeDelta;
            fly.localPosition = parent.InverseTransformPoint(item.position);
            fly.localRotation = Quaternion.identity;
            fly.localScale = new Vector3(facing, 1, 1);
            flyerSuds.color = Color.white;
            flyer.gameObject.SetActive(true);
            item.localScale = Vector3.one;
            item.localRotation = Quaternion.identity;
            item.anchoredPosition = itemHome;

            // 1) 헹굼: 물줄기를 맞으며 거품이 사라진다
            rinse.SetActive(true);
            Vector2 itemAt = fxLayer.InverseTransformPoint(item.position);
            for (float t = 0; t < rinseSeconds; t += Time.unscaledDeltaTime)
            {
                flyerSuds.color = new Color(1, 1, 1, 1f - t / rinseSeconds);
                if (Random.value < 0.35f) SpawnBubble(itemAt + new Vector2(Random.Range(-80f, 80f), Random.Range(-20f, 30f)), Random.Range(10f, 24f));
                yield return null;
            }
            rinse.SetActive(false);
            flyerSuds.color = new Color(1, 1, 1, 0);

            // 2) 반짝
            for (int i = 0; i < sparkles.Length; i++) StartCoroutine(Twinkle(sparkles[i], itemAt, i * 0.07f));
            yield return new WaitForSecondsRealtime(0.2f);

            // 3) 거꾸로 뒤집히며 건조대 칸으로 날아간다 (칸의 잔은 거꾸로 엎어 둔 모습)
            int slot = Dish.Rack.IndexOf(drying);
            if (slot >= 0 && slot < rack.Length)
            {
                var target = rack[slot].rectTransform;
                Vector3 from = fly.localPosition, to = parent.InverseTransformPoint(target.position);
                Vector2 sizeFrom = fly.sizeDelta, sizeTo = target.sizeDelta;
                float scaleFrom = fly.localScale.x;
                float angleTo = target.localEulerAngles.z;
                for (float t = 0; t < flySeconds; t += Time.unscaledDeltaTime)
                {
                    float k = Mathf.SmoothStep(0, 1, t / flySeconds);
                    fly.localPosition = Vector3.Lerp(from, to, k) + Vector3.up * (flyArc * Mathf.Sin(k * Mathf.PI));
                    fly.sizeDelta = Vector2.Lerp(sizeFrom, sizeTo, k);
                    fly.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, angleTo, k));
                    fly.localScale = new Vector3(Mathf.Lerp(scaleFrom, 1f, k), 1, 1);
                    yield return null;
                }
            }
            flyer.gameObject.SetActive(false);

            // 4) 다음 잔
            finishing = false;
            flying = null;
            flipsWanted = flipsShown = 0;
            facing = 1f;
            hadItem = false;
            Refresh(); // 다음 잔이 있으면 DropIn 이 시작된다
        }

        IEnumerator Twinkle(Image sparkle, Vector2 center, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            var rt = sparkle.rectTransform;
            rt.anchoredPosition = center + new Vector2(Random.Range(-90f, 90f), Random.Range(-40f, 80f));
            sparkle.gameObject.SetActive(true);
            const float life = 0.45f;
            for (float t = 0; t < life; t += Time.unscaledDeltaTime)
            {
                float k = t / life;
                rt.localScale = Vector3.one * (1.2f * Mathf.Sin(k * Mathf.PI));
                rt.localRotation = Quaternion.Euler(0, 0, 90f * k);
                yield return null;
            }
            sparkle.gameObject.SetActive(false);
        }

        // 다음 잔이 바구니 쪽(왼쪽 위)에서 싱크대로 툭 떨어진다
        IEnumerator DropIn()
        {
            var rt = sinkItem.rectTransform;
            rt.localScale = new Vector3(facing, 1, 1);
            Vector2 from = itemHome + new Vector2(-260f, 160f);
            for (float t = 0; t < dropSeconds; t += Time.unscaledDeltaTime)
            {
                float k = t / dropSeconds;
                rt.anchoredPosition = Vector2.Lerp(from, itemHome, 1f - (1f - k) * (1f - k)) + Vector2.up * (60f * Mathf.Sin(k * Mathf.PI));
                rt.localRotation = Quaternion.Euler(0, 0, 20f * (1f - k));
                yield return null;
            }
            rt.anchoredPosition = itemHome;
            rt.localRotation = Quaternion.identity;
            // 물에 닿으며 살짝 눌렸다가 돌아옴
            for (float t = 0; t < 0.12f; t += Time.unscaledDeltaTime)
            {
                float s = Mathf.Sin(t / 0.12f * Mathf.PI);
                rt.localScale = new Vector3(facing * (1f + 0.08f * s), 1f - 0.1f * s, 1);
                yield return null;
            }
            rt.localScale = new Vector3(facing, 1, 1);
            Vector2 at = fxLayer.InverseTransformPoint(rt.position);
            for (int i = 0; i < 3; i++) SpawnBubble(at + new Vector2(Random.Range(-60f, 60f), Random.Range(-30f, 0f)), Random.Range(10f, 20f));
        }
    }
}
