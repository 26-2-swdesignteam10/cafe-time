using UnityEngine;

namespace CafeTime.Cafe
{
    // 포스기 상호작용 (SFR-026): 줄 맨 앞 손님의 주문 채팅과 포스기 화면을 함께 연다. 줄이 비어 있으면 안내 문구
    public class PosStation : MonoBehaviour
    {
        [SerializeField] PosScreen pos;

        static PosStation instance;

        // 포스기 화면이 열려 있는 동안은 줄 맨 앞 손님의 인내심이 줄지 않는다 (주문받는 중)
        public static bool TakingOrder => instance != null && instance.pos.isActiveAndEnabled;

        void Awake() => instance = this;

        void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        // Interactable.onInteract 에 연결
        public void Interact()
        {
            var party = CustomerManager.Instance.FrontParty;
            if (party == null)
            {
                Toast.Show(ToastMessage.NoCustomer);
                return;
            }
            pos.Open(party);
        }
    }
}
