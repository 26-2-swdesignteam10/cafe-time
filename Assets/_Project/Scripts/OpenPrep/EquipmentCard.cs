using System;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // 장비 업그레이드 탭의 장비 카드 하나 (ADR-0057): 아이콘, 이름, 단계 별, 다음 단계 가격
    public class EquipmentCard : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Image frame;
        [SerializeField] Sprite frameNormal;
        [SerializeField] Sprite frameSelected;
        [SerializeField] TMP_Text nameText;
        [SerializeField] Image[] stars;
        [SerializeField] TMP_Text priceText;

        public void Init(Action onClick) => button.onClick.AddListener(() => onClick());

        public void Show(EquipmentDefinition definition, int level, bool selected, Sprite starFull, Sprite starEmpty, Color priceColor, Color maxColor)
        {
            frame.sprite = selected ? frameSelected : frameNormal;
            nameText.text = definition.DisplayName;
            for (int i = 0; i < stars.Length; i++) stars[i].sprite = i < level ? starFull : starEmpty;
            bool max = level >= EquipmentDefinition.MaxLevel;
            priceText.text = max ? "최고 단계" : $"{definition.PriceOf(level + 1):N0}원";
            priceText.color = max ? maxColor : priceColor;
        }
    }
}
