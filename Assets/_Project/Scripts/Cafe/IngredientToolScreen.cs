using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 음료 제조실 재료 작업 창 (SFR-046, 047, ADR-0024·0028): 선반 재료를 누르면 열린다. 재료 종류에 따라 도구 그림이 바뀐다.
    // 시럽·연유 = 펌프 누르기, 청 = 숟가락을 컵까지 끌기, 티백 = 상자에서 끌어 물 담긴 컵에 담그기 (1회 투입),
    // 우유·탄산수 = 누르고 있는 동안 붓기. 오른쪽에는 저울 위 잔과 무게를 크게 보여 준다
    public class IngredientToolScreen : Popup
    {
        enum Tool { Pump, Cheong, TeaBag, Milk, Soda }

        [SerializeField] Button toolButton;
        [SerializeField] HoldButton toolHold;
        [SerializeField] Image toolImage;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text weightText;
        [SerializeField] Image cupImage;
        [Tooltip("큰 컵 그림: HOT 빈 잔·물·만드는 중, ICE 빈 잔·물·만드는 중")]
        [SerializeField] Sprite[] cupSprites = new Sprite[6];
        [Tooltip("잔에 티백이 들어 있을 때 겹치는 그림: 도자기 잔, 플라스틱 컵")]
        [SerializeField] Image teaBagImage;
        [SerializeField] Sprite[] teaBagSprites = new Sprite[2];

        [Header("도구 그림: Pump, Cheong, TeaBag, Milk, Soda 순서")]
        [SerializeField] Sprite[] idleSprites = new Sprite[5];
        [SerializeField] Sprite[] activeSprites = new Sprite[5]; // 펌프 눌림 / 숟가락 빠진 청 병 / - / 붓는 중
        [SerializeField] GameObject[] streams = new GameObject[5]; // 도구별 줄기 (티백은 비워 둬도 됨)
        [Tooltip("줄기 길이(표시 px): 출구에서 잔 안까지. 도자기 잔과 플라스틱 컵은 입구 높이가 달라 따로 (Art/UI/DrinkTools/Generation.md)")]
        [SerializeField] float[] ceramicStreamHeights = { 160, 106, 0, 86, 106 };
        [SerializeField] float[] plasticStreamHeights = { 121, 67, 0, 47, 67 };

        [Tooltip("펌프가 눌린 그림과 방울을 보여 주는 시간(초)")]
        [SerializeField] float pressShowSeconds = 0.35f;

        [Header("끌기: 청 숟가락, 티백")]
        [SerializeField] PointerDragRelay cheongGrab;
        [SerializeField] PointerDragRelay teaGrab;
        [SerializeField] Image dragImage;          // 마우스를 따라오는 숟가락·티백 (피벗 = 잡는 점)
        [SerializeField] Sprite spoonFull;
        [SerializeField] Vector2 spoonSize;
        [SerializeField] Vector2 spoonPivot;
        [SerializeField] Sprite teaBag;
        [SerializeField] Vector2 teaBagSize;
        [SerializeField] Vector2 teaBagPivot;

        [Header("청: 컵 위에서 숟가락을 기울여 붓기")]
        [SerializeField] Image spoonPourImage;
        [Tooltip("숟가락 붓기 줄기 위치 (창 왼쪽 위 기준 x, y, w, h): 도자기 잔, 플라스틱 컵")]
        [SerializeField] Rect[] cheongStreamRects = new Rect[2];
        [SerializeField] float spoonPourSeconds = 0.5f;

        [Header("티백 담그기 (1회 재생)")]
        [SerializeField] Image steepImage;
        [SerializeField] Sprite[] steepCeramic;
        [SerializeField] Sprite[] steepPlastic;
        [SerializeField] float steepFramesPerSecond = 8f;

        IngredientDefinition ingredient;
        Tool tool;
        float activeTimer;   // 펌프 눌림 / 숟가락 붓기 남은 시간
        bool pouring;        // 붓기형: 이번 프레임에 붓는 중
        bool dragging;
        bool steeping;
        float steepTime;

        static DrinkStation Station => DrinkStation.Instance;
        bool IsPour => tool == Tool.Milk || tool == Tool.Soda;
        bool IsDrag => tool == Tool.Cheong || tool == Tool.TeaBag;

        protected override void Awake()
        {
            base.Awake();
            toolButton.onClick.AddListener(Use);
            toolHold.Holding = Pour;
            foreach (var grab in new[] { cheongGrab, teaGrab })
            {
                grab.Began += BeginDrag;
                grab.Dragging += Drag;
                grab.Ended += EndDrag;
            }
            Station.Changed += Refresh;
        }

        void OnDestroy()
        {
            if (Station != null) Station.Changed -= Refresh;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dragging = false;
            steeping = false;
            dragImage.enabled = false;
            steepImage.enabled = false;
        }

        public void Open(IngredientDefinition target)
        {
            ingredient = target;
            tool = ToolOf(target);
            nameText.text = target.DisplayName;
            activeTimer = 0;
            pouring = false;
            Open(); // 처음 열 때 Awake 가 돌아야 하므로 먼저 켠다
            toolHold.enabled = IsPour;
            cheongGrab.gameObject.SetActive(tool == Tool.Cheong);
            teaGrab.gameObject.SetActive(tool == Tool.TeaBag);
            ShowTool();
            Refresh();
        }

        static Tool ToolOf(IngredientDefinition i)
        {
            if (i.Id == "milk") return Tool.Milk;
            if (i.Id == "soda") return Tool.Soda;
            if (i.Category == IngredientCategory.Tea) return Tool.TeaBag;
            if (i.Id.StartsWith("cheong_")) return Tool.Cheong;
            return Tool.Pump; // 시럽, 연유
        }

        // 펌프 클릭 1회 투입 (SFR-046)
        void Use()
        {
            if (IsPour || IsDrag) return;
            if (TryAdd()) activeTimer = pressShowSeconds;
        }

        bool TryAdd()
        {
            float before = Count();
            Station.AddIngredient(ingredient);
            return Count() > before; // 재료 부족 등으로 들어가지 않으면 false (안내는 DrinkStation 이 띄움)
        }

        // 누르고 있는 동안 붓기 (SFR-047)
        bool Pour(float seconds)
        {
            if (!IsPour) return false;
            pouring = Station.Pour(ingredient, seconds);
            return pouring;
        }

        float Count() => Station.OnScale != null ? Station.OnScale.Contents.Get(ingredient.Id) : 0f;

        // ── 끌기 (청 숟가락, 티백) ──

        void BeginDrag(PointerEventData e)
        {
            if (activeTimer > 0 || steeping) return; // 앞 동작이 끝난 뒤에
            dragging = true;
            bool spoon = tool == Tool.Cheong;
            dragImage.sprite = spoon ? spoonFull : teaBag;
            dragImage.rectTransform.sizeDelta = spoon ? spoonSize : teaBagSize;
            dragImage.rectTransform.pivot = spoon ? spoonPivot : teaBagPivot;
            dragImage.rectTransform.SetAsLastSibling();
            dragImage.enabled = true;
            Drag(e);
        }

        void Drag(PointerEventData e)
        {
            if (!dragging) return;
            var parent = (RectTransform)dragImage.rectTransform.parent;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, e.position, e.pressEventCamera, out var local))
                dragImage.rectTransform.anchoredPosition = local;
        }

        void EndDrag(PointerEventData e)
        {
            if (!dragging) return;
            dragging = false;
            dragImage.enabled = false;
            if (!RectTransformUtility.RectangleContainsScreenPoint(cupImage.rectTransform, e.position, e.pressEventCamera)) return; // 컵 밖: 그냥 돌아감

            if (tool == Tool.Cheong)
            {
                if (TryAdd()) activeTimer = spoonPourSeconds;
                return;
            }
            // 티백은 물이 담긴 컵에만 담근다
            var cup = Station.OnScale;
            if (cup.Contents.Get(RecipeKeys.HotWater) + cup.Contents.Get(RecipeKeys.ColdWater) <= 0)
            {
                Toast.Show(ToastMessage.WaterFirst);
                return;
            }
            steeping = true; // TryAdd 가 부르는 Refresh 에서 컵을 맑은 물 그림으로 두도록 먼저 켠다
            steepTime = 0;
            if (!TryAdd()) steeping = false;
            Refresh();
        }

        void Update()
        {
            activeTimer -= Time.deltaTime;
            if (steeping)
            {
                var frames = Station.OnScale != null && Station.OnScale.Type == CupType.Plastic ? steepPlastic : steepCeramic;
                steepTime += Time.deltaTime;
                int frame = (int)(steepTime * steepFramesPerSecond);
                if (frame >= frames.Length)
                {
                    steeping = false;
                    Refresh(); // 우러난 컵 + 티백 덧그림으로
                }
                else steepImage.sprite = frames[frame];
            }
            steepImage.enabled = steeping;
            ShowTool();
            pouring = false; // HoldButton 이 매 프레임 다시 켠다
        }

        void ShowTool()
        {
            int t = (int)tool;
            bool timed = activeTimer > 0;
            // 도구 그림: 펌프 눌림, 숟가락이 빠진 청 병(끄는 중·붓는 중), 기울인 우유·탄산수
            bool active = tool switch
            {
                Tool.Pump => timed,
                Tool.Cheong => dragging || timed,
                Tool.Milk or Tool.Soda => pouring,
                _ => false,
            };
            var sprite = active && activeSprites[t] != null ? activeSprites[t] : idleSprites[t];
            if (toolImage.sprite != sprite) toolImage.sprite = sprite;
            bool stream = IsPour ? pouring : timed;
            for (int i = 0; i < streams.Length; i++)
                if (streams[i] != null && streams[i].activeSelf != (stream && i == t)) streams[i].SetActive(stream && i == t);
            spoonPourImage.enabled = tool == Tool.Cheong && timed;
        }

        void Refresh()
        {
            if (!gameObject.activeSelf) return;
            var cup = Station.OnScale;
            if (cup == null) { Close(); return; } // 잔이 없으면 작업할 수 없음
            int type = cup.Type == CupType.Ceramic ? 0 : 1;
            // 담그는 동안은 맑은 물 그림 위에 담그기 덧그림을 보여 주고, 끝나면 우러난 컵 + 티백 덧그림
            cupImage.sprite = steeping ? cupSprites[type * 3 + 1] : StationScreen.BigCupSprite(cup, cupSprites);
            bool hasTeaBag = cup.Contents.HasTeabag(GameBalance.Instance) && !steeping;
            teaBagImage.enabled = hasTeaBag;
            if (hasTeaBag) teaBagImage.sprite = teaBagSprites[type];
            weightText.text = $"{Station.Weight:0}g";
            var heights = type == 0 ? ceramicStreamHeights : plasticStreamHeights;
            for (int i = 0; i < streams.Length; i++)
            {
                if (streams[i] == null) continue;
                var rt = (RectTransform)streams[i].transform;
                if (i == (int)Tool.Cheong)
                {
                    var r = cheongStreamRects[type]; // 숟가락 붓기 위치
                    rt.anchoredPosition = new Vector2(r.x, -r.y);
                    rt.sizeDelta = new Vector2(r.width, r.height);
                }
                else rt.sizeDelta = new Vector2(rt.sizeDelta.x, heights[i]); // 위끝(출구) 고정
            }
        }
    }
}
