namespace CafeTime.Cafe
{
    // 인플루언서 촬영 결과 (SFR-009 DAY 6, ADR-0048). 대표 메뉴를 받은 등급과 마감 여부로 정한다
    public enum InfluencerOutcome
    {
        Great,   // 대표 메뉴를 완벽 등급으로 대접
        Good,    // 좋음 등급
        Bad,     // 나쁨·실패 등급, 다른 메뉴
        Expired, // 마감 시각까지 받지 못함 (평판·손님 수는 Bad 와 같음)
    }
}
