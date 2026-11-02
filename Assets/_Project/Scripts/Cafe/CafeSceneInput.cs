using CafeTime.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.Cafe
{
    // 영업 씬 공통 키 입력
    // ESC: 기기 화면 등 열린 UI를 닫음. UI가 없을 때 일시정지 메뉴 열기는 SFR-023 구현 시 추가
    public class CafeSceneInput : MonoBehaviour
    {
        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                Popup.CloseTop();
        }
    }
}
