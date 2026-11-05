using System.Collections.Generic;
using CafeTime.Core;

namespace CafeTime.Cafe
{
    // 영업 중 재료 재고 사용 (SFR-049). 재고는 저장 데이터의 기본 단위(정수 g·개)라
    // 우유처럼 조금씩 붓는 양은 소수점을 모아 두었다가 1g 단위로 차감한다
    public static class Pantry
    {
        static readonly Dictionary<string, float> carry = new Dictionary<string, float>();   // 아직 차감하지 않은 소수점 사용량
        static readonly Dictionary<string, int> opening = new Dictionary<string, int>();     // 영업 시작 때 재고 (남은 양 % 기준)

        static SaveData Save => SaveSystem.Current;

        // 영업 시작 때 호출
        public static void StartDay()
        {
            carry.Clear();
            opening.Clear();
            foreach (var stock in Save.stocks) opening[stock.id] = stock.amount;
        }

        public static float Available(string id) => Save.GetStock(id) - (carry.TryGetValue(id, out var c) ? c : 0f);

        // 부족하면 false (호출한 화면이 "재료가 부족합니다." 표시)
        public static bool TryUse(string id, float amount)
        {
            if (Available(id) + 0.0001f < amount) return false;
            float used = (carry.TryGetValue(id, out var c) ? c : 0f) + amount;
            int whole = (int)used;
            Save.AddStock(id, -whole);
            carry[id] = used - whole;
            return true;
        }

        // 남은 양 % (영업 시작 때 재고 = 100%)
        public static int PercentLeft(string id)
        {
            int start = opening.TryGetValue(id, out var s) ? s : 0;
            if (start <= 0) return Available(id) > 0 ? 100 : 0;
            return UnityEngine.Mathf.Clamp(UnityEngine.Mathf.CeilToInt(Available(id) / start * 100f), 0, 100);
        }
    }
}
