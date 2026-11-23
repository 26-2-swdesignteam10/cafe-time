using UnityEngine;

namespace CafeTime.Core
{
    // 단골손님 1명의 설정 (SFR-009 DAY 9 "익숙한 얼굴들", ADR-0049). GameBalance 의 단골 목록 한 줄이다
    [System.Serializable]
    public class RegularCustomer
    {
        [Tooltip("저장 데이터에서 방문 횟수를 찾는 id")]
        public string id;
        [Tooltip("주문 대사에서 스스로 밝히는 이름")]
        public string displayName;
        [Tooltip("손님 외형 번호 (CustomerManager.looks 순서, 0부터)")]
        public int lookIndex;
        [Tooltip("늘 마시는 메뉴 id")]
        public string menuId;
        [Tooltip("늘 마시는 메뉴 HOT/ICE (온도 고정 메뉴면 무시)")]
        public Temperature temperature;
        [Tooltip("이 DAY 부터 매일 한 번 방문")]
        public int firstDay;

        public RegularCustomer(string id, string displayName, int lookIndex, string menuId, Temperature temperature, int firstDay)
        {
            this.id = id;
            this.displayName = displayName;
            this.lookIndex = lookIndex;
            this.menuId = menuId;
            this.temperature = temperature;
            this.firstDay = firstDay;
        }
    }
}
