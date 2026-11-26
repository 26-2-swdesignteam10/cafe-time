using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 화면 하단 양손 칸 + 우클릭 메뉴 (SFR-021)
    public class HandInventoryView : MonoBehaviour
    {
        [SerializeField] HandSlotView[] slots;
        [SerializeField] RectTransform contextMenu;
        [SerializeField] Button discardButton;
        [SerializeField] Transform dragLayer;

        int menuHand = -1;

        public Transform DragLayer => dragLayer;

        public HandSlotView Slot(int hand) => System.Array.Find(slots, s => s.Hand == hand);

        // 지금 손 칸 상태 (FlyToNewSlot 에 넘길 '꺼내기 전' 상태)
        public static HeldItem[] Snapshot()
        {
            var items = new HeldItem[HandInventory.SlotCount];
            for (int i = 0; i < items.Length; i++) items[i] = HandInventory.Instance[i];
            return items;
        }

        // 기기 화면에서 꺼낸 물건이 source 그림 자리에서 새로 채워진 손 칸으로 날아간다 (컵 선반·샷 컵 공용)
        public static void FlyToNewSlot(HeldItem[] before, Image source, float seconds)
        {
            int hand = -1;
            for (int i = 0; i < before.Length; i++)
                if (before[i] == null && HandInventory.Instance[i] != null) { hand = i; break; }
            if (hand < 0 || source == null) return;
            var view = FindFirstObjectByType<HandInventoryView>();
            var slot = view != null ? view.Slot(hand) : null;
            if (slot == null) return;
            var rt = source.rectTransform;
            float scale = slot.IconHeight > 0 ? rt.rect.height * rt.lossyScale.y / slot.IconHeight : 1f;
            slot.FlyIn(rt.TransformPoint(rt.rect.center), Mathf.Clamp(scale, 0.5f, 2.5f), seconds);
        }

        void Start()
        {
            HandInventory.Instance.Changed += Refresh;
            discardButton.onClick.AddListener(Discard);
            contextMenu.gameObject.SetActive(false);
            Refresh();
        }

        void OnDestroy()
        {
            if (HandInventory.Instance != null) HandInventory.Instance.Changed -= Refresh;
        }

        void Update()
        {
            // 메뉴 바깥을 누르면 닫힘
            if (menuHand >= 0 && UnityEngine.InputSystem.Mouse.current != null &&
                UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame &&
                !RectTransformUtility.RectangleContainsScreenPoint(contextMenu, UnityEngine.InputSystem.Mouse.current.position.ReadValue()))
                CloseMenu();
        }

        void Refresh()
        {
            foreach (var slot in slots) slot.Refresh(HandInventory.Instance[slot.Hand], HandInventory.Instance.Selected == slot.Hand);
        }

        public void OpenMenu(HandSlotView slot)
        {
            if (HandInventory.Instance[slot.Hand] is DirtyDish) return; // 버릴 수 없는 물건 (ADR-0052)
            menuHand = slot.Hand;
            contextMenu.position = slot.transform.position;
            contextMenu.gameObject.SetActive(true);
        }

        void Discard()
        {
            if (menuHand >= 0) HandInventory.Instance.Discard(menuHand);
            CloseMenu();
        }

        void CloseMenu()
        {
            menuHand = -1;
            contextMenu.gameObject.SetActive(false);
        }
    }
}
