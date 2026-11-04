using System.Linq;
using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;

namespace CafeTime.Cafe
{
    // (임시) 음료 제조실·쉐이크 제조실: 메뉴를 누르면 '완벽' 등급의 완성 음료를 바로 손에 든다.
    // 제조 기기(SFR-028~055)가 구현되면 각 기기 화면으로 교체하고 이 클래스는 삭제한다 (ADR-0013)
    public class TempDrinkMakerScreen : Popup
    {
        [SerializeField] MenuCategory[] categories;
        [SerializeField] Transform menuContainer;
        [SerializeField] MenuButtonView menuTemplate;
        [SerializeField] TemperatureSelector temperature;

        protected override void Awake()
        {
            base.Awake();
            menuTemplate.gameObject.SetActive(false);
            foreach (var menu in GameBalance.Instance.Menus.Where(m => categories.Contains(m.Category)))
                Instantiate(menuTemplate, menuContainer).Setup(menu, Make);
        }

        void Make(MenuDefinition menu)
        {
            if (!HandInventory.Instance.TryAdd(new HeldItem(menu, temperature.For(menu), DrinkGrade.Perfect)))
                Toast.Show(ToastMessage.HandsFull);
        }
    }
}
