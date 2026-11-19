using System.Collections;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.Score
{
    // 득점 결과 화면 (SFR-067, 068): 오늘의 기록·점수, 총점, 이전 최고 기록 비교 → 다음 날 준비(하루 정산)
    // 랭킹 보기 버튼은 랭킹 서버(SFR-069, 070)와 함께 추가한다
    // 연출 (2026-11-18 사용자 선택): 줄이 하나씩 나타나고 총점이 0부터 올라간 뒤, 최고 기록이면 도장이 찍힌다.
    // 클릭·Enter 로 연출을 건너뛰고, 끝난 뒤 Enter·버튼으로 다음 화면. 감점 0회 줄은 흐리게
    public class ScoreScreen : MonoBehaviour
    {
        [SerializeField] TMP_Text dayText;
        [Header("완료한 주문 / 손님 만족도")]
        [SerializeField] TMP_Text ordersCount;
        [SerializeField] TMP_Text ordersPoints;
        [SerializeField] TMP_Text satisfactionPoints;
        [Header("감점 사항 (횟수, 감점): 손님 이탈, 버린 재료·음료, 쟁반 방치, 청소 미처리, 주문 순서 어김")]
        [SerializeField] TMP_Text[] penaltyCounts;
        [SerializeField] TMP_Text[] penaltyPoints;
        [Header("총점")]
        [SerializeField] TMP_Text totalText;
        [SerializeField] TMP_Text bestText;
        [SerializeField] GameObject newBestLabel; // "최고 기록 달성!"
        [SerializeField] TMP_Text newBestDelta;   // "+120"
        [SerializeField] Button nextButton;
        [Header("연출")]
        [Tooltip("위에서부터 나타날 줄: 완료한 주문, 손님 만족도, 감점 5줄 순서")]
        [SerializeField] CanvasGroup[] rows;
        [SerializeField] CanvasGroup totalGroup; // 총점·최고 기록 묶음
        [SerializeField] float rowInterval = 0.18f;
        [SerializeField] float fadeSeconds = 0.15f;
        [SerializeField] float countSeconds = 1f;
        [SerializeField] float stampSeconds = 0.2f;
        [SerializeField, Range(0, 1)] float zeroRowAlpha = 0.35f;

        float[] rowAlpha;
        int total;
        bool newBest;
        bool skip;
        bool done;

        void Start()
        {
            SaveSystem.EnsureCurrentForPreview();
            var result = DayResult.Today ?? new DayResult();
            var score = new DayScore(result, GameBalance.Instance);
            result.score = score.Total;

            dayText.text = $"DAY {SaveSystem.Current.day}";
            ordersCount.text = $"{result.ordersCompleted}건";
            ordersPoints.text = $"+{score.OrderPoints:N0}";
            satisfactionPoints.text = $"+{score.SatisfactionPoints:N0}";

            // 첫 두 줄(가산)은 항상 진하게, 감점 줄은 0회면 흐리게
            // 연출 연결이 없는 예전 Score 씬이어도 동작하게 (Build Score Scene 전): 없는 묶음은 건너뛴다
            if (rows == null) rows = new CanvasGroup[0];
            rowAlpha = new float[rows.Length];
            for (int i = 0; i < rowAlpha.Length; i++) rowAlpha[i] = 1f;

            int[] counts = { result.customersLost, result.itemsDiscarded, result.traysLeft, result.cleaningMissed, result.ordersSkipped };
            int[] points = { score.LostPenalty, score.DiscardPenalty, score.TrayLeftPenalty, score.CleaningPenalty, score.SkippedPenalty };
            for (int i = 0; i < counts.Length && i < penaltyCounts.Length; i++)
            {
                penaltyCounts[i].text = $"{counts[i]}회";
                penaltyPoints[i].text = points[i] > 0 ? $"-{points[i]:N0}" : "0";
                if (counts[i] == 0 && 2 + i < rowAlpha.Length) rowAlpha[2 + i] = zeroRowAlpha;
            }

            total = score.Total;
            int best = SaveSystem.Current.bestScore; // 갱신·저장은 하루 정산에서 (SFR-071)
            bestText.text = $"{best:N0}";
            newBest = score.Total > best;
            newBestDelta.text = $"+{score.Total - best:N0}";

            nextButton.onClick.AddListener(Next);
            StartCoroutine(Reveal());
        }

        IEnumerator Reveal()
        {
            foreach (var row in rows) SetAlpha(row, 0f);
            SetAlpha(totalGroup, 0f);
            totalText.text = "0";
            newBestLabel.SetActive(false);
            newBestDelta.gameObject.SetActive(false);
            nextButton.interactable = false;

            for (int i = 0; i < rows.Length && !skip; i++)
            {
                StartCoroutine(Fade(rows[i], rowAlpha[i]));
                yield return Wait(rowInterval);
            }
            StartCoroutine(Fade(totalGroup, 1f));
            for (float t = 0; t < countSeconds && !skip; t += Time.unscaledDeltaTime)
            {
                totalText.text = $"{Mathf.RoundToInt(total * Mathf.SmoothStep(0, 1, t / countSeconds)):N0}";
                yield return null;
            }
            Finish();
            if (newBest && !skip) yield return Stamp(newBestLabel.transform);
        }

        // 모든 줄·총점을 최종 상태로
        void Finish()
        {
            for (int i = 0; i < rows.Length; i++) SetAlpha(rows[i], rowAlpha[i]);
            SetAlpha(totalGroup, 1f);
            totalText.text = $"{total:N0}";
            newBestLabel.SetActive(newBest);
            newBestDelta.gameObject.SetActive(newBest);
            nextButton.interactable = true;
            done = true;
        }

        static void SetAlpha(CanvasGroup group, float alpha)
        {
            if (group != null) group.alpha = alpha;
        }

        IEnumerator Fade(CanvasGroup group, float target)
        {
            if (group == null) yield break;
            for (float t = 0; t < fadeSeconds && !skip; t += Time.unscaledDeltaTime)
            {
                group.alpha = target * t / fadeSeconds;
                yield return null;
            }
            group.alpha = target;
        }

        // 도장 찍기: 크게 나타나 제자리로 줄어든다
        IEnumerator Stamp(Transform stamp)
        {
            for (float t = 0; t < stampSeconds && !skip; t += Time.unscaledDeltaTime)
            {
                stamp.localScale = Vector3.one * Mathf.Lerp(1.6f, 1f, t / stampSeconds);
                yield return null;
            }
            stamp.localScale = Vector3.one;
        }

        IEnumerator Wait(float seconds)
        {
            for (float t = 0; t < seconds && !skip; t += Time.unscaledDeltaTime) yield return null;
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            bool enter = keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame);
            bool click = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            if (!done)
            {
                if (enter || click) { skip = true; Finish(); }
            }
            else if (enter) Next();
        }

        void Next()
        {
            enabled = false;
            nextButton.interactable = false;
            SceneLoader.Load(SceneNames.Settlement);
        }
    }
}
