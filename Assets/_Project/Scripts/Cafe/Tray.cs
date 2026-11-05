using System.Collections.Generic;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님이 반납하는 쟁반 (SFR-060). 내용물은 실제로 받은 음료·디저트를 기준으로 만든다
    public class Tray
    {
        public List<TrayItemKind> Items { get; } = new List<TrayItemKind>();

        public bool IsEmpty => Items.Count == 0;

        public static Tray FromServed(IReadOnlyList<HeldItem> served, GameBalance balance)
        {
            var tray = new Tray();
            foreach (var item in served)
            {
                if (item is DessertItem)
                {
                    tray.Items.Add(TrayItemKind.DirtyPlate);
                    continue;
                }
                if (!(item is Cup cup)) continue;
                tray.Items.Add(cup.Type == CupType.Ceramic ? TrayItemKind.DirtyCeramic : TrayItemKind.DirtyPlastic);
                // 빨대: 아이스 음료, 스무디·쉐이크 / 티백: 티 메뉴
                if (cup.Temperature == Temperature.Ice || cup.Type == CupType.Plastic) tray.Items.Add(TrayItemKind.Straw);
                if (cup.Contents.HasTeabag(balance)) tray.Items.Add(TrayItemKind.UsedTeabag);
            }
            if (Random.value < balance.NapkinChance) tray.Items.Add(TrayItemKind.Napkin); // 냅킨은 무작위
            return tray;
        }

        public static bool IsTrash(TrayItemKind kind) =>
            kind == TrayItemKind.Straw || kind == TrayItemKind.Napkin || kind == TrayItemKind.UsedTeabag;
    }
}
