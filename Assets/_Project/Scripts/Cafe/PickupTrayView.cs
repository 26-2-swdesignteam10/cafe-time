using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 픽업대 트레이 1개 = 주문 1건 (SFR-056): 주문 번호, 메뉴 줄, 음료 자리(주문 개수만큼), 서빙 버튼.
    // 빈 자리에는 놓아야 할 음료 그림을 흐리게 보여 준다. 트레이·빈 자리를 누르거나 손 칸에서 끌어다 놓으면 놓이고,
    // 놓인 음료를 누르면 손으로 되돌린다
    public class PickupTrayView : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        [SerializeField] PickupScreen owner;
        [SerializeField] TMP_Text numberText;
        [SerializeField] TMP_Text linesText;
        [SerializeField] Transform slotContainer;
        [SerializeField] Button slotTemplate;   // 자리 그림 + 자식 "Icon"
        [SerializeField] Button serveButton;
        [SerializeField, Range(0f, 1f)] float hintAlpha = 0.35f; // 빈 자리의 놓아야 할 음료 그림

        readonly List<Button> slots = new List<Button>();
        Order order;

        void Awake()
        {
            slotTemplate.gameObject.SetActive(false);
            serveButton.onClick.AddListener(() => owner.Serve(order));
        }

        public void Setup(Order order, IReadOnlyList<HeldItem> placed)
        {
            this.order = order;
            numberText.text = $"#{order.Number}";
            var sb = new StringBuilder();
            for (int i = 0; i < order.Lines.Count; i++)
            {
                var line = order.Lines[i];
                if (i > 0) sb.Append('\n');
                sb.Append($"{OrderText.ItemLabel(line.Menu, line.Temperature)} x{line.Quantity}");
            }
            linesText.text = sb.ToString();

            // 자리마다 주문한 음료 1개. 놓인 물건은 맞는 메뉴 자리부터, 맞는 자리가 없으면 남은 빈 자리에
            var wanted = order.Lines.SelectMany(l => Enumerable.Repeat(l, l.Quantity)).ToList();
            var filled = new HeldItem[wanted.Count];
            var rest = new List<HeldItem>();
            foreach (var item in placed)
            {
                int index = wanted.FindIndex(w => w.Matches(item.Menu, item.Temperature));
                while (index >= 0 && filled[index] != null)
                    index = wanted.FindIndex(index + 1, w => w.Matches(item.Menu, item.Temperature));
                if (index >= 0) filled[index] = item;
                else rest.Add(item);
            }
            for (int i = 0; i < filled.Length && rest.Count > 0; i++)
            {
                if (filled[i] != null) continue;
                filled[i] = rest[0];
                rest.RemoveAt(0);
            }

            while (slots.Count < wanted.Count) slots.Add(Instantiate(slotTemplate, slotContainer));
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].gameObject.SetActive(i < wanted.Count);
                if (i >= wanted.Count) continue;
                var item = filled[i];
                var icon = slots[i].transform.Find("Icon").GetComponent<Image>();
                icon.sprite = item != null ? ItemIcons.Of(item) : ItemIcons.ForMenu(wanted[i].Menu, wanted[i].Temperature);
                icon.enabled = icon.sprite != null;
                icon.color = new Color(1f, 1f, 1f, item != null ? 1f : hintAlpha);
                slots[i].onClick.RemoveAllListeners();
                if (item != null) slots[i].onClick.AddListener(() => owner.TakeBack(order, item));
                else slots[i].onClick.AddListener(() => owner.Click(order));
            }

            serveButton.interactable = placed.Count > 0;
            gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) owner.Click(order);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var hand = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<HandSlotView>() : null;
            if (hand != null) owner.PlaceFromHand(order, hand.Hand);
        }
    }
}
