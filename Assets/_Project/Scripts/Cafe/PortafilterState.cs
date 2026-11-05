namespace CafeTime.Cafe
{
    // 포터필터 상태 (SFR-030)
    public enum PortafilterState
    {
        Empty,   // 빈 포터필터
        Ground,  // 원두가루 담김
        Tamped,  // 탬핑 완료
        Used,    // 사용 완료(찌꺼기)
    }
}
