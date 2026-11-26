using System;
using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 퇴식구 (SFR-059~062): 쟁반 칸 n개. 쟁반 위 쓰레기는 쓰레기통에 버리고, 잔·접시는 손으로 가져가
    // 설거지실의 설거지 대기 바구니에 넣는다 (ADR-0052, 예전 설거지통 칸은 없앰)
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

        // 쟁반 위 쓰레기 하나를 쓰레기통에. 잔·접시면 false (SFR-061)
        public bool Trash(int slot, int index)
        {
            if (!TryGet(slot, index, out var kind) || !Tray.IsTrash(kind)) return false;
            Remove(slot, index);
            return true;
        }

        // 쟁반 위 잔·접시 하나를 손 칸으로 (ADR-0052). 쓰레기이거나 손이 가득 차면 false
        public bool TakeDish(int slot, int index)
        {
            if (!TryGet(slot, index, out var kind) || Tray.IsTrash(kind)) return false;
            if (!HandInventory.Instance.TryAdd(new DirtyDish(kind)))
            {
                Toast.Show(ToastMessage.HandsFull);
                return false;
            }
            Remove(slot, index);
            return true;
        }

        // 알바생이 쟁반 하나를 통째로 정리한다: 쓰레기는 버리고 잔·접시는 들고 간다 (ADR-0057). 빈 칸이면 null
        public List<TrayItemKind> ClearSlot(int slot)
        {
            var tray = Slots[slot];
            if (tray == null) return null;
            var dishes = tray.Items.Where(k => !Tray.IsTrash(k)).ToList();
            Slots[slot] = null;
            Changed?.Invoke();
            return dishes;
        }

        bool TryGet(int slot, int index, out TrayItemKind kind)
        {
            var tray = slot >= 0 && slot < Slots.Length ? Slots[slot] : null;
            bool ok = tray != null && index >= 0 && index < tray.Items.Count;
            kind = ok ? tray.Items[index] : default;
            return ok;
        }

        void Remove(int slot, int index)
        {
            var tray = Slots[slot];
            tray.Items.RemoveAt(index);
            if (tray.IsEmpty) Slots[slot] = null; // 모두 치우면 쟁반이 없어짐
            Changed?.Invoke();
        }
    }
}
