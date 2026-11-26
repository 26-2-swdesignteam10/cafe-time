using TMPro;
using UnityEngine;

namespace CafeTime.EspressoShot
{
    // 그라인더 LCD 글자. 분쇄 중에는 남은 시간 (재고 없음, ADR-0056)
    public class GrinderDisplay : MonoBehaviour
    {
        public Grinder grinder;
        public TMP_Text label;

        void Reset() { label = GetComponent<TMP_Text>(); }

        void Update()
        {
            if (grinder == null || label == null) return;

            if (grinder.Grinding)
                label.text = "분쇄 중…\n" + Mathf.CeilToInt(grinder.TimeLeft) + "초 남음";
            else
                label.text = "";
        }
    }
}
