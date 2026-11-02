using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CafeTime.Cafe
{
    // E 키로 상호작용할 수 있는 맵 오브젝트 (기기, 퇴식구, 설거지실 등) (SFR-020)
    // onInteract 에 기기 화면 Open 등을 연결. 동작이 복잡하면 상속해서 Interact()를 override
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        static readonly List<Interactable> all = new List<Interactable>();

        public static IReadOnlyList<Interactable> All => all;

        [SerializeField] string actionLabel = "사용"; // 키 안내 문구: "E 열기" / "E 사용"
        [SerializeField] GameObject highlight;        // 플레이어가 다가가면 켜지는 테두리 (SFR-013)
        [SerializeField] float promptHeight = 0.3f;   // 오브젝트 위쪽 끝에서 키 안내까지의 높이
        [SerializeField] UnityEvent onInteract = new UnityEvent();

        Collider2D area;

        public string ActionLabel => actionLabel;
        public UnityEvent OnInteract => onInteract;
        public Vector3 PromptPosition => new Vector3(area.bounds.center.x, area.bounds.max.y + promptHeight, 0);

        void Awake()
        {
            area = GetComponent<Collider2D>();
            SetHighlighted(false);
        }

        void OnEnable() => all.Add(this);

        void OnDisable() => all.Remove(this);

        // 충돌 영역 가장자리까지의 거리. 큰 기기도 어느 쪽에서 다가가든 같은 기준
        public float DistanceTo(Vector2 point) => Vector2.Distance(point, area.ClosestPoint(point));

        public void SetHighlighted(bool on)
        {
            if (highlight != null) highlight.SetActive(on);
        }

        public virtual void Interact() => onInteract.Invoke();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => all.Clear();
    }
}
