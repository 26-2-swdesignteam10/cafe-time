using UnityEngine;

namespace CafeTime.Core
{
    // 영업 중 인플루언서 방문 이벤트 1건의 설정 (SFR-009 DAY 6, ADR-0048). GameBalance 의 인플루언서 목록 한 줄이다.
    // 시각은 게임 시간(시, 소수 가능: 13.5 = 13:30). 평판 변동은 그날 정산에 더해지고,
    // 손님 수 배율은 다음 DAY 부터 손님 입장 빈도에 곱해진다 (1.15 = 손님 약 15% 증가)
    [System.Serializable]
    public class InfluencerEvent
    {
        [Tooltip("이벤트가 일어나는 DAY")]
        public int day;
        [Tooltip("인플루언서가 방문해 추천·촬영을 요청하는 게임 시각 (시)")]
        public float requestHour;
        [Tooltip("픽업대에서 음료를 받아야 하는 마감 게임 시각 (시)")]
        public float deadlineHour;
        [Tooltip("추천하는 카페 대표 메뉴 id")]
        public string menuId;
        [Tooltip("대표 메뉴 HOT/ICE (온도 고정 메뉴면 무시)")]
        public Temperature temperature;
        [Tooltip("수락·거절 결과를 저장할 선택 id (SaveData.choices, SFR-071). 촬영 결과는 '<id>_result'")]
        public string choiceId;

        [Header("완벽 등급으로 대접")]
        public float greatReputation;
        public float greatVisitorRate;
        [Header("좋음 등급으로 대접")]
        public float goodReputation;
        public float goodVisitorRate;
        [Header("나쁨·실패·다른 메뉴·마감 넘김")]
        public float badReputation;
        public float badVisitorRate;

        public InfluencerEvent(int day, float requestHour, float deadlineHour, string menuId, Temperature temperature, string choiceId)
        {
            this.day = day;
            this.requestHour = requestHour;
            this.deadlineHour = deadlineHour;
            this.menuId = menuId;
            this.temperature = temperature;
            this.choiceId = choiceId;
        }
    }
}
