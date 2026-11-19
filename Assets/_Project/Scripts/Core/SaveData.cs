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
        public List<IngredientStock> stocks = new List<IngredientStock>(); // 재료 재고, 기본 단위 (ADR-0010)
        public List<ChoiceRecord> choices = new List<ChoiceRecord>(); // 스토리·이벤트 선택 결과 (SFR-008, 071)

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

        public int GetStock(string id)
        {
            var stock = stocks.Find(s => s.id == id);
            return stock != null ? stock.amount : 0;
        }

        // 재고 증감. 남은 재고는 다음 날로 이월된다 (SFR-010)
        public void AddStock(string id, int amount)
        {
            var stock = stocks.Find(s => s.id == id);
            if (stock == null)
            {
                stock = new IngredientStock { id = id };
                stocks.Add(stock);
            }
            stock.amount += amount;
        }
    }
}
