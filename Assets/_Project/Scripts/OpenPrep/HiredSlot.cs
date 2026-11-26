using System;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // '오늘의 알바생' 자리 하나 (ADR-0057). 비어 있으면 빈 자리 상자를 보여 준다
    public class HiredSlot : MonoBehaviour
    {
        [SerializeField] GameObject filled;
        [SerializeField] GameObject empty;
        [SerializeField] Image portrait;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text taskText;
        [SerializeField] TMP_Text wageText;
        [SerializeField] Button cancelButton;

        public void Init(Action onCancel) => cancelButton.onClick.AddListener(() => onCancel());

        public void Show(StaffDefinition definition, Sprite face)
        {
            filled.SetActive(definition != null);
            empty.SetActive(definition == null);
            if (definition == null) return;
            portrait.sprite = face;
            portrait.enabled = face != null;
            nameText.text = definition.DisplayName;
            taskText.text = definition.TaskText;
            wageText.text = $"일당 {definition.Wage:N0}원";
        }
    }
}
