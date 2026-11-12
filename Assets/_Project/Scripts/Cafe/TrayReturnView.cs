using System.Linq;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 맵의 퇴식구 그림 (SFR-060): 반납된 쟁반 수만큼 잔·접시·쓰레기가 담긴 쟁반이 카운터 앞쪽부터 보인다.
    // 쟁반이 없으면 빈 카운터. 창을 열지 않아도 쌓인 정도를 알 수 있다
    public class TrayReturnView : MonoBehaviour
    {
        [SerializeField] SpriteRenderer target;
        [Tooltip("쟁반 0개, 1개, 2개 ... 일 때 그림")]
        [SerializeField] Sprite[] byCount;

        void Start()
        {
            TrayReturn.Instance.Changed += Refresh;
            Refresh();
        }

        void OnDestroy()
        {
            if (TrayReturn.Instance != null) TrayReturn.Instance.Changed -= Refresh;
        }

        void Refresh()
        {
            int count = TrayReturn.Instance.Slots.Count(tray => tray != null);
            var sprite = byCount[Mathf.Clamp(count, 0, byCount.Length - 1)];
            if (sprite != null) target.sprite = sprite;
        }
    }
}
