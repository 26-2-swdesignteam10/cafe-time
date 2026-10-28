namespace CafeTime.Core
{
    // 하루 정산 후 자동 저장되는 진행 데이터 (SFR-071)
    [System.Serializable]
    public class SaveData
    {
        public string nickname;
        public int day = 1;   // 다음에 진행할 DAY
        public int money;     // 보유 자금
        // 재료 재고, 카페 평판, 스토리 선택 결과는 해당 기능 구현 시 추가
    }
}
