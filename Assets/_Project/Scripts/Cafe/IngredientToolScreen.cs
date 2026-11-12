using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 음료 제조실 재료 작업 창 (SFR-046, 047): 선반 재료를 누르면 열린다. 재료 종류에 따라 도구 그림이 바뀐다.
    // 시럽·연유 = 펌프 누르기, 청 = 숟가락으로 뜨기, 티백 = 상자에서 1개 넣기 (클릭 1회 투입),
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
        [SerializeField] Sprite[] activeSprites = new Sprite[5]; // 누른 순간 / 붓는 중 (티백은 비워 둬도 됨)
        [SerializeField] GameObject[] streams = new GameObject[5]; // 도구별 줄기 (티백은 비워 둬도 됨)
        [Tooltip("줄기 길이(표시 px): 출구에서 잔 안까지. 도자기 잔과 플라스틱 컵은 입구 높이가 달라 따로 (Art/UI/DrinkTools/Generation.md)")]
        [SerializeField] float[] ceramicStreamHeights = { 160, 106, 0, 86, 106 };
        [SerializeField] float[] plasticStreamHeights = { 121, 67, 0, 47, 67 };

        [Tooltip("클릭형 도구(펌프·청)가 눌린 그림과 방울을 보여 주는 시간(초)")]
        [SerializeField] float pressShowSeconds = 0.35f;

        IngredientDefinition ingredient;
        Tool tool;
        float activeTimer;   // 클릭형: 눌린 모습 남은 시간
        bool pouring;        // 붓기형: 이번 프레임에 붓는 중

        static DrinkStation Station => DrinkStation.Instance;
        bool IsPour => tool == Tool.Milk || tool == Tool.Soda;

        protected override void Awake()
        {
            base.Awake();
            toolButton.onClick.AddListener(Use);
            toolHold.Holding = Pour;
            Station.Changed += Refresh;
        }

        void OnDestroy()
        {
            if (Station != null) Station.Changed -= Refresh;
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
            ShowTool(false);
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

        // 클릭 1회 투입 (SFR-046)
        void Use()
        {
            if (IsPour) return;
            float before = Count();
            Station.AddIngredient(ingredient);
            if (Count() == before) return; // 재료 부족 등으로 들어가지 않음 (안내는 DrinkStation 이 띄움)
            activeTimer = pressShowSeconds;
        }

        // 누르고 있는 동안 붓기 (SFR-047)
        bool Pour(float seconds)
        {
            if (!IsPour) return false;
            pouring = Station.Pour(ingredient, seconds);
            return pouring;
        }

        float Count() => Station.OnScale != null ? Station.OnScale.Contents.Get(ingredient.Id) : 0f;

        void Update()
        {
            bool active;
            if (IsPour)
            {
                active = pouring;
                pouring = false; // HoldButton 이 매 프레임 다시 켠다
            }
            else
            {
                activeTimer -= Time.deltaTime;
                active = activeTimer > 0;
            }
            ShowTool(active);
        }

        void ShowTool(bool active)
        {
            int t = (int)tool;
            var sprite = active && activeSprites[t] != null ? activeSprites[t] : idleSprites[t];
            if (toolImage.sprite != sprite) toolImage.sprite = sprite;
            for (int i = 0; i < streams.Length; i++)
                if (streams[i] != null && streams[i].activeSelf != (active && i == t)) streams[i].SetActive(active && i == t);
        }

        void Refresh()
        {
            if (!gameObject.activeSelf) return;
            var cup = Station.OnScale;
            if (cup == null) { Close(); return; } // 잔이 없으면 작업할 수 없음
            cupImage.sprite = StationScreen.BigCupSprite(cup, cupSprites);
            bool teaBag = cup.Contents.HasTeabag(GameBalance.Instance);
            teaBagImage.enabled = teaBag;
            if (teaBag) teaBagImage.sprite = teaBagSprites[cup.Type == CupType.Ceramic ? 0 : 1];
            weightText.text = $"{Station.Weight:0}g";
            var heights = cup.Type == CupType.Ceramic ? ceramicStreamHeights : plasticStreamHeights;
            for (int i = 0; i < streams.Length; i++)
            {
                if (streams[i] == null) continue;
                var rt = (RectTransform)streams[i].transform;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, heights[i]); // 위끝(출구) 고정
            }
        }
    }
}
