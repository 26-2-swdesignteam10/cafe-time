using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 포스기 주문 목록의 한 줄 (SFR-026). 눌러서 선택 → 수량 +/−, 선택 삭제
    public class OrderRowView : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TMP_Text text;
        [SerializeField] GameObject selectedHighlight;

        public void Setup(OrderLine line, bool selected, Action onClick)
        {
            text.text = $"{OrderText.ItemLabel(line.Menu, line.Temperature)} x{line.Quantity}<pos=75%>₩{line.Price:N0}";
            selectedHighlight.SetActive(selected);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick());
            gameObject.SetActive(true);
        }
    }
}
