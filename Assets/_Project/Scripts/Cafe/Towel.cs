using UnityEngine;

namespace CafeTime.Cafe
{
    // 행주 (SFR-066). 행주 걸이에서 들고, 청소 요청 테이블을 닦는다
    public class Towel : HeldItem
    {
        public override Sprite Icon => ItemIcons.Get(ItemSprite.Towel);
    }
}
