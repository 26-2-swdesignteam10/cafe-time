using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.Title
{
    // 시작 화면 메인 메뉴 (SFR-001): 게임 시작 / 게임 설명 / 설정 / 랭킹 / 게임 종료
    public class TitleMenu : MonoBehaviour
    {
        [Header("메뉴 버튼")]
        [SerializeField] Button startButton;
        [SerializeField] Button howToButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button rankingButton;
        [SerializeField] Button quitButton;

        [Header("팝업")]
        [SerializeField] GameStartPopup gameStartPopup;
        [SerializeField] Popup howToPopup;
        [SerializeField] SettingsPopup settingsPopup;
        [SerializeField] Popup rankingPopup;   // 랭킹 서버 구현 전까지 연결 실패 안내만 표시 (SFR-070)
        [SerializeField] ConfirmPopup confirmPopup;

        void Awake()
        {
            GameSettings.Apply();

            startButton.onClick.AddListener(gameStartPopup.Open);
            howToButton.onClick.AddListener(howToPopup.Open);
            settingsButton.onClick.AddListener(settingsPopup.Open);
            rankingButton.onClick.AddListener(rankingPopup.Open);
            quitButton.onClick.AddListener(() => confirmPopup.Open("게임을 종료하시겠습니까?", QuitGame)); // SFR-006
        }

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                Popup.CloseTop();
        }

        static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
