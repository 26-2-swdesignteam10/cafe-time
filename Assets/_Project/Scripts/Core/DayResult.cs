namespace CafeTime.Core
{
    // 하루 영업 결과. 영업 씬이 채우고 득점 결과·하루 정산 화면이 읽는다 (ADR-0011, SFR-067, SFR-071)
    // 매출과 팁은 영업 중 보유 자금에 바로 더해지므로(SFR-027, 058) 여기 값은 표시용 기록이다.
    [System.Serializable]
    public class DayResult
    {
        public int visitors;          // 방문 손님 수
        public int drinksSold;        // 판매한 음료 수
        public int revenue;           // 오늘 수익 (결제 금액 합계 - 환불)
        public int tips;              // 받은 팁 합계
        public float reputationChange; // 손님 만족도 평균에 따른 평판 변동 (SFR-072, 기준 임시)

        // 득점 결과 화면(SFR-067, 068)용 기록
        public int ordersCompleted;    // 서빙 완료한 주문 수
        public float satisfactionSum;  // 서빙 완료한 일행 만족도 합 (0~100)
        public int customersLost;      // 오래 기다리다 떠난 일행 수 (감점)
        public int itemsDiscarded;     // 버린 재료·음료·디저트 횟수 (감점)
        public int traysLeft;          // 퇴식구가 가득 차 테이블에 두고 간 쟁반 수 (감점, SFR-062)
        public int cleaningMissed;     // 제한 시간 안에 처리하지 못한 청소 요청 수 (감점, SFR-066)
        public int score;              // 득점 결과 화면에서 계산한 오늘의 총점 (SFR-067)

        // 오늘 영업 결과. 오픈 준비에서 영업을 시작할 때 새로 만든다
        public static DayResult Today { get; private set; }

        public static void StartNewDay() => Today = new DayResult();
    }
}
