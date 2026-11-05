using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 화면 상단 주문표 1장 (SFR-013): 주문 번호, 메뉴명, HOT/ICE, 수량. 누르면 픽업대 배정(SFR-056)
    public class TicketView : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TMP_Text numberText;
        [SerializeField] TMP_Text linesText;

        public void Setup(Order order)
        {
            numberText.text = $"#{order.Number}";
            var sb = new StringBuilder();
            foreach (var line in order.Lines)
                sb.Append($"{OrderText.ItemLabel(line.Menu, line.Temperature)} x{line.Quantity}\n");
            linesText.text = sb.ToString();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OrderBoard.Instance.ClickTicket(order));
            gameObject.SetActive(true);
        }
    }
}
