using CafeTime.Ending;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // CafeTime > Build Ending Scene : 엔딩 씬(SFR-073) 생성 (ADR-0006)
    // 전체 화면 그림 + 아래쪽 글상자(제목, 멘트) + 메인 메뉴로 버튼
    public static class EndingSceneBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Ending.unity";
        const string Art = "Assets/_Project/Art/UI/Ending";

        static Sprite S(string name) => LoadSprite($"{Art}/{name}.png");

        [MenuItem("CafeTime/Build Ending Scene")]
        public static void Build()
        {
            if (GameFont == null) { Debug.LogError("폰트 에셋이 없습니다. CafeTime > Setup Fonts 를 먼저 실행하세요."); return; }
            if (!ConfirmOverwrite(ScenePath, "Ending 씬")) return;

            PrepareSprites(Art);
            var canvas = NewSceneWithCanvas(Color.black);
            var screen = canvas.gameObject.AddComponent<EndingScreen>();
            var so = new SerializedObject(screen);

            var background = NewUI("Background", canvas);
            Stretch(background);
            var image = background.gameObject.AddComponent<Image>();
            image.sprite = S("Ending01");
            image.raycastTarget = false;
            Set(so, "background", image);

            var box = Centered("TextBox", canvas, 0, -330, 1500, 300);
            AddImage(box, S("TextBox"), new Color32(0x3E, 0x27, 0x1A, 0xF0));
            Set(so, "title", Label(box, "Title", S("Title01"), 750, 100, PivotCenter));
            Set(so, "line", Label(box, "Line", S("Line01"), 750, 195, PivotCenter));

            var names = new[] { "01", "02", "03", "04" };
            SetArray(so, "backgrounds", System.Array.ConvertAll(names, n => (Object)S($"Ending{n}")));
            SetArray(so, "titles", System.Array.ConvertAll(names, n => (Object)S($"Title{n}")));
            SetArray(so, "lines", System.Array.ConvertAll(names, n => (Object)S($"Line{n}")));
            Set(so, "mainMenuButton", AddButton(TopLeft("MainMenuButton", canvas, 1440, 590, 420, 96), S("MainMenuButton"), Cream));
            so.ApplyModifiedPropertiesWithoutUndo();

            SaveSceneAndAddToBuild(ScenePath);
            Debug.Log("Ending 씬 생성 완료: " + ScenePath);
        }
    }
}
