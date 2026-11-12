using CafeTime.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 음료 판정 결과 표시 (SFR-051, 055, ADR-0027): 배경 패널 없이 완성품 그림이 퐁 떠오르고 완성도 도장이 쾅 찍힌다.
    // 메뉴 이름은 완성품 아래 글자. 잠깐 보였다가 사라진다
    public class JudgeResultView : MonoBehaviour
    {
        [SerializeField] RectTransform drink;
        [SerializeField] Image drinkImage;
        [SerializeField] RectTransform stamp;
        [SerializeField] Image stampImage;
        [SerializeField] TMP_Text menuText;

        [Tooltip("DrinkGrade 순서대로 (완벽, 좋음, 아쉬워요, 실패)")]
        [SerializeField] Sprite[] stamps = new Sprite[4];

        [Header("완성품 그림: menuIds[i] 의 HOT / ICE 그림 (없는 온도는 비워 둠)")]
        [SerializeField] string[] menuIds;
        [SerializeField] Sprite[] hotDrinks;
        [SerializeField] Sprite[] iceDrinks;
        [SerializeField] Sprite failedHot;
        [SerializeField] Sprite failedIce;

        [SerializeField] float duration = 2.5f;
        [SerializeField] float stampDelay = 0.3f;

        float shownAt;

        void Awake() => gameObject.SetActive(false);

        public void Show(Cup cup)
        {
            bool ceramic = cup.Type == CupType.Ceramic;
            Sprite sprite = null;
            if (cup.Menu != null)
            {
                int i = Array.IndexOf(menuIds, cup.Menu.Id);
                if (i >= 0) sprite = cup.Temperature == Temperature.Hot ? hotDrinks[i] : iceDrinks[i];
            }
            if (sprite == null) sprite = ceramic ? failedHot : failedIce; // 레시피에 없는 조합
            drinkImage.sprite = sprite;
            stampImage.sprite = stamps[(int)cup.Grade];
            menuText.text = cup.Menu != null ? OrderText.ItemLabel(cup.Menu, cup.Temperature) : "알 수 없는 음료";

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            shownAt = Time.unscaledTime;
            Animate(0);
        }

        void Update()
        {
            float t = Time.unscaledTime - shownAt;
            if (t >= duration) { gameObject.SetActive(false); return; }
            Animate(t);
        }

        // 완성품: 0.25초 동안 커지며 살짝 튀어 오름. 도장: stampDelay 뒤 크게 나타났다가 쾅 찍힘
        void Animate(float t)
        {
            drink.localScale = Vector3.one * Pop(Mathf.Clamp01(t / 0.25f));
            float s = (t - stampDelay) / 0.15f;
            stamp.gameObject.SetActive(s > 0);
            if (s > 0) stamp.localScale = Vector3.one * Mathf.Lerp(1.8f, 1f, Mathf.Clamp01(s));
        }

        // 0 → 1.1 → 1
        static float Pop(float x) => x < 0.7f ? Mathf.Lerp(0f, 1.1f, x / 0.7f) : Mathf.Lerp(1.1f, 1f, (x - 0.7f) / 0.3f);
    }
}
