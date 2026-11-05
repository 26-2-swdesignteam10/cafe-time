using System;
using CafeTime.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 재료 선반의 재료 1개 (SFR-046, 047, 049, 053). 클릭 1회 투입 또는 누르고 있는 동안 붓기.
    // 마우스를 올리면 재료 이름과 남은 양 표시
    public class IngredientSlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Button button;
        [SerializeField] HoldButton hold;
        [SerializeField] Image icon;

        public IngredientDefinition Ingredient { get; private set; }

        Action<IngredientSlotView> hoverEnter;
        Action hoverExit;

        public void Setup(IngredientDefinition ingredient, Action<IngredientDefinition> onClick, Func<IngredientDefinition, float, bool> onHold,
            Action<IngredientSlotView> onHoverEnter, Action onHoverExit)
        {
            Ingredient = ingredient;
            icon.sprite = ingredient.Icon;
            icon.enabled = icon.sprite != null;
            if (onHold != null) hold.Holding = seconds => onHold(ingredient, seconds);
            else button.onClick.AddListener(() => onClick(ingredient));
            hold.enabled = onHold != null;
            hoverEnter = onHoverEnter;
            hoverExit = onHoverExit;
            gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData) => hoverEnter?.Invoke(this);

        public void OnPointerExit(PointerEventData eventData) => hoverExit?.Invoke();

        // "바닐라 시럽 80%" / "캐모마일 티백 7개"
        public string Describe()
        {
            if (Ingredient.Category == IngredientCategory.Tea)
                return $"{Ingredient.DisplayName} {Mathf.FloorToInt(Pantry.Available(Ingredient.Id))}개";
            return $"{Ingredient.DisplayName} {Pantry.PercentLeft(Ingredient.Id)}%";
        }
    }
}
