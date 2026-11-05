using CafeTime.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.Cafe
{
    // 영업 씬 공통 키 입력 (SFR-023)
    // ESC: 기기 화면 등 UI가 열려 있으면 그 화면을 닫고, 아무것도 없으면 일시정지 메뉴
    public class CafeSceneInput : MonoBehaviour
    {
        [SerializeField] PauseMenu pauseMenu;

        void Update()
        {
            if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;
            if (!Popup.CloseTop() && pauseMenu != null && !(GameClock.Instance != null && GameClock.Instance.IsClosed))
                pauseMenu.Open();
        }
    }
}
