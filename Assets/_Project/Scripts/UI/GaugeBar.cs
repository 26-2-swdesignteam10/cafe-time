using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.UI
{
    // 기기 창 진행 게이지. 구조는 틀(GaugeFrame, Sliced) > FillArea > 채움(GaugeFill, Sliced) (StationUiBuilder.Progress).
    // 채움을 비율로 자르지 않고 너비로 줄여서 캡슐 양끝이 둥근 모양을 유지한다
    public static class GaugeBar
    {
        // 0~1 이면 게이지를 보이고 채움, 음수면 틀째 숨김
        public static void Show(Image fill, float progress)
        {
            if (fill == null) return;
            fill.transform.parent.parent.gameObject.SetActive(progress >= 0f);
            fill.rectTransform.anchorMax = new Vector2(Mathf.Clamp01(progress), 1f);
        }
    }
}
