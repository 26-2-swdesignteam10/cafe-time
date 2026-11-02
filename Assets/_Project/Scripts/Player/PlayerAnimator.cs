using System;
using UnityEngine;

namespace CafeTime.Player
{
    // 방향별 걷기/대기 애니메이션 (SFR-018)
    // 스프라이트를 프레임 단위로 바꿔 끼우는 방식. 실제 캐릭터 그림이 나오면 Inspector에서 스프라이트만 교체
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        [Serializable]
        class DirectionFrames
        {
            public Sprite idle;
            public Sprite[] walk;
        }

        [SerializeField] PlayerMovement movement;
        [SerializeField] DirectionFrames down;
        [SerializeField] DirectionFrames up;
        [SerializeField] DirectionFrames left;
        [SerializeField] DirectionFrames right;
        [SerializeField] float walkFps = 8f;
        [SerializeField] float runFpsMultiplier = 1.6f; // 달리기 중 재생 속도 배율

        SpriteRenderer spriteRenderer;
        float frameTime;

        void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

        void Update()
        {
            var frames = FramesFor(movement.Facing);

            // 정지 시 마지막으로 바라본 방향의 대기 모션
            if (!movement.IsMoving || frames.walk.Length == 0)
            {
                spriteRenderer.sprite = frames.idle;
                frameTime = 0;
                return;
            }

            frameTime += Time.deltaTime * walkFps * (movement.IsRunning ? runFpsMultiplier : 1f);
            spriteRenderer.sprite = frames.walk[(int)frameTime % frames.walk.Length];
        }

        DirectionFrames FramesFor(Vector2 facing)
        {
            if (facing == Vector2.up) return up;
            if (facing == Vector2.left) return left;
            if (facing == Vector2.right) return right;
            return down;
        }
    }
}
