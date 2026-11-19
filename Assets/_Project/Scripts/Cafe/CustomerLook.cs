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
        // 걷기 프레임 (오른발·서기·왼발·서기 순으로 반복, 2026-11-19 사용자 요청). 비어 있으면 서 있는 그림
        public Sprite[] walkDown;
        public Sprite[] walkUp;
        public Sprite[] walkLeft;
        public Sprite[] walkRight;
        public Sprite portrait;  // 포스기 주문 채팅 옆 초상화 (SFR-026)

        // 앉은 그림. facing = 앉아서 바라보는 방향 (SFR-024)
        public Sprite Seated(Vector2 facing)
        {
            if (facing.x < 0 && sitLeft != null) return sitLeft;
            if (facing.x > 0 && sitRight != null) return sitRight;
            return sit;
        }

        // 걷는 중인 그림. frame 은 계속 늘어나는 프레임 번호
        public Sprite Walking(Vector2 direction, int frame)
        {
            var frames = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? (direction.x < 0 ? walkLeft : walkRight)
                : (direction.y > 0 ? walkUp : walkDown);
            var sprite = frames != null && frames.Length > 0 ? frames[frame % frames.Length] : null;
            return sprite != null ? sprite : Facing(direction); // 빠진 프레임은 서 있는 그림 (빈 칸이면 깜빡임)
        }

        public Sprite Facing(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                return direction.x < 0 ? left : right;
            return direction.y > 0 ? up : down;
        }
    }
}
