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
        [SerializeField] GameObject quitMessageGraphic; // 문구 이미지. 기본 스프라이트는 "게임을 종료할까요?"

        Action onYes;
        Image messageImage;
        Sprite quitMessageSprite;

        protected override void Awake()
        {
            base.Awake();
            if (quitMessageGraphic != null)
            {
                messageImage = quitMessageGraphic.GetComponent<Image>();
                quitMessageSprite = messageImage.sprite;
            }
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
                messageImage.sprite = quitMessageSprite;
                quitMessageGraphic.SetActive(quitting);
                messageText.gameObject.SetActive(!quitting);
            }
            this.onYes = onYes;
            Open();
        }

        // 고정 문구를 디자인 이미지로 표시 (docs/ART_STYLE.md 4장)
        public void Open(Sprite message, Action onYes)
        {
            messageImage.sprite = message;
            quitMessageGraphic.SetActive(true);
            messageText.gameObject.SetActive(false);
            this.onYes = onYes;
            Open();
        }
    }
}
