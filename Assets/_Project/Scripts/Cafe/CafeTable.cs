using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 테이블 1개 (SFR-024). 일행 1개가 함께 앉는다.
    // 청소 요청(SFR-066)이 있거나 쟁반이 남아 있으면 손님이 앉을 수 없다
    public class CafeTable : MonoBehaviour
    {
        [Tooltip("앉는 자리 (왼쪽 의자, 오른쪽 의자, 뒤쪽 2자리 순)")]
        [SerializeField] Transform[] seats;
        [Tooltip("테이블로 걸어올 때 지나는 가로 통로의 y")]
        [SerializeField] float laneY;
        [Tooltip("인내심 게이지 위치")]
        [SerializeField] Transform gaugePoint;
        [SerializeField] GameObject cleanIcon;    // 청소 요청 아이콘
        [SerializeField] GameObject trayOnTable;  // 두고 간 쟁반 그림
        [SerializeField] TableCleanSpot cleanSpot; // 행주를 들고 E (요청이 있을 때만 켜짐)

        public CustomerParty Party { get; set; }
        public Tray LeftTray { get; private set; }
        public bool CleaningRequested { get; private set; }
        public float CleaningLeft { get; set; }       // 청소 제한 시간 남은 초
        public bool CleaningMissed { get; set; }      // 제한 시간이 지나 감점을 받았는지

        public bool IsFree => Party == null && !CleaningRequested && LeftTray == null;
        public float LaneY => laneY;
        public Transform GaugePoint => gaugePoint;

        public Vector3 Seat(int index) => seats[Mathf.Clamp(index, 0, seats.Length - 1)].position;

        void Awake() => Refresh();

        public void RequestCleaning(float timeLimit)
        {
            CleaningRequested = true;
            CleaningLeft = timeLimit;
            CleaningMissed = false;
            Refresh();
        }

        public void LeaveTray(Tray tray)
        {
            LeftTray = tray;
            Refresh();
        }

        // 청소 완료: 남은 쟁반은 퇴식구로 (가득 차 있으면 테이블에 남음)
        public void Clean()
        {
            CleaningRequested = false;
            TryMoveTray();
            Refresh();
        }

        public void TryMoveTray()
        {
            if (LeftTray == null || CleaningRequested || !TrayReturn.Instance.TryReturn(LeftTray)) return;
            LeftTray = null;
            Refresh();
        }

        void Refresh()
        {
            if (cleanIcon != null) cleanIcon.SetActive(CleaningRequested);
            if (trayOnTable != null) trayOnTable.SetActive(LeftTray != null);
            if (cleanSpot != null) cleanSpot.enabled = CleaningRequested;
        }
    }
}
