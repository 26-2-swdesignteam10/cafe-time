using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 픽업대 칸 1개 (SFR-056). 손 칸에서 끌어다 놓기, 눌러서 선택
    public class PickupSlotView : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        [SerializeField] PickupScreen owner;
        [SerializeField] Image background;
        [SerializeField] Image icon;
        [SerializeField] TMP_Text orderText;       // 배정된 주문 번호
        [SerializeField] GameObject unassignedLabel; // "미배정"

        static readonly Color Normal = Color.white;
        static readonly Color Selected = new Color32(0xFF, 0xD8, 0x8A, 0xFF);

        public HeldItem Item { get; set; }
        public Order Assigned { get; set; }

        public void Refresh(bool selected)
        {
            icon.sprite = ItemIcons.Of(Item);
            icon.enabled = Item != null;
            orderText.text = Assigned != null ? $"#{Assigned.Number}" : "";
            unassignedLabel.SetActive(Item != null && Assigned == null);
            background.color = selected ? Selected : Normal;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var hand = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<HandSlotView>() : null;
            if (hand != null) owner.PlaceFromHand(this, hand.Hand);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) owner.Click(this);
        }
    }
}
