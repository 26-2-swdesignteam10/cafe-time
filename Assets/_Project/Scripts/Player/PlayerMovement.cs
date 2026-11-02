using System.Collections.Generic;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.Player
{
    // 캐릭터 이동 (SFR-015), 달리기 (SFR-016), 충돌 (SFR-017)
    // - Rigidbody2D(Dynamic, 중력 0)에 속도를 주어 이동 → 벽·기기·손님의 Collider2D에 막힘
    // - 손님 NPC는 Kinematic Rigidbody2D로 만들어야 플레이어에게 밀리지 않음
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        static readonly (Key key, Key arrow, Vector2 dir)[] DirectionKeys =
        {
            (Key.W, Key.UpArrow, Vector2.up),
            (Key.S, Key.DownArrow, Vector2.down),
            (Key.A, Key.LeftArrow, Vector2.left),
            (Key.D, Key.RightArrow, Vector2.right),
        };

        [SerializeField] float walkSpeed = 3f;      // 이동 속도 (추후 결정)
        [SerializeField] float runMultiplier = 1.6f; // 달리기 속도 배율 (추후 결정)

        // 누르고 있는 방향을 누른 순서대로 보관. 마지막 원소가 현재 이동 방향
        readonly List<Vector2> heldDirections = new List<Vector2>();
        Rigidbody2D body;
        Vector2 moveDirection;

        public bool IsMoving => moveDirection != Vector2.zero;
        public bool IsRunning { get; private set; }
        public Vector2 Facing { get; private set; } = Vector2.down; // 마지막으로 바라본 방향 (SFR-018)

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0;
            body.freezeRotation = true;
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            // UI가 열려 있으면 이동 입력 무시 (SFR-015)
            if (keyboard == null || Popup.AnyOpen)
            {
                heldDirections.Clear();
                moveDirection = Vector2.zero;
                IsRunning = false;
                return;
            }

            // 두 방향을 같이 누르면 나중에 누른 방향, 그 키를 떼면 아직 누르고 있는 방향으로 이동
            foreach (var (key, arrow, dir) in DirectionKeys)
            {
                bool held = keyboard[key].isPressed || keyboard[arrow].isPressed;
                if (!held) heldDirections.Remove(dir);
                else if (!heldDirections.Contains(dir)) heldDirections.Add(dir);
            }

            moveDirection = heldDirections.Count > 0 ? heldDirections[heldDirections.Count - 1] : Vector2.zero;
            IsRunning = IsMoving && keyboard.shiftKey.isPressed;
            if (IsMoving) Facing = moveDirection;
        }

        // 키를 떼면 즉시 정지 (가속/감속 없음)
        void FixedUpdate()
        {
            body.linearVelocity = moveDirection * (IsRunning ? walkSpeed * runMultiplier : walkSpeed);
        }
    }
}
