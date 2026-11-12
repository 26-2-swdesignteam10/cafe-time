using System;
using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 포스기 화면 (SFR-026, 027). 플레이어가 손님 대사를 보고 직접 입력하고, 잘못 입력해도 그대로 주문서에 들어간다
    public class PosScreen : Popup
    {
        [Header("메뉴")]
        [Tooltip("MenuCategory 순서대로 (커피, 음료, 스무디·쉐이크, 디저트)")]
        [SerializeField] Button[] categoryTabs;
        [SerializeField] Sprite tabNormal;
        [SerializeField] Sprite tabSelected;
        [SerializeField] Transform menuContainer;
        [SerializeField] MenuButtonView menuTemplate;
        [SerializeField] TemperatureSelector temperature;
        [Tooltip("메뉴 카드 그림: pictureMenuIds[i] 메뉴에 pictures[i] (음료는 Art/UI/DrinkResult 완성품 그림)")]
        [SerializeField] string[] pictureMenuIds;
        [SerializeField] Sprite[] pictures;

        [Header("주문 목록")]
        [SerializeField] Transform rowContainer;
        [SerializeField] OrderRowView rowTemplate;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;
        [SerializeField] Button deleteButton;
        [SerializeField] Button cancelButton;
        [SerializeField] Button payButton;
        [SerializeField] TMP_Text totalText;
        [SerializeField] ReceiptPopup receipt;

        [Header("손님 주문 채팅")]
        [SerializeField] OrderChatView chat;
        [Tooltip("포스기 화면이 열려 있는 동안 숨길 HUD (주문 채팅 자리와 겹치는 양손 칸)")]
        [SerializeField] GameObject[] hideWhileOpen;

        readonly List<MenuButtonView> menuButtons = new List<MenuButtonView>();
        readonly List<OrderRowView> rows = new List<OrderRowView>();
        readonly List<OrderLine> lines = new List<OrderLine>();
        CustomerParty party;
        int selected = -1;
        int openedFrame = -1;

        protected override void Awake()
        {
            base.Awake();
            menuTemplate.gameObject.SetActive(false);
            rowTemplate.gameObject.SetActive(false);
            foreach (var menu in GameBalance.Instance.Menus)
            {
                var view = Instantiate(menuTemplate, menuContainer);
                view.Setup(menu, Add);
                int picture = Array.IndexOf(pictureMenuIds, menu.Id);
                if (picture >= 0) view.SetIcon(pictures[picture]);
                menuButtons.Add(view);
            }
            for (int i = 0; i < categoryTabs.Length; i++)
            {
                var category = (MenuCategory)i;
                categoryTabs[i].onClick.AddListener(() => ShowCategory(category));
            }
            minusButton.onClick.AddListener(() => ChangeQuantity(-1));
            plusButton.onClick.AddListener(() => ChangeQuantity(+1));
            deleteButton.onClick.AddListener(DeleteSelected);
            cancelButton.onClick.AddListener(() => { lines.Clear(); selected = -1; Refresh(); }); // 주문 취소: 목록 비우기
            payButton.onClick.AddListener(Pay);
        }

        public void Open(CustomerParty customerParty)
        {
            party = customerParty;
            openedFrame = Time.frameCount;
            lines.Clear();
            selected = -1;
            Open();
            ShowCategory(MenuCategory.Coffee);
            Refresh();
            if (chat != null) chat.Show(party.Sentences); // 주문 대사를 보면서 입력하도록 함께 표시
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetHudHidden(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SetHudHidden(false);
        }

        void SetHudHidden(bool hidden)
        {
            if (hideWhileOpen == null) return;
            foreach (var go in hideWhileOpen)
                if (go != null) go.SetActive(!hidden);
        }

        void ShowCategory(MenuCategory category)
        {
            foreach (var view in menuButtons) view.gameObject.SetActive(view.Menu.Category == category);
            for (int i = 0; i < categoryTabs.Length; i++)
                categoryTabs[i].image.sprite = i == (int)category ? tabSelected : tabNormal;
        }

        void Add(MenuDefinition menu)
        {
            var temp = temperature.For(menu);
            int index = lines.FindIndex(l => l.Matches(menu, temp));
            if (index >= 0) lines[index].Quantity++;
            else
            {
                lines.Add(new OrderLine(menu, temp, 1));
                index = lines.Count - 1;
            }
            selected = index;
            Refresh();
        }

        void ChangeQuantity(int delta)
        {
            if (selected < 0) return;
            lines[selected].Quantity += delta;
            if (lines[selected].Quantity <= 0) DeleteSelected();
            else Refresh();
        }

        // Enter 로 결제 (SFR-027). 포스기를 연 바로 그 프레임의 Enter(대화 넘기기 등)는 무시
        void Update()
        {
            // 마우스로 누른 버튼이 선택 상태로 남으면 Enter 가 그 버튼을 다시 눌러 버리므로 선택을 풀어 둔다
            var events = EventSystem.current;
            if (events != null && events.currentSelectedGameObject != null) events.SetSelectedGameObject(null);

            if (Time.frameCount == openedFrame) return;
            var keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame))
                Pay();
        }

        void DeleteSelected()
        {
            if (selected < 0) return;
            lines.RemoveAt(selected);
            selected = Mathf.Min(selected, lines.Count - 1);
            Refresh();
        }

        void Refresh()
        {
            while (rows.Count < lines.Count) rows.Add(Instantiate(rowTemplate, rowContainer));
            for (int i = 0; i < rows.Count; i++)
            {
                if (i >= lines.Count) { rows[i].gameObject.SetActive(false); continue; }
                int index = i;
                rows[i].Setup(lines[i], i == selected, () => { selected = index; Refresh(); });
            }
            int total = lines.Sum(l => l.Price);
            totalText.text = $"₩{total:N0}";
            payButton.interactable = lines.Count > 0; // 주문 목록이 비어 있으면 결제 비활성화
            bool hasSelection = selected >= 0;
            minusButton.interactable = plusButton.interactable = deleteButton.interactable = hasSelection;
        }

        void Pay()
        {
            if (lines.Count == 0 || party == null) return;
            var order = CustomerManager.Instance.TakeOrder(party, lines.Select(l => new OrderLine(l.Menu, l.Temperature, l.Quantity)).ToList(),
                lines.Sum(l => l.Price));
            party = null;
            Close();
            if (order != null) receipt.Open(order); // 결제 직전에 손님이 떠났으면 주문 없음
        }
    }
}
