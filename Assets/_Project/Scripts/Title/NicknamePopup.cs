using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Title
{
    // 닉네임 입력 창 (SFR-003). 입력 후 새 게임 시작 → 프롤로그 스토리(SFR-007)
    public class NicknamePopup : Popup
    {
        [SerializeField] TMP_InputField nicknameInput;
        [SerializeField] GameObject overwriteWarning; // 저장 데이터가 있을 때만 표시
        [SerializeField] Button confirmButton;
        [SerializeField] Button cancelButton;
        [SerializeField] int maxLength = 10; // 글자 수 제한은 추후 결정

        protected override void Awake()
        {
            base.Awake();
            nicknameInput.characterLimit = maxLength;
            nicknameInput.onValueChanged.AddListener(_ => RefreshConfirmButton());
            confirmButton.onClick.AddListener(StartNewGame);
            cancelButton.onClick.AddListener(Close);
        }

        public override void Open()
        {
            nicknameInput.text = "";
            overwriteWarning.SetActive(SaveSystem.HasSave);
            RefreshConfirmButton();
            base.Open();
            nicknameInput.ActivateInputField();
        }

        void RefreshConfirmButton()
        {
            confirmButton.interactable = !string.IsNullOrWhiteSpace(nicknameInput.text);
        }

        void StartNewGame()
        {
            SaveSystem.StartNewGame(nicknameInput.text.Trim());
            SceneLoader.Load(SceneNames.Story);
        }
    }
}
