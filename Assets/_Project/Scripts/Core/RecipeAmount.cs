using UnityEngine;

namespace CafeTime.Core
{
    // 레시피 재료 한 줄: 재료 id(또는 RecipeKeys) + 정량 (SFR-051). 정량 단위는 재료의 기본 단위(g·개), 샷·얼음은 횟수
    // 값 타입이어야 한다: 클래스였을 때 기본 레시피들이 같은 줄 객체를 공유해, 에셋을 읽어 들이며 한 레시피 값이
    // 다른 레시피로 번지는 버그가 있었다 (2026-11-18, 커피 샷이 딸기청으로 바뀜)
    [System.Serializable]
    public struct RecipeAmount
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
