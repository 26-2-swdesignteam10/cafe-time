using System;
using System.Collections;
using System.Collections.Generic;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 영업 중 NPC 이벤트 대화창 (SFR-008, 009, ADR-0042): 화자 이름·초상화·대사를 차례로 보여 주고,
    // 마지막 대사에서 수락/거절 선택지를 띄운 뒤 고른 결과에 맞는 답 대사를 보여 주고 닫힌다.
    // 열려 있는 동안 게임 시간과 손님이 멈추고(일시정지 메뉴와 같은 방식), 플레이어 이동·상호작용은 Popup.AnyOpen 으로 막힌다.
    // 선택 전에는 ESC 로 닫을 수 없다
    public class EventDialog : Popup
    {
        [SerializeField] TMP_Text speakerText;
        [SerializeField] TMP_Text dialogueText;
        [SerializeField] Image portrait;
        [SerializeField] GameObject choicePanel;
        [SerializeField] Button acceptButton;
        [SerializeField] Button declineButton;
        [SerializeField, Min(0.001f)] float typingSpeed = 0.03f;

        // 포스기 Enter 결제 등 아래 화면의 키 입력을 막을 때 쓴다
        public static bool IsShowing { get; private set; }

        IReadOnlyList<string> lines;
        Func<bool, string> onChoice; // 고른 결과(수락 = true) → 답 대사
        int index;
        bool typing;
        bool waitingChoice;
        bool replied;
        bool closable;
        int ignoreFrame = -1;
        Coroutine typingRoutine;

        protected override void Awake()
        {
            base.Awake();
            acceptButton.onClick.AddListener(() => Choose(true));
            declineButton.onClick.AddListener(() => Choose(false));
        }

        // lines 의 마지막 대사가 끝나면 선택지를 띄운다. onChoice 는 고른 결과를 처리하고 답 대사를 돌려준다
        public void Show(string speaker, Sprite face, IReadOnlyList<string> dialogue, Func<bool, string> choice)
        {
            lines = dialogue;
            onChoice = choice;
            index = 0;
            replied = waitingChoice = closable = false;
            speakerText.text = speaker;
            portrait.sprite = face;
            portrait.enabled = face != null;
            choicePanel.SetActive(false);
            ignoreFrame = Time.frameCount;
            Open();
            Display(lines[0]);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            IsShowing = true;
            GameClock.Paused = true;
            Time.timeScale = 0f;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            IsShowing = false;
            GameClock.Paused = false;
            Time.timeScale = 1f;
        }

        // ESC(Popup.CloseTop)로는 선택 전에 닫히지 않는다
        public override void Close()
        {
            if (closable) base.Close();
        }

        // SFR-008: 클릭 또는 아무 키로 대사 진행
        void Update()
        {
            if (waitingChoice || Time.frameCount == ignoreFrame) return;
            bool pressed = (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current.escapeKey.wasPressedThisFrame)
                || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
            if (pressed) Advance();
        }

        void Advance()
        {
            if (typing)
            {
                FinishTyping();
                return;
            }
            if (replied)
            {
                closable = true;
                Close();
                return;
            }
            if (index < lines.Count - 1) Display(lines[++index]);
        }

        void Display(string text)
        {
            if (typingRoutine != null) StopCoroutine(typingRoutine);
            dialogueText.text = text;
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();
            typingRoutine = StartCoroutine(Type(dialogueText.textInfo.characterCount));
        }

        // SFR-008: 타이핑 효과. 시간이 멈춰 있으므로 실제 시간으로 진행
        IEnumerator Type(int count)
        {
            typing = true;
            for (int i = 1; i <= count; i++)
            {
                dialogueText.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
            typingRoutine = null;
            OnLineShown();
        }

        void FinishTyping()
        {
            if (typingRoutine != null) StopCoroutine(typingRoutine);
            typingRoutine = null;
            dialogueText.maxVisibleCharacters = int.MaxValue;
            OnLineShown();
        }

        void OnLineShown()
        {
            typing = false;
            if (!replied && index == lines.Count - 1)
            {
                waitingChoice = true;
                choicePanel.SetActive(true);
            }
        }

        void Choose(bool accept)
        {
            if (!waitingChoice) return;
            waitingChoice = false;
            choicePanel.SetActive(false);
            ignoreFrame = Time.frameCount; // 버튼 클릭이 같은 프레임에 대사 진행으로 처리되지 않게
            replied = true;
            Display(onChoice?.Invoke(accept) ?? "");
        }
    }
}
