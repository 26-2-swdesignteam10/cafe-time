using System;
using CafeTime.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CafeTime.Cafe
{
    // 손 인벤토리 (SFR-021): 4칸, 칸당 1개, 겹쳐 들 수 없음 (2026-11-17 사용자 요청으로 2칸 → 4칸, ADR-0038)
    public class HandInventory : MonoBehaviour
    {
        public const int SlotCount = 4;

        public static HandInventory Instance { get; private set; }

        readonly HeldItem[] hands = new HeldItem[SlotCount];

        public event Action Changed;

        // 기기 화면의 물건(스쿱 등)을 손 칸으로 끌어 놓았을 때 (SFR-040)
        public event Action<int, PointerEventData> DroppedOnHand;

        // 클릭으로 선택한 손 (SFR-034 "인벤토리에서 빈 컵을 선택한 뒤 컵 자리를 클릭"). 없으면 -1
        public int Selected { get; private set; } = -1;

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public HeldItem this[int hand] => hands[hand];

        public bool HasSpace => Array.IndexOf(hands, null) >= 0;

        public bool Holds<T>() where T : HeldItem => Array.Exists(hands, h => h is T);

        // 조건에 맞는 물건을 든 첫 칸. 없으면 -1
        public int Find(Predicate<HeldItem> match) => Array.FindIndex(hands, h => h != null && match(h));

        // 빈 칸(왼쪽부터)에 든다. 모든 칸이 차 있으면 false → 호출한 화면이 '인벤토리가 가득 찼습니다.' 표시
        public bool TryAdd(HeldItem item)
        {
            int hand = Array.IndexOf(hands, null);
            if (hand < 0) return false;
            hands[hand] = item;
            Changed?.Invoke();
            return true;
        }

        public HeldItem Take(int hand)
        {
            var item = hands[hand];
            hands[hand] = null;
            if (Selected == hand) Selected = -1;
            if (item != null) Changed?.Invoke();
            return item;
        }

        // 손에 든 물건의 상태(샷·얼음 등)를 바꾼 뒤 화면 갱신
        public void NotifyChanged() => Changed?.Invoke();

        public void Select(int hand)
        {
            Selected = Selected == hand || hands[hand] == null ? -1 : hand;
            Changed?.Invoke();
        }

        public void RaiseDropped(int hand, PointerEventData eventData) => DroppedOnHand?.Invoke(hand, eventData);

        // 우클릭 메뉴 '버리기' (SFR-021)
        //  - 커피잔·접시: 내용물만 버리고 잔·접시는 설거지 대기로
        //  - 샷이 든 컵: 샷만 버리고 빈 컵은 손에 남음 / 얼음이 든 스쿱도 같음
        //  - 빈 샷 컵·빈 스쿱·행주: 제자리에 내려놓음
        //  - 퇴식구에서 가져온 더러운 잔·접시: 버릴 수 없음 (설거지 대기 바구니에 직접 넣어야 함, ADR-0052)
        //  버린 재료·음료는 감점 사항 (SFR-068)
        public void Discard(int hand)
        {
            var item = hands[hand];
            switch (item)
            {
                case DirtyDish _:
                    break;
                case Cup cup:
                    if (!cup.IsEmpty) Penalty();
                    Take(hand);
                    DishStation.Instance.Enqueue(cup.Type == CupType.Ceramic ? TrayItemKind.DirtyCeramic : TrayItemKind.DirtyPlastic);
                    break;
                case DessertItem _:
                    Penalty();
                    Take(hand);
                    DishStation.Instance.Enqueue(TrayItemKind.DirtyPlate);
                    break;
                case ShotGlass glass when glass.HasShot:
                    glass.HasShot = false;
                    Penalty();
                    Changed?.Invoke();
                    break;
                case ShotGlass glass:
                    Take(hand);
                    CafeTime.EspressoShot.ShotCupSlot.Return(glass); // SFR-034
                    break;
                case IceScoop scoop when scoop.HasIce:
                    scoop.HasIce = false; // 얼음은 구매 재료가 아니라 감점 없음
                    Changed?.Invoke();
                    break;
                case IceScoop _:
                    Take(hand);
                    IceMaker.Instance.ReturnScoop();
                    break;
                default:
                    Take(hand); // 행주는 행주 걸이로
                    break;
            }
        }

        static void Penalty()
        {
            if (DayResult.Today != null) DayResult.Today.itemsDiscarded++;
        }
    }
}
