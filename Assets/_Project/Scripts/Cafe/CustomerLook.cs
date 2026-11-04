using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 외형 1종의 스프라이트 묶음 (Art/Sprites/Customers)
    [System.Serializable]
    public class CustomerLook
    {
        public Sprite down;
        public Sprite up;
        public Sprite left;
        public Sprite right;
        public Sprite sit;

        public Sprite Facing(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                return direction.x < 0 ? left : right;
            return direction.y > 0 ? up : down;
        }
    }
}
