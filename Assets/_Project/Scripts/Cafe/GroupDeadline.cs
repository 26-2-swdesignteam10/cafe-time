using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 주문 내역 영수증·픽업대 트레이의 단체 주문 표시 줄 ('단체 주문' 글자 이미지 + "~16:00") (ADR-0042)
    public static class GroupDeadline
    {
        public static void Show(Order order, GameObject row, TMP_Text deadline)
        {
            if (row == null) return;
            row.SetActive(order.IsGroup);
            if (order.IsGroup && deadline != null) deadline.text = $"~{order.Group.DeadlineText}";
        }
    }
}
