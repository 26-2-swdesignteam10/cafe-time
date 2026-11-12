using UnityEngine;

namespace CafeTime.Core
{
    // 하루 점수 계산 (SFR-068). 총점 = 완료한 주문 점수 + 손님 만족도 점수 - 감점. 항목별 수치는 임시값 (GameBalance)
    public class DayScore
    {
        public int OrderPoints { get; }
        public int SatisfactionPoints { get; }
        public int LostPenalty { get; }
        public int DiscardPenalty { get; }
        public int TrayLeftPenalty { get; }
        public int CleaningPenalty { get; }
        public int SkippedPenalty { get; }

        public int Penalty => LostPenalty + DiscardPenalty + TrayLeftPenalty + CleaningPenalty + SkippedPenalty;
        public int Total => Mathf.Max(0, OrderPoints + SatisfactionPoints - Penalty);

        public DayScore(DayResult r, GameBalance b)
        {
            OrderPoints = r.ordersCompleted * b.PointsPerOrder;
            // 만족도 0~100 을 일행마다 그대로 더한다
            SatisfactionPoints = Mathf.RoundToInt(r.satisfactionSum * b.PointsPerSatisfaction);
            LostPenalty = r.customersLost * b.PenaltyLost;
            DiscardPenalty = r.itemsDiscarded * b.PenaltyDiscarded;
            TrayLeftPenalty = r.traysLeft * b.PenaltyTrayLeft;
            CleaningPenalty = r.cleaningMissed * b.PenaltyCleaning;
            SkippedPenalty = r.ordersSkipped * b.PenaltyOrderSkipped;
        }
    }
}
