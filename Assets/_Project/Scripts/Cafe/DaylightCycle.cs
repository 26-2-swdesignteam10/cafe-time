using CafeTime.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CafeTime.Cafe
{
    // 영업 시간대 연출 (2026-11-19 사용자 요청): 09~12 쨍한 아침, 12~15 낮, 15~18 노을.
    // 12시·15시 경계 전후 게임 시간 blendHours 동안 서서히 바뀌고, 노을은 영업 종료로 갈수록 짙어진다.
    // 창문은 두 겹(아래 = 앞 시간대 그림, 위 = 다음 시간대 그림을 알파로)으로 교차 페이드. 색 값은 docs/ART_STYLE.md "시간대 조명"
    public class DaylightCycle : MonoBehaviour
    {
        [SerializeField] Light2D globalLight;
        [SerializeField] SpriteRenderer[] windowBase;
        [SerializeField] SpriteRenderer[] windowOver;
        [SerializeField] Sprite morningWindow;
        [SerializeField] Sprite noonWindow;
        [SerializeField] Sprite sunsetWindow;

        [Header("조명 (색, 강도)")]
        [SerializeField] Color morningLight = Color.white;
        [SerializeField] float morningIntensity = 1f;
        [SerializeField] Color noonLight = Color.white;
        [SerializeField] float noonIntensity = 1f;
        [SerializeField] Color sunsetStartLight = Color.white;
        [SerializeField] float sunsetStartIntensity = 1f;
        [SerializeField] Color sunsetEndLight = Color.white;
        [SerializeField] float sunsetEndIntensity = 1f;

        [Header("시각 (게임 시)")]
        [SerializeField] float noonHour = 12f;
        [SerializeField] float sunsetHour = 15f;
        [SerializeField] float blendHours = 0.5f;

        void Update()
        {
            var clock = GameClock.Instance;
            if (clock == null) return;
            float hour = clock.Hour;
            float half = blendHours / 2f;
            float toNoon = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(noonHour - half, noonHour + half, hour));
            float toSunset = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(sunsetHour - half, sunsetHour + half, hour));
            float deepen = Mathf.InverseLerp(sunsetHour + half, GameBalance.Instance.CloseHour, hour);

            var sunsetColor = Color.Lerp(sunsetStartLight, sunsetEndLight, deepen);
            float sunsetIntensity = Mathf.Lerp(sunsetStartIntensity, sunsetEndIntensity, deepen);
            globalLight.color = Color.Lerp(Color.Lerp(morningLight, noonLight, toNoon), sunsetColor, toSunset);
            globalLight.intensity = Mathf.Lerp(Mathf.Lerp(morningIntensity, noonIntensity, toNoon), sunsetIntensity, toSunset);

            // 두 경계는 겹치지 않으므로 노을로 넘어가기 시작하면 아래 그림은 낮
            bool late = toSunset > 0f;
            ShowWindows(late ? noonWindow : morningWindow, late ? sunsetWindow : noonWindow, late ? toSunset : toNoon);
        }

        void ShowWindows(Sprite under, Sprite over, float alpha)
        {
            for (int i = 0; i < windowBase.Length; i++)
            {
                if (windowBase[i].sprite != under) windowBase[i].sprite = under;
                if (windowOver[i].sprite != over) windowOver[i].sprite = over;
                windowOver[i].color = new Color(1, 1, 1, alpha);
            }
        }
    }
}
