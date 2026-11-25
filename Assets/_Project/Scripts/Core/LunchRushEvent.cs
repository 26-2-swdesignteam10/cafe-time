using UnityEngine;

namespace CafeTime.Core
{
    // 점심시간 러시 이벤트 1건의 설정 (SFR-009 DAY 11, ADR-0050). GameBalance 의 점심 러시 목록 한 줄이다.
    // 시각은 게임 시간(시, 소수 가능: 11.5 = 11:30), 입장 간격은 실제 초
    [System.Serializable]
    public class LunchRushEvent
    {
        [Tooltip("이벤트가 일어나는 DAY")]
        public int day;
        [Tooltip("러시가 시작되는 게임 시각 (시)")]
        public float startHour;
        [Tooltip("러시가 끝나는 게임 시각 (시)")]
        public float endHour;
        [Tooltip("러시 동안 손님 일행 입장 간격 (최소~최대, 실제 초). 평소 DAY 난이도 표 대신 쓴다")]
        public Vector2 arrivalInterval;
        [Tooltip("러시 손님 일행 1·2·3·4명이 될 가중치")]
        public int[] partySizeWeights;
        [Tooltip("러시 손님이 주문하는 메뉴 id")]
        public string menuId;
        [Tooltip("러시 메뉴 HOT/ICE (온도 고정 메뉴면 무시)")]
        public Temperature temperature;
        [Tooltip("러시 손님 1명이 러시 메뉴를 고를 확률. 아니면 평소처럼 무작위 음료")]
        [Range(0f, 1f)] public float menuChance;
        [Tooltip("러시 손님이 음료를 받고 머무는 시간 배율 (0.5 = 평소의 절반만 머물고 회사로 돌아감)")]
        [Range(0.1f, 1f)] public float stayMultiplier;

        public LunchRushEvent(int day, float startHour, float endHour, float minInterval, float maxInterval, int[] weights,
            string menuId, Temperature temperature, float menuChance, float stayMultiplier)
        {
            this.day = day;
            this.startHour = startHour;
            this.endHour = endHour;
            arrivalInterval = new Vector2(minInterval, maxInterval);
            partySizeWeights = weights;
            this.menuId = menuId;
            this.temperature = temperature;
            this.menuChance = menuChance;
            this.stayMultiplier = stayMultiplier;
        }
    }
}
