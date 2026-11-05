namespace CafeTime.Core
{
    // 엔딩 4종 (SFR-073)
    public enum EndingType
    {
        ClosedCafe = 1,     // 엔딩 01 닫힌 카페: 정산 때 임대료를 못 냄
        BarelyKept = 2,     // 엔딩 02 가까스로 지킨 꿈
        Settled = 3,        // 엔딩 03 자리를 잡은 카페
        BelovedCafe = 4,    // 엔딩 04 모두가 사랑하는 카페
    }
}
