using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 양손 인벤토리 (SFR-021): 왼손 / 오른손 2칸, 칸당 1개, 겹쳐 들 수 없음
    public class HandInventory : MonoBehaviour
    {
        public const int Left = 0;
        public const int Right = 1;

        public static HandInventory Instance { get; private set; }

        readonly HeldItem[] hands = new HeldItem[2];

        public event Action Changed;

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public HeldItem this[int hand] => hands[hand];

        public bool HasSpace => hands[Left] == null || hands[Right] == null;

        // 빈 손(왼손 먼저)에 든다. 양손이 차 있으면 false → 호출한 화면이 '인벤토리가 가득 찼습니다.' 표시
        public bool TryAdd(HeldItem item)
        {
            int hand = hands[Left] == null ? Left : hands[Right] == null ? Right : -1;
            if (hand < 0) return false;
            hands[hand] = item;
            Changed?.Invoke();
            return true;
        }

        public HeldItem Take(int hand)
        {
            var item = hands[hand];
            hands[hand] = null;
            if (item != null) Changed?.Invoke();
            return item;
        }

        // 우클릭 메뉴 '버리기'. 버린 음료·디저트는 감점 사항 (SFR-021, 068)
        // 잔·접시를 설거지 대기로 보내는 처리는 설거지실(SFR-063~065) 구현 시 추가
        public void Discard(int hand)
        {
            if (Take(hand) != null && DayResult.Today != null)
                DayResult.Today.itemsDiscarded++;
        }
    }
}
