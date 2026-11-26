using System.Collections.Generic;
using CafeTime.Score;
using TMPro;
using UnityEditor;
using UnityEngine;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // CafeTime > Build Score Scene : 득점 결과 씬(SFR-067) 생성 (ADR-0006)
    // 좌표는 Panel.png 표시 크기(1100x920) 기준, 왼쪽 위 기준. 배치는 Art/UI/Score/Generation.md 2026-11-18 배치표 (줄 사이 짧은 점선은 생략)
    public static class ScoreSceneBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Score.unity";
        const string Art = "Assets/_Project/Art/UI/Score";
        static readonly Color Evening = new Color32(0x2A, 0x1C, 0x14, 0xFF);
        // 칠판 화면 색 (Art/UI/Score/Generation.md 2026-11-18 칠판 리디자인)
        static readonly Color Board = new Color32(0x1F, 0x36, 0x2B, 0xFF);
        static readonly Color Chalk = new Color32(0xFC, 0xF0, 0xCA, 0xFF);
        static readonly Color ChalkYellow = new Color32(0xF1, 0xD8, 0x89, 0xFF);
        static readonly Color ChalkPink = new Color32(0xED, 0xA6, 0xA1, 0xFF);

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
            AddImage(panel, S("Panel"), Board); // 제목 "득점 결과"는 칠판 그림에 포함

            Set(so, "dayText", AddText(TopLeft("Day", panel, 800, 76, 190, 44), "DAY 1", 32, Chalk, TextAlignmentOptions.Right));

            // 오늘의 기록
            Label(panel, "LabelRecord", S("LabelRecord"), 110, 170, PivotLeft);
            var rows = new List<Object>();
            rows.Add(Row(panel, "Orders", 225, out var ordersCount, out var ordersPoints, ChalkYellow));
            Set(so, "ordersCount", ordersCount);
            Set(so, "ordersPoints", ordersPoints);
            rows.Add(Row(panel, "Satisfaction", 280, out var satisfactionCount, out var satisfactionPoints, ChalkYellow));
            Object.DestroyImmediate(satisfactionCount.gameObject); // 만족도는 횟수 없음
            Set(so, "satisfactionPoints", satisfactionPoints);
            AddImage(TopLeft("Divider 1", panel, 100, 320, 900, 12), S("Divider"), Chalk);

            // 감점 사항: 손님 이탈, 버린 재료·음료, 쟁반 방치, 청소 미처리, 주문 순서 어김, 마감 초과 (ADR-0055: 6줄이 구분선 625 위에 들어가게 간격 38)
            Label(panel, "LabelPenalty", S("LabelPenalty"), 110, 365, PivotLeft);
            string[] penalties = { "Lost", "Discarded", "TrayLeft", "Cleaning", "OrderSkipped", "Overtime" };
            var counts = new Object[penalties.Length];
            var points = new Object[penalties.Length];
            for (int i = 0; i < penalties.Length; i++)
            {
                rows.Add(Row(panel, penalties[i], 410 + i * 38, out var count, out var point, ChalkPink));
                counts[i] = count;
                points[i] = point;
            }
            SetArray(so, "penaltyCounts", counts);
            SetArray(so, "penaltyPoints", points);
            SetArray(so, "rows", rows.ToArray());
            AddImage(TopLeft("Divider 2", panel, 100, 625, 900, 12), S("Divider"), Chalk);

            // 총점(분필 동그라미 안), 이전 최고 기록, 최고 기록 달성 도장
            var totalGroup = Group(panel, "Total");
            var total = totalGroup.transform;
            Set(so, "totalGroup", totalGroup);
            Label(total, "LabelTotal", S("LabelTotal"), 110, 690, PivotLeft);
            AddImage(TopLeft("TotalCircle", total, 615, 640, 380, 104), S("TotalCircle"), Color.clear);
            Set(so, "totalText", AddText(TopLeft("Total", total, 640, 650, 330, 84), "0", 76, Chalk, TextAlignmentOptions.Center));
            Label(total, "LabelBest", S("LabelBest"), 110, 758, PivotLeft);
            Set(so, "bestText", AddText(TopLeft("Best", total, 280, 735, 220, 46), "0", 30, Chalk, TextAlignmentOptions.Left));
            Set(so, "newBestLabel", Label(total, "NewBest", S("NewBest"), 585 + 125, 735 + 24, PivotCenter).gameObject); // 가운데 기준으로 도장 연출
            Set(so, "newBestDelta", AddText(TopLeft("NewBestDelta", total, 845, 736, 145, 46), "+0", 30, ChalkYellow, TextAlignmentOptions.Right));

            Set(so, "nextButton", AddButton(TopLeft("NextButton", panel, 340, 800, 420, 96), S("NextButton"), Cream));
            so.ApplyModifiedPropertiesWithoutUndo();

            SaveSceneAndAddToBuild(ScenePath);
            Debug.Log("Score 씬 생성 완료: " + ScenePath);
        }

        // 연출용 묶음: 패널과 같은 크기라 안쪽 좌표는 패널 기준 그대로. 줄마다 하나씩 나타난다
        static CanvasGroup Group(Transform panel, string name)
        {
            var group = TopLeft(name, panel, 0, 0, 1100, 920).gameObject.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;
            return group;
        }

        // [아이콘][항목 이름]   n회   ±점수
        static CanvasGroup Row(Transform panel, string name, float y, out TextMeshProUGUI count, out TextMeshProUGUI points, Color pointColor)
        {
            var row = Group(panel, $"Row {name}");
            AddImage(TopLeft("Icon", row.transform, 103, y - 19, 38, 38), S($"Icon{name}"), Color.clear);
            Label(row.transform, "Label", S($"Label{name}"), 155, y, PivotLeft);
            count = AddText(TopLeft("Count", row.transform, 520, y - 23, 180, 46), "0", 28, Chalk, TextAlignmentOptions.Right);
            points = AddText(TopLeft("Points", row.transform, 760, y - 23, 230, 46), "0", 30, pointColor, TextAlignmentOptions.Right);
            return row;
        }
    }
}
