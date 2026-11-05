using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 음료 판정 결과 표시 (SFR-051, 055): 메뉴명 + 등급 글자 이미지. 잠깐 보였다가 사라진다
    public class JudgeResultView : MonoBehaviour
    {
        [SerializeField] TMP_Text menuText;
        [SerializeField] Image gradeImage;
        [Tooltip("DrinkGrade 순서대로 (완벽, 좋음, 나쁨, 실패한 음료)")]
        [SerializeField] Sprite[] grades;
        [SerializeField] float duration = 2.5f;

        float hideAt;

        void Awake() => gameObject.SetActive(false);

        public void Show(Cup cup)
        {
            menuText.text = cup.Menu != null ? OrderText.ItemLabel(cup.Menu, cup.Temperature) : "";
            var sprite = grades[(int)cup.Grade];
            gradeImage.sprite = sprite;
            if (sprite != null) gradeImage.rectTransform.sizeDelta = sprite.rect.size * 0.5f; // 글자 이미지는 절반 크기
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            hideAt = Time.unscaledTime + duration;
        }

        void Update()
        {
            if (Time.unscaledTime >= hideAt) gameObject.SetActive(false);
        }
    }
}
