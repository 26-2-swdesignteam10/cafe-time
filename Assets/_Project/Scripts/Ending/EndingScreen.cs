using CafeTime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Ending
{
    // 엔딩 화면 (SFR-073): 하루 정산이 정한 엔딩(Endings.Pending)의 그림·제목·멘트. 게임이 끝났으므로 저장 데이터를 지운다
    public class EndingScreen : MonoBehaviour
    {
        [SerializeField] Image background;
        [SerializeField] Image title;
        [SerializeField] Image line;
        [Tooltip("엔딩 01~04 순서")]
        [SerializeField] Sprite[] backgrounds;
        [SerializeField] Sprite[] titles;
        [SerializeField] Sprite[] lines;
        [SerializeField] Button mainMenuButton;

        void Start()
        {
            int index = Mathf.Clamp((int)Endings.Pending - 1, 0, backgrounds.Length - 1);
            background.sprite = backgrounds[index];
            SetLabel(title, titles[index]);
            SetLabel(line, lines[index]);
            if (!SaveSystem.IsPreview) SaveSystem.Delete(); // 이어하기 대상 없음
            mainMenuButton.onClick.AddListener(() => SceneLoader.Load(SceneNames.Title));
        }

        static void SetLabel(Image image, Sprite sprite)
        {
            image.sprite = sprite;
            if (sprite != null) image.rectTransform.sizeDelta = sprite.rect.size * 0.5f; // 글자 이미지는 절반 크기
        }
    }
}
