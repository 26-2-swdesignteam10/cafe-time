using UnityEngine;

namespace CafeTime.Cafe
{
    // 퇴식구 쟁반에서 손으로 가져온 다 쓴 잔·접시 (SFR-061, ADR-0052).
    // 설거지실 화면의 '설거지 대기' 바구니에 끌어 넣어야 설거지할 수 있다. 버리기 메뉴로 없앨 수 없다
    public class DirtyDish : HeldItem
    {
        public TrayItemKind Kind { get; }

        public DirtyDish(TrayItemKind kind) => Kind = kind;

        public override Sprite Icon => ItemIcons.Of(Kind);
    }
}
