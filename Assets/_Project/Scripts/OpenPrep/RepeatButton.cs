using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // 누르면 1번, 계속 누르고 있으면 잠시 뒤부터 빠르게 반복하는 버튼 (재료 카드 수량 -/+, SFR-010)
    // 같은 오브젝트의 Button 은 그림 전환과 비활성화 표시에만 쓰고, 클릭 처리는 여기서 한다
    [RequireComponent(typeof(Button))]
    public class RepeatButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] float repeatDelay = 0.4f;     // 누른 뒤 반복을 시작할 때까지 (초)
        [SerializeField] float repeatInterval = 0.08f; // 반복 간격 (초)

        public event Action Pressed;

        Button button;
        bool held;
        float nextTime;

        void Awake() => button = GetComponent<Button>();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !button.IsInteractable()) return;
            held = true;
            nextTime = Time.unscaledTime + repeatDelay;
            Pressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData) => held = false;

        public void OnPointerExit(PointerEventData eventData) => held = false;

        void OnDisable() => held = false;

        void Update()
        {
            if (!held) return;
            if (!button.IsInteractable()) { held = false; return; } // 수량이 0이 되어 - 가 비활성화되면 멈춤
            while (held && Time.unscaledTime >= nextTime)
            {
                nextTime += repeatInterval;
                Pressed?.Invoke();
                if (!button.IsInteractable()) held = false;
            }
        }
    }
}
