using System;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.Cafe
{
    // 손님 주문 대사 대화창 (SFR-026). 클릭·Enter·Space 로 넘기면 다음 동작(포스기 화면) 실행
    // 스토리 대화창(SFR-008)이 main 에 들어오면 그 대화창으로 바꿀 수 있다
    public class SpeechPopup : Popup
    {
        [SerializeField] TMP_Text sentenceText;

        Action onDone;
        bool skipFrame;

        public void Open(string sentence, Action onDone)
        {
            sentenceText.text = sentence;
            this.onDone = onDone;
            skipFrame = true; // 연 프레임의 입력(E 키)으로 바로 넘어가지 않도록
            Open();
        }

        void Update()
        {
            if (skipFrame) { skipFrame = false; return; }
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            bool next = (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame ||
                                              keyboard.numpadEnterKey.wasPressedThisFrame)) ||
                        (mouse != null && mouse.leftButton.wasPressedThisFrame);
            if (!next) return;
            var done = onDone;
            onDone = null;
            Close();
            done?.Invoke();
        }
    }
}
