using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 쉐이크 제조실 화면 (SFR-052~055, ADR-0029). 원액은 통의 국자를, 우유는 팩을 믹서기로 끌어 넣고, 얼음은 손의 스쿱을 끌어 놓는다.
    // 갈기 버튼을 누르고 있는 동안 믹서기가 흔들리며 갈리고, 붓기 버튼을 누르면 용기가 컵 위로 기울어져 부은 뒤 판정한다
    public class ShakeRoomScreen : StationScreen
    {
        [Header("원액 통: baseIds 순서 (딸기, 망고, 한라봉, 멜론, 쿠키)")]
        [SerializeField] string[] baseIds;
        [SerializeField] PointerDragRelay[] baseJars;
        [SerializeField] GameObject[] jarScoops;   // 통에 꽂힌 국자 (끄는 동안 숨김)
        [SerializeField] Sprite[] scoopSprites;    // 마우스를 따라오는 원액 국자
        [SerializeField] Vector2 scoopSize;
        [SerializeField] Vector2 scoopPivot;       // 잡는 점 (정규화)
        [SerializeField] IngredientTooltip tooltip;

        [Header("우유 팩")]
        [SerializeField] PointerDragRelay milkCarton;  // 팩 몸통 잡는 영역
        [SerializeField] Image milkImage;              // 선반 위 팩 그림 (끄는 동안 숨김)
        [SerializeField] Sprite milkIdle;
        [SerializeField] Vector2 milkSize;
        [SerializeField] Vector2 milkPivot;
        [SerializeField] Sprite milkPour;
        [Tooltip("믹서기 위에서 기울인 팩 위치 (창 왼쪽 위 기준 x, y, w, h)")]
        [SerializeField] Rect milkPourRect;
        [SerializeField] RectTransform milkStream;
        [Tooltip("우유 줄기 길이 맞추기 (창 y): 출구, 내용물 위끝(최대 눈금), 내용물 높이. 줄기 끝은 수면보다 조금 아래")]
        [SerializeField] Vector3 milkStreamFit = new Vector3(221, 313, 237);

        [Header("끌기 공용")]
        [SerializeField] Image dragImage;          // 마우스를 따라오는 국자·우유 팩 (창 왼쪽 위 기준)
        [Tooltip("국자를 놓거나 우유를 붓는 판정 영역 (용기 + 그 위)")]
        [SerializeField] RectTransform blenderArea;

        [Header("믹서기")]
        [SerializeField] DropZone blenderZone;     // 손의 얼음 스쿱을 놓는 곳
        [SerializeField] RectTransform shaker;     // 본체 + 용기 (갈 때 흔들림)
        [SerializeField] RectTransform jar;        // 용기 (붓기 때 기울어짐)
        [SerializeField] Image chunkyImage;        // 갈기 전 내용물 (Filled Vertical)
        [SerializeField] Image smoothImage;        // 다 간 내용물 (Filled Vertical, 가는 동안 점점 진해짐)
        [Tooltip("baseIds 순서 + 마지막 칸은 원액 없음(우유·얼음만)")]
        [SerializeField] Sprite[] chunkySprites;
        [SerializeField] Sprite[] smoothSprites;
        [SerializeField] Color[] flavorColors;     // 붓는 줄기 색
        [Tooltip("용기 최대 눈금까지 찼을 때의 무게(g)")]
        [SerializeField] float capacity = 450f;
        [SerializeField] GameObject lid;
        [SerializeField] Image whirl;              // 내용물과 같은 영역, 같은 높이로 자름
        [SerializeField] TMP_Text weightText;
        [SerializeField] HoldButton blendHold;
        [SerializeField] Image blendProgress;
        [SerializeField] float shakePixels = 4f;
        [SerializeField] float shakeDegrees = 1.5f;

        [Header("붓기")]
        [SerializeField] Button pourButton;
        [SerializeField] Vector2 pourPosition;     // 붓는 자세의 용기 anchoredPosition
        [SerializeField] float pourAngle;
        [SerializeField] Image pourStream;
        [Tooltip("붓는 줄기 길이: 도자기 잔, 플라스틱 컵 (입구 높이가 다름)")]
        [SerializeField] float[] pourStreamHeights = { 228, 180 };
        [SerializeField] float pourSeconds = 1.2f;

        [Header("컵 자리")]
        [SerializeField] DropZone cupSpot;
        [SerializeField] Image cupImage;
        [Tooltip("큰 컵 그림: HOT 빈 잔·물·만드는 중, ICE 빈 잔·물·만드는 중")]
        [SerializeField] Sprite[] cupSprites = new Sprite[6];
        [SerializeField] JudgeResultView result;

        static Blender Mixer => Blender.Instance;

        protected override IWorkStatus Work => Mixer;

        IngredientDefinition[] bases;
        IngredientDefinition milk;
        int draggingBase = -1;      // 끄는 중인 원액 통 (-1 = 없음)
        bool draggingMilk;
        bool milkBlocked;           // 이번 끌기에서 붓기 실패 (재고 없음 등) → 안내를 반복하지 않음
        Vector2 pointer;            // 마지막 마우스 위치 (화면 좌표)
        Camera pointerCamera;
        int lastBlendFrame = -10;
        float fill;                 // 보이는 내용물 높이 0~1
        float pourTime = -1;        // 붓는 연출 진행 시간 (-1 = 안 함)
        Vector2 jarRestPosition;
        Vector2 shakerRestPosition;
        int pourFlavor;

        bool Blending => Time.frameCount - lastBlendFrame <= 1;

        protected override void Awake()
        {
            base.Awake();
            Mixer.Changed += RefreshIfOpen;
            Mixer.Judged += result.Show;
            jarRestPosition = jar.anchoredPosition;
            shakerRestPosition = shaker.anchoredPosition;

            var balance = GameBalance.Instance;
            bases = new IngredientDefinition[baseIds.Length];
            for (int i = 0; i < baseIds.Length; i++)
            {
                int index = i;
                bases[i] = balance.FindIngredient(baseIds[i]);
                var grab = baseJars[i];
                grab.Entered += () => { if (!IsDragging) tooltip.Show(grab.transform, () => IngredientSlotView.Describe(bases[index])); };
                grab.Exited += tooltip.Hide;
                grab.Began += e => BeginScoop(index, e);
                grab.Dragging += Drag;
                grab.Ended += EndScoop;
            }
            milk = balance.FindIngredient("milk");
            milkCarton.Entered += () => { if (!IsDragging) tooltip.Show(milkCarton.transform, () => IngredientSlotView.Describe(milk)); };
            milkCarton.Exited += tooltip.Hide;
            milkCarton.Began += BeginMilk;
            milkCarton.Dragging += Drag;
            milkCarton.Ended += EndMilk;
            tooltip.Hide();

            blenderZone.Dropped += e =>
            {
                int hand = DropZone.HandOf(e);
                if (hand >= 0 && pourTime < 0) Mixer.AddIceFromHand(hand);
            };
            blendHold.Holding = seconds =>
            {
                if (pourTime >= 0 || IsDragging || !Mixer.BlendHold(seconds)) return false;
                lastBlendFrame = Time.frameCount;
                return true;
            };
            BindSpot(cupSpot, Mixer.PlaceCup, Mixer.TakeCup, () => Mixer.CupSpot != null);
            pourButton.onClick.AddListener(StartPour);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Mixer == null) return;
            Mixer.Changed -= RefreshIfOpen;
            Mixer.Judged -= result.Show;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (pourTime >= 0 && Mixer != null) FinishPour(); // 붓는 도중 닫으면 바로 붓기
            CancelDrag();
        }

        protected override void Refresh()
        {
            weightText.text = $"{Mixer.Weight:0}g";
            int flavor = Flavor();
            chunkyImage.sprite = chunkySprites[flavor];
            smoothImage.sprite = smoothSprites[flavor];
            ShowIcon(cupImage, BigCupSprite(Mixer.CupSpot, cupSprites));
        }

        // 가장 많이 넣은 원액의 그림·색 (원액이 없으면 마지막 칸)
        int Flavor()
        {
            int best = baseIds.Length;
            float most = 0;
            for (int i = 0; i < baseIds.Length; i++)
            {
                float amount = Mixer.Jar.Get(baseIds[i]);
                if (amount > most) { most = amount; best = i; }
            }
            return best;
        }

        void Update()
        {
            ShowProgress(blendProgress, Mixer.Progress);
            UpdateMilk();
            UpdatePour();

            // 내용물 높이: 넣을수록 차오르고, 붓는 동안 줄어든다
            float target = pourTime >= 0 ? fill : Mathf.Clamp01(Mixer.Weight / capacity);
            fill = Mathf.MoveTowards(fill, target, Time.deltaTime * 1.5f);
            chunkyImage.fillAmount = smoothImage.fillAmount = whirl.fillAmount = fill;
            chunkyImage.enabled = smoothImage.enabled = fill > 0.001f;
            float progress = Mixer.Progress;
            var c = smoothImage.color;
            c.a = Mixer.State == BlenderState.Blended ? 1f : progress > 0 ? progress : 0f;
            smoothImage.color = c;

            // 가는 동안: 뚜껑, 소용돌이, 흔들림
            bool blending = Blending;
            if (lid.activeSelf != blending) lid.SetActive(blending);
            if (whirl.gameObject.activeSelf != blending) whirl.gameObject.SetActive(blending);
            shaker.anchoredPosition = shakerRestPosition + (blending ? Random.insideUnitCircle * shakePixels : Vector2.zero);
            shaker.localEulerAngles = new Vector3(0, 0, blending ? Random.Range(-shakeDegrees, shakeDegrees) : 0);
        }

        // ── 원액 국자 ──

        bool IsDragging => draggingBase >= 0 || draggingMilk;

        bool CanDrag => pourTime < 0 && !IsDragging;

        void BeginScoop(int index, PointerEventData e)
        {
            if (!CanDrag) return;
            draggingBase = index;
            jarScoops[index].SetActive(false);
            tooltip.Hide();
            ShowDragImage(scoopSprites[index], scoopSize, scoopPivot);
            Drag(e);
        }

        void EndScoop(PointerEventData e)
        {
            if (draggingBase < 0) return;
            var ingredient = bases[draggingBase];
            bool over = OverBlender(e.position, e.pressEventCamera);
            CancelDrag();
            if (over) Mixer.AddBase(ingredient); // 1회분 (갈기 시작 뒤에는 Blender 가 막음)
        }

        // ── 우유 팩: 믹서기 위에 있는 동안 붓기 ──

        void BeginMilk(PointerEventData e)
        {
            if (!CanDrag) return;
            draggingMilk = true;
            milkBlocked = false;
            milkImage.enabled = false;
            tooltip.Hide();
            ShowDragImage(milkIdle, milkSize, milkPivot);
            Drag(e);
        }

        void EndMilk(PointerEventData e)
        {
            if (draggingMilk) CancelDrag();
        }

        void UpdateMilk()
        {
            if (!draggingMilk) return;
            bool pouring = !milkBlocked && OverBlender(pointer, pointerCamera);
            if (pouring && !Mixer.Pour(milk, Time.deltaTime))
            {
                pouring = false;
                milkBlocked = true;
            }
            if (milkStream.gameObject.activeSelf != pouring) milkStream.gameObject.SetActive(pouring);
            if (pouring)
            {
                // 줄기 끝을 지금 수면 조금 아래에 (출구는 위끝 고정)
                float surface = milkStreamFit.y + milkStreamFit.z * (1f - fill);
                milkStream.sizeDelta = new Vector2(milkStream.sizeDelta.x, surface + 13f - milkStreamFit.x);
                dragImage.sprite = milkPour;
                dragImage.rectTransform.pivot = new Vector2(0, 1);
                dragImage.rectTransform.sizeDelta = milkPourRect.size;
                dragImage.rectTransform.anchoredPosition = new Vector2(milkPourRect.x, -milkPourRect.y);
            }
            else
            {
                dragImage.sprite = milkIdle;
                dragImage.rectTransform.pivot = milkPivot;
                dragImage.rectTransform.sizeDelta = milkSize;
                FollowPointer();
            }
        }

        // ── 끌기 공용 ──

        void ShowDragImage(Sprite sprite, Vector2 size, Vector2 pivot)
        {
            dragImage.sprite = sprite;
            dragImage.rectTransform.sizeDelta = size;
            dragImage.rectTransform.pivot = pivot;
            dragImage.rectTransform.SetAsLastSibling();
            dragImage.enabled = true;
        }

        void Drag(PointerEventData e)
        {
            if (!IsDragging) return;
            pointer = e.position;
            pointerCamera = e.pressEventCamera;
            if (draggingBase >= 0) FollowPointer(); // 우유 팩은 UpdateMilk 가 붓는 자세와 함께 정한다
        }

        void FollowPointer()
        {
            var parent = (RectTransform)dragImage.rectTransform.parent;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, pointer, pointerCamera, out var local)) return;
            var rect = parent.rect;
            dragImage.rectTransform.anchoredPosition = local - new Vector2(rect.xMin, rect.yMax); // 창 왼쪽 위 기준
        }

        bool OverBlender(Vector2 screen, Camera cam) => RectTransformUtility.RectangleContainsScreenPoint(blenderArea, screen, cam);

        void CancelDrag()
        {
            if (draggingBase >= 0) jarScoops[draggingBase].SetActive(true);
            draggingBase = -1;
            draggingMilk = false;
            milkImage.enabled = true;
            milkStream.gameObject.SetActive(false);
            dragImage.enabled = false;
        }

        // ── 붓기 연출 (SFR-055) ──

        void StartPour()
        {
            if (pourTime >= 0 || IsDragging) return;
            if (!Mixer.CanPour) { Mixer.PourOut(); return; } // 잔 없음·덜 갈림 안내
            pourFlavor = Flavor();
            pourStream.color = flavorColors[pourFlavor];
            var size = pourStream.rectTransform.sizeDelta;
            pourStream.rectTransform.sizeDelta = new Vector2(size.x, pourStreamHeights[Mixer.CupSpot.Type == CupType.Ceramic ? 0 : 1]);
            pourTime = 0;
        }

        void UpdatePour()
        {
            if (pourTime < 0) return;
            pourTime += Time.deltaTime;
            float tilt = Mathf.SmoothStep(0, 1, Mathf.Clamp01(pourTime / (pourSeconds * 0.25f)));
            jar.anchoredPosition = Vector2.Lerp(jarRestPosition, pourPosition, tilt);
            jar.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, pourAngle, tilt));
            bool streaming = tilt >= 1f;
            if (pourStream.gameObject.activeSelf != streaming) pourStream.gameObject.SetActive(streaming);
            if (streaming) fill = Mathf.MoveTowards(fill, 0, Time.deltaTime / (pourSeconds * 0.6f));
            if (pourTime >= pourSeconds * 0.85f) FinishPour();
        }

        void FinishPour()
        {
            pourTime = -1;
            pourStream.gameObject.SetActive(false);
            jar.anchoredPosition = jarRestPosition;
            jar.localEulerAngles = Vector3.zero;
            fill = 0;
            Mixer.PourOut(); // 판정 → 결과 표시
        }
    }
}
