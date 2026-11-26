using System;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 재료 선반의 재료 1개 (SFR-046, 047, 049, 053). 클릭 1회 투입 또는 누르고 있는 동안 붓기.
    // 마우스를 올리면 재료 이름 표시
    public class IngredientSlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Button button;
        [SerializeField] HoldButton hold;
        [SerializeField] Image icon;
        [SerializeField] TMP_Text nameText; // 칸 아래 재료 이름 (없으면 비워 둠)

        public IngredientDefinition Ingredient { get; private set; }

        Action<IngredientSlotView> hoverEnter;
        Action hoverExit;

        public void Setup(IngredientDefinition ingredient, Action<IngredientDefinition> onClick, Func<IngredientDefinition, float, bool> onHold,
            Action<IngredientSlotView> onHoverEnter, Action onHoverExit)
        {
            Ingredient = ingredient;
            icon.sprite = ingredient.Icon;
            icon.enabled = icon.sprite != null;
            if (nameText != null) nameText.text = ingredient.DisplayName;
            if (onHold != null) hold.Holding = seconds => onHold(ingredient, seconds);
            else button.onClick.AddListener(() => onClick(ingredient));
            hold.enabled = onHold != null;
            hoverEnter = onHoverEnter;
            hoverExit = onHoverExit;
            gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData) => hoverEnter?.Invoke(this);

        public void OnPointerExit(PointerEventData eventData) => hoverExit?.Invoke();

        // 마우스를 올렸을 때 보여 줄 재료 이름 (재고 없음, ADR-0056)
        public string Describe() => Describe(Ingredient);

        public static string Describe(IngredientDefinition ingredient) => ingredient.DisplayName;
    }
}
