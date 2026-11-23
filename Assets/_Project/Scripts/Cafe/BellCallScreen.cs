using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 진동벨 호출 창 (ADR-0040): 계산기처럼 번호를 눌러 주문 번호를 입력하고 호출한다.
    // 호출에 성공하면 창을 닫고, 없는 번호·빈 트레이면 화면에 안내만 띄운다. 키보드 숫자·Backspace·Enter 도 받는다
    public class BellCallScreen : Popup
    {
        [SerializeField] PickupScreen pickup;
        [Tooltip("0~9 순서")]
        [SerializeField] Button[] digitButtons = new Button[10];
        [SerializeField] Button backButton;
        [SerializeField] Button clearButton;
        [SerializeField] Button callButton;
        [SerializeField] TMP_Text numberText;
        [SerializeField] TMP_Text messageText;
        [SerializeField] int maxDigits = 3;

        string input = "";

        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < digitButtons.Length; i++)
            {
                int digit = i;
                digitButtons[i].onClick.AddListener(() => Type(digit));
            }
            backButton.onClick.AddListener(Back);
            clearButton.onClick.AddListener(Clear);
            callButton.onClick.AddListener(Call);
        }

        public override void Open()
        {
            base.Open();
            Clear();
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null || EventDialog.IsShowing) return; // 이벤트 대화 중 키는 대사 넘기기 (SFR-008)
            for (int i = 0; i <= 9; i++)
                if (keyboard[Key.Digit0 + i].wasPressedThisFrame || keyboard[Key.Numpad0 + i].wasPressedThisFrame) Type(i);
            if (keyboard.backspaceKey.wasPressedThisFrame) Back();
            if (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame) Call();
        }

        void Type(int digit)
        {
            if (input.Length >= maxDigits || (input.Length == 0 && digit == 0)) return;
            input += digit;
            Show("");
        }

        void Back()
        {
            if (input.Length > 0) input = input.Substring(0, input.Length - 1);
            Show("");
        }

        void Clear()
        {
            input = "";
            Show("");
        }

        void Call()
        {
            if (input.Length == 0) { Show("번호를 눌러 주세요"); return; }
            switch (pickup.Call(int.Parse(input)))
            {
                case PickupScreen.CallResult.Called: Close(); return;
                case PickupScreen.CallResult.NoSuchOrder: Show("없는 주문 번호예요"); break;
                case PickupScreen.CallResult.EmptyTray: Show("트레이에 먼저 올려 주세요"); break;
                case PickupScreen.CallResult.AlreadyCalled: Show("이미 호출한 번호예요"); break;
            }
            input = "";
            numberText.text = "";
        }

        void Show(string message)
        {
            numberText.text = input;
            messageText.text = message;
        }
    }
}
