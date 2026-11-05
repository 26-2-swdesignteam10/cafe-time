namespace CafeTime.Core
{
    // 구매 재료가 아닌 레시피 재료의 key. 구매 재료는 IngredientDefinition.Id 를 그대로 쓴다
    public static class RecipeKeys
    {
        public const string Shot = "shot";             // 에스프레소 샷 (횟수, SFR-048)
        public const string Ice = "ice";               // 얼음 스쿱 (횟수, SFR-048)
        public const string HotWater = "water_hot";    // 정수기 뜨거운 물 (g, SFR-043)
        public const string ColdWater = "water_cold";  // 정수기 차가운 물 (g)
    }
}
