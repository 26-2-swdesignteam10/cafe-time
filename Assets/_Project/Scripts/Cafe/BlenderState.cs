namespace CafeTime.Cafe
{
    // 믹서기 용기 상태 (SFR-053, 054)
    public enum BlenderState
    {
        Empty,     // 빈 용기
        Filled,    // 재료 담김
        Blending,  // 갈기 중
        Blended,   // 갈린 스무디
    }
}
