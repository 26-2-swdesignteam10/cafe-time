using System.Collections.Generic;
using UnityEngine;

namespace CafeTime.Core
{
    // 메뉴 1개의 HOT 또는 ICE 레시피 (SFR-051). 재료 조합(key 집합)으로 메뉴를 알아보고, 정량과의 오차로 등급을 매긴다.
    // HOT 은 도자기 잔, ICE 는 플라스틱 컵에 만들어야 한다. 정량은 임시값 (docs/GAME_DATA.md 4장)
    [System.Serializable]
    public class RecipeDefinition
    {
        [SerializeField] string menuId;
        [SerializeField] Temperature temperature;
        [SerializeField] List<RecipeAmount> amounts = new List<RecipeAmount>();

        public string MenuId => menuId;
        public Temperature Temperature => temperature;
        public IReadOnlyList<RecipeAmount> Amounts => amounts;

        public RecipeDefinition(string menuId, Temperature temperature, params RecipeAmount[] amounts)
        {
            this.menuId = menuId;
            this.temperature = temperature;
            this.amounts = new List<RecipeAmount>(amounts);
        }
    }
}
