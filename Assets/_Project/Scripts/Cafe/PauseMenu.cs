using CafeTime.Core;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 일시정지 메뉴 (SFR-023): 열려 있는 동안 게임 시간 정지. 계속하기 / 설정 / 메인 메뉴로
    public class PauseMenu : Popup
    {
        [SerializeField] Button continueButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button mainMenuButton;
        [SerializeField] SettingsPopup settings;
        [SerializeField] ConfirmPopup confirm;
        [SerializeField] Sprite confirmMessage; // "오늘 진행한 영업은 저장되지 않습니다. 메인 메뉴로 돌아가시겠습니까?"

        protected override void Awake()
        {
            base.Awake();
            continueButton.onClick.AddListener(Close);
            settingsButton.onClick.AddListener(settings.Open);
            mainMenuButton.onClick.AddListener(() => confirm.Open(confirmMessage, GoToTitle));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameClock.Paused = true;
            Time.timeScale = 0f; // 손님 걸음 등도 멈춤
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameClock.Paused = false;
            Time.timeScale = 1f;
        }

        // 그날 영업은 저장되지 않는다. 이어하기는 마지막 저장(직전 정산)부터 (SFR-071)
        static void GoToTitle()
        {
            Time.timeScale = 1f;
            SceneLoader.Load(SceneNames.Title);
        }
    }
}
