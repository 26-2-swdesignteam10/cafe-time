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
        public Sprite sit;       // 테이블 뒤 자리: 정면으로 앉음
        public Sprite sitLeft;   // 오른쪽 의자: 왼쪽(테이블)을 보고 앉음
        public Sprite sitRight;  // 왼쪽 의자: 오른쪽(테이블)을 보고 앉음

        // 앉은 그림. facing = 앉아서 바라보는 방향 (SFR-024)
        public Sprite Seated(Vector2 facing)
        {
            if (facing.x < 0 && sitLeft != null) return sitLeft;
            if (facing.x > 0 && sitRight != null) return sitRight;
            return sit;
        }

        public Sprite Facing(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                return direction.x < 0 ? left : right;
            return direction.y > 0 ? up : down;
        }
    }
}
