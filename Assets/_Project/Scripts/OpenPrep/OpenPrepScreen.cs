using System.Collections.Generic;
using System.Text;
using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // 오픈 준비 화면 (SFR-010): 보유 자금으로 재료를 장바구니에 담아 구매한 뒤 영업 시작
    public class OpenPrepScreen : MonoBehaviour
    {
        // 고정 문구(오픈 준비, 보유금, 탭 이름, 총액 ...)는 씬의 이미지 라벨이고, 여기서는 바뀌는 값만 다룬다
        [Header("상단")]
        [SerializeField] TMP_Text dayText;   // "DAY 3"
        [SerializeField] TMP_Text moneyText; // "₩182,000"

        [Header("재료 목록")]
        [Tooltip("IngredientCategory 순서대로 (커피, 우유·액체, 시럽·청, 티, 스무디, 디저트)")]
        [SerializeField] Button[] categoryTabs;
        [SerializeField] Sprite tabNormal;
        [SerializeField] Sprite tabSelected;
        [SerializeField] Transform cardContainer;
        [SerializeField] IngredientCard cardPrefab;

        [Header("장바구니")]
        [SerializeField] TMP_Text cartText;
        [SerializeField] GameObject cartEmptyLabel;   // "담은 재료가 없습니다."
        [SerializeField] TMP_Text totalText;          // "₩31,500"
        [SerializeField] TMP_Text remainingText;      // 구매 후 잔액 = 보유금 - 총액
        [SerializeField] TMP_Text rentText;           // 오늘 임대료 (하루 정산 때 낸다, SFR-071)
        [SerializeField] GameObject boughtLabel;      // "구매 완료!"
        [SerializeField] Button buyButton;
        [SerializeField] Button startButton;
        [SerializeField] ConfirmPopup confirmPopup;
        [SerializeField] Sprite unboughtMessage;      // "장바구니에 구매하지 않은 재료가 있습니다. 그대로 영업을 시작할까요?"

        [SerializeField] Color normalTextColor = new Color32(0x5A, 0x35, 0x20, 0xFF);
        [SerializeField] Color warningColor = new Color32(0xC0, 0x39, 0x2B, 0xFF);

        readonly List<IngredientCard> cards = new List<IngredientCard>();
        readonly Dictionary<IngredientDefinition, int> cart = new Dictionary<IngredientDefinition, int>(); // 구매 단위 수량
        IngredientCategory currentCategory;

        SaveData Save => SaveSystem.Current;

        void Start()
        {
            SaveSystem.EnsureCurrentForPreview();
            var balance = GameBalance.Instance;
            if (balance == null) return;

            foreach (var definition in balance.Ingredients)
            {
                var card = Instantiate(cardPrefab, cardContainer);
                card.Init(definition, ChangeQuantity);
                cards.Add(card);
            }
            for (int i = 0; i < categoryTabs.Length; i++)
            {
                var category = (IngredientCategory)i;
                categoryTabs[i].onClick.AddListener(() => SelectCategory(category));
            }
            buyButton.onClick.AddListener(Buy);
            startButton.onClick.AddListener(TryStartBusiness);

            dayText.text = $"DAY {Save.day}";
            boughtLabel.SetActive(false);
            SelectCategory(IngredientCategory.Coffee);
            Refresh();
        }

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                Popup.CloseTop();
        }

        void SelectCategory(IngredientCategory category)
        {
            currentCategory = category;
            foreach (var card in cards)
                card.gameObject.SetActive(card.Definition.Category == category);
            for (int i = 0; i < categoryTabs.Length; i++)
                categoryTabs[i].image.sprite = i == (int)category ? tabSelected : tabNormal;
        }

        void ChangeQuantity(IngredientDefinition definition, int delta)
        {
            cart.TryGetValue(definition, out int quantity);
            quantity = Mathf.Max(0, quantity + delta);
            if (quantity == 0) cart.Remove(definition);
            else cart[definition] = quantity;
            boughtLabel.SetActive(false);
            Refresh();
        }

        int CartTotal()
        {
            int total = 0;
            foreach (var pair in cart) total += pair.Key.Price * pair.Value; // 단가 × 수량
            return total;
        }

        void Refresh()
        {
            moneyText.text = $"₩{Save.money:N0}";

            foreach (var card in cards)
            {
                cart.TryGetValue(card.Definition, out int quantity);
                card.Refresh(Save.GetStock(card.Definition.Id), quantity);
            }

            var sb = new StringBuilder();
            foreach (var pair in cart)
                sb.Append($"{pair.Key.DisplayName} x{pair.Value}<pos=68%>₩{pair.Key.Price * pair.Value:N0}\n");
            cartText.text = sb.ToString();
            cartEmptyLabel.SetActive(cart.Count == 0);

            int total = CartTotal();
            bool affordable = total <= Save.money;
            totalText.text = $"₩{total:N0}";
            totalText.color = affordable ? normalTextColor : warningColor;
            int remaining = Save.money - total;
            remainingText.text = remaining < 0 ? $"-₩{-remaining:N0}" : $"₩{remaining:N0}";
            remainingText.color = affordable ? normalTextColor : warningColor;
            rentText.text = $"₩{GameBalance.Instance.GetRent(Save.day):N0}";
            buyButton.interactable = total > 0 && affordable; // 총액이 보유금보다 많으면 비활성화
        }

        void Buy()
        {
            int total = CartTotal();
            if (total <= 0 || total > Save.money) return;

            Save.money -= total;
            foreach (var pair in cart)
                Save.AddStock(pair.Key.Id, pair.Key.AmountPerUnit * pair.Value);
            cart.Clear();
            // 저장은 하루 정산 때만 한다 (ADR-0005). 영업 도중 종료하면 구매도 취소된다.
            Refresh();
            boughtLabel.SetActive(true);
        }

        void TryStartBusiness()
        {
            if (cart.Count > 0)
                confirmPopup.Open(unboughtMessage, StartBusiness);
            else
                StartBusiness();
        }

        static void StartBusiness()
        {
            DayResult.StartNewDay();
            SceneLoader.LoadFirstAvailable(SceneNames.Cafe, SceneNames.Settlement);
        }
    }
}
