using System.Text;
using CafeTime.UI;
using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 결제 후 주문 내역 창 (SFR-027): 주문 번호, 메뉴, HOT/ICE, 수량, 금액, 총합. 확인 → 맵으로 복귀
    // 고정 문구(주문 내역, 주문 번호, 총합, 확인)는 이미지, 값만 여기서 채운다
    public class ReceiptPopup : Popup
    {
        [SerializeField] TMP_Text numberText;
        [SerializeField] TMP_Text linesText;
        [SerializeField] TMP_Text sumText;

        public void Open(Order order)
        {
            numberText.text = $"#{order.Number}";
            var sb = new StringBuilder();
            foreach (var line in order.Lines)
                sb.Append($"{OrderText.ItemLabel(line.Menu, line.Temperature)} x{line.Quantity}<pos=70%>₩{line.Price:N0}\n");
            linesText.text = sb.ToString();
            sumText.text = $"₩{order.Paid:N0}";
            Open();
        }
    }
}
