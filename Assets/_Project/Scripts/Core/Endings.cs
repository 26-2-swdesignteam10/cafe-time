namespace CafeTime.Core
{
    // 엔딩 판정 (SFR-073). 하루 정산 화면이 정하고 엔딩 씬이 읽는다. 기준은 임시값 (GameBalance, docs/GAME_DATA.md 8장)
    public static class Endings
    {
        public static EndingType Pending { get; set; } = EndingType.BarelyKept;

        // DAY 15 정산을 마친 뒤의 엔딩. 최종 수익 = 최종 보유금 - 시작 자금
        public static EndingType Decide(SaveData save, GameBalance balance)
        {
            int profit = save.money - balance.StartingMoney;
            if (profit >= balance.Ending04Profit && save.reputation >= balance.Ending04Reputation) return EndingType.BelovedCafe;
            if (profit >= balance.Ending03Profit && save.reputation >= balance.Ending03Reputation) return EndingType.Settled;
            return EndingType.BarelyKept;
        }
    }
}
