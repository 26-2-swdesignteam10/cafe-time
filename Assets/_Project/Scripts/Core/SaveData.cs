using System.Collections.Generic;

namespace CafeTime.Core
{
    // 하루 정산 후 자동 저장되는 진행 데이터 (SFR-071)
    [System.Serializable]
    public class SaveData
    {
        public string nickname;
        public int day = 1;   // 다음에 진행할 DAY
        public int money;     // 보유 자금
        public float reputation = 3f; // 카페 평판 ★1~5 (SFR-072)
        public int bestScore;         // 하루 최고 점수 (SFR-067). 하루 정산에서 갱신해 저장
        public float visitorRate = 1f; // 손님 입장 빈도 배율. 인플루언서 방문 결과로 바뀌고 다음 DAY 부터 적용 (SFR-009, ADR-0048)
        public List<ChoiceRecord> choices = new List<ChoiceRecord>(); // 스토리·이벤트 선택 결과 (SFR-008, 071)
        public List<RegularVisit> regulars = new List<RegularVisit>(); // 단골손님 방문 횟수 (SFR-009, ADR-0049)
        public int[] equipmentLevels = new int[0]; // 장비 업그레이드 단계, Equipment 순서 (ADR-0057)
        public List<string> hiredStaff = new List<string>(); // 오늘 고용한 알바생 id. 하루 정산에서 일당을 내고 비운다 (ADR-0057)

        public int GetLevel(Equipment equipment)
        {
            int i = (int)equipment;
            return equipmentLevels != null && i < equipmentLevels.Length ? equipmentLevels[i] : 0;
        }

        public void SetLevel(Equipment equipment, int level)
        {
            int i = (int)equipment;
            if (equipmentLevels == null || equipmentLevels.Length <= i) System.Array.Resize(ref equipmentLevels, i + 1);
            equipmentLevels[i] = level;
        }

        public int GetRegularVisits(string id) => regulars.Find(r => r.id == id)?.visits ?? 0;

        public void AddRegularVisit(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            var record = regulars.Find(r => r.id == id);
            if (record == null)
            {
                record = new RegularVisit { id = id };
                regulars.Add(record);
            }
            record.visits++;
        }

        // 선택 결과 기록. 같은 선택 지점을 다시 고르면(같은 DAY 재시작) 덮어쓴다
        public void SetChoice(string id, string result)
        {
            if (string.IsNullOrEmpty(id)) return;
            var record = choices.Find(c => c.id == id);
            if (record == null)
            {
                record = new ChoiceRecord { id = id };
                choices.Add(record);
            }
            record.result = result;
        }

        // 고른 적이 없으면 null
        public string GetChoice(string id) => choices.Find(c => c.id == id)?.result;
    }
}
