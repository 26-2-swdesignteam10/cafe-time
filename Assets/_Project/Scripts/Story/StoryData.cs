
using System;

namespace CafeTime.Story
{
    public static class StoryData
    {
        // SFR-007, SFR-009: DAY별 메인 스토리 및 장면 배경
        public static (string speaker, string text, string backgroundId)[] GetLines(int day)
        {
            switch (day)
            {
                case 1:
                    return new (string, string, string)[]
                    {
                        ("주인공", "드디어 카페를 열었다. 퇴직금으로 시작한 작은 베이커리 카페.", "CafeOutside"),
                        ("뉴스", "해외 전쟁의 영향으로 주식시장이 급격히 하락하고 있습니다.", "News"),
                        ("주인공", "뭐? 하필 지금 주식이 폭락한다고?", "CafeOutside"),
                        ("주인공", "지금 팔면 손해가 너무 큰데...", ""),
                        ("뉴스", "전문가들은 앞으로 약 15일이 중요한 고비가 될 것으로 전망하고 있습니다.", "News"),
                        ("주인공", "건물주에게 매일 임대료를 내기로 했으니까...", "CafeOutside"),
                        ("주인공", "좋아. 15일 동안 카페 운영으로 버텨보자.", "")
                    };

                case 2:
                    return new (string, string, string)[]
                    {
                        ("주인공", "하... 첫날부터 이렇게 정신없을 줄은 몰랐네.", "CafeInside"),
                        ("주인공", "그래도 임대료는 냈고.", ""),
                        ("주인공", "재료도 생각보다 많이 썼네. 발주도 신경 써야겠다.", ""),
                        ("주인공", "앞으로 14일... 일단 내일도 열어보자.", "")
                    };

                default:
                    return Array.Empty<(string, string, string)>();
            }
        }

        // SFR-009: 해당 DAY에 메인 스토리가 있는지 확인
        public static bool HasStory(int day)
        {
            return GetLines(day).Length > 0;
        }
    }
}
