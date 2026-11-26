using System;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // 알바생 고용 탭의 알바생 카드 하나 (ADR-0057): 초상화, 이름, 맡는 일, 일당, 고용 버튼
    public class StaffCard : MonoBehaviour
    {
        public enum State { Hirable, Hired, Locked, Full }

        [SerializeField] Image portrait;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text taskText;
        [SerializeField] TMP_Text noteText;   // "DAY 10부터 지원해요" 등
        [SerializeField] TMP_Text wageText;
        [SerializeField] Button hireButton;
        [SerializeField] TMP_Text hireLabel;
        [SerializeField] Sprite buttonNormal;
        [SerializeField] Sprite buttonDisabled;

        public void Init(Action onHire) => hireButton.onClick.AddListener(() => onHire());

        public void Show(StaffDefinition definition, Sprite face, State state)
        {
            portrait.sprite = face;
            portrait.enabled = face != null;
            portrait.color = state == State.Locked ? new Color(0.35f, 0.3f, 0.28f) : Color.white; // 아직 지원하지 않은 알바생은 어둡게
            nameText.text = definition.DisplayName;
            taskText.text = definition.TaskText;
            wageText.text = $"일당 {definition.Wage:N0}원";
            noteText.text = state == State.Locked ? $"DAY {definition.UnlockDay}부터 지원해요" : "";
            hireLabel.text = state switch
            {
                State.Hired => "고용함",
                State.Locked => "잠김",
                State.Full => "자리 없음",
                _ => "고용하기",
            };
            bool on = state == State.Hirable;
            hireButton.interactable = on;
            hireButton.image.sprite = on ? buttonNormal : buttonDisabled;
        }
    }
}
