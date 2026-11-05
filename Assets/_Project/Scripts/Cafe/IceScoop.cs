using UnityEngine;

namespace CafeTime.Cafe
{
    // 얼음 스쿱 (SFR-039): 빈 스쿱 / 얼음이 담긴 스쿱. 제빙기에 1개가 있다
    public class IceScoop : HeldItem
    {
        public bool HasIce { get; set; }

        public override Sprite Icon => ItemIcons.Get(HasIce ? ItemSprite.ScoopIce : ItemSprite.Scoop);
    }
}
