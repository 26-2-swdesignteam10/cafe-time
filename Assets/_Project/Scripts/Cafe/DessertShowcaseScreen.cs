using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 베이커리 진열장 (SFR-056): 재고 확인 전용. 손님이 직접 가져가고 포스기에서 계산한다.
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
            // 기존 씬을 재생성하지 않아도 8종을 두 줄로 보여 준다.
            var layout = container.GetComponent<LayoutGroup>();
            if (layout != null) layout.enabled = false;
            foreach (var menu in GameBalance.Instance.Menus.Where(m => m.IsDessert))
            {
                var view = Instantiate(template, container);
                view.Setup(menu, _ => { });
                view.SetInteractable(false);
                var rect = (RectTransform)view.transform;
                rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
                rect.sizeDelta = new Vector2(200, 252);
                rect.localScale = Vector3.one * 0.7f;
                rect.anchoredPosition = new Vector2((views.Count % 4) * 180, -(views.Count / 4) * 180);
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
    }
}
