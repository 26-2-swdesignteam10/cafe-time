using System.Collections.Generic;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 영업 중 상시 표시 UI (SFR-013): DAY, 시간, 보유 자금, 당일 임대료, 평판, 주요 재료 재고, 상단 주문표
    // 고정 문구(보유금, 임대료, 평판)는 씬의 글자 이미지이고 여기서는 값만 채운다
    public class CafeHud : MonoBehaviour
    {
        [Header("상태")]
        [SerializeField] TMP_Text dayText;
        [SerializeField] TMP_Text clockText;
        [SerializeField] TMP_Text moneyText;
        [SerializeField] TMP_Text rentText;
        [SerializeField] Image[] stars;
        [SerializeField] Sprite starFull;
        [SerializeField] Sprite starEmpty;

        [Header("주요 재료 재고")]
        [SerializeField] string[] stockIds = { "bean", "milk" };
        [SerializeField] Image[] stockIcons;
        [SerializeField] TMP_Text[] stockTexts;
        [SerializeField] Color normalColor = new Color32(0xFA, 0xEA, 0xC2, 0xFF);
        [SerializeField] Color warningColor = new Color32(0xFF, 0x7A, 0x5C, 0xFF);

        [Header("주문표")]
        [SerializeField] Transform ticketContainer;
        [SerializeField] TicketView ticketTemplate;

        readonly List<TicketView> tickets = new List<TicketView>();
        GameBalance balance;

        static SaveData Save => SaveSystem.Current;

        void Start()
        {
            balance = GameBalance.Instance;
            dayText.text = $"DAY {Save.day}";
            rentText.text = $"₩{balance.GetRent(Save.day):N0}";
            for (int i = 0; i < stockIds.Length && i < stockIcons.Length; i++)
            {
                var ingredient = balance.FindIngredient(stockIds[i]);
                stockIcons[i].sprite = ingredient != null ? ingredient.Icon : null;
            }
            ticketTemplate.gameObject.SetActive(false);
            OrderBoard.Instance.Changed += RefreshTickets;
            RefreshTickets();
        }

        void OnDestroy()
        {
            if (OrderBoard.Instance != null) OrderBoard.Instance.Changed -= RefreshTickets;
        }

        void Update()
        {
            clockText.text = GameClock.Instance.TimeText;
            moneyText.text = $"₩{Save.money:N0}";
            int filled = Mathf.RoundToInt(Save.reputation);
            for (int i = 0; i < stars.Length; i++) stars[i].sprite = i < filled ? starFull : starEmpty;
            for (int i = 0; i < stockIds.Length && i < stockTexts.Length; i++)
            {
                var ingredient = balance.FindIngredient(stockIds[i]);
                int amount = Save.GetStock(stockIds[i]);
                stockTexts[i].text = ingredient != null ? ingredient.FormatAmount(amount) : amount.ToString();
                stockTexts[i].color = amount < balance.LowStockWarning ? warningColor : normalColor; // 부족 경고 (기준 임시)
            }
        }

        void RefreshTickets()
        {
            var orders = OrderBoard.Instance.Orders;
            while (tickets.Count < orders.Count) tickets.Add(Instantiate(ticketTemplate, ticketContainer));
            for (int i = 0; i < tickets.Count; i++)
            {
                if (i < orders.Count) tickets[i].Setup(orders[i]);
                else tickets[i].gameObject.SetActive(false);
            }
        }
    }
}
