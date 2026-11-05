using UnityEngine;

namespace CafeTime.Cafe
{
    // 에스프레소 샷을 받는 컵 (SFR-034): 빈 컵 / 샷이 든 컵. 에스프레소 머신에 2개가 있다
    public class ShotGlass : HeldItem
    {
        public bool HasShot { get; set; }

        public override Sprite Icon => ItemIcons.Get(HasShot ? ItemSprite.ShotGlassFull : ItemSprite.ShotGlass);
    }
}
