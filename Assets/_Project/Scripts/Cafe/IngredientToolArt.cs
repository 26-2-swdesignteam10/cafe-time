using UnityEngine;

namespace CafeTime.Cafe
{
    // 재료 작업 창의 재료 전용 그림 (2026-11-17 사용자 요청: 딸기청은 딸기청 병처럼 재료마다 따로, ADR-0038).
    // 비어 있는 칸은 도구 공용 그림을 쓴다. 그림은 공용 그림과 같은 캔버스·기하라 잡는 영역·줄기 위치가 같다 (Art/UI/DrinkTools/Generation.md)
    [System.Serializable]
    public class IngredientToolArt
    {
        public string ingredientId;
        public Sprite idle;     // 펌프 병 / 숟가락 꽂힌 병 / 티백 상자 / 팩·병
        public Sprite active;   // 펌프 눌림 / 숟가락 빠진 병 / 붓는 팩·병
        public Sprite spoon;    // 숟가락형: 끄는 숟가락
        public Sprite spoonPour; // 숟가락형: 컵 위에서 기울인 숟가락
        public bool powder;     // 숟가락형 가루 (낙하 그림이 액체 대신 가루)
        public Color streamColor = Color.white; // 중립색 줄기·가루에 곱하는 색
    }
}
