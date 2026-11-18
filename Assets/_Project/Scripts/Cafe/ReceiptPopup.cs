using System.Text;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.Cafe
{
    // 결제 후 영수증 창 (SFR-027): 주문 번호, 메뉴, HOT/ICE, 수량, 금액, 합계. 확인 → 맵으로 복귀
    // 고정 문구(영수증, 주문번호, 메뉴·수량·금액, 합계, 확인)는 영수증 그림에 인쇄되어 있고, 값만 여기서 채운다
    public class ReceiptPopup : Popup
    {
        [SerializeField] TMP_Text numberText;
        [SerializeField] TMP_Text linesText;     // 메뉴 이름 열
        [SerializeField] TMP_Text quantityText;  // 수량 열 (없으면 이름 열에 붙임)
        [SerializeField] TMP_Text priceText;     // 금액 열 (없으면 이름 열에 붙임)
        [SerializeField] TMP_Text sumText;

        int openedFrame = -1;

        public void Open(Order order)
        {
            openedFrame = Time.frameCount;
            numberText.text = $"#{order.Number}";
            var names = new StringBuilder();
            var quantities = new StringBuilder();
            var prices = new StringBuilder();
            bool columns = quantityText != null && priceText != null;
            foreach (var line in order.Lines)
            {
                if (columns)
                {
                    names.Append(OrderText.ItemLabel(line.Menu, line.Temperature)).Append('\n');
                    quantities.Append(line.Quantity).Append('\n');
                    prices.Append($"{line.Price:N0}\n");
                }
                else names.Append($"{OrderText.ItemLabel(line.Menu, line.Temperature)} x{line.Quantity}<pos=70%>{line.Price:N0}원\n");
            }
            linesText.text = names.ToString();
            if (columns)
            {
                quantityText.text = quantities.ToString();
                priceText.text = prices.ToString();
            }
            sumText.text = $"{order.Paid:N0}원"; // Jua 에 ₩ 글자가 없어 원으로 표기 (ADR-0039)
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
