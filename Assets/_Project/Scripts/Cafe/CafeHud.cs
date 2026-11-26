using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 영업 중 상시 표시 UI (SFR-013): DAY, 시간, 보유 자금, 평판, 주문 내역·레시피 버튼 (재료 재고는 표시하지 않음, ADR-0023)
    // DAY 는 DAY별 그림, 보유금은 코인 아이콘 + "150,000원", 평판은 별 + 소수 첫째 자리 숫자. 임대료는 표시하지 않는다 (2026-11-17 사용자 요청)
    public class CafeHud : MonoBehaviour
    {
        [Header("상태")]
        [SerializeField] Image dayImage;
        [Tooltip("DAY 1 ~ 15 그림 (Art/UI/CafeHud/Day1~15.png)")]
        [SerializeField] Sprite[] daySprites;
        [SerializeField] TMP_Text dayText; // 그림이 없을 때만 글자로 표시
        [SerializeField] TMP_Text clockText;
        [Tooltip("마감 시각(21:00)을 넘기면 시계 글자색 (ADR-0055)")]
        [SerializeField] Color overtimeColor = new Color32(0xD9, 0x3A, 0x2E, 0xFF);
        [SerializeField] TMP_Text moneyText;
        [SerializeField] Image[] stars;
        [SerializeField] TMP_Text reputationText; // 별 옆 "3.4"
        [SerializeField] Sprite starFull;
        [SerializeField] Sprite starEmpty;

        [Header("주문 내역·레시피 버튼")]
        [SerializeField] Button ordersButton;
        [SerializeField] Button recipeButton;
        [SerializeField] OrdersScreen ordersScreen;
        [SerializeField] RecipeBookScreen recipeScreen;
        [SerializeField] GameObject orderBadge;   // 대기 중인 주문 수 (0이면 숨김)
        [SerializeField] TMP_Text orderCountText;

        static SaveData Save => SaveSystem.Current;

        Color clockColor;

        void Start()
        {
            int day = Save.day;
            var daySprite = daySprites != null && day >= 1 && day <= daySprites.Length ? daySprites[day - 1] : null;
            if (dayImage != null)
            {
                dayImage.sprite = daySprite;
                dayImage.enabled = daySprite != null;
            }
            if (dayText != null)
            {
                dayText.text = $"DAY {day}";
                dayText.enabled = daySprite == null;
            }
            clockColor = clockText.color;
            ordersButton.onClick.AddListener(ordersScreen.Open);
            recipeButton.onClick.AddListener(recipeScreen.Open);
            OrderBoard.Instance.Changed += RefreshOrderCount;
            RefreshOrderCount();
        }

        void OnDestroy()
        {
            if (OrderBoard.Instance != null) OrderBoard.Instance.Changed -= RefreshOrderCount;
        }

        void Update()
        {
            clockText.text = GameClock.Instance.TimeText;
            clockText.color = GameClock.Instance.IsOvertime ? overtimeColor : clockColor;
            moneyText.text = $"{Save.money:N0}원";
            int filled = Mathf.RoundToInt(Save.reputation);
            for (int i = 0; i < stars.Length; i++) stars[i].sprite = i < filled ? starFull : starEmpty;
            if (reputationText != null) reputationText.text = Save.reputation.ToString("0.0");
        }

        void RefreshOrderCount()
        {
            int count = OrderBoard.Instance.Orders.Count;
            orderBadge.SetActive(count > 0);
            orderCountText.text = count.ToString();
        }
    }
}
