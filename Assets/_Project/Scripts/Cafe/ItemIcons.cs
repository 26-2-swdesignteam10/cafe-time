using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 물건 아이콘 (Art/Icons/Items). 디저트는 재료 아이콘을 재사용
    public class ItemIcons : MonoBehaviour
    {
        [Tooltip("ItemSprite 순서대로")]
        [SerializeField] Sprite[] sprites;
        [Tooltip("메뉴 그림: menuPictureIds[i] 메뉴의 HOT 그림 menuPicturesHot[i], ICE·온도 없음 그림 menuPictures[i]. 포스기 카드와 손 칸·픽업대의 완성 음료가 같은 그림을 쓴다")]
        [SerializeField] string[] menuPictureIds;
        [SerializeField] Sprite[] menuPictures;
        [SerializeField] Sprite[] menuPicturesHot;

        static ItemIcons instance;

        void Awake() => instance = this;

        void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        public static Sprite Get(ItemSprite id) =>
            instance != null && (int)id < instance.sprites.Length ? instance.sprites[(int)id] : null;

        // 완성 음료·디저트 아이콘. 메뉴 그림이 있으면 온도에 맞는 그림, 그 온도 그림이 없으면 다른 온도 그림
        public static Sprite ForMenu(MenuDefinition menu, Temperature temperature)
        {
            int picture = instance != null ? System.Array.IndexOf(instance.menuPictureIds, menu.Id) : -1;
            if (picture >= 0)
            {
                var hot = At(instance.menuPicturesHot, picture);
                var other = At(instance.menuPictures, picture);
                var found = temperature == Temperature.Hot ? hot ?? other : other ?? hot;
                if (found != null) return found;
            }
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

        static Sprite At(Sprite[] list, int index) => list != null && index < list.Length && list[index] != null ? list[index] : null;

        public static Sprite Of(HeldItem item) => item?.Icon;

        public static Sprite Of(TrayItemKind kind) => Get((ItemSprite)System.Enum.Parse(typeof(ItemSprite), kind.ToString()));
    }
}
