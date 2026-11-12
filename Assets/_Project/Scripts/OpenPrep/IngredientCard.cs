using System;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // 오픈 준비 화면의 재료 카드 1장: 아이콘, 이름, 현재 재고, 단가, 장바구니 수량 -/+ (SFR-010)
    // -/+ 는 길게 누르면 빠르게 반복하고, 재고가 구매 단위 1개 분량보다 적으면 재고 글자를 경고색으로 표시한다
    public class IngredientCard : MonoBehaviour
    {
        [SerializeField] Image iconImage;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text stockText;
        [SerializeField] TMP_Text priceText;
        [SerializeField] TMP_Text quantityText;
        [SerializeField] Button minusButton;
        [SerializeField] RepeatButton minusRepeat;
        [SerializeField] RepeatButton plusRepeat;
        [SerializeField] Color stockColor = new Color32(0x5A, 0x35, 0x20, 0xFF); // 빌더의 TextBrown
        [SerializeField] Color lowStockColor = new Color32(0xC0, 0x39, 0x2B, 0xFF);

        public IngredientDefinition Definition { get; private set; }

        public void Init(IngredientDefinition definition, Action<IngredientDefinition, int> onQuantityDelta)
        {
            Definition = definition;
            iconImage.sprite = definition.Icon;
            iconImage.enabled = definition.Icon != null;
            nameText.text = definition.DisplayName;
            priceText.text = $"₩{definition.Price:N0} / 1{definition.PurchaseUnit}";
            minusRepeat.Pressed += () => onQuantityDelta(definition, -1);
            plusRepeat.Pressed += () => onQuantityDelta(definition, +1);
        }

        // stock: 기본 단위 재고(전날 남은 재고 포함), quantity: 장바구니에 담은 구매 단위 수량
        public void Refresh(int stock, int quantity)
        {
            stockText.text = Definition.FormatAmount(stock); // "재고" 글자는 카드의 이미지 라벨
            stockText.color = stock < Definition.AmountPerUnit ? lowStockColor : stockColor;
            quantityText.text = quantity.ToString();
            minusButton.interactable = quantity > 0;
        }
    }
}
