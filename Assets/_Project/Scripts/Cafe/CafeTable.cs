using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님 테이블 1개 (SFR-024). 일행 1개가 함께 앉는다
    public class CafeTable : MonoBehaviour
    {
        [Tooltip("앉는 자리 (왼쪽 의자, 오른쪽 의자, 뒤쪽 2자리 순)")]
        [SerializeField] Transform[] seats;
        [Tooltip("테이블로 걸어올 때 지나는 가로 통로의 y")]
        [SerializeField] float laneY;
        [Tooltip("인내심 게이지 위치")]
        [SerializeField] Transform gaugePoint;

        public CustomerParty Party { get; set; }
        public bool IsFree => Party == null;
        public float LaneY => laneY;
        public Transform GaugePoint => gaugePoint;

        public Vector3 Seat(int index) => seats[Mathf.Clamp(index, 0, seats.Length - 1)].position;
    }
}
