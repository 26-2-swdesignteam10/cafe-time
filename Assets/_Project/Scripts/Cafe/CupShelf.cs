using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 커피잔 선반의 깨끗한 잔·접시 재고 (SFR-028, 064, 065). 총 개수는 고정, 매일 영업 시작 때 모두 선반에 있다.
    // 사용한 잔·접시는 퇴식구 → 설거지 → 건조를 거쳐야 돌아온다
    public class CupShelf : MonoBehaviour
    {
        public static CupShelf Instance { get; private set; }

        int ceramic;
        int plastic;
        int plates;

        public event Action Changed;

        public int Plates => plates;

        void Awake()
        {
            Instance = this;
            var balance = GameBalance.Instance;
            ceramic = balance.CeramicCups;
            plastic = balance.PlasticCups;
            plates = balance.Plates;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public int Count(CupType type) => type == CupType.Ceramic ? ceramic : plastic;

        // 선택한 종류의 빈 커피잔 1개를 손에 든다 (SFR-028)
        public void TakeCup(CupType type)
        {
            if (Count(type) <= 0)
            {
                Toast.Show(ToastMessage.NoCleanCup);
                return;
            }
            if (!HandInventory.Instance.TryAdd(new Cup(type)))
            {
                Toast.Show(ToastMessage.HandsFull);
                return;
            }
            if (type == CupType.Ceramic) ceramic--;
            else plastic--;
            Changed?.Invoke();
        }

        // 손에 든 빈 커피잔을 선반에 되돌려 놓음 (SFR-028). 내용물이 든 잔은 안 됨 (버리기는 손 칸 메뉴로)
        public bool ReturnCup(int hand)
        {
            if (!(HandInventory.Instance[hand] is Cup cup)) return false;
            if (!cup.IsEmpty)
            {
                Toast.Show(ToastMessage.EmptyCupOnly);
                return false;
            }
            HandInventory.Instance.Take(hand);
            if (cup.Type == CupType.Ceramic) ceramic++;
            else plastic++;
            Changed?.Invoke();
            return true;
        }

        // 디저트를 담을 접시 (SFR-064). 없으면 false
        public bool TryTakePlate()
        {
            if (plates <= 0) return false;
            plates--;
            Changed?.Invoke();
            return true;
        }

        // 건조가 끝난 잔·접시 (SFR-065)
        public void Return(TrayItemKind dish)
        {
            if (dish == TrayItemKind.DirtyCeramic) ceramic++;
            else if (dish == TrayItemKind.DirtyPlastic) plastic++;
            else if (dish == TrayItemKind.DirtyPlate) plates++;
            Changed?.Invoke();
        }
    }
}
