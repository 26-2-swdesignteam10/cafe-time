namespace CafeTime.Cafe
{
    // 반납된 쟁반 위의 물건 (SFR-060, 061). 다 마신 잔·빈 접시는 설거지, 나머지는 쓰레기
    public enum TrayItemKind
    {
        DirtyCeramic,
        DirtyPlastic,
        DirtyPlate,
        Straw,
        Napkin,
        UsedTeabag,
    }
}
