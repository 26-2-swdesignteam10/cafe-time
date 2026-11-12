using CafeTime.Cafe;
using UnityEngine;

namespace CafeTime.EspressoShot
{
    public enum SlotType
    {
        Rack,       // 보관 자리 (포터필터 2개가 처음 놓이는 곳)
        Grinder,    // 그라인더
        Tamp,       // 탬핑 스테이션
        GroupHead,  // 그룹헤드 (머신 장착부, 2개)
        Knockbox    // 넉박스
    }

    // 포터필터를 1개 올릴 수 있는 자리. 배경 Image(Raycast Target ON)가 있어야 드롭이 인식돼요.
    public class PortafilterSlot : MonoBehaviour
    {
        public SlotType type;
        [Tooltip("처음부터 이 자리에 놓여 있을 포터필터")]
        public Portafilter startingPortafilter;
        [Tooltip("GroupHead 만: 아래 컵 자리 (SFR-034). 비워 두면 컵 없이 추출")]
        public ShotCupSlot cup;
        [Tooltip("놓인 포터필터의 세로 위치 보정 (캔버스 단위). 그림 크기가 슬롯과 다를 때 사용")]
        public float portafilterOffsetY;
        [Tooltip("GroupHead 만: 추출 중에 켜는 커피 줄기")]
        public GameObject extractStream;
        [Tooltip("Knockbox 만: 찌꺼기 털기 창. 비워 두면 대는 즉시 빈 필터가 됨")]
        public KnockScreen knockScreen;

        public Portafilter Current { get; private set; }
        public bool Locked { get; set; }   // 추출/분쇄 중에는 분리 불가
        public float TimeLeft { get; set; } // 추출 남은 시간(초). LCD 표시용

        void Start()
        {
            if (startingPortafilter != null) startingPortafilter.AttachTo(this);
        }

        // 드롭 시 호출. false면 포터필터는 원래 자리로 돌아감.
        public bool TryAccept(Portafilter pf)
        {
            if (Current != null && Current != pf) return false; // 이미 다른 포터필터가 있음
            // SFR-033: 탬핑 완료된 것만 그룹헤드에 장착
            if (type == SlotType.GroupHead && pf.State != PortafilterState.Tamped)
            {
                Toast.Show(ToastMessage.TampedOnly);
                return false;
            }
            pf.AttachTo(this);
            return true;
        }

        // Portafilter.AttachTo 에서 호출
        internal void Place(Portafilter pf)
        {
            Current = pf;
            // SFR-036: 넉박스에 사용 완료(찌꺼기) 포터필터를 대면 털기 창이 열리고, 다 털면 빈 필터로
            if (type != SlotType.Knockbox || pf.State != PortafilterState.Used) return;
            if (knockScreen == null) pf.SetState(PortafilterState.Empty);
            else knockScreen.Open(() => { if (Current == pf) pf.SetState(PortafilterState.Empty); });
        }

        internal void Release(Portafilter pf)
        {
            if (Current == pf) Current = null;
        }

        // 그룹헤드 옆 "추출" 버튼의 OnClick 에 연결
        public void OnClickExtract()
        {
            if (type != SlotType.GroupHead) return;
            EspressoMachine.Instance.StartExtraction(this);
        }
    }
}
