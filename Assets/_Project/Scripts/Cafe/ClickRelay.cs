using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CafeTime.Cafe
{
    // 왼쪽 클릭을 화면 스크립트로 넘겨 주는 부품. 끌다가 놓은 것은 클릭으로 치지 않는다 (Unity 기본 동작).
    // 퇴식구 쟁반 위 잔·접시를 눌러 손으로 가져올 때 쓴다 (ADR-0052)
    public class ClickRelay : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) Clicked?.Invoke();
        }
    }
}
