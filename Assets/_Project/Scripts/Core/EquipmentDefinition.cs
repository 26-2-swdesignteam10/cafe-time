using UnityEngine;

namespace CafeTime.Core
{
    // 장비 하나의 업그레이드 정보 (ADR-0057). 단계는 0(기본) ~ 3. 효과 수치는 GameBalance 의 "장비 업그레이드 효과"
    [System.Serializable]
    public class EquipmentDefinition
    {
        public const int MaxLevel = 3;

        [SerializeField] Equipment id;
        [SerializeField] string displayName;
        [SerializeField] string description;
        [Tooltip("1·2·3단계로 올리는 가격")]
        [SerializeField] int[] prices;
        [Tooltip("1·2·3단계 효과 한 줄 설명")]
        [SerializeField] string[] stages;

        public EquipmentDefinition(Equipment id, string displayName, string description, int[] prices, string[] stages)
        {
            this.id = id;
            this.displayName = displayName;
            this.description = description;
            this.prices = prices;
            this.stages = stages;
        }

        public Equipment Id => id;
        public string DisplayName => displayName;
        public string Description => description;

        // level 단계로 올리는 가격 (1 ~ 3)
        public int PriceOf(int level) => prices[Mathf.Clamp(level - 1, 0, prices.Length - 1)];
        public string StageText(int level) => stages[Mathf.Clamp(level - 1, 0, stages.Length - 1)];
    }
}
