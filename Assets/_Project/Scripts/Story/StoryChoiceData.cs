
using System;

namespace CafeTime.Story
{
    public static class StoryChoiceData
    {
        // SFR-008: 선택지 문구, 선택 후 대사 및 표정
        public static (
            string label,
            string speaker,
            string text,
            string portraitId,
            string resultId
        )[] GetOptions(string choiceId)
        {
            switch (choiceId)
            {
                case "day2_supplies":
                    return new (string, string, string, string, string)[]
                    {
                        (
                            "재료를 넉넉하게 발주하자.",
                            "주인공",
                            "재료가 부족해서 손님을 놓치는 일은 없어야지!",
                            "Main_Determined",
                            "day2_supplies_extra"
                        ),
                        (
                            "필요한 만큼만 구매하자.",
                            "주인공",
                            "당장은 돈을 아껴야 하니까 필요한 것만 사자.",
                            "Main_Worried",
                            "day2_supplies_careful"
                        )
                    };

                default:
                    return Array.Empty<(
                        string label,
                        string speaker,
                        string text,
                        string portraitId,
                        string resultId
                    )>();
            }
        }
    }
}
