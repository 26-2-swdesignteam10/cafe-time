using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Title
{
    // 시작 방식 선택 창 (SFR-002): 처음부터 시작 / 이어하기 / 메인화면으로
    public class GameStartPopup : Popup
    {
        [SerializeField] Button newGameButton;
        [SerializeField] Button continueButton;
        [SerializeField] TMP_Text continueInfoText;
        [SerializeField] NicknamePopup nicknamePopup;

        protected override void Awake()
        {
            base.Awake();
            newGameButton.onClick.AddListener(nicknamePopup.Open);
            continueButton.onClick.AddListener(ContinueGame);
        }

        public override void Open()
        {
            SaveData save = SaveSystem.Load();
            continueButton.interactable = save != null; // 저장 데이터가 없으면 비활성화
            continueInfoText.text = save != null ? $"DAY {save.day} · ₩{save.money:N0}" : "저장된 데이터 없음";
            base.Open();
        }

        static void ContinueGame()
        {
            if (SaveSystem.ContinueGame())
                SceneLoader.Load(SceneNames.OpenPrep);
        }
    }
}
