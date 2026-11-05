using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 퇴식구 (SFR-059~062): 쟁반 칸 n개. 쟁반 위 물건을 쓰레기통 / 설거지통으로 분류한다
    public class TrayReturn : MonoBehaviour
    {
        public static TrayReturn Instance { get; private set; }

        public Tray[] Slots { get; private set; }

        public event Action Changed;

        public bool HasSpace => Array.IndexOf(Slots, null) >= 0;

        void Awake()
        {
            Instance = this;
            Slots = new Tray[GameBalance.Instance.TrayReturnSlots];
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // 빈 쟁반 칸에 둔다. 가득 차 있으면 false → 손님이 테이블에 두고 감 (SFR-062)
        public bool TryReturn(Tray tray)
        {
            if (tray == null || tray.IsEmpty) return true;
            int free = Array.IndexOf(Slots, null);
            if (free < 0) return false;
            Slots[free] = tray;
            Changed?.Invoke();
            return true;
        }

        // 쟁반 위 물건 하나를 쓰레기통(toDishBin=false) 또는 설거지통으로. 맞지 않는 곳이면 false (SFR-061)
        public bool Sort(int slot, int index, bool toDishBin)
        {
            var tray = Slots[slot];
            if (tray == null || index < 0 || index >= tray.Items.Count) return false;
            var kind = tray.Items[index];
            if (Tray.IsTrash(kind) == toDishBin) return false;
            tray.Items.RemoveAt(index);
            if (toDishBin) DishStation.Instance.Enqueue(kind);
            if (tray.IsEmpty) Slots[slot] = null; // 모두 분류하면 쟁반이 치워짐
            Changed?.Invoke();
            return true;
        }
    }
}
