namespace CafeTime.Core
{
    // 지금 게임의 장비 단계 (ADR-0057). 저장 데이터가 없으면(테스트 씬) 기본 단계 0
    public static class Upgrades
    {
        public static int Level(Equipment equipment) => SaveSystem.Current?.GetLevel(equipment) ?? 0;

        public static bool Has(Equipment equipment, int level) => Level(equipment) >= level;
    }
}
