using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CafeTime.Cafe
{
    // 누르고 있는 동안 계속 동작하는 버튼 (우유·탄산수 붓기, SFR-047). Holding 이 false 를 돌려주면 멈춤
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Func<float, bool> Holding { get; set; } // 인자: 이번 프레임 시간(초)

        bool held;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) held = true;
        }

        public void OnPointerUp(PointerEventData eventData) => held = false;

        public void OnPointerExit(PointerEventData eventData) => held = false;

        void OnDisable() => held = false;

        void Update()
        {
            if (held && Holding != null && !Holding(Time.deltaTime)) held = false;
        }
    }
}
