using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 화면 하단 손 칸 1개 (SFR-021)
    //  - 끌어서 기기 화면·픽업대에 놓기, 기기 화면의 물건을 끌어와 놓기. 끄는 동안 칸은 비어 보이고, 놓을 곳이 없으면 칸으로 날아 돌아온다
    //  - 왼쪽 클릭: 선택 (선택한 뒤 컵 자리를 클릭해 놓기, SFR-034) / 오른쪽 클릭: '버리기' 메뉴
    public class HandSlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
    {
        [SerializeField] int hand;
        [SerializeField] Image frame;
        [SerializeField] Image icon;
        [SerializeField] HandInventoryView owner;

        static readonly Color Normal = Color.white;
        static readonly Color Selected = new Color32(0xFF, 0xD8, 0x8A, 0xFF);

        DragGhost ghost;      // 끄는 중
        DragGhost returning;  // 놓은 뒤 판단·복귀 중
        HeldItem dragged;     // 끌기 시작할 때 들고 있던 물건
        bool lifted;          // 아이콘을 들어 올려 칸이 비어 보이는 중

        public int Hand => hand;

        public void Refresh(HeldItem item, bool selected)
        {
            icon.sprite = ItemIcons.Of(item);
            ShowIcon();
            frame.color = selected ? Selected : Normal;
        }

        void ShowIcon() => icon.enabled = icon.sprite != null && !lifted;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (HandInventory.Instance[hand] == null) return;
            if (eventData.button == PointerEventData.InputButton.Right) owner.OpenMenu(this);
            else if (eventData.button == PointerEventData.InputButton.Left && !eventData.dragging) HandInventory.Instance.Select(hand);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (HandInventory.Instance[hand] == null) return;
            if (returning != null) returning.Finish();
            ghost = DragGhost.Create(icon, owner.DragLayer, eventData.position); // 칸에서 떼어 커서를 따라다님
            dragged = HandInventory.Instance[hand];
            lifted = true;
            ShowIcon();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ghost != null) ghost.Follow(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData) => Release();

        // 끄는 중에 손 칸이 숨겨져도(주문 채팅 등) 아이콘이 화면에 남지 않게
        void OnDisable() => Release();

        // 놓은 뒤에도 같은 물건을 들고 있으면(받아 준 곳이 없음) 아이콘이 칸으로 날아 돌아온다
        void Release()
        {
            if (ghost == null) return;
            var item = dragged;
            dragged = null;
            returning = ghost;
            ghost = null;
            returning.Release(icon.transform, () => HandInventory.Instance != null && HandInventory.Instance[hand] == item, Land);
        }

        void Land()
        {
            returning = null;
            if (this == null) return;
            lifted = false;
            ShowIcon();
        }

        public void OnDrop(PointerEventData eventData) => HandInventory.Instance.RaiseDropped(hand, eventData);
    }
}
