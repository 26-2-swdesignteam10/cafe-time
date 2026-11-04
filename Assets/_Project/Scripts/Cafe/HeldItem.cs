using CafeTime.Core;

namespace CafeTime.Cafe
{
    // 손에 들거나 픽업대에 올린 완성 음료·디저트 1개 (SFR-021, 056)
    public class HeldItem
    {
        public MenuDefinition Menu { get; }
        public Temperature Temperature { get; }
        public DrinkGrade Grade { get; }

        public HeldItem(MenuDefinition menu, Temperature temperature, DrinkGrade grade)
        {
            Menu = menu;
            Temperature = temperature;
            Grade = grade;
        }

        public string Label => OrderText.ItemLabel(Menu, Temperature);
    }
}
