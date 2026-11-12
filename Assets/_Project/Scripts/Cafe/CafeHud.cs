using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 영업 중 상시 표시 UI (SFR-013): DAY, 시간, 보유 자금, 당일 임대료, 평판, 주문 내역·레시피 버튼 (재료 재고는 표시하지 않음, ADR-0023)
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

        [Header("주문 내역·레시피 버튼")]
        [SerializeField] Button ordersButton;
        [SerializeField] Button recipeButton;
        [SerializeField] OrdersScreen ordersScreen;
        [SerializeField] RecipeBookScreen recipeScreen;
        [SerializeField] GameObject orderBadge;   // 대기 중인 주문 수 (0이면 숨김)
        [SerializeField] TMP_Text orderCountText;

        GameBalance balance;

        static SaveData Save => SaveSystem.Current;

        void Start()
        {
            balance = GameBalance.Instance;
            dayText.text = $"DAY {Save.day}";
            rentText.text = $"₩{balance.GetRent(Save.day):N0}";
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
            moneyText.text = $"₩{Save.money:N0}";
            int filled = Mathf.RoundToInt(Save.reputation);
            for (int i = 0; i < stars.Length; i++) stars[i].sprite = i < filled ? starFull : starEmpty;
        }

        void RefreshOrderCount()
        {
            int count = OrderBoard.Instance.Orders.Count;
            orderBadge.SetActive(count > 0);
            orderCountText.text = count.ToString();
        }
    }
}
