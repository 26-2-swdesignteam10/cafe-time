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
            detailText.text = $"₩{menu.Price:N0}";
            if (icon != null)
            {
                icon.sprite = ItemIcons.ForMenu(menu, menu.IsDessert ? Temperature.None : Temperature.Ice);
                icon.enabled = icon.sprite != null;
            }
            button.onClick.AddListener(() => onClick(menu));
            gameObject.SetActive(true);
        }

        public void SetDetail(string text) => detailText.text = text;

        // 포스기 메뉴 카드: 손 칸 아이콘 대신 완성품 그림 (ADR-0029)
        public void SetIcon(Sprite sprite)
        {
            if (icon == null || sprite == null) return;
            icon.sprite = sprite;
            icon.enabled = true;
        }
    }
}
