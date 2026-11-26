namespace CafeTime.Core
{
    // 알바생이 맡는 일 (ADR-0057). 여러 개를 함께 맡을 수 있다
    [System.Flags]
    public enum StaffTask
    {
        None = 0,
        Tables = 1, // 청소 요청이 뜬 테이블 닦기, 손님이 두고 간 쟁반 치우기
        Trays = 2,  // 퇴식구 쟁반 정리 (쓰레기는 버리고 잔·접시는 설거지 대기 바구니로)
        Dishes = 4, // 설거지
    }
}
