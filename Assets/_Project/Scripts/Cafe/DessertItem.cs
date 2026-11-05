using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 접시에 담은 디저트 1개 (SFR-056). 디저트 진열장에서 꺼낸다
    public class DessertItem : HeldItem
    {
        readonly MenuDefinition menu;

        public DessertItem(MenuDefinition menu) => this.menu = menu;

        public override bool Servable => true;
        public override MenuDefinition Menu => menu;
        public override DrinkGrade Grade => DrinkGrade.Perfect;
        public override Sprite Icon => ItemIcons.ForMenu(menu, Temperature.None);
    }
}
