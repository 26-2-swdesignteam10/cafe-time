using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 주문 내역 영수증·픽업대 트레이의 이벤트 주문 마감 표시 줄
    //  단체 주문: '단체 주문' 글자 이미지 + "~16:00" (ADR-0042)
    //  인플루언서: 글자 이미지 없이 "인플루언서 ~15:00" (ADR-0048, 새 글자 이미지 없이 폰트로 표시)
    public static class GroupDeadline
    {
        const string InfluencerLabel = "인플루언서";

        public static void Show(Order order, GameObject row, TMP_Text deadline)
        {
            if (row == null) return;
            row.SetActive(order.IsEvent);
            var label = row.transform.Find("Label"); // CafeUiBuilder.ReceiptRow 의 글자 이미지
            if (label != null) label.gameObject.SetActive(order.IsGroup);
            if (deadline == null) return;
            if (order.IsGroup) deadline.text = $"~{order.Group.DeadlineText}";
            else if (order.IsInfluencer) deadline.text = $"{InfluencerLabel} ~{order.Influencer.DeadlineText}";
        }
    }
}
