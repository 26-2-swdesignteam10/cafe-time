using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 디저트 진열장 (SFR-056): 디저트를 꺼내 손에 든다. 재고는 오픈 준비에서 산 수량과 연동
    public class DessertShowcaseScreen : Popup
    {
        [SerializeField] Transform container;
        [SerializeField] MenuButtonView template;

        readonly List<MenuButtonView> views = new List<MenuButtonView>();

        static SaveData Save => SaveSystem.Current;

        protected override void Awake()
        {
            base.Awake();
            template.gameObject.SetActive(false);
            foreach (var menu in GameBalance.Instance.Menus.Where(m => m.IsDessert))
            {
                var view = Instantiate(template, container);
                view.Setup(menu, Take);
                views.Add(view);
            }
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        void Refresh()
        {
            foreach (var view in views) view.SetDetail($"{Save.GetStock(view.Menu.StockId)}개"); // "재고" 글자는 이미지
        }

        void Take(MenuDefinition menu)
        {
            if (Save.GetStock(menu.StockId) <= 0)
            {
                Toast.Show(ToastMessage.NoStock);
                return;
            }
            if (!HandInventory.Instance.HasSpace)
            {
                Toast.Show(ToastMessage.HandsFull);
                return;
            }
            // 디저트는 접시에 담아 꺼낸다. 접시는 설거지·건조 후 돌아옴 (SFR-064)
            if (!CupShelf.Instance.TryTakePlate())
            {
                Toast.Show(ToastMessage.NoPlate);
                return;
            }
            HandInventory.Instance.TryAdd(new DessertItem(menu));
            Save.AddStock(menu.StockId, -1);
            Refresh();
        }
    }
}
