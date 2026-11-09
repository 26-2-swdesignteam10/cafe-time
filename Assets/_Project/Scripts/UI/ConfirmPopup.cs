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
        bool graphicCached;

        protected override void Awake()
        {
            base.Awake();
            CacheGraphic();
            yesButton.onClick.AddListener(() =>
            {
                Close();
                onYes?.Invoke();
            });
            noButton.onClick.AddListener(Close);
        }

        // 팝업이 꺼진 채로 씬이 시작되면 Awake 는 처음 열 때에야 실행된다.
        // Open 에서 문구 이미지를 먼저 쓰기 때문에, 한 번만 준비하는 함수를 Awake 와 Open 양쪽에서 부른다.
        void CacheGraphic()
        {
            if (graphicCached) return;
            graphicCached = true;
            if (quitMessageGraphic == null) return;
            messageImage = quitMessageGraphic.GetComponent<Image>();
            quitMessageSprite = messageImage.sprite; // 바꾸기 전의 기본(종료) 문구를 기억
        }

        public void Open(string message, Action onYes)
        {
            CacheGraphic();
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
            CacheGraphic();
            messageImage.sprite = message;
            quitMessageGraphic.SetActive(true);
            messageText.gameObject.SetActive(false);
            this.onYes = onYes;
            Open();
        }
    }
}
