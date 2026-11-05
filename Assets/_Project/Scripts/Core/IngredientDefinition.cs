using UnityEngine;

namespace CafeTime.Core
{
    // 재료 한 종류의 정의. 재고는 기본 단위(g·개·회)로 다룬다 (ADR-0010, docs/GAME_DATA.md 3장)
    [System.Serializable]
    public class IngredientDefinition
    {
        [Tooltip("저장 파일에 남는 고유 id. 한번 정하면 바꾸지 않는다")]
        [SerializeField] string id;
        [SerializeField] string displayName;
        [SerializeField] IngredientCategory category;
        [Tooltip("구매 단위 이름 (봉, 팩, 병 ...)")]
        [SerializeField] string purchaseUnit;
        [Tooltip("구매 단위 1개에 들어 있는 기본 단위 양")]
        [SerializeField] int amountPerUnit;
        [Tooltip("기본 단위 이름 (g, 개, 회)")]
        [SerializeField] string baseUnit;
        [Tooltip("구매 단위 1개 가격")]
        [SerializeField] int price;
        [SerializeField] Sprite icon;

        public string Id => id;
        public string DisplayName => displayName;
        public IngredientCategory Category => category;
        public string PurchaseUnit => purchaseUnit;
        public int AmountPerUnit => amountPerUnit;
        public string BaseUnit => baseUnit;
        public int Price => price;
        public Sprite Icon => icon;

        public IngredientDefinition(string id, string displayName, IngredientCategory category,
            string purchaseUnit, int amountPerUnit, string baseUnit, int price)
        {
            this.id = id;
            this.displayName = displayName;
            this.category = category;
            this.purchaseUnit = purchaseUnit;
            this.amountPerUnit = amountPerUnit;
            this.baseUnit = baseUnit;
            this.price = price;
        }

        // 기본 단위 양을 표시용 문자열로 (예: 1,200g / 12회 / 3개)
        public string FormatAmount(int amount) => $"{amount:N0}{baseUnit}";
    }
}
