using System.Collections.Generic;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 1명: 경로를 따라 걷기, 앉기 (SFR-024). 길찾기 없이 CustomerManager가 정한 꺾은선 경로로만 움직인다
    [RequireComponent(typeof(SpriteRenderer))]
    public class Customer : MonoBehaviour
    {
        const float Speed = 2.2f; // 걷는 속도 (칸/초)

        readonly Queue<Vector3> path = new Queue<Vector3>();
        SpriteRenderer spriteRenderer;
        CustomerLook look;
        bool sitting;

        public bool IsMoving => path.Count > 0;

        public static Customer Create(Transform parent, CustomerLook look, Vector3 position)
        {
            var go = new GameObject("Customer");
            go.transform.SetParent(parent, false);
            go.transform.position = position;
            var customer = go.AddComponent<Customer>();
            customer.look = look;
            customer.spriteRenderer.sprite = look.up;

            // 플레이어가 손님을 밀고 지나가지 못하도록 발밑 충돌 영역 (Kinematic, SFR-017)
            var body = go.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            var feet = go.AddComponent<BoxCollider2D>();
            feet.size = new Vector2(0.5f, 0.25f);
            feet.offset = new Vector2(0, 0.12f);
            return customer;
        }

        void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

        public void Walk(IEnumerable<Vector3> points)
        {
            path.Clear();
            foreach (var p in points) path.Enqueue(p);
            sitting = false;
        }

        public void Sit()
        {
            sitting = true;
            spriteRenderer.sprite = look.sit;
        }

        public void Face(Vector2 direction) => spriteRenderer.sprite = look.Facing(direction);

        void Update()
        {
            if (path.Count == 0 || sitting) return;
            var target = path.Peek();
            var offset = target - transform.position;
            if (offset.sqrMagnitude > 0.0001f) Face(offset);
            transform.position = Vector3.MoveTowards(transform.position, target, Speed * GameClock.Delta);
            if ((transform.position - target).sqrMagnitude < 0.0001f) path.Dequeue();
        }
    }
}
