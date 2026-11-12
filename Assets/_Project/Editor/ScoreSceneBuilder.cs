using CafeTime.Score;
using TMPro;
using UnityEditor;
using UnityEngine;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // CafeTime > Build Score Scene : 득점 결과 씬(SFR-067) 생성 (ADR-0006)
    // 좌표는 Panel.png 표시 크기(1100x920) 기준, 왼쪽 위 기준
    public static class ScoreSceneBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Score.unity";
        const string Art = "Assets/_Project/Art/UI/Score";
        static readonly Color Evening = new Color32(0x2A, 0x1C, 0x14, 0xFF);
        static readonly Color Minus = new Color32(0xB0, 0x3A, 0x2E, 0xFF);

        static Sprite S(string name) => LoadSprite($"{Art}/{name}.png");

        [MenuItem("CafeTime/Build Score Scene")]
        public static void Build()
        {
            if (GameFont == null) { Debug.LogError("폰트 에셋이 없습니다. CafeTime > Setup Fonts 를 먼저 실행하세요."); return; }
            if (!ConfirmOverwrite(ScenePath, "Score 씬")) return;

            GameBalanceAssetBuilder.CreateOrUpdate();
            PrepareSprites(Art);

            var canvas = NewSceneWithCanvas(Evening);
            AddBackground(canvas, S("Background"), Evening, 0f);
            var screen = canvas.gameObject.AddComponent<ScoreScreen>();
            var so = new SerializedObject(screen);

            var panel = Centered("Panel", canvas, 0, 0, 1100, 920);
            AddImage(panel, S("Panel"), Cream);

            // 오늘의 기록: DAY n
            Label(panel, "LabelRecord", S("LabelRecord"), 110, 165, PivotLeft);
            Set(so, "dayText", AddText(TopLeft("Day", panel, 760, 140, 230, 50), "DAY 1", 32, TextBrown, TextAlignmentOptions.Right));

            Row(panel, "LabelOrders", 225, out var ordersCount, out var ordersPoints, DarkCoffee);
            Set(so, "ordersCount", ordersCount);
            Set(so, "ordersPoints", ordersPoints);
            Row(panel, "LabelSatisfaction", 280, out var satisfactionCount, out var satisfactionPoints, DarkCoffee);
            Object.DestroyImmediate(satisfactionCount.gameObject); // 만족도는 횟수 없음
            Set(so, "satisfactionPoints", satisfactionPoints);
            AddImage(TopLeft("Divider 1", panel, 100, 320, 900, 12), S("Divider"), TextBrown);

            // 감점 사항: 손님 이탈, 버린 재료·음료, 쟁반 방치, 청소 미처리, 주문 순서 어김 (5줄이 구분선 625 위에 들어가게 간격 42)
            Label(panel, "LabelPenalty", S("LabelPenalty"), 110, 370, PivotLeft);
            string[] penalties = { "LabelLost", "LabelDiscarded", "LabelTrayLeft", "LabelCleaning", "LabelOrderSkipped" };
            var counts = new Object[penalties.Length];
            var points = new Object[penalties.Length];
            for (int i = 0; i < penalties.Length; i++)
            {
                Row(panel, penalties[i], 420 + i * 42, out var count, out var point, Minus);
                counts[i] = count;
                points[i] = point;
            }
            SetArray(so, "penaltyCounts", counts);
            SetArray(so, "penaltyPoints", points);
            AddImage(TopLeft("Divider 2", panel, 100, 625, 900, 12), S("Divider"), TextBrown);

            // 총점, 이전 최고 기록
            Label(panel, "LabelTotal", S("LabelTotal"), 130, 680, PivotLeft);
            Set(so, "totalText", AddText(TopLeft("Total", panel, 600, 650, 400, 64), "0", 48, DarkCoffee, TextAlignmentOptions.Right));
            Label(panel, "LabelBest", S("LabelBest"), 130, 745, PivotLeft);
            Set(so, "bestText", AddText(TopLeft("Best", panel, 300, 722, 220, 46), "0", 30, TextBrown, TextAlignmentOptions.Left));
            Set(so, "newBestLabel", Label(panel, "NewBest", S("NewBest"), 640, 745, PivotLeft).gameObject);
            Set(so, "newBestDelta", AddText(TopLeft("NewBestDelta", panel, 820, 722, 180, 46), "+0", 30,
                new Color32(0xD9, 0xA0, 0x3A, 0xFF), TextAlignmentOptions.Right));

            Set(so, "nextButton", AddButton(TopLeft("NextButton", panel, 340, 800, 420, 96), S("NextButton"), Cream));
            so.ApplyModifiedPropertiesWithoutUndo();

            SaveSceneAndAddToBuild(ScenePath);
            Debug.Log("Score 씬 생성 완료: " + ScenePath);
        }

        // [항목 이름]   n회   +점수
        static void Row(Transform panel, string label, float y, out TextMeshProUGUI count, out TextMeshProUGUI points, Color pointColor)
        {
            Label(panel, label, S(label), 140, y, PivotLeft);
            count = AddText(TopLeft($"{label} Count", panel, 520, y - 23, 180, 46), "0", 28, TextBrown, TextAlignmentOptions.Right);
            points = AddText(TopLeft($"{label} Points", panel, 760, y - 23, 240, 46), "0", 28, pointColor, TextAlignmentOptions.Right);
        }
    }
}
