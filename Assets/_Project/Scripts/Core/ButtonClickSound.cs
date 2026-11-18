using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CafeTime.Core
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickSound : MonoBehaviour
    {
        Button button;
        [SerializeField] bool playClickSound = true;

        // 게임 시작 시 연결하고, 이후 씬 전환도 감지한다.
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Initialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            AttachToButtons();
        }

        static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AttachToButtons();
        }

        static void AttachToButtons()
        {
            // 꺼져 있는 팝업과 버튼 템플릿도 포함한다.
            var buttons = Object.FindObjectsByType<Button>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var target in buttons)
            {
                if (target.GetComponent<ButtonClickSound>() == null)
                    target.gameObject.AddComponent<ButtonClickSound>();
            }
        }

        void OnEnable()
        {
            button = GetComponent<Button>();

            // 중복 연결 방지
            button.onClick.RemoveListener(PlayClick);
            button.onClick.AddListener(PlayClick);
        }

        void OnDisable()
        {
            if (button != null)
                button.onClick.RemoveListener(PlayClick);
        }

        void PlayClick()
        {
            // 이 버튼에서 기본 클릭음을 끈 경우
            if (!playClickSound) return;

            if (button == null || !button.IsInteractable()) return;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayButtonClick();
        }
    }
}
