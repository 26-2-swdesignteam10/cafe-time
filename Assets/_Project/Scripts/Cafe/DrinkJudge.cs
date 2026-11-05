using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 음료 완성 판정 (SFR-051, 055)
    //  - 들어간 재료 조합(key 집합)이 같은 레시피를 메뉴로 인식. 없으면 '실패한 음료'
    //  - 재료별 오차 비율 중 가장 큰 값으로 완벽 / 좋음 / 나쁨
    //  - HOT 은 도자기 잔, ICE 는 플라스틱 컵. 잔이 다르면 정량과 관계없이 '나쁨'
    public static class DrinkJudge
    {
        public static void Finish(Cup cup, GameBalance balance)
        {
            var keys = cup.Contents.Keys.ToList();
            var recipe = balance.Recipes.FirstOrDefault(r =>
                r.Amounts.Count == keys.Count && r.Amounts.All(a => keys.Contains(a.Key)));
            var menu = recipe != null ? balance.FindMenu(recipe.MenuId) : null;
            if (menu == null)
            {
                cup.Finish(null, Temperature.None, DrinkGrade.Failed);
                return;
            }

            float worst = recipe.Amounts.Max(a => Mathf.Abs(cup.Contents.Get(a.Key) - a.Amount) / a.Amount);
            var grade = worst <= balance.PerfectTolerance ? DrinkGrade.Perfect
                : worst <= balance.GoodTolerance ? DrinkGrade.Good : DrinkGrade.Bad;
            var rightCup = recipe.Temperature == Temperature.Hot ? CupType.Ceramic : CupType.Plastic;
            if (cup.Type != rightCup) grade = DrinkGrade.Bad;
            cup.Finish(menu, recipe.Temperature, grade);
        }
    }
}
