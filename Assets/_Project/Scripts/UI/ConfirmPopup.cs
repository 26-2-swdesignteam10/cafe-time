using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.UI
{
    // 예/아니오 확인 팝업. 게임 종료(SFR-006), 메인 메뉴로(SFR-023) 등에서 재사용
    public class ConfirmPopup : Popup
    {
        [SerializeField] TMP_Text messageText;
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;
        [SerializeField] GameObject quitMessageGraphic;

        Action onYes;

        protected override void Awake()
        {
            base.Awake();
            yesButton.onClick.AddListener(() =>
            {
                Close();
                onYes?.Invoke();
            });
            noButton.onClick.AddListener(Close);
        }

        public void Open(string message, Action onYes)
        {
            messageText.text = message;
            if (quitMessageGraphic != null)
            {
                bool quitting = message == "게임을 종료하시겠습니까?";
                quitMessageGraphic.SetActive(quitting);
                messageText.gameObject.SetActive(!quitting);
            }
            this.onYes = onYes;
            Open();
        }
    }
}
