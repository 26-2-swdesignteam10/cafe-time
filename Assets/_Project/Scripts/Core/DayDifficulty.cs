using UnityEngine;

namespace CafeTime.Core
{
    // DAY 1개의 손님 난이도 (SFR-024, 025, 057). GameBalance 의 DAY별 표 한 줄이다. 시간은 실제 초
    [System.Serializable]
    public class DayDifficulty
    {
        [Tooltip("손님 일행 입장 간격 (최소~최대, 실제 초)")]
        public Vector2 arrivalInterval;
        [Tooltip("일행 1·2·3·4명이 될 가중치")]
        public int[] partySizeWeights;
        [Tooltip("줄 맨 앞에서 주문을 기다리는 인내심 (실제 초)")]
        public float orderPatience;
        [Tooltip("주문 후 음료를 기다리는 인내심 (실제 초)")]
        public float servePatience;

        public DayDifficulty(float minInterval, float maxInterval, int[] weights, float order, float serve)
        {
            arrivalInterval = new Vector2(minInterval, maxInterval);
            partySizeWeights = weights;
            orderPatience = order;
            servePatience = serve;
        }
    }
}
