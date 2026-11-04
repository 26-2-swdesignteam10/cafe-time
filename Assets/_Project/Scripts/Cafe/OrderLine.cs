using CafeTime.Core;

namespace CafeTime.Cafe
{
    // 주문 한 줄: 메뉴 + HOT/ICE + 수량 (SFR-026)
    public class OrderLine
    {
        public MenuDefinition Menu { get; }
        public Temperature Temperature { get; }
        public int Quantity { get; set; }

        public OrderLine(MenuDefinition menu, Temperature temperature, int quantity)
        {
            Menu = menu;
            Temperature = temperature;
            Quantity = quantity;
        }

        public int Price => Menu.Price * Quantity;

        public bool Matches(MenuDefinition menu, Temperature temperature) => Menu == menu && Temperature == temperature;
    }
}
