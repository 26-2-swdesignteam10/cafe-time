using System.Collections.Generic;
using UnityEngine;

namespace CafeTime.Core
{
    // 영업 중 단체 주문 이벤트 1건의 설정 (SFR-009 DAY 4, ADR-0042). GameBalance 의 단체 주문 목록 한 줄이다.
    // 시각은 게임 시간(시, 소수 가능: 10.5 = 10:30).
    // reserveDay 가 있으면 예약 주문이다 (SFR-009 DAY 14 → 15, ADR-0051): reserveDay 의 reserveHour 에 예약 수락/거절을 고르고,
    // 수락했으면 day 의 requestHour 에 선택 없이 주문이 등록된다
    [System.Serializable]
    public class GroupOrderEvent
    {
        [Tooltip("이벤트가 일어나는 DAY")]
        public int day;
        [Tooltip("단체 손님이 주문을 요청하는 게임 시각 (시)")]
        public float requestHour;
        [Tooltip("픽업대에서 서빙해야 하는 마감 게임 시각 (시)")]
        public float deadlineHour;
        [Tooltip("주문을 모두 정확히 맞췄을 때 주문 금액에 더 주는 보너스 비율 (0.3 = 30%)")]
        [Range(0f, 2f)] public float bonusRate;
        [Tooltip("주문 메뉴 (메뉴 id, HOT/ICE, 잔 수)")]
        public List<GroupOrderEntry> lines;
        [Tooltip("수락·거절 결과를 저장할 선택 id (SaveData.choices, SFR-071). 예약 주문이면 예약 선택 결과")]
        public string choiceId;
        [Tooltip("예약을 받는 DAY. 0 이면 예약 없이 당일 요청")]
        public int reserveDay;
        [Tooltip("예약을 요청하는 게임 시각 (시, reserveDay 영업 중)")]
        public float reserveHour;

        public GroupOrderEvent(int day, float requestHour, float deadlineHour, float bonusRate, string choiceId, List<GroupOrderEntry> lines)
        {
            this.day = day;
            this.requestHour = requestHour;
            this.deadlineHour = deadlineHour;
            this.bonusRate = bonusRate;
            this.choiceId = choiceId;
            this.lines = lines;
        }
    }
}
