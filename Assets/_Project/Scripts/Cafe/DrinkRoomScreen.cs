using System.Linq;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 음료 제조실 화면 (SFR-044~051): 재료 선반, 전자저울, 비우기, 완성.
    // 선반 재료를 누르면 재료별 작업 창(시럽 펌프, 청, 티백, 우유·탄산수 붓기)이 열린다 (IngredientToolScreen)
    public class DrinkRoomScreen : StationScreen
    {
        // 시럽·청·티 분류 외에 선반에 두는 재료 (콜드브루·말차·시나몬은 ADR-0038)
        static readonly string[] ExtraIds = { "milk", "soda", "condensed_milk", "cold_brew", "matcha", "cinnamon" };
        static readonly string[] AfterBlueSyrup = { "cold_brew", "matcha", "cinnamon" }; // 블루 시럽 바로 뒤에 둠 (2026-11-18 사용자 요청)

        [SerializeField] Transform shelf;
        [SerializeField] IngredientSlotView slotTemplate;
        [SerializeField] IngredientTooltip tooltip;
        [SerializeField] DropZone scale;
        [SerializeField] Image cupImage;
        [Tooltip("저울 위 큰 컵 그림: HOT 빈 잔·물·만드는 중, ICE 빈 잔·물·만드는 중")]
        [SerializeField] Sprite[] cupSprites = new Sprite[6];
        [SerializeField] CupContentsView cupContents;
        [SerializeField] TMP_Text weightText;
        [SerializeField] Button emptyButton;
        [SerializeField] Button completeButton;
        [SerializeField] JudgeResultView result;
        [SerializeField] IngredientToolScreen toolScreen;
        [SerializeField] Button steamButton;   // 우유 스티밍 창 열기 (ADR-0044)
        [SerializeField] SteamScreen steamScreen;

        static DrinkStation Station => DrinkStation.Instance;

        protected override void Awake()
        {
            base.Awake();
            Station.Changed += RefreshIfOpen;
            Station.Judged += result.Show;

            // 선반 구성 (SFR-044): 우유·탄산수, 연유, 시럽(블루 시럽 뒤에 콜드브루·말차·시나몬), 청, 티백
            slotTemplate.gameObject.SetActive(false);
            var balance = GameBalance.Instance;
            var shelfItems = balance.Ingredients.Where(i => i.Category == IngredientCategory.SyrupCheong || i.Category == IngredientCategory.Tea ||
                                                            ExtraIds.Contains(i.Id)).ToList();
            var moved = shelfItems.Where(i => AfterBlueSyrup.Contains(i.Id)).ToList();
            var items = shelfItems.Except(moved).ToList();
            int blue = items.FindIndex(i => i.Id == "syrup_blue");
            items.InsertRange(blue >= 0 ? blue + 1 : items.Count, moved);
            foreach (var ingredient in items)
                Instantiate(slotTemplate, shelf).Setup(ingredient, OpenTool, null, tooltip.Show, tooltip.Hide);
            tooltip.Hide();

            BindSpot(scale, PlaceOnScale, Station.TakeCup, () => Station.OnScale != null);
            emptyButton.onClick.AddListener(Station.Empty);
            completeButton.onClick.AddListener(Station.Complete);
            steamButton.onClick.AddListener(() => { tooltip.Hide(); steamScreen.Open(); }); // 잔이 없어도 피처에 우유를 붓고 데울 수 있다
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Station == null) return;
            Station.Changed -= RefreshIfOpen;
            Station.Judged -= result.Show;
        }

        // 잔이 저울 위에 있어야 작업 창을 연다
        void OpenTool(IngredientDefinition ingredient)
        {
            if (Station.OnScale == null) { Toast.Show(ToastMessage.CupFirst); return; }
            tooltip.Hide();
            toolScreen.Open(ingredient);
        }

        // 작업 창은 음료 제조실 위에 겹쳐 열리므로, 제조실을 닫으면 같이 닫는다
        public override void Close()
        {
            if (toolScreen != null && toolScreen.gameObject.activeSelf) toolScreen.Close();
            if (steamScreen != null && steamScreen.gameObject.activeSelf) steamScreen.Close();
            base.Close();
        }

        // 커피잔은 저울 위에, 샷 컵·얼음 스쿱은 저울 위 잔에 붓기 (SFR-045, 048)
        static bool PlaceOnScale(int hand) =>
            HandInventory.Instance[hand] is Cup ? Station.PlaceCup(hand) : Station.PourFromHand(hand);

        protected override void Refresh()
        {
            ShowCup(cupImage, cupContents, Station.OnScale, cupSprites);
            weightText.text = $"{Station.Weight:0}g"; // 커피잔 무게는 빼고 내용물 무게만 (영점)
        }
    }
}
