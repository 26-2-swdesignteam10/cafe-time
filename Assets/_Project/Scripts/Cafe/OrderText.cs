using System.Collections.Generic;
using System.Text;
using CafeTime.Core;

namespace CafeTime.Cafe
{
    // 주문 관련 표시 문자열 (메뉴 이름은 데이터라 폰트로 표시한다, docs/ART_STYLE.md 4장)
    public static class OrderText
    {
        static readonly string[] CupCounts = { "", "한", "두", "세", "네", "다섯", "여섯", "일곱", "여덟" };
        static readonly string[] PieceCounts = { "", "하나", "두 개", "세 개", "네 개", "다섯 개", "여섯 개", "일곱 개", "여덟 개" };

        // "ICE 바닐라라떼" / "딸기 케이크"
        public static string ItemLabel(MenuDefinition menu, Temperature temperature) =>
            temperature == Temperature.None ? menu.DisplayName : $"{(temperature == Temperature.Hot ? "HOT" : "ICE")} {menu.DisplayName}";

        // 손님 주문 대사 (SFR-026): "아이스 바닐라라떼 한 잔이랑 딸기 케이크 하나 주세요."
        public static string Sentence(IReadOnlyList<OrderLine> lines)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                bool last = i == lines.Count - 1;
                if (line.Temperature != Temperature.None && line.Menu.OnlyTemperature == Temperature.None)
                    sb.Append(line.Temperature == Temperature.Hot ? "따뜻한 " : "아이스 ");
                sb.Append(line.Menu.DisplayName).Append(' ');
                int n = UnityEngine.Mathf.Clamp(line.Quantity, 1, CupCounts.Length - 1);
                if (line.Menu.IsDessert)
                    sb.Append(PieceCounts[n]).Append(last ? "" : "랑 "); // 하나랑 / 두 개랑
                else
                    sb.Append(CupCounts[n]).Append(" 잔").Append(last ? "" : "이랑 ");
            }
            sb.Append(" 주세요.");
            return sb.ToString();
        }
    }
}
