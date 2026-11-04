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
            foreach (var slot in slots) slot.Refresh(HandInventory.Instance[slot.Hand]);
        }

        public void OpenMenu(HandSlotView slot)
        {
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
