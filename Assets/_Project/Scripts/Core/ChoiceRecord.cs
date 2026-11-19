namespace CafeTime.Core
{
    // 저장 파일의 스토리·이벤트 선택 결과 한 줄 (SFR-008, 071). JsonUtility가 Dictionary를 저장하지 못해 목록으로 저장
    [System.Serializable]
    public class ChoiceRecord
    {
        public string id;     // 선택 지점 (예: "day4_group_order")
        public string result; // 고른 결과 (예: "accept", "decline")
    }
}
