using System;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // 오픈 준비 화면의 재료 카드 1장: 아이콘, 이름, 현재 재고, 단가, 장바구니 수량 -/+ (SFR-010)
    public class IngredientCard : MonoBehaviour
    {
        [SerializeField] Image iconImage;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text stockText;
        [SerializeField] TMP_Text priceText;
        [SerializeField] TMP_Text quantityText;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;

        public IngredientDefinition Definition { get; private set; }

        public void Init(IngredientDefinition definition, Action<IngredientDefinition, int> onQuantityDelta)
        {
            Definition = definition;
            iconImage.sprite = definition.Icon;
            iconImage.enabled = definition.Icon != null;
            nameText.text = definition.DisplayName;
            priceText.text = $"₩{definition.Price:N0} / 1{definition.PurchaseUnit}";
            minusButton.onClick.AddListener(() => onQuantityDelta(definition, -1));
            plusButton.onClick.AddListener(() => onQuantityDelta(definition, +1));
        }

        // stock: 기본 단위 재고(전날 남은 재고 포함), quantity: 장바구니에 담은 구매 단위 수량
        public void Refresh(int stock, int quantity)
        {
            stockText.text = Definition.FormatAmount(stock); // "재고" 글자는 카드의 이미지 라벨
            quantityText.text = quantity.ToString();
            minusButton.interactable = quantity > 0;
        }
    }
}
