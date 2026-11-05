
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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

        // SFR-008: 선택지 UI
        [SerializeField] private GameObject choicePanel;
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private GameObject nextHint;

        [SerializeField, Min(0.001f)]
        private float typingSpeed = 0.04f;

        private (
            string speaker,
            string text,
            string backgroundId,
            string portraitId,
            string choiceId
        )[] lines;

        private (
            string label,
            string speaker,
            string text,
            string portraitId,
            string resultId
        )[] currentChoices;

        private UnityAction[] choiceListeners;

        private int currentIndex;
        private int totalCharacters;
        private int ignoreAdvanceFrame = -1;

        private bool finished;
        private bool isTyping;
        private bool waitingForChoice;
        private bool showingChoiceReply;

        private Coroutine typingCoroutine;

        // SFR-007, SFR-009: 현재 DAY 스토리 불러오기
        private void Start()
        {
            if (speakerText == null ||
                dialogueText == null ||
                skipButton == null ||
                backgroundImage == null ||
                portraitImage == null ||
                choicePanel == null ||
                choiceButtons == null ||
                choiceButtons.Length == 0)
            {
                Debug.LogError("StoryManager의 UI 연결을 확인하세요.");
                enabled = false;
                return;
            }

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (choiceButtons[i] == null)
                {
                    Debug.LogError("Choice Buttons 연결을 확인하세요.");
                    enabled = false;
                    return;
                }
            }

            skipButton.onClick.AddListener(SkipStory);

            choiceListeners = new UnityAction[choiceButtons.Length];

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                int index = i;

                choiceListeners[i] = () => SelectChoice(index);

                choiceButtons[i].onClick.AddListener(
                    choiceListeners[i]);
            }

            choicePanel.SetActive(false);

            int currentDay = SaveSystem.Current != null
                ? SaveSystem.Current.day
                : previewDay;

            if (dayText != null)
            {
                dayText.text = "DAY " + currentDay;
            }

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
            if (finished || waitingForChoice ||
                Time.frameCount == ignoreAdvanceFrame)
            {
                return;
            }

            bool keyPressed =
                Keyboard.current != null &&
                Keyboard.current.anyKey.wasPressedThisFrame;

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

        // SFR-007: 대사에 따른 배경 변경
        private void SetBackground(string backgroundId)
        {
            if (string.IsNullOrEmpty(backgroundId) ||
                storyBackgrounds == null)
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

        // SFR-008: 대사별 초상화 및 표정 변경
        private void SetPortrait(string portraitId)
        {
            Sprite selectedSprite = null;

            if (speakerPortraits != null)
            {
                foreach (var entry in speakerPortraits)
                {
                    if (entry != null &&
                        entry.Id == portraitId)
                    {
                        selectedSprite = entry.Sprite;
                        break;
                    }
                }
            }

            portraitImage.sprite = selectedSprite;
            portraitImage.enabled = selectedSprite != null;
        }

        // SFR-008: 일반 대사 출력
        private void ShowDialogue()
        {
            showingChoiceReply = false;

            DisplayDialogue(
                lines[currentIndex].speaker,
                lines[currentIndex].text,
                lines[currentIndex].backgroundId,
                lines[currentIndex].portraitId);
        }

        // SFR-008: 대사, 배경, 초상화 표시
        private void DisplayDialogue(
            string speaker,
            string text,
            string backgroundId,
            string portraitId)
        {
            StopTyping();

            speakerText.text = speaker;
            dialogueText.text = text;

            SetBackground(backgroundId);
            SetPortrait(portraitId);

            if (nextHint != null)
            {
                nextHint.SetActive(true);
            }

            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            totalCharacters = dialogueText.textInfo.characterCount;

            if (totalCharacters == 0)
            {
                dialogueText.maxVisibleCharacters = int.MaxValue;
                OnLineFullyVisible();
                return;
            }

            isTyping = true;
            typingCoroutine = StartCoroutine(TypeDialogue());
        }

        // SFR-008: 타이핑 효과
        private IEnumerator TypeDialogue()
        {
            for (int i = 1; i <= totalCharacters; i++)
            {
                yield return new WaitForSecondsRealtime(typingSpeed);

                dialogueText.maxVisibleCharacters = i;
            }

            isTyping = false;
            typingCoroutine = null;

            OnLineFullyVisible();
        }

        // SFR-008: 대사 출력 완료 후 선택지 확인
        private void OnLineFullyVisible()
        {
            if (finished || showingChoiceReply)
            {
                return;
            }

            string choiceId = lines[currentIndex].choiceId;

            if (!string.IsNullOrEmpty(choiceId))
            {
                ShowChoices(choiceId);
            }
        }

        // SFR-008: 선택지 버튼 표시
        private void ShowChoices(string choiceId)
        {
            var options = StoryChoiceData.GetOptions(choiceId);

            if (options.Length == 0)
            {
                Debug.LogWarning(
                    $"선택지 '{choiceId}'를 찾을 수 없습니다.");
                return;
            }

            if (options.Length > choiceButtons.Length)
            {
                Debug.LogError(
                    $"선택지 '{choiceId}'에 필요한 버튼 수가 부족합니다.");
                return;
            }

            currentChoices = options;
            waitingForChoice = true;

            if (nextHint != null)
            {
                nextHint.SetActive(false);
            }

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                bool visible = i < options.Length;

                choiceButtons[i].gameObject.SetActive(visible);

                if (!visible)
                {
                    continue;
                }

                TMP_Text buttonText =
                    choiceButtons[i].GetComponentInChildren<TMP_Text>(true);

                if (buttonText != null)
                {
                    buttonText.text = options[i].label;
                }
            }

            choicePanel.SetActive(true);
        }

        // SFR-008: 선택 결과에 따른 대사 분기
        private void SelectChoice(int choiceIndex)
        {
            if (!waitingForChoice ||
                currentChoices == null ||
                choiceIndex < 0 ||
                choiceIndex >= currentChoices.Length)
            {
                return;
            }

            var selected = currentChoices[choiceIndex];

            waitingForChoice = false;
            choicePanel.SetActive(false);

            // 버튼 클릭이 같은 프레임에서 대사 진행으로
            // 중복 처리되지 않도록 방지
            ignoreAdvanceFrame = Time.frameCount;

            // 추후 SaveData의 선택 결과 저장과 연결
            Debug.Log(
                $"스토리 선택 결과: {selected.resultId}");

            showingChoiceReply = true;

            // 배경 ID가 빈 문자열이므로 현재 배경 유지
            DisplayDialogue(
                selected.speaker,
                selected.text,
                "",
                selected.portraitId);
        }

        // SFR-008: 대사 진행
        public void NextDialogue()
        {
            if (finished || waitingForChoice)
            {
                return;
            }

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

        // SFR-008: 타이핑 중 입력 시 전체 대사 출력
        private void ShowFullDialogue()
        {
            StopTyping();

            dialogueText.maxVisibleCharacters = int.MaxValue;

            OnLineFullyVisible();
        }

        private void StopTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            isTyping = false;
        }

        // SFR-007: 스토리 건너뛰기
        public void SkipStory()
        {
            if (finished)
            {
                return;
            }

            FinishStory();
        }

        // SFR-007: 스토리 종료 후 오픈 준비 씬으로 이동
        private void FinishStory()
        {
            finished = true;
            waitingForChoice = false;

            StopTyping();
            choicePanel.SetActive(false);

            if (Application.CanStreamedLevelBeLoaded(
                SceneNames.OpenPrep))
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

            if (choiceListeners == null ||
                choiceButtons == null)
            {
                return;
            }

            for (int i = 0; i < choiceListeners.Length; i++)
            {
                if (choiceButtons[i] != null)
                {
                    choiceButtons[i].onClick.RemoveListener(
                        choiceListeners[i]);
                }
            }
        }
    }
}
