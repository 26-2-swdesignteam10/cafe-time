using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 물건 아이콘 (Art/Icons/Items). 디저트는 재료 아이콘을 재사용
    public class ItemIcons : MonoBehaviour
    {
        [Tooltip("ItemSprite 순서대로")]
        [SerializeField] Sprite[] sprites;

        static ItemIcons instance;

        void Awake() => instance = this;

        void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        public static Sprite Get(ItemSprite id) =>
            instance != null && (int)id < instance.sprites.Length ? instance.sprites[(int)id] : null;

        // 완성 음료·디저트 아이콘
        public static Sprite ForMenu(MenuDefinition menu, Temperature temperature)
        {
            switch (menu.Category)
            {
                case MenuCategory.Coffee:
                    return Get(temperature == Temperature.Hot ? ItemSprite.HotCoffee : ItemSprite.IceCoffee);
                case MenuCategory.Beverage:
                    return Get(temperature == Temperature.Hot ? ItemSprite.HotTea : ItemSprite.IceDrink);
                case MenuCategory.Smoothie:
                    return Get(ItemSprite.Smoothie);
                default:
                    var ingredient = GameBalance.Instance.FindIngredient(menu.StockId);
                    return ingredient != null ? ingredient.Icon : null;
            }
        }

        public static Sprite Of(HeldItem item) => item?.Icon;

        public static Sprite Of(TrayItemKind kind) => Get((ItemSprite)System.Enum.Parse(typeof(ItemSprite), kind.ToString()));
    }
}
