
using System;

namespace CafeTime.Story
{
    public static class StoryData
    {
        // SFR-007, SFR-009: DAY별 메인 스토리 및 장면 배경
        // SFR-008: 대사별 초상화 및 선택지 설정
        public static (
            string speaker,
            string text,
            string backgroundId,
            string portraitId,
            string choiceId
        )[] GetLines(int day)
        {
            switch (day)
            {
                // DAY 1: 프롤로그
                case 1:
                    return new (string, string, string, string, string)[]
                    {
                        ("주인공", "드디어 퇴직금으로 카페를 열었다.", "CafeOutside", "Main_Default", ""),
                        ("뉴스", "해외 전쟁의 영향으로 주식시장이 급격히 하락하고 있습니다.", "News", "", ""),
                        ("주인공", "뭐? 하필 지금 주식이 폭락한다고?", "CafeOutside", "Main_Embarrassed", ""),
                        ("주인공", "지금 팔면 손해가 너무 큰데...", "", "Main_Worried", ""),
                        ("뉴스", "전문가들은 앞으로 약 15일이 중요한 고비가 될 것으로 전망하고 있습니다.", "News", "", ""),
                        ("주인공", "건물주에게 매일 임대료를 내기로 했으니까...", "CafeOutside", "Main_Worried", ""),
                        ("주인공", "좋아. 15일 동안 카페 운영으로 버텨보자.", "", "Main_Determined", "")
                    };

                // DAY 2: 첫 영업을 마친 후
                case 2:
                    return new (string, string, string, string, string)[]
                    {
                        ("주인공", "하... 첫날부터 이렇게 정신없을 줄은 몰랐네.", "CafeInside", "Main_Worried", ""),
                        ("주인공", "그래도 임대료는 냈고.", "", "Main_Default", ""),
                        ("주인공", "재료도 생각보다 많이 썼네. 발주도 신경 써야겠다.", "", "Main_Worried", ""),
                        ("주인공", "앞으로 14일... 일단 내일도 열어보자.", "", "Main_Determined", "")
                    };

                // DAY 5: 자금 압박
                case 5:
                    return new (string, string, string, string, string)[]
                    {
                        ("주인공", "어디 보자... 오늘도 장부부터 확인해야겠네.", "CafeInside", "Main_Default", ""),
                        ("주인공", "임대료에 재료비까지... 생각보다 돈이 빠르게 줄고 있어.", "", "Main_Worried", ""),
                        ("주인공", "분명 손님도 꽤 받았는데, 남는 돈은 왜 이렇게 적은 거야?", "", "Main_Embarrassed", ""),
                        ("뉴스", "해외 정세의 불확실성이 이어지며 주식시장의 변동성이 계속되고 있습니다.", "News2", "", ""),
                        ("주인공", "아직 주식도 회복될 기미가 없네.", "CafeInside", "Main_Worried", ""),
                        ("주인공", "지금 팔자니 손해가 크고, 임대료는 매일 내야 하고...", "", "Main_Worried", ""),
                        ("주인공", "가만히 걱정만 하고 있을 수는 없어.", "", "Main_Default", ""),
                        ("주인공", "오늘은 손님 한 명 한 명에게 더 신경 써보자!", "", "Main_Happy", "")
                    };

                // DAY 9: 익숙한 얼굴들
                case 9:
                    return new (string, string, string, string, string)[]
                    {
                        ("단골 손님", "사장님! 오늘도 저번에 마셨던 걸로 부탁드려요.", "CafeOutside", "", ""),
                        ("주인공", "아, 아이스 바닐라라떼 맞으시죠?", "", "Main_Default", ""),
                        ("단골 손님", "맞아요! 이제 제가 뭘 마시는지 기억하시네요?", "", "", ""),
                        ("주인공", "자주 찾아주시니까 저도 모르게 외웠어요.", "", "Main_Happy", ""),
                        ("단골 손님", "여기는 올 때마다 마음이 편해요. 오래오래 열어주세요!", "", "", ""),
                        ("주인공", "오래오래라...", "", "Main_Default", ""),
                        ("주인공", "사실 난 15일만 어떻게든 버텨보려고 했는데.", "", "Main_Worried", ""),
                        ("주인공", "이렇게 찾아주는 손님들이 있다는 게 꽤 기쁘네.", "", "Main_Default", ""),
                        ("주인공", "내일도 맛있는 커피를 준비해야겠다.", "", "Main_Happy", "")
                    };

                // DAY 12: 카페에 대한 애착
                case 12:
                    return new (string, string, string, string, string)[]
                    {
                        ("주인공", "벌써 12일째라니. 시간이 참 빠르네.", "CafeOutside", "Main_Default", ""),
                        ("주인공", "처음에는 그저 15일 동안 임대료만 내면서 버틸 생각이었는데...", "CafeOutside", "Main_Worried", ""),
                        ("주인공", "요즘은 가게 문을 열기도 전에 손님들 얼굴이 떠올라.", "", "Main_Default", ""),
                        ("주인공", "내가 만든 커피를 마시고 웃던 사람들, 매일 같은 자리에 앉는 단골들...", "", "Main_Default", ""),
                        ("주인공", "어느새 이 카페가 내 일상의 일부가 됐구나.", "CafeInside", "Main_Happy", ""),
                        ("주인공", "15일이 끝나면 이곳은 어떻게 되는 걸까?", "", "Main_Worried", ""),
                        ("주인공", "이제는 단순히 돈 때문에 지키고 싶은 게 아니야.", "", "Main_Determined", ""),
                        ("주인공", "가능하다면 앞으로도 계속 이 카페의 문을 열고 싶어.", "", "Main_Happy", "")
                    };

                // DAY 14: 전쟁 협상 및 주식 회복 소식
                case 14:
                    return new (string, string, string, string, string)[]
                    {
                        ("뉴스", "해외 전쟁 당사국들이 협상에 나서면서 시장의 불안이 다소 완화되고 있습니다.", "News3", "", ""),
                        ("뉴스", "이에 따라 주식시장도 점차 회복세를 보이고 있습니다.", "", "", ""),
                        ("주인공", "뭐? 드디어 협상이 시작됐다고?", "CafeInside", "Main_Embarrassed", ""),
                        ("주인공", "기다리던 소식이 드디어 들려오네.", "", "Main_Default", ""),
                        ("주인공", "조금만 더 기다리면 주식도 회복될 수 있을까?", "", "Main_Worried", ""),
                        ("주인공", "그동안 이 날만 기다렸는데... 이상하게 마음이 복잡하네.", "", "Main_Worried", ""),
                        ("주인공", "처음에는 돈을 지키기 위해 카페를 운영했지만, 지금은 달라.", "", "Main_Default", ""),
                        ("주인공", "손님들과 함께했던 이 공간을 쉽게 포기하고 싶지 않아.", "", "Main_Determined", ""),
                        ("주인공", "좋아. 마지막 날까지 제대로 해보자!", "", "Main_Happy", "")
                    };

                // DAY 15: 마지막 하루
                case 15:
                    return new (string, string, string, string, string)[]
                    {
                        ("주인공", "드디어 15일째 아침이 밝았네.", "CafeOutside", "Main_Default", ""),
                        ("주인공", "처음에는 이 날이 오기만을 기다렸는데...", "", "Main_Worried", ""),
                        ("주인공", "막상 마지막 날이라고 생각하니까 기분이 묘해.", "", "Main_Default", ""),
                        ("주인공", "임대료 때문에 걱정하고, 재료가 떨어져서 당황하고...", "CafeOutside", "Main_Embarrassed", ""),
                        ("주인공", "그래도 내 커피를 좋아해 주는 손님들을 만났어.", "", "Main_Default", ""),
                        ("주인공", "15일 전의 나는 이 카페가 이렇게 소중해질 줄 몰랐겠지.", "", "Main_Happy", ""),
                        ("주인공", "오늘 영업이 어떻게 끝날지는 아직 모르겠어.", "", "Main_Worried", ""),
                        ("주인공", "하지만 후회는 남기고 싶지 않아.", "", "Main_Determined", ""),
                        ("주인공", "자, 마지막 날 영업도 힘내서 시작해보자!", "", "Main_Happy", "")
                    };

                // DAY 3, 4, 6, 7, 8, 10, 11, 13:
                // 메인 스토리 없음. 영업 또는 인게임 이벤트 진행
                default:
                    return Array.Empty<(
                        string speaker,
                        string text,
                        string backgroundId,
                        string portraitId,
                        string choiceId
                    )>();
            }
        }

        // SFR-009: 해당 DAY에 메인 스토리가 있는지 확인
        public static bool HasStory(int day)
        {
            return GetLines(day).Length > 0;
        }
    }
}
