namespace CafeTime.Core
{
    // 저장 파일의 재료 재고 한 줄 (ADR-0010). JsonUtility가 Dictionary를 저장하지 못해 목록으로 저장
    [System.Serializable]
    public class IngredientStock
    {
        public string id;
        public int amount; // 기본 단위 양
    }
}
