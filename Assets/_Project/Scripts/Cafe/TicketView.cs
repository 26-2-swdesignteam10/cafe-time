using System.Text;
using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 주문 내역 창의 영수증 1장 (SFR-013): 주문 번호, 메뉴명·HOT/ICE·수량, 결제 금액, 픽업대에 모인 개수.
    // 높이는 메뉴 줄 수에 맞춰 늘어난다
    public class TicketView : MonoBehaviour
    {
        [SerializeField] TMP_Text numberText;
        [SerializeField] TMP_Text linesText;
        [SerializeField] TMP_Text totalText;
        [SerializeField] TMP_Text pickupText;

        public void Setup(Order order, int placed)
        {
            numberText.text = $"#{order.Number}";
            var sb = new StringBuilder();
            for (int i = 0; i < order.PreparationLines.Count; i++)
            {
                var line = order.PreparationLines[i];
                if (i > 0) sb.Append('\n');
                sb.Append($"{OrderText.ItemLabel(line.Menu, line.Temperature)} x{line.Quantity}");
            }
            linesText.text = sb.ToString();
            totalText.text = $"₩{order.Paid:N0}";
            pickupText.text = $"{placed}/{order.ItemCount}";
            gameObject.SetActive(true);
        }
    }
}
