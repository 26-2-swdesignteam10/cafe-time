using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.Score
{
    // 득점 결과 화면 (SFR-067, 068): 오늘의 기록·점수, 총점, 이전 최고 기록 비교 → 다음 날 준비(하루 정산)
    // 랭킹 보기 버튼은 랭킹 서버(SFR-069, 070)와 함께 추가한다
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

            int[] counts = { result.customersLost, result.itemsDiscarded, result.traysLeft, result.cleaningMissed, result.ordersSkipped };
            int[] points = { score.LostPenalty, score.DiscardPenalty, score.TrayLeftPenalty, score.CleaningPenalty, score.SkippedPenalty };
            for (int i = 0; i < counts.Length && i < penaltyCounts.Length; i++)
            {
                penaltyCounts[i].text = $"{counts[i]}회";
                penaltyPoints[i].text = points[i] > 0 ? $"-{points[i]:N0}" : "0";
            }

            totalText.text = $"{score.Total:N0}";
            int best = SaveSystem.Current.bestScore; // 갱신·저장은 하루 정산에서 (SFR-071)
            bestText.text = $"{best:N0}";
            bool newBest = score.Total > best;
            newBestLabel.SetActive(newBest);
            newBestDelta.gameObject.SetActive(newBest);
            newBestDelta.text = $"+{score.Total - best:N0}";

            nextButton.onClick.AddListener(Next);
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame)) Next();
        }

        void Next()
        {
            enabled = false;
            nextButton.interactable = false;
            SceneLoader.Load(SceneNames.Settlement);
        }
    }
}
