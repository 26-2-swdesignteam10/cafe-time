namespace CafeTime.Core
{
    // 저장 파일의 단골손님 방문 기록 한 줄 (SFR-009, 071, ADR-0049). 주문한 날만 센다
    [System.Serializable]
    public class RegularVisit
    {
        public string id;   // RegularCustomer.id
        public int visits;  // 지금까지 주문한 방문 횟수
    }
}
