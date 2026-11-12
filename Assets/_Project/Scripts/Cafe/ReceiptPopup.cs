using System.Text;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.Cafe
{
    // 결제 후 주문 내역 창 (SFR-027): 주문 번호, 메뉴, HOT/ICE, 수량, 금액, 총합. 확인 → 맵으로 복귀
    // 고정 문구(주문 내역, 주문 번호, 총합, 확인)는 이미지, 값만 여기서 채운다
    public class ReceiptPopup : Popup
    {
        [SerializeField] TMP_Text numberText;
        [SerializeField] TMP_Text linesText;
        [SerializeField] TMP_Text sumText;

        int openedFrame = -1;

        public void Open(Order order)
        {
            openedFrame = Time.frameCount;
            numberText.text = $"#{order.Number}";
            var sb = new StringBuilder();
            foreach (var line in order.Lines)
                sb.Append($"{OrderText.ItemLabel(line.Menu, line.Temperature)} x{line.Quantity}<pos=70%>₩{line.Price:N0}\n");
            linesText.text = sb.ToString();
            sumText.text = $"₩{order.Paid:N0}";
            Open();
        }

        // Enter 로도 확인 (SFR-027). 이 창을 연 바로 그 프레임의 Enter 는 결제를 누른 키이므로 무시
        void Update()
        {
            if (Time.frameCount == openedFrame) return;
            var keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame))
                Close();
        }
    }
}
