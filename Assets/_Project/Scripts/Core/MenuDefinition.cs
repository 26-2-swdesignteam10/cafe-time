using UnityEngine;

namespace CafeTime.Core
{
    // 판매 메뉴 하나의 정의 (SFR-026). 판매가는 임시값 (docs/GAME_DATA.md 4장)
    [System.Serializable]
    public class MenuDefinition
    {
        [Tooltip("고유 id. 한번 정하면 바꾸지 않는다")]
        [SerializeField] string id;
        [SerializeField] string displayName;
        [SerializeField] MenuCategory category;
        [SerializeField] int price;
        [SerializeField] bool hot;
        [SerializeField] bool ice;
        [Tooltip("디저트만: 오픈 준비에서 산 재고의 재료 id (SFR-056)")]
        [SerializeField] string stockId;

        public string Id => id;
        public string DisplayName => displayName;
        public MenuCategory Category => category;
        public int Price => price;
        public bool IsDessert => category == MenuCategory.Dessert;
        public string StockId => stockId;

        public bool Allows(Temperature temperature) =>
            IsDessert ? temperature == Temperature.None : temperature == Temperature.Hot ? hot : temperature == Temperature.Ice && ice;

        // HOT / ICE 중 하나만 되는 메뉴의 온도. 둘 다 되거나 디저트면 None
        public Temperature OnlyTemperature => IsDessert || (hot && ice) ? Temperature.None : hot ? Temperature.Hot : Temperature.Ice;

        public MenuDefinition(string id, string displayName, MenuCategory category, int price, bool hot, bool ice, string stockId = "")
        {
            this.id = id;
            this.displayName = displayName;
            this.category = category;
            this.price = price;
            this.hot = hot;
            this.ice = ice;
            this.stockId = stockId;
        }
    }
}
