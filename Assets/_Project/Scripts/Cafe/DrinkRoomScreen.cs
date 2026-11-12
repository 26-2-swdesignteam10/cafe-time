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
        static readonly string[] PourIds = { "milk", "soda" };

        [SerializeField] Transform shelf;
        [SerializeField] IngredientSlotView slotTemplate;
        [SerializeField] IngredientTooltip tooltip;
        [SerializeField] DropZone scale;
        [SerializeField] Image cupImage;
        [Tooltip("저울 위 큰 컵 그림: HOT 빈 잔·물·만드는 중, ICE 빈 잔·물·만드는 중")]
        [SerializeField] Sprite[] cupSprites = new Sprite[6];
        [SerializeField] TMP_Text weightText;
        [SerializeField] Button emptyButton;
        [SerializeField] Button completeButton;
        [SerializeField] JudgeResultView result;
        [SerializeField] IngredientToolScreen toolScreen;

        static DrinkStation Station => DrinkStation.Instance;

        protected override void Awake()
        {
            base.Awake();
            Station.Changed += RefreshIfOpen;
            Station.Judged += result.Show;

            // 선반 구성 (SFR-044): 시럽, 청, 연유·티백, 우유·탄산수
            slotTemplate.gameObject.SetActive(false);
            var balance = GameBalance.Instance;
            var items = balance.Ingredients.Where(i => i.Category == IngredientCategory.SyrupCheong || i.Category == IngredientCategory.Tea ||
                                                       i.Id == "condensed_milk" || PourIds.Contains(i.Id));
            foreach (var ingredient in items)
                Instantiate(slotTemplate, shelf).Setup(ingredient, OpenTool, null, tooltip.Show, tooltip.Hide);
            tooltip.Hide();

            BindSpot(scale, PlaceOnScale, Station.TakeCup, () => Station.OnScale != null);
            emptyButton.onClick.AddListener(Station.Empty);
            completeButton.onClick.AddListener(Station.Complete);
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
            base.Close();
        }

        // 커피잔은 저울 위에, 샷 컵·얼음 스쿱은 저울 위 잔에 붓기 (SFR-045, 048)
        static bool PlaceOnScale(int hand) =>
            HandInventory.Instance[hand] is Cup ? Station.PlaceCup(hand) : Station.PourFromHand(hand);

        protected override void Refresh()
        {
            ShowIcon(cupImage, BigCupSprite(Station.OnScale, cupSprites));
            weightText.text = $"{Station.Weight:0}g"; // 커피잔 무게는 빼고 내용물 무게만 (영점)
        }
    }
}
