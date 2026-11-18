using System;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 메뉴 버튼 1개: 이름 + 가격(또는 재고) + 아이콘. 포스기·임시 제조실·디저트 진열장이 같이 쓴다
    public class MenuButtonView : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text detailText;
        [SerializeField] Image icon; // 없으면 비워 둠

        public MenuDefinition Menu { get; private set; }

        public void Setup(MenuDefinition menu, Action<MenuDefinition> onClick)
        {
            Menu = menu;
            nameText.text = menu.DisplayName;
            detailText.text = $"{menu.Price:N0}원";
            ShowIcon(menu.IsDessert ? Temperature.None : Temperature.Ice);
            button.onClick.AddListener(() => onClick(menu));
            gameObject.SetActive(true);
        }

        // 카드 그림을 해당 온도 그림으로 (포스기에서 HOT/ICE 를 바꿀 때)
        public void ShowIcon(Temperature temperature)
        {
            if (icon == null) return;
            icon.sprite = ItemIcons.ForMenu(Menu, temperature);
            icon.enabled = icon.sprite != null;
        }

        public void SetInteractable(bool value) => button.interactable = value;

        public void SetDetail(string text) => detailText.text = text;
    }
}
