using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 레시피북 창 (SFR-051): HUD 의 레시피 버튼으로 연다. 위쪽 분류 탭(커피·음료·스무디), 왼쪽 메뉴 목록,
    // 오른쪽에 고른 메뉴의 레시피 카드 1장과 HOT/ICE 선택 버튼. 레시피는 GameBalance 데이터를 그대로 보여 준다 (디저트는 레시피 없음)
    public class RecipeBookScreen : Popup
    {
        static readonly MenuCategory[] Categories = { MenuCategory.Coffee, MenuCategory.Beverage, MenuCategory.Smoothie };

        [Header("탭 (Categories 순서)")]
        [SerializeField] Button[] tabs;
        [SerializeField] Sprite tabOn;
        [SerializeField] Sprite tabOff;

        [Header("메뉴 목록")]
        [SerializeField] Transform menuContainer;
        [SerializeField] Button menuTemplate;
        [SerializeField] Sprite menuItem;
        [SerializeField] Sprite menuItemSelected;

        [Header("레시피")]
        [SerializeField] TMP_Text menuNameText;
        [SerializeField] RecipeCardView card;

        [Header("HOT/ICE 선택 (그 온도의 레시피가 없으면 누를 수 없음)")]
        [SerializeField] Button hotToggle;
        [SerializeField] Button iceToggle;
        [SerializeField] Sprite hotOn;
        [SerializeField] Sprite hotOff;
        [SerializeField] Sprite iceOn;
        [SerializeField] Sprite iceOff;

        [Header("구매 재료가 아닌 레시피 재료 아이콘 (RecipeKeys)")]
        [SerializeField] Sprite shotIcon;
        [SerializeField] Sprite iceIcon;
        [SerializeField] Sprite hotWaterIcon;
        [SerializeField] Sprite coldWaterIcon;

        readonly List<Button> menuButtons = new List<Button>();
        List<MenuDefinition> menus = new List<MenuDefinition>();
        int category, selected;
        Temperature temperature = Temperature.Hot; // 메뉴를 바꿔도 고른 온도를 유지 (그 온도가 없는 메뉴면 있는 쪽으로)

        protected override void Awake()
        {
            base.Awake();
            menuTemplate.gameObject.SetActive(false);
            for (int i = 0; i < tabs.Length; i++)
            {
                int index = i;
                tabs[i].onClick.AddListener(() => ShowCategory(index));
            }
            hotToggle.onClick.AddListener(() => ShowTemperature(Temperature.Hot));
            iceToggle.onClick.AddListener(() => ShowTemperature(Temperature.Ice));
        }

        public override void Open()
        {
            base.Open();
            ShowCategory(category); // 마지막으로 보던 분류·메뉴를 다시 보여 줌
        }

        void ShowCategory(int index)
        {
            if (index != category) selected = 0;
            category = index;
            for (int i = 0; i < tabs.Length; i++) tabs[i].image.sprite = i == category ? tabOn : tabOff;

            var balance = GameBalance.Instance;
            menus = balance.Menus.Where(m => m.Category == Categories[category]).ToList();
            while (menuButtons.Count < menus.Count)
            {
                var button = Instantiate(menuTemplate, menuContainer);
                int item = menuButtons.Count;
                button.onClick.AddListener(() => ShowMenu(item));
                menuButtons.Add(button);
            }
            for (int i = 0; i < menuButtons.Count; i++)
            {
                menuButtons[i].gameObject.SetActive(i < menus.Count);
                if (i < menus.Count) menuButtons[i].GetComponentInChildren<TMP_Text>().text = menus[i].DisplayName;
            }
            ShowMenu(Mathf.Clamp(selected, 0, Mathf.Max(0, menus.Count - 1)));
        }

        void ShowMenu(int index)
        {
            selected = index;
            for (int i = 0; i < menuButtons.Count; i++) menuButtons[i].image.sprite = i == selected ? menuItemSelected : menuItem;
            var menu = selected < menus.Count ? menus[selected] : null;
            menuNameText.text = menu != null ? menu.DisplayName : "";
            ShowTemperature(temperature);
        }

        static RecipeDefinition Find(MenuDefinition menu, Temperature temperature) =>
            menu == null ? null : GameBalance.Instance.Recipes.FirstOrDefault(r => r.MenuId == menu.Id && r.Temperature == temperature);

        // HOT 전용·ICE 전용 메뉴는 없는 온도의 버튼을 비활성화하고 있는 쪽 레시피를 보여 준다
        void ShowTemperature(Temperature wanted)
        {
            var menu = selected < menus.Count ? menus[selected] : null;
            var hot = Find(menu, Temperature.Hot);
            var ice = Find(menu, Temperature.Ice);
            if (wanted == Temperature.Hot ? hot != null : ice != null) temperature = wanted;
            else if (hot != null || ice != null) temperature = hot != null ? Temperature.Hot : Temperature.Ice;

            hotToggle.interactable = hot != null;
            iceToggle.interactable = ice != null;
            hotToggle.image.sprite = temperature == Temperature.Hot ? hotOn : hotOff;
            iceToggle.image.sprite = temperature == Temperature.Ice ? iceOn : iceOff;
            Fill(temperature == Temperature.Hot ? hot : ice);
        }

        void Fill(RecipeDefinition recipe)
        {
            card.gameObject.SetActive(recipe != null);
            if (recipe == null) return;
            for (int row = 0; row < card.Rows; row++)
            {
                if (row >= recipe.Amounts.Count) { card.ShowRow(row, false); continue; }
                var part = recipe.Amounts[row];
                var (icon, label, amount) = Describe(part);
                card.SetRow(row, icon, label, amount);
            }
        }

        // 레시피 재료 한 줄의 아이콘·이름·양. 샷·얼음·물은 구매 재료가 아니라서 이름과 단위를 여기서 정한다
        (Sprite, string, string) Describe(RecipeAmount part)
        {
            int amount = Mathf.RoundToInt(part.Amount);
            switch (part.Key)
            {
                case RecipeKeys.Shot: return (shotIcon, "에스프레소 샷", $"{amount}샷");
                case RecipeKeys.Ice: return (iceIcon, "얼음", $"{amount}스쿱");
                case RecipeKeys.HotWater: return (hotWaterIcon, "뜨거운 물", $"{amount}g");
                case RecipeKeys.ColdWater: return (coldWaterIcon, "차가운 물", $"{amount}g");
            }
            var ingredient = GameBalance.Instance.FindIngredient(part.Key);
            return ingredient != null
                ? (ingredient.Icon, ingredient.DisplayName, ingredient.FormatAmount(amount))
                : (null, part.Key, amount.ToString());
        }
    }
}
