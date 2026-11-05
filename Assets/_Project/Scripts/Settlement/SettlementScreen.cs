using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CafeTime.Settlement
{
    // 하루 정산 화면 (SFR-071): 영수증 표시, 임대료 차감, 평판 반영(SFR-072), 자동 저장, 다음 DAY로 이동
    public class SettlementScreen : MonoBehaviour
    {
        // 고정 문구(영업 정산, 항목 이름, 경고, 엔딩 보기)는 씬의 이미지 라벨이고, 여기서는 바뀌는 값만 다룬다
        [Header("영수증")]
        [SerializeField] TMP_Text dayText; // "DAY 3"
        [SerializeField] TMP_Text visitorsText;
        [SerializeField] TMP_Text drinksText;
        [SerializeField] TMP_Text revenueText;
        [SerializeField] TMP_Text tipsText;
        [SerializeField] TMP_Text rentText;
        [SerializeField] TMP_Text moneyText;
        [SerializeField] Image[] stars;
        [SerializeField] Sprite starFull;
        [SerializeField] Sprite starEmpty;
        [SerializeField] TMP_Text reputationText;
        [SerializeField] GameObject rentWarningLabel; // "임대료를 낼 돈이 부족합니다..."

        [Header("버튼")]
        [SerializeField] Button nextButton;
        [SerializeField] TMP_Text nextButtonText;     // "DAY 4 시작"
        [SerializeField] GameObject endingLabel;      // "엔딩 보기"

        enum Next { NextDay, Ending }
        Next next;

        void Start()
        {
            SaveSystem.EnsureCurrentForPreview();
            var balance = GameBalance.Instance;
            if (balance == null) return;

            var save = SaveSystem.Current;
            var result = DayResult.Today ?? new DayResult();
            int day = save.day;
            int rent = balance.GetRent(day);

            dayText.text = $"DAY {day}";
            visitorsText.text = $"{result.visitors:N0}명";
            drinksText.text = $"{result.drinksSold:N0}잔";
            revenueText.text = $"₩{result.revenue:N0}";
            tipsText.text = $"₩{result.tips:N0}";
            rentText.text = $"-₩{rent:N0}";

            // 손님 만족도에 따라 평판 변동 (SFR-072)
            save.reputation = Mathf.Clamp(save.reputation + result.reputationChange, 1f, 5f);
            ShowReputation(save.reputation, result.reputationChange);

            if (save.money < rent)
            {
                // 임대료를 낼 보유금이 부족하면 엔딩 01(닫힌 카페) (SFR-071, 073). 저장하지 않는다
                moneyText.text = $"₩{save.money:N0}";
                rentWarningLabel.SetActive(true);
                next = Next.Ending;
                Endings.Pending = EndingType.ClosedCafe;
            }
            else
            {
                save.money -= rent; // 당일 임대료 자동 차감
                save.day = day + 1;
                save.bestScore = Mathf.Max(save.bestScore, result.score); // 하루 최고 점수 (SFR-067)
                SaveSystem.SaveCurrent(); // 하루 정산 후 자동 저장 (ADR-0005)
                moneyText.text = $"₩{save.money:N0}";
                rentWarningLabel.SetActive(false);
                next = day >= GameBalance.TotalDays ? Next.Ending : Next.NextDay; // DAY 15 정산 후 엔딩
                if (next == Next.Ending) Endings.Pending = Endings.Decide(save, balance);
                nextButtonText.text = $"DAY {save.day} 시작";
            }
            endingLabel.SetActive(next == Next.Ending);
            nextButtonText.gameObject.SetActive(next == Next.NextDay);

            nextButton.onClick.AddListener(GoNext);
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame))
                GoNext();
        }

        void ShowReputation(float reputation, float change)
        {
            int filled = Mathf.RoundToInt(reputation);
            for (int i = 0; i < stars.Length; i++)
                stars[i].sprite = i < filled ? starFull : starEmpty;
            string sign = change > 0 ? "+" : "";
            reputationText.text = $"{reputation:0.0} ({sign}{change:0.0})";
        }

        void GoNext()
        {
            enabled = false; // 중복 입력 방지
            nextButton.interactable = false;
            if (next == Next.Ending)
                SceneLoader.LoadFirstAvailable(SceneNames.Ending, SceneNames.Title);
            else
                // 스토리가 없는 DAY는 Story 씬이 바로 오픈 준비로 넘긴다 (ADR-0011)
                SceneLoader.LoadFirstAvailable(SceneNames.Story, SceneNames.OpenPrep);
        }
    }
}
