using UnityEngine;

namespace CafeTime.Core
{
    // 레시피 재료 한 줄: 재료 id(또는 RecipeKeys) + 정량 (SFR-051). 정량 단위는 재료의 기본 단위(g·개), 샷·얼음은 횟수
    [System.Serializable]
    public class RecipeAmount
    {
        [SerializeField] string key;
        [SerializeField] float amount;

        public string Key => key;
        public float Amount => amount;

        public RecipeAmount(string key, float amount)
        {
            this.key = key;
            this.amount = amount;
        }
    }
}
