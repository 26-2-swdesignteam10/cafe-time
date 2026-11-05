using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손에 들 수 있는 물건 1개 (SFR-021): 커피잔, 샷 컵, 얼음 스쿱, 디저트, 행주 (ADR-0014)
    public abstract class HeldItem
    {
        public abstract Sprite Icon { get; }

        // 픽업대에 올릴 수 있는 완성 음료·디저트인지 (SFR-056)
        public virtual bool Servable => false;

        // 서빙 판정용 (SFR-068). 완성 음료·디저트가 아니거나 실패한 음료면 null
        public virtual MenuDefinition Menu => null;
        public virtual Temperature Temperature => Temperature.None;
        public virtual DrinkGrade Grade => DrinkGrade.Failed;
    }
}
