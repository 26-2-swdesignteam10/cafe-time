
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using CafeTime.Core;

namespace CafeTime.Story
{
    public class StoryManager : MonoBehaviour
    {
        [SerializeField, Range(1, 15)]
        private int previewDay = 1;

        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button skipButton;

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image portraitImage;

        [SerializeField] private StoryBackgroundEntry[] storyBackgrounds;
        [SerializeField] private StoryPortraitEntry[] speakerPortraits;

        [SerializeField, Min(0.001f)]
        private float typingSpeed = 0.04f;

        private (string speaker, string text, string backgroundId)[] lines;

        private int currentIndex;
        private int totalCharacters;

        private bool finished;
        private bool isTyping;

        private Coroutine typingCoroutine;

        // SFR-007, SFR-009: 현재 DAY의 스토리 불러오기
        private void Start()
        {
            if (dayText == null || speakerText == null ||
                dialogueText == null || skipButton == null ||
                backgroundImage == null || portraitImage == null)
            {
                Debug.LogError("StoryManager의 UI 연결을 확인하세요.");
                enabled = false;
                return;
            }

            skipButton.onClick.AddListener(SkipStory);

            int currentDay = SaveSystem.Current != null
                ? SaveSystem.Current.day
                : previewDay;

            dayText.text = "DAY " + currentDay;

            lines = StoryData.GetLines(currentDay);
            currentIndex = 0;

            if (lines.Length == 0)
            {
                FinishStory();
                return;
            }

            ShowDialogue();
        }

        // SFR-008: 마우스 및 키보드 입력으로 대사 진행
        private void Update()
        {
            if (finished) return;

            bool keyPressed = false;

            if (Keyboard.current != null)
            {
                foreach (var key in Keyboard.current.allKeys)
                {
                    if (key.wasPressedThisFrame)
                    {
                        keyPressed = true;
                        break;
                    }
                }
            }

            bool mouseClicked = false;

            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                RectTransform skipRect =
                    skipButton.GetComponent<RectTransform>();

                Canvas canvas =
                    skipButton.GetComponentInParent<Canvas>();

                Camera uiCamera = null;

                if (canvas != null &&
                    canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    uiCamera = canvas.worldCamera;
                }

                Vector2 mousePosition =
                    Mouse.current.position.ReadValue();

                bool clickedSkip =
                    RectTransformUtility.RectangleContainsScreenPoint(
                        skipRect, mousePosition, uiCamera);

                mouseClicked = !clickedSkip;
            }

            if (keyPressed || mouseClicked)
            {
                NextDialogue();
            }
        }

        // SFR-007: 스토리 장면에 따른 배경 이미지 변경
        private void SetBackground(string backgroundId)
        {
            if (string.IsNullOrEmpty(backgroundId))
            {
                return;
            }

            if (storyBackgrounds == null)
            {
                return;
            }

            foreach (var entry in storyBackgrounds)
            {
                if (entry != null &&
                    entry.Id == backgroundId &&
                    entry.Sprite != null)
                {
                    backgroundImage.sprite = entry.Sprite;
                    backgroundImage.enabled = true;
                    return;
                }
            }

            Debug.LogWarning(
                $"스토리 배경 '{backgroundId}'를 찾을 수 없습니다.");
        }

        // SFR-008: 화자에 따른 캐릭터 초상화 변경
        private void SetPortrait(string speaker)
        {
            Sprite selectedSprite = null;

            if (speakerPortraits != null)
            {
                foreach (var entry in speakerPortraits)
                {
                    if (entry != null && entry.Speaker == speaker)
                    {
                        selectedSprite = entry.Sprite;
                        break;
                    }
                }
            }

            portraitImage.sprite = selectedSprite;
            portraitImage.enabled = selectedSprite != null;
        }

        // SFR-008: 대사, 초상화, 배경 표시
        private void ShowDialogue()
        {
            speakerText.text = lines[currentIndex].speaker;
            dialogueText.text = lines[currentIndex].text;

            SetBackground(lines[currentIndex].backgroundId);
            SetPortrait(lines[currentIndex].speaker);

            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            totalCharacters = dialogueText.textInfo.characterCount;

            isTyping = true;
            typingCoroutine = StartCoroutine(TypeDialogue());
        }

        // SFR-008: 대사 타이핑 효과
        private IEnumerator TypeDialogue()
        {
            for (int i = 1; i <= totalCharacters; i++)
            {
                yield return new WaitForSecondsRealtime(typingSpeed);
                dialogueText.maxVisibleCharacters = i;
            }

            isTyping = false;
            typingCoroutine = null;
        }

        // SFR-008: 대사 진행
        public void NextDialogue()
        {
            if (finished) return;

            if (isTyping)
            {
                ShowFullDialogue();
                return;
            }

            currentIndex++;

            if (currentIndex >= lines.Length)
            {
                FinishStory();
                return;
            }

            ShowDialogue();
        }

        // SFR-008: 타이핑 중 입력 시 대사 전체 표시
        private void ShowFullDialogue()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            dialogueText.maxVisibleCharacters = int.MaxValue;
            isTyping = false;
        }

        // SFR-007: 스토리 건너뛰기
        public void SkipStory()
        {
            if (finished) return;

            FinishStory();
        }

        // SFR-007: 스토리 종료 후 오픈 준비 씬으로 이동
        private void FinishStory()
        {
            finished = true;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            isTyping = false;

            if (Application.CanStreamedLevelBeLoaded(SceneNames.OpenPrep))
            {
                SceneLoader.Load(SceneNames.OpenPrep);
            }
            else
            {
                speakerText.text = "시스템";
                dialogueText.text =
                    "스토리 재생 완료! OpenPrep 씬은 아직 준비되지 않았습니다.";

                dialogueText.maxVisibleCharacters = int.MaxValue;

                portraitImage.enabled = false;
                skipButton.interactable = false;
            }
        }

        private void OnDestroy()
        {
            if (skipButton != null)
            {
                skipButton.onClick.RemoveListener(SkipStory);
            }
        }
    }
}
