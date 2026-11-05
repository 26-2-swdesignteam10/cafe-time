using CafeTime.Settlement;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // CafeTime > Build Settlement Scene : 하루 정산 씬(SFR-071) 생성 (ADR-0006, 최초 생성용)
    public static class SettlementSceneBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Settlement.unity";
        const string Art = "Assets/_Project/Art/UI/Settlement";
        static readonly Color Night = new Color32(0x14, 0x1C, 0x33, 0xFF);
        static readonly Color Amber = new Color32(0xDF, 0x70, 0x15, 0xFF);
        static readonly Color Gold = new Color32(0xC5, 0x7D, 0x1F, 0xFF);

        [MenuItem("CafeTime/Build Settlement Scene")]
        public static void Build()
        {
            if (GameFont == null) { Debug.LogError("폰트 에셋이 없습니다. CafeTime > Setup Fonts 를 먼저 실행하세요."); return; }
            if (!ConfirmOverwrite(ScenePath, "Settlement 씬")) return;

            GameBalanceAssetBuilder.CreateOrUpdate();
            PrepareSprites(Art);

            var canvas = NewSceneWithCanvas(Night);
            AddBackground(canvas, LoadSprite($"{Art}/NightBackground.png"), Night, 0f); // 야간 배경
            var screen = canvas.gameObject.AddComponent<SettlementScreen>();
            var so = new SerializedObject(screen);

            // 영수증 560x840 (SFR-071). 좌표는 Receipt.png 기준: 종이 x 61~499, 로고 아래 끝 y 184, 점선 y 576
            var receipt = Centered("Receipt", canvas, 0, 70, 560, 840);
            AddImage(receipt, LoadSprite($"{Art}/Receipt.png"), Cream);
            // 제목: "DAY n" + [영업 정산]  ([ ] = 글자 이미지)
            Set(so, "dayText", AddText(TopLeft("Day", receipt, 60, 195, 170, 56), "DAY 1", 34, DarkCoffee, TextAlignmentOptions.Right));
            Label(receipt, "SettlementLabel", LoadSprite($"{Art}/LabelSettlement.png"), 245, 223, PivotLeft);

            string[] labels = { "LabelVisitors", "LabelDrinks", "LabelRevenue", "LabelTips", "LabelRent" };
            string[] fields = { "visitorsText", "drinksText", "revenueText", "tipsText", "rentText" };
            for (int i = 0; i < labels.Length; i++)
            {
                var value = Row(receipt, labels[i], 260 + i * 50, 26);
                if (fields[i] == "rentText") value.color = Warning;
                Set(so, fields[i], value);
            }
            Set(so, "moneyText", Row(receipt, "LabelMoney", 590, 30));

            // 평판 별점 (SFR-072)
            Label(receipt, "LabelReputation", LoadSprite($"{Art}/LabelReputation.png"), 85, 673, PivotLeft);
            var starFull = LoadSprite($"{Art}/Star.png");
            var starEmpty = LoadSprite($"{Art}/StarEmpty.png");
            var stars = new Object[5];
            for (int i = 0; i < stars.Length; i++)
                stars[i] = AddImage(TopLeft($"Star {i + 1}", receipt, 170 + i * 38, 655, 34, 34), starFull, Gold);
            SetArray(so, "stars", stars);
            Set(so, "starFull", starFull);
            Set(so, "starEmpty", starEmpty);
            Set(so, "reputationText", AddText(TopLeft("ReputationValue", receipt, 360, 650, 115, 46), "3.0 (+0.0)", 20, TextBrown, TextAlignmentOptions.Right));
            Set(so, "rentWarningLabel", Label(receipt, "RentWarning", LoadSprite($"{Art}/WarningRent.png"), 280, 740, PivotCenter).gameObject);

            // DAY (n) 시작 버튼 (클릭 또는 Enter)
            var next = Centered("NextButton", canvas, 0, -445, 520, 110);
            Set(so, "nextButton", AddButton(next, LoadSprite($"{Art}/NextDayButton.png"), Amber));
            Set(so, "nextButtonText", StretchText(next, "Label", "DAY 2 시작", 40, DarkCoffee));
            Set(so, "endingLabel", CenterLabel(next, "EndingLabel", LoadSprite($"{Art}/LabelEnding.png")).gameObject);

            so.ApplyModifiedPropertiesWithoutUndo();
            SaveSceneAndAddToBuild(ScenePath);
            Debug.Log("Settlement 씬 생성 완료: " + ScenePath);
        }

        // 왼쪽 항목 이름(글자 이미지) + 오른쪽 값(폰트). 값 텍스트를 돌려줌
        static TextMeshProUGUI Row(RectTransform receipt, string label, float y, float size)
        {
            Label(receipt, label, LoadSprite($"{Art}/{label}.png"), 85, y + 23, PivotLeft);
            return AddText(TopLeft(label + " Value", receipt, 255, y, 220, 46), "-", size, DarkCoffee, TextAlignmentOptions.Right);
        }
    }
}
