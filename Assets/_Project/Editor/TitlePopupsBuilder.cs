using CafeTime.Title;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // 시작 화면의 닉네임 입력(SFR-003) · 게임 설명(SFR-005) · 랭킹(SFR-070) 팝업을 디자인 에셋으로 구성 (ADR-0006)
    // CafeTime > Build Title Popups : 이미 있는 프리팹의 내용만 다시 만든다 (Title 씬은 그대로)
    public static class TitlePopupsBuilder
    {
        const string ArtRoot = "Assets/_Project/Art/UI";
        const string PrefabDir = "Assets/_Project/Prefabs/Title";

        [MenuItem("CafeTime/Build Title Popups")]
        public static void RebuildAll()
        {
            Rebuild("NicknamePopup", PopulateNickname);
            Rebuild("HowToPopup", PopulateHowTo);
            Rebuild("RankingPopup", PopulateRanking);
            Debug.Log("닉네임 · 게임 설명 · 랭킹 팝업 프리팹을 다시 만들었습니다.");
        }

        public static GameObject CreateNickname(Transform canvas) => Create<NicknamePopup>(canvas, "NicknamePopup", PopulateNickname);
        public static GameObject CreateHowTo(Transform canvas) => Create<Popup>(canvas, "HowToPopup", PopulateHowTo);
        public static GameObject CreateRanking(Transform canvas) => Create<Popup>(canvas, "RankingPopup", PopulateRanking);

        static void Rebuild(string name, System.Action<GameObject> populate)
        {
            string path = $"{PrefabDir}/{name}.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null)
            {
                Debug.LogWarning($"프리팹이 없습니다 (CafeTime > Build Title Scene 으로 생성): {path}");
                return;
            }
            var root = PrefabUtility.LoadPrefabContents(path);
            try
            {
                while (root.transform.childCount > 0)
                    Object.DestroyImmediate(root.transform.GetChild(0).gameObject);
                populate(root);
                PrefabUtility.SaveAsPrefabAsset(root, path);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        static GameObject Create<T>(Transform canvas, string name, System.Action<GameObject> populate) where T : Popup
        {
            var root = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(T));
            root.transform.SetParent(canvas, false);
            Stretch((RectTransform)root.transform);
            populate(root);
            return root;
        }

        // 반투명 배경(뒤쪽 메뉴 클릭 차단) + 가운데 패널 이미지
        static RectTransform Window(GameObject root, string folder, float width, float height)
        {
            PrepareSprites($"{ArtRoot}/{folder}");
            var dim = root.GetComponent<Image>();
            dim.sprite = null;
            dim.color = new Color(0, 0, 0, 0.5f);
            var window = Centered("Window", root.transform, 0, 0, width, height);
            AddImage(window, LoadSprite($"{ArtRoot}/{folder}/Panel.png"), Cream, true);
            return window;
        }

        // 닉네임 입력 820x580. Panel.png 제목 영역은 y 164까지, 안쪽 종이는 x 31~788, y ~540
        static void PopulateNickname(GameObject root)
        {
            const string Art = ArtRoot + "/Nickname";
            var window = Window(root, "Nickname", 820, 580);
            var so = new SerializedObject(root.GetComponent<NicknamePopup>());

            Set(so, "overwriteWarning", Label(window, "OverwriteWarning", LoadSprite($"{Art}/Warning.png"), 410, 205, PivotCenter).gameObject);
            Label(window, "Info", LoadSprite($"{Art}/Info.png"), 410, 255, PivotCenter);
            Set(so, "nicknameInput", InputField(window, LoadSprite($"{Art}/InputField.png"), LoadSprite($"{Art}/Placeholder.png")));
            // 버튼 580x156 그림을 절반 크기로 (입력 칸 아래 ~ 패널 안쪽 바닥 520 사이)
            Set(so, "confirmButton", AddButton(TopLeft("StartButton", window, 110, 432, 290, 78), LoadSprite($"{Art}/StartButton.png"), Cream));
            Set(so, "cancelButton", AddButton(TopLeft("CancelButton", window, 420, 432, 290, 78), LoadSprite($"{Art}/CancelButton.png"), Cream));
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // 입력 칸 600x88. 입력한 닉네임은 바뀌는 값이라 폰트, 안내 문구(placeholder)는 글자 이미지
        static TMP_InputField InputField(RectTransform window, Sprite background, Sprite placeholder)
        {
            var go = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());
            go.name = "NicknameInput";
            var rt = (RectTransform)go.transform;
            rt.SetParent(window, false);
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(110, -295);
            rt.sizeDelta = new Vector2(600, 88);
            var image = go.GetComponent<Image>();
            image.sprite = background;
            image.type = Image.Type.Simple;
            image.color = Color.white;

            var input = go.GetComponent<TMP_InputField>();
            input.fontAsset = GameFont;
            input.pointSize = 38;
            input.textComponent.color = DarkCoffee;
            input.textComponent.alignment = TextAlignmentOptions.Center;

            // 기본 TMP placeholder 를 글자 이미지로 교체 (TMP_InputField.placeholder 는 Graphic 이면 됨)
            var old = input.placeholder.gameObject;
            var area = old.transform.parent;
            Object.DestroyImmediate(old);
            var ph = CenterLabel(area, "Placeholder", placeholder);
            input.placeholder = ph;
            return input;
        }

        // 게임 설명 1400x900. Panel.png 제목 장식은 y 262까지, 안쪽 종이는 x 45~1354, y ~820
        static void PopulateHowTo(GameObject root)
        {
            const string Art = ArtRoot + "/HowTo";
            var window = Window(root, "HowTo", 1400, 900);
            var so = new SerializedObject(root.GetComponent<Popup>());

            // 소제목·흐름도는 이미지, 긴 본문은 문구 수정이 잦아 폰트로 둔다
            Label(window, "HeadingStory", LoadSprite($"{Art}/HeadingStory.png"), 100, 292, PivotLeft);
            Body(window, "StoryText", 320, 150,
                "퇴직금으로 작은 베이커리 카페를 연 주인공.\n" +
                "하지만 해외 전쟁으로 주식이 폭락하면서 자금이 바닥나 버렸습니다.\n" +
                "주식을 팔지 않고 버티려면, 카페 매출만으로 매일 임대료를 내야 합니다.");
            Label(window, "HeadingRules", LoadSprite($"{Art}/HeadingRules.png"), 100, 500, PivotLeft);
            Body(window, "RulesText", 528, 90,
                "15일 동안 매일 임대료를 내며 카페를 운영하세요.\n" +
                "임대료를 내지 못하면 카페는 문을 닫게 됩니다.");
            Label(window, "HeadingFlow", LoadSprite($"{Art}/HeadingFlow.png"), 100, 640, PivotLeft);
            AddImage(TopLeft("FlowDiagram", window, 110, 670, 1180, 110), LoadSprite($"{Art}/FlowDiagram.png"), Cream).preserveAspect = true;

            Set(so, "closeButton", AddButton(TopLeft("CloseButton", window, 1262, 48, 70, 70), LoadSprite($"{ArtRoot}/Settings/Close.png"), Cream));
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void Body(RectTransform window, string name, float y, float height, string text)
        {
            var t = AddText(TopLeft(name, window, 130, y, 1160, height), text, 28, TextBrown, TextAlignmentOptions.TopLeft);
            t.lineSpacing = 20;
        }

        // 랭킹 900x700 (서버 구현 전: 연결 실패 안내). Panel.png 제목 장식은 y 207까지, 안쪽 종이 y ~660
        static void PopulateRanking(GameObject root)
        {
            const string Art = ArtRoot + "/Ranking";
            var window = Window(root, "Ranking", 900, 700);
            var so = new SerializedObject(root.GetComponent<Popup>());
            Label(window, "ServerError", LoadSprite($"{Art}/ServerError.png"), 450, 390, PivotCenter);
            Set(so, "closeButton", AddButton(TopLeft("CloseButton", window, 305, 540, 290, 62), LoadSprite($"{Art}/CloseButton.png"), Cream));
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
