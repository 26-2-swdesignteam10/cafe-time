using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 완성 음료·디저트 아이콘 (Art/Icons/Items, 디저트는 재료 아이콘 재사용)
    public class ItemIcons : MonoBehaviour
    {
        [SerializeField] Sprite hotCoffee;
        [SerializeField] Sprite iceCoffee;
        [SerializeField] Sprite hotTea;
        [SerializeField] Sprite iceDrink;
        [SerializeField] Sprite smoothie;

        public static ItemIcons Instance { get; private set; }

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public Sprite For(MenuDefinition menu, Temperature temperature)
        {
            switch (menu.Category)
            {
                case MenuCategory.Coffee:
                    return temperature == Temperature.Hot ? hotCoffee : iceCoffee;
                case MenuCategory.Beverage:
                    return temperature == Temperature.Hot ? hotTea : iceDrink;
                case MenuCategory.Smoothie:
                    return smoothie;
                default:
                    var ingredient = GameBalance.Instance.FindIngredient(menu.StockId);
                    return ingredient != null ? ingredient.Icon : null;
            }
        }

        public static Sprite Of(HeldItem item) => Instance != null && item != null ? Instance.For(item.Menu, item.Temperature) : null;
    }
}
