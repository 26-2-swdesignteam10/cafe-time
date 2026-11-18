using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] TMP_Text statusText;
        [Tooltip("단체 주문일 때만 보이는 줄: '단체 주문' 글자 이미지 + 마감 시각 (ADR-0042)")]
        [SerializeField] GameObject groupRow;
        [SerializeField] TMP_Text deadlineText;

        public void Setup(Order order, int placed, bool called = false)
        {
            numberText.text = $"#{order.Number}";
            var sb = new StringBuilder();
            for (int i = 0; i < order.Lines.Count; i++)
            {
                var line = order.Lines[i];
                if (i > 0) sb.Append('\n');
                sb.Append(OrderText.ItemLabel(line.Menu, line.Temperature));
                sb.Append($"\n<size=18><pos=55%>{line.Quantity}개<pos=72%>{line.Price:N0}원</size>");
            }
            linesText.text = sb.ToString();
            totalText.text = $"{order.Paid:N0}원";
            pickupText.text = $"{placed}/{order.ItemCount}";
            if (statusText != null) statusText.text = called ? "손님 호출 중" : placed >= order.ItemCount ? "픽업 준비 완료" : "음료 준비 중";
            GroupDeadline.Show(order, groupRow, deadlineText);
            gameObject.SetActive(true);
            // SFR-013: 줄바꿈까지 계산한 종이 높이를 가로 목록이 배치하기 전에 확정한다
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }
    }
}
