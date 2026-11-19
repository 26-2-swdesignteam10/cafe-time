using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 우유 스티밍 창 (ADR-0046): 음료 제조실에서 연다. 우유팩을 꾹 눌러 피처에 붓고, 스팀 완드를 꾹 눌러 데우고,
    // 피처를 꾹 눌러 저울 위 잔에 붓는다. 온도계에 적정 구간이 있고, 벗어난 우유를 넣은 잔은 판정 때 등급이 한 단계 내려간다.
    // 피처 상태는 DrinkStation.Pitcher 에 있어 창을 닫아도 유지된다. 좌표는 Art/UI/SteamRoom/Generation.md 배치표
    public class SteamScreen : Popup
    {
        [Header("꾹 누르는 영역")]
        [SerializeField] HoldButton milkHold;
        [SerializeField] HoldButton steamHold;
        [SerializeField] HoldButton pourHold;
        [SerializeField] Button emptyButton;

        [Header("그림")]
        [SerializeField] GameObject carton;        // 세워 둔 우유팩
        [SerializeField] GameObject cartonPour;    // 피처에 붓는 우유팩 (그림 위치가 달라 따로)
        [SerializeField] Image pitcherImage;       // 원위치 피처
        [SerializeField] Sprite pitcherEmpty;
        [SerializeField] Sprite pitcherMilk;
        [SerializeField] GameObject pitcherPour;   // 잔 위로 들어 붓는 피처 (붓는 동안 원위치 피처는 숨김)
        [SerializeField] Image wandImage;
        [SerializeField] Sprite wandOff;
        [SerializeField] Sprite wandOn;
        [SerializeField] GameObject steamFx;       // 스팀 중 증기 (SpriteLoop)
        [SerializeField] GameObject milkStream;    // 우유팩 → 피처
        [SerializeField] GameObject pitcherStream; // 피처 → 잔 (위끝 고정, 잔 종류별 길이)
        [Tooltip("피처→잔 줄기 길이: 도자기 잔, 플라스틱 컵")]
        [SerializeField] float[] pitcherStreamHeights = { 120, 80 };

        [Header("온도계 (세로 채움, 0 ~ 최고 온도)")]
        [SerializeField] Image thermoFill;

        [Header("글자")]
        [SerializeField] TMP_Text pitcherText;
        [SerializeField] TMP_Text temperatureText;
        [SerializeField] TMP_Text weightText;
        [SerializeField] Color rightTemperatureColor = new Color32(0x3E, 0x8E, 0x41, 0xFF);
        [SerializeField] Color wrongTemperatureColor = new Color32(0xB0, 0x3A, 0x2E, 0xFF);

        [Header("잔")]
        [SerializeField] Image cupImage;
        [Tooltip("큰 컵 그림: HOT 빈 잔·물·만드는 중, ICE 빈 잔·물·만드는 중")]
        [SerializeField] Sprite[] cupSprites = new Sprite[6];
        [SerializeField] CupContentsView cupContents;

        static DrinkStation Station => DrinkStation.Instance;

        // 이번 프레임에 누르고 있는 동작 (그림 전환용). LateUpdate 에서 초기화
        bool pouringMilk, steaming, pouringPitcher;

        protected override void Awake()
        {
            base.Awake();
            milkHold.Holding = dt => pouringMilk = Station.PourMilkToPitcher(dt);
            steamHold.Holding = dt => steaming = Station.Steam(dt);
            pourHold.Holding = dt => pouringPitcher = Station.PourPitcher(dt);
            emptyButton.onClick.AddListener(Station.EmptyPitcher);
            Station.Changed += Refresh;
        }

        void OnDestroy()
        {
            if (Station != null) Station.Changed -= Refresh;
        }

        public override void Open()
        {
            base.Open();
            Refresh();
            ShowActions();
        }

        void LateUpdate()
        {
            ShowActions();
            pouringMilk = steaming = pouringPitcher = false;
        }

        void ShowActions()
        {
            var pitcher = Station.Pitcher;
            SetActive(carton, !pouringMilk);
            SetActive(cartonPour, pouringMilk);
            pitcherImage.sprite = pitcher.IsEmpty ? pitcherEmpty : pitcherMilk;
            pitcherImage.enabled = !pouringPitcher;
            SetActive(pitcherPour, pouringPitcher);
            wandImage.sprite = steaming ? wandOn : wandOff;
            SetActive(steamFx, steaming);
            SetActive(milkStream, pouringMilk);
            SetActive(pitcherStream, pouringPitcher);
        }

        static void SetActive(GameObject go, bool on)
        {
            if (go != null && go.activeSelf != on) go.SetActive(on);
        }

        void Refresh()
        {
            if (!gameObject.activeSelf) return;
            var balance = GameBalance.Instance;
            var pitcher = Station.Pitcher;
            pitcherText.text = $"{pitcher.Milk:0}g";
            temperatureText.text = pitcher.IsEmpty ? "-" : $"{pitcher.Temperature:0}°C";
            temperatureText.color = pitcher.RightTemperature(balance) ? rightTemperatureColor : wrongTemperatureColor;
            thermoFill.fillAmount = pitcher.IsEmpty ? 0f : pitcher.Temperature / balance.MaxMilkTemperature;

            var cup = Station.OnScale;
            StationScreen.ShowCup(cupImage, cupContents, cup, cupSprites);
            weightText.text = $"{Station.Weight:0}g";
            if (cup != null && pitcherStream != null)
            {
                var rt = (RectTransform)pitcherStream.transform;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, pitcherStreamHeights[cup.Type == CupType.Ceramic ? 0 : 1]); // 위끝(피처 입구) 고정
            }
        }
    }
}
