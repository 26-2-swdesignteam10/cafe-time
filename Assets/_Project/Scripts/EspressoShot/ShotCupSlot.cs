using System.Collections.Generic;
using CafeTime.Cafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.EspressoShot
{
    // SFR-034: 그룹헤드 아래 컵 자리. 샷 컵(빈 컵 / 샷이 든 컵)을 1개 놓을 수 있다.
    //  - 손 칸에서 빈 컵을 끌어다 놓거나, 손 칸을 선택한 뒤 이 자리를 클릭하면 배치
    //  - 컵이 있을 때 클릭하면 손으로 회수 (추출 중에는 불가)
    // 배경 Image(Raycast Target ON)가 있어야 클릭·드롭이 인식돼요.
    public class ShotCupSlot : MonoBehaviour, IPointerClickHandler, IDropHandler
    {
        static readonly List<ShotCupSlot> all = new List<ShotCupSlot>();

        public PortafilterSlot head;       // 위쪽 그룹헤드 슬롯
        public Image glassIcon;            // 컵 그림 (없으면 숨김)
        public bool startWithGlass = true; // 영업 시작 때 이 자리에 빈 컵이 놓여 있음 (머신에 컵 2개)

        public ShotGlass Glass { get; private set; }

        void Awake()
        {
            if (startWithGlass) Glass = new ShotGlass();
            all.Add(this);
        }

        void OnDestroy() => all.Remove(this);

        void OnEnable() => Refresh();

        void Refresh()
        {
            if (glassIcon == null) return;
            glassIcon.sprite = ItemIcons.Of(Glass);
            glassIcon.enabled = glassIcon.sprite != null;
        }

        // 추출 완료 (EspressoMachine 에서 호출)
        public void FillShot()
        {
            if (Glass == null) return;
            Glass.HasShot = true;
            Refresh();
        }

        public void OnDrop(PointerEventData eventData)
        {
            int hand = DropZone.HandOf(eventData);
            if (hand >= 0) Place(hand);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || HandInventory.Instance == null) return;
            if (Glass != null) Take();
            else if (HandInventory.Instance.Selected >= 0) Place(HandInventory.Instance.Selected);
        }

        void Place(int hand)
        {
            if (Glass != null || !(HandInventory.Instance[hand] is ShotGlass)) return;
            Glass = (ShotGlass)HandInventory.Instance.Take(hand);
            Refresh();
        }

        void Take()
        {
            if (head != null && head.Locked)
            {
                Toast.Show(ToastMessage.Extracting);
                return;
            }
            if (!HandInventory.Instance.TryAdd(Glass))
            {
                Toast.Show(ToastMessage.HandsFull);
                return;
            }
            Glass = null;
            Refresh();
        }

        // 손에서 내려놓은('버리기') 빈 샷 컵은 비어 있는 컵 자리로 돌아간다
        public static void Return(ShotGlass glass)
        {
            foreach (var slot in all)
            {
                if (slot.Glass != null) continue;
                slot.Glass = glass;
                slot.Refresh();
                return;
            }
        }
    }
}
