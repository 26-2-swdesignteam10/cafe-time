using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 화면 위쪽에 잠깐 뜨는 안내 문구. 문구는 글자 이미지 (docs/ART_STYLE.md 4장)
    public class Toast : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] Image label;
        [Tooltip("ToastMessage 순서대로")]
        [SerializeField] Sprite[] messages;
        [SerializeField] float duration = 2f;

        static Toast instance;

        float hideAt;

        void Awake()
        {
            instance = this;
            panel.SetActive(false);
        }

        void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        public static void Show(ToastMessage message)
        {
            if (instance == null) return;
            var sprite = instance.messages[(int)message];
            instance.label.sprite = sprite;
            if (sprite != null) instance.label.rectTransform.sizeDelta = sprite.rect.size * 0.5f; // 글자 이미지는 절반 크기
            instance.panel.SetActive(true);
            instance.hideAt = Time.unscaledTime + instance.duration;
        }

        void Update()
        {
            if (panel.activeSelf && Time.unscaledTime >= hideAt) panel.SetActive(false);
        }
    }
}
