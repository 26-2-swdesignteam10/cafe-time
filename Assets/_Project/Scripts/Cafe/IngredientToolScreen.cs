using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 음료 제조실 재료 작업 창 (SFR-046, 047, ADR-0024·0028): 선반 재료를 누르면 열린다. 재료 종류에 따라 도구 그림이 바뀐다.
    // 시럽·연유 = 펌프 머리를 아래로 끌어 누르기 (ADR-0035), 청·말차·시나몬 = 숟가락을 컵까지 끌기, 티백 = 상자에서 끌어 물 담긴 컵에 담그기 (1회 투입),
    // 우유·탄산수·콜드브루 = 누르고 있는 동안 붓기. 오른쪽에는 저울 위 잔과 무게를 크게 보여 준다.
    // 그림은 재료마다 전용 그림(IngredientToolArt)이 있으면 그것, 없으면 도구 공용 그림 (ADR-0038)
    public class IngredientToolScreen : Popup
    {
        enum Tool { Pump, Spoon, TeaBag, Milk, Soda, ColdBrew }

        [SerializeField] HoldButton toolHold;
        [SerializeField] Image toolImage;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text weightText;
        [SerializeField] Image cupImage;
        [Tooltip("큰 컵 그림: HOT 빈 잔·물·만드는 중, ICE 빈 잔·물·만드는 중")]
        [SerializeField] Sprite[] cupSprites = new Sprite[6];
        [SerializeField] CupContentsView cupContents;
        [Tooltip("잔에 티백이 들어 있을 때 겹치는 그림: 도자기 잔, 플라스틱 컵")]
        [SerializeField] Image teaBagImage;
        [SerializeField] Sprite[] teaBagSprites = new Sprite[2];

        [Header("도구 공용 그림: Pump, Spoon, TeaBag, Milk, Soda, ColdBrew 순서")]
        [SerializeField] Sprite[] idleSprites = new Sprite[6];
        [SerializeField] Sprite[] activeSprites = new Sprite[6]; // 펌프 눌림 / 숟가락 빠진 병 / - / 붓는 중
        [SerializeField] GameObject[] streams = new GameObject[6]; // 도구별 줄기 (티백은 비워 둬도 됨)
        [Tooltip("숟가락형 가루 재료의 낙하 (위치는 숟가락 줄기와 같음)")]
        [SerializeField] GameObject powderStream;
        [Tooltip("줄기 길이(표시 px): 출구에서 잔 안까지. 도자기 잔과 플라스틱 컵은 입구 높이가 달라 따로 (Art/UI/DrinkTools/Generation.md)")]
        [SerializeField] float[] ceramicStreamHeights = { 160, 106, 0, 86, 106, 106 };
        [SerializeField] float[] plasticStreamHeights = { 121, 67, 0, 47, 67, 67 };

        [Header("재료 전용 그림 (없으면 공용 그림)")]
        [SerializeField] IngredientToolArt[] arts;

        [Tooltip("펌프 방울을 보여 주는 시간(초)")]
        [SerializeField] float pressShowSeconds = 0.35f;

        [Header("펌프: 머리를 아래로 끌어 누르기 (ADR-0035)")]
        [SerializeField] PointerDragRelay pumpGrab;   // 펌프 머리 잡는 영역
        [Tooltip("끝까지 눌렀다고 보는 아래 방향 끌기 거리 (창 좌표 px). 그림의 머리 이동 거리 25px")]
        [SerializeField] float pumpPressDistance = 25f;

        [Header("끌기: 숟가락, 티백")]
        [SerializeField] PointerDragRelay cheongGrab;
        [SerializeField] PointerDragRelay teaGrab;
        [SerializeField] Image dragImage;          // 마우스를 따라오는 숟가락·티백 (피벗 = 잡는 점)
        [SerializeField] Sprite spoonFull;
        [SerializeField] Vector2 spoonSize;
        [SerializeField] Vector2 spoonPivot;
        [Tooltip("숟가락 그림에서 청이 담긴 머리 위치 (정규화). 마우스나 머리가 컵 위에 있으면 넣는다")]
        [SerializeField] Vector2 spoonHead = new Vector2(0.85f, 0.5f);
        [SerializeField] Sprite teaBag;
        [SerializeField] Vector2 teaBagSize;
        [SerializeField] Vector2 teaBagPivot;

        [Header("숟가락: 컵 위에서 기울여 붓기")]
        [SerializeField] Image spoonPourImage;
        [SerializeField] Sprite spoonPour;
        [Tooltip("숟가락 붓기 줄기 위치 (창 왼쪽 위 기준 x, y, w, h): 도자기 잔, 플라스틱 컵")]
        [SerializeField] Rect[] cheongStreamRects = new Rect[2];
        [SerializeField] float spoonPourSeconds = 0.5f;

        [Header("티백 담그기 (1회 재생)")]
        [SerializeField] Image steepImage;
        [SerializeField] Sprite[] steepCeramic;
        [SerializeField] Sprite[] steepPlastic;
        [SerializeField] float steepFramesPerSecond = 8f;

        IngredientDefinition ingredient;
        IngredientToolArt art; // 이 재료의 전용 그림 (없으면 null)
        Tool tool;
        float activeTimer;   // 펌프 눌림 / 숟가락 붓기 남은 시간
        bool pouring;        // 붓기형: 이번 프레임에 붓는 중
        bool dragging;
        bool steeping;
        float steepTime;
        bool pumpHeld;       // 펌프 머리를 잡고 있음
        float pumpStartY;    // 잡은 지점 (창 좌표)
        float pumpProgress;  // 0 = 올라옴, 1 = 끝까지 눌림
        bool pumped;         // 이번 누르기에서 이미 1펌프 넣음 (놓고 다시 눌러야 다음 펌프)

        static DrinkStation Station => DrinkStation.Instance;
        bool IsPour => tool == Tool.Milk || tool == Tool.Soda || tool == Tool.ColdBrew;
        bool IsPowder => art != null && art.powder;

        protected override void Awake()
        {
            base.Awake();
            toolHold.Holding = Pour;
            pumpGrab.Began += BeginPump;
            pumpGrab.Dragging += DragPump;
            pumpGrab.Ended += EndPump;
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
            if (AudioManager.Instance != null)
                AudioManager.Instance.SetWaterPouring(false, this);
            dragging = false;
            steeping = false;
            pumpHeld = false;
            dragImage.enabled = false;
            steepImage.enabled = false;
        }

        public void Open(IngredientDefinition target)
        {
            ingredient = target;
            art = System.Array.Find(arts ?? new IngredientToolArt[0], a => a.ingredientId == target.Id);
            tool = ToolOf(target);
            nameText.text = target.DisplayName;
            activeTimer = 0;
            pouring = false;
            Open(); // 처음 열 때 Awake 가 돌아야 하므로 먼저 켠다
            toolHold.enabled = IsPour;
            pumpGrab.gameObject.SetActive(tool == Tool.Pump);
            cheongGrab.gameObject.SetActive(tool == Tool.Spoon);
            spoonPourImage.sprite = art != null && art.spoonPour != null ? art.spoonPour : spoonPour;
            foreach (var stream in streams) TintStream(stream);
            TintStream(powderStream);
            teaGrab.gameObject.SetActive(tool == Tool.TeaBag);
            ShowTool();
            Refresh();
        }

        Tool ToolOf(IngredientDefinition i)
        {
            if (i.Id == "milk") return Tool.Milk;
            if (i.Id == "soda") return Tool.Soda;
            if (i.Id == "cold_brew") return Tool.ColdBrew;
            if (i.Category == IngredientCategory.Tea) return Tool.TeaBag;
            if (i.Id.StartsWith("cheong_") || IsPowder) return Tool.Spoon;
            return Tool.Pump; // 시럽, 연유
        }

        // 펌프 방울·숟가락 낙하는 중립색 그림에 재료 색을 곱한다. 우유·탄산수 줄기는 원래 색
        void TintStream(GameObject stream)
        {
            if (stream == null) return;
            bool tinted = stream == powderStream || stream == streams[(int)Tool.Pump] || stream == streams[(int)Tool.Spoon];
            stream.GetComponent<Image>().color = tinted && art != null ? art.streamColor : Color.white;
        }

        // ── 펌프: 머리를 아래로 끝까지 끌면 1펌프 투입 (SFR-046, ADR-0035) ──

        void BeginPump(PointerEventData e)
        {
            if (!PumpLocalY(e, out pumpStartY)) return;
            pumpHeld = true;
            pumped = false;
            pumpProgress = 0;
        }

        void DragPump(PointerEventData e)
        {
            if (!pumpHeld || !PumpLocalY(e, out float y)) return;
            pumpProgress = Mathf.Clamp01((pumpStartY - y) / pumpPressDistance); // 창 좌표는 위가 +, 아래로 끌수록 커짐
            if (pumpProgress >= 1f && !pumped)
            {
                pumped = true;
                if (TryAdd()) activeTimer = pressShowSeconds;
            }
        }

        void EndPump(PointerEventData e) => pumpHeld = false; // 놓으면 머리가 올라온다

        bool PumpLocalY(PointerEventData e, out float y)
        {
            bool ok = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)toolImage.rectTransform.parent, e.position, e.pressEventCamera, out var local);
            y = local.y;
            return ok;
        }

        bool TryAdd()
        {
            float before = Count();
            Station.AddIngredient(ingredient);

            bool added = Count() > before;

            // SFR-046: 재료가 실제로 들어간 경우에만 효과음 재생
            if (added && AudioManager.Instance != null)
            {
                if (tool == Tool.Pump)
                {
                    AudioManager.Instance.PlayPump();
                }
                else if (ingredient.Id.StartsWith("cheong_"))
                {
                    AudioManager.Instance.PlayCheong();
                }
            }

            return added;
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
            bool spoon = tool == Tool.Spoon;
            dragImage.sprite = spoon ? art != null && art.spoon != null ? art.spoon : spoonFull : teaBag;
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
            bool overCup = tool == Tool.Spoon
                ? PointerDragRelay.Over(cupImage.rectTransform, e, dragImage.rectTransform, spoonHead) // 손잡이 끝(마우스) 또는 숟가락 머리
                : RectTransformUtility.RectangleContainsScreenPoint(cupImage.rectTransform, e.position, e.pressEventCamera);
            if (!overCup) return; // 컵 밖: 그냥 돌아감

            if (tool == Tool.Spoon)
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

        // 숟가락 붓기 줄기 위치 (도자기 잔, 플라스틱 컵)
        void PlaceSpoonStream(RectTransform rt, int type)
        {
            var r = cheongStreamRects[type];
            rt.anchoredPosition = new Vector2(r.x, -r.y);
            rt.sizeDelta = new Vector2(r.width, r.height);
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

            if (AudioManager.Instance != null)
                AudioManager.Instance.SetWaterPouring(
                    IsPour && pouring,
                    this
                );

            pouring = false; // HoldButton 이 매 프레임 다시 켠다
        }

        void ShowTool()
        {
            int t = (int)tool;
            bool timed = activeTimer > 0;
            // 도구 그림: 펌프 눌림, 숟가락이 빠진 병(끄는 중·붓는 중), 기울인 우유·탄산수·콜드브루
            bool active = tool switch
            {
                Tool.Pump => pumpHeld && pumpProgress >= 0.5f, // 절반 넘게 끌면 눌린 그림
                Tool.Spoon => dragging || timed,
                Tool.Milk or Tool.Soda or Tool.ColdBrew => pouring,
                _ => false,
            };
            var idle = art != null && art.idle != null ? art.idle : idleSprites[t];
            var pressed = art != null && art.active != null ? art.active : activeSprites[t];
            var sprite = active && pressed != null ? pressed : idle;
            if (toolImage.sprite != sprite) toolImage.sprite = sprite;
            bool stream = IsPour ? pouring : timed;
            bool powderFalls = stream && IsPowder;
            for (int i = 0; i < streams.Length; i++)
            {
                bool on = stream && !powderFalls && i == t;
                if (streams[i] != null && streams[i].activeSelf != on) streams[i].SetActive(on);
            }
            if (powderStream != null && powderStream.activeSelf != powderFalls) powderStream.SetActive(powderFalls);
            spoonPourImage.enabled = tool == Tool.Spoon && timed;
        }

        void Refresh()
        {
            if (!gameObject.activeSelf) return;
            var cup = Station.OnScale;
            if (cup == null) { Close(); return; } // 잔이 없으면 작업할 수 없음
            int type = cup.Type == CupType.Ceramic ? 0 : 1;
            // 담그는 동안은 맑은 물 그림 위에 담그기 덧그림을 보여 주고, 끝나면 우러난 컵 + 티백 덧그림
            if (cupContents != null) StationScreen.ShowCup(cupImage, cupContents, cup, cupSprites, steeping);
            else cupImage.sprite = steeping ? cupSprites[type * 3 + 1] : StationScreen.BigCupSprite(cup, cupSprites);
            bool hasTeaBag = cup.Contents.HasTeabag(GameBalance.Instance) && !steeping && cupContents == null; // 내용물 층이 있으면 그쪽이 티백을 그린다
            teaBagImage.enabled = hasTeaBag;
            if (hasTeaBag) teaBagImage.sprite = teaBagSprites[type];
            weightText.text = $"{Station.Weight:0}g";
            var heights = type == 0 ? ceramicStreamHeights : plasticStreamHeights;
            for (int i = 0; i < streams.Length; i++)
            {
                if (streams[i] == null) continue;
                var rt = (RectTransform)streams[i].transform;
                if (i == (int)Tool.Spoon) PlaceSpoonStream(rt, type);
                else rt.sizeDelta = new Vector2(rt.sizeDelta.x, heights[i]); // 위끝(출구) 고정
            }
            if (powderStream != null) PlaceSpoonStream((RectTransform)powderStream.transform, type);
        }
    }
}
