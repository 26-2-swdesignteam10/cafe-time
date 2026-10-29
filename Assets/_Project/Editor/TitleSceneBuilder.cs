using System.IO;
using System.Linq;
using CafeTime.Title;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

namespace CafeTime.EditorTools
{
    // 시작 화면 초기 생성 도구 (처음 한 번만 사용).
    //   1) CafeTime > Setup Fonts       : TMP 기본 리소스 + 한글 폰트 에셋 생성
    //   2) CafeTime > Build Title Scene : 팝업 프리팹 + Title 씬 생성
    // 생성 후에는 씬/프리팹을 에디터에서 직접 수정한다. (다시 실행하면 수정 내용을 덮어씀)
    public static class TitleSceneBuilder
    {
        const string FontPath = "Assets/_Project/Art/Fonts/Galmuri11.ttf";
        const string FontAssetPath = "Assets/_Project/Art/Fonts/Galmuri11 SDF.asset";
        const string TmpSettingsPath = "Assets/TextMesh Pro/Resources/TMP Settings.asset";
        const string TmpEssentialsPackage = "Packages/com.unity.ugui/Package Resources/TMP Essential Resources.unitypackage";
        const string ScenePath = "Assets/_Project/Scenes/Title.unity";
        const string BackgroundPath = "Assets/_Project/Art/Title/CafeInterior.png";
        const string SignPath = "Assets/_Project/Art/Title/TitleSignText.png";
        const string StartButtonPath = "Assets/_Project/Art/Title/MenuStart.png";
        const string HowToButtonPath = "Assets/_Project/Art/Title/MenuHowTo.png";
        const string SettingsButtonPath = "Assets/_Project/Art/Title/MenuSettings.png";
        const string RankingButtonPath = "Assets/_Project/Art/Title/MenuRanking.png";
        const string QuitButtonPath = "Assets/_Project/Art/Title/MenuQuit.png";
        const string SharedPrefabDir = "Assets/_Project/Prefabs/UI";
        const string TitlePrefabDir = "Assets/_Project/Prefabs/Title";

        static readonly Color WarmBg = new Color32(0xE9, 0xC0, 0x8E, 0xFF);
        static readonly Color Paper = new Color32(0xF6, 0xE3, 0xC3, 0xFF);
        static readonly Color Cream = new Color32(0xFB, 0xEF, 0xD9, 0xFF);
        static readonly Color Wood = new Color32(0x8B, 0x45, 0x2A, 0xFF);
        static readonly Color DarkBrown = new Color32(0x3E, 0x27, 0x1A, 0xFF);
        static readonly Color Orange = new Color32(0xF2, 0xA9, 0x3B, 0xFF);
        static readonly Color Warning = new Color32(0xC0, 0x39, 0x2B, 0xFF);

        static TMP_FontAsset font;
        static Sprite rounded;

        [MenuItem("CafeTime/Setup Fonts")]
        public static void SetupFonts()
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(TmpSettingsPath) == null)
                AssetDatabase.ImportPackage(TmpEssentialsPackage, false);

            if (AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath) == null)
            {
                var source = AssetDatabase.LoadAssetAtPath<Font>(FontPath);
                var asset = TMP_FontAsset.CreateFontAsset(source, 48, 5, GlyphRenderMode.SDFAA, 2048, 2048,
                    AtlasPopulationMode.Dynamic, true);
                asset.name = "Galmuri11 SDF";
                AssetDatabase.CreateAsset(asset, FontAssetPath);
                asset.atlasTextures[0].name = "Galmuri11 SDF Atlas";
                asset.material.name = "Galmuri11 SDF Material";
                AssetDatabase.AddObjectToAsset(asset.atlasTextures[0], asset);
                AssetDatabase.AddObjectToAsset(asset.material, asset);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 새로 만드는 TMP 텍스트의 기본 폰트도 한글 폰트로
            var settings = AssetDatabase.LoadAssetAtPath<TMP_Settings>(TmpSettingsPath);
            if (settings != null)
            {
                var so = new SerializedObject(settings);
                so.FindProperty("m_defaultFontAsset").objectReferenceValue =
                    AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);
                so.ApplyModifiedPropertiesWithoutUndo();
                AssetDatabase.SaveAssets();
            }
        }

        [MenuItem("CafeTime/Build Title Scene")]
        public static void Build()
        {
            font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);
            if (font == null)
            {
                Debug.LogError("폰트 에셋이 없습니다. 먼저 CafeTime > Setup Fonts 를 실행하세요.");
                return;
            }
            if (File.Exists(ScenePath) && !EditorUtility.DisplayDialog("Title 씬 다시 생성",
                    "Title 씬과 시작 화면 팝업 프리팹을 새로 만듭니다.\n에디터에서 수정한 내용은 사라집니다.", "덮어쓰기", "취소"))
                return;

            rounded = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            foreach (var path in new[] { BackgroundPath, SignPath, StartButtonPath, HowToButtonPath,
                SettingsButtonPath, RankingButtonPath, QuitButtonPath })
            {
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null) continue;
                importer.textureType = TextureImporterType.Sprite;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }
            Directory.CreateDirectory(SharedPrefabDir);
            Directory.CreateDirectory(TitlePrefabDir);
            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = WarmBg;
            camGo.AddComponent<AudioListener>();
            camGo.transform.position = new Vector3(0, 0, -10);

            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            var canvas = canvasGo.transform;

            var menu = canvasGo.AddComponent<TitleMenu>();
            var refs = new SerializedObject(menu);

            BuildMainScreen(canvas, refs);

            // 팝업은 프리팹으로 저장 (팀원이 팝업별로 나눠 작업해도 씬 충돌이 적도록)
            var nickname = LoadOrBuildPrefab(canvas, TitlePrefabDir, "NicknamePopup", BuildNicknamePopup);
            var gameStart = LoadOrBuildPrefab(canvas, TitlePrefabDir, "GameStartPopup", BuildGameStartPopup);
            var howTo = LoadOrBuildPrefab(canvas, TitlePrefabDir, "HowToPopup", BuildHowToPopup);
            var ranking = LoadOrBuildPrefab(canvas, TitlePrefabDir, "RankingPopup", BuildRankingPopup);
            var settings = LoadOrBuildPrefab(canvas, SharedPrefabDir, "SettingsPopup", BuildSettingsPopup);
            var confirm = LoadOrBuildPrefab(canvas, SharedPrefabDir, "ConfirmPopup", BuildConfirmPopup);

            // 프리팹끼리의 참조는 씬에서 연결
            var startRefs = new SerializedObject(gameStart.GetComponent<GameStartPopup>());
            Set(startRefs, "nicknamePopup", nickname.GetComponent<NicknamePopup>());
            startRefs.ApplyModifiedPropertiesWithoutUndo();

            Set(refs, "gameStartPopup", gameStart.GetComponent<GameStartPopup>());
            Set(refs, "howToPopup", howTo.GetComponent<Popup>());
            Set(refs, "settingsPopup", settings.GetComponent<SettingsPopup>());
            Set(refs, "rankingPopup", ranking.GetComponent<Popup>());
            Set(refs, "confirmPopup", confirm.GetComponent<ConfirmPopup>());
            refs.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, ScenePath);

            // 빌드 씬 목록 맨 앞에 Title 등록 (삭제된 씬은 정리)
            var buildScenes = EditorBuildSettings.scenes
                .Where(s => s.path != ScenePath && File.Exists(s.path)).ToList();
            buildScenes.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = buildScenes.ToArray();

            AssetDatabase.SaveAssets();
            Debug.Log("Title 씬 생성 완료: " + ScenePath);
        }

        // ───────── 화면 구성 ─────────

        // 배경 + 타이틀 간판 + 메뉴 버튼 (SFR-001)
        static void BuildMainScreen(Transform canvas, SerializedObject refs)
        {
            var bg = NewUI("Background", canvas);
            Stretch(bg);
            var bgImage = bg.gameObject.AddComponent<Image>();
            bgImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundPath);
            bgImage.color = bgImage.sprite == null ? WarmBg : Color.white;
            bgImage.preserveAspect = true;
            bgImage.raycastTarget = false;

            var sign = NewUI("TitleSign", canvas);
            sign.anchorMin = sign.anchorMax = new Vector2(0.5f, 0.824f);
            sign.sizeDelta = new Vector2(1080, 380);
            var signImage = sign.gameObject.AddComponent<Image>();
            signImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SignPath);
            signImage.raycastTarget = false;

            var menuRoot = NewUI("MainMenu", canvas);
            menuRoot.anchorMin = menuRoot.anchorMax = new Vector2(0.5f, 350f / 1080f);
            menuRoot.sizeDelta = new Vector2(550, 620);
            var menuLayout = menuRoot.gameObject.AddComponent<VerticalLayoutGroup>();
            menuLayout.spacing = 20;
            menuLayout.childControlWidth = menuLayout.childControlHeight = true;
            menuLayout.childForceExpandWidth = true;
            menuLayout.childForceExpandHeight = false;
            Set(refs, "startButton", MakeTitleButton(menuRoot, "게임 시작", StartButtonPath));
            Set(refs, "howToButton", MakeTitleButton(menuRoot, "게임 설명", HowToButtonPath));
            Set(refs, "settingsButton", MakeTitleButton(menuRoot, "설정", SettingsButtonPath));
            Set(refs, "rankingButton", MakeTitleButton(menuRoot, "랭킹", RankingButtonPath));
            Set(refs, "quitButton", MakeTitleButton(menuRoot, "게임 종료", QuitButtonPath));
        }

        static Button MakeTitleButton(Transform parent, string label, string spritePath)
        {
            var rt = NewUI(label + " Button", parent);
            rt.sizeDelta = new Vector2(550, 108);
            rt.gameObject.AddComponent<LayoutElement>().preferredHeight = 108;
            var image = rt.gameObject.AddComponent<Image>();
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            image.color = Color.white;
            var button = rt.gameObject.AddComponent<Button>();
            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color32(0xFF, 0xF1, 0xD0, 0xFF);
            colors.pressedColor = new Color32(0xD8, 0xB2, 0x85, 0xFF);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;
            return button;
        }

        // 칠판 스타일 시작 방식 선택 창 (SFR-002)
        static GameObject BuildGameStartPopup(Transform canvas)
        {
            return GameStartPopupBuilder.Create(canvas);
        }

        // 닉네임 입력 (SFR-003)
        static GameObject BuildNicknamePopup(Transform canvas)
        {
            var (root, window) = MakePopup(canvas, "NicknamePopup", "닉네임 입력", new Vector2(820, 580), Paper, DarkBrown);
            var so = new SerializedObject(root.AddComponent<NicknamePopup>());
            var warning = MakeText(window, "주의! 기존에 플레이했던 데이터가 사라집니다", 30, Warning);
            warning.name = "OverwriteWarning";
            Set(so, "overwriteWarning", warning.gameObject);
            MakeText(window, "입력한 닉네임은 랭킹에 표시됩니다", 28, Wood);
            Set(so, "nicknameInput", MakeInputField(window, "닉네임을 입력하세요"));
            var row = MakeRow(window);
            Set(so, "confirmButton", MakeButton(row, "시작", Orange));
            Set(so, "cancelButton", MakeButton(row, "취소", Cream));
            so.ApplyModifiedPropertiesWithoutUndo();
            return root;
        }

        // 게임 설명 (SFR-005)
        static GameObject BuildHowToPopup(Transform canvas)
        {
            var (root, window) = MakePopup(canvas, "HowToPopup", "게임 설명", new Vector2(1400, 900), Paper, DarkBrown);
            var so = new SerializedObject(root.AddComponent<Popup>());
            var text = MakeText(window,
                "<b>스토리</b>\n" +
                "퇴직금으로 작은 베이커리 카페를 연 주인공.\n" +
                "하지만 해외 전쟁으로 주식이 폭락하면서 자금이 바닥나 버렸습니다.\n" +
                "주식을 팔지 않고 버티려면, 카페 매출만으로 매일 임대료를 내야 합니다.\n\n" +
                "<b>진행 규칙</b>\n" +
                "15일 동안 매일 임대료를 내며 카페를 운영하세요.\n" +
                "임대료를 내지 못하면 카페는 문을 닫게 됩니다.\n\n" +
                "<b>하루의 흐름</b>\n" +
                "오픈 준비  →  주문 받기  →  음료 제조 및 매장 운영  →  하루 정산",
                32, DarkBrown, TextAlignmentOptions.TopLeft);
            text.gameObject.AddComponent<LayoutElement>().flexibleHeight = 1;
            Set(so, "closeButton", MakeCloseButton(window));
            so.ApplyModifiedPropertiesWithoutUndo();
            return root;
        }

        // 랭킹 (서버 구현 전: SFR-070 연결 실패 안내)
        static GameObject BuildRankingPopup(Transform canvas)
        {
            var (root, window) = MakePopup(canvas, "RankingPopup", "랭킹", new Vector2(900, 700), Paper, DarkBrown);
            var so = new SerializedObject(root.AddComponent<Popup>());
            MakeText(window, "랭킹 서버에 연결할 수 없습니다.", 34, Wood)
                .gameObject.AddComponent<LayoutElement>().flexibleHeight = 1;
            Set(so, "closeButton", MakeButton(window, "닫기", Cream));
            so.ApplyModifiedPropertiesWithoutUndo();
            return root;
        }

        // 설정: 왼쪽 탭 + 오른쪽 내용 (SFR-004)
        static GameObject BuildSettingsPopup(Transform canvas)
        {
            return SettingsPopupBuilder.Create(canvas);
        }

        // 예/아니오 확인 팝업 (SFR-006, SFR-023)
        static GameObject BuildConfirmPopup(Transform canvas)
        {
            return ConfirmPopupBuilder.Create(canvas);
        }

        // ───────── helpers ─────────

        // 비활성 상태로 프리팹 저장 후 씬에는 프리팹 인스턴스로 남김
        static GameObject SavePrefab(GameObject go, string dir)
        {
            go.SetActive(false);
            return PrefabUtility.SaveAsPrefabAssetAndConnect(go, $"{dir}/{go.name}.prefab", InteractionMode.AutomatedAction);
        }

        static GameObject LoadOrBuildPrefab(Transform canvas, string dir, string name,
            System.Func<Transform, GameObject> build)
        {
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>($"{dir}/{name}.prefab");
            if (existing != null)
                return (GameObject)PrefabUtility.InstantiatePrefab(existing, canvas);
            return SavePrefab(build(canvas), dir);
        }

        static void Set(SerializedObject so, string field, Object value)
        {
            var prop = so.FindProperty(field);
            if (prop == null) Debug.LogError($"필드 없음: {so.targetObject.GetType().Name}.{field}");
            else prop.objectReferenceValue = value;
        }

        static RectTransform NewUI(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return (RectTransform)go.transform;
        }

        static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }

        // 둥근 모서리 + 나무색 테두리 박스
        static RectTransform MakeBox(Transform parent, string name, Color fill, float border)
        {
            var rt = NewUI(name, parent);
            var img = rt.gameObject.AddComponent<Image>();
            img.sprite = rounded;
            img.type = Image.Type.Sliced;
            img.color = fill;
            if (border > 0)
            {
                var outline = rt.gameObject.AddComponent<Outline>();
                outline.effectColor = Wood;
                outline.effectDistance = new Vector2(border, border);
            }
            return rt;
        }

        static TextMeshProUGUI MakeText(Transform parent, string text, float size, Color color,
            TextAlignmentOptions align = TextAlignmentOptions.Center)
        {
            var t = NewUI("Text", parent).gameObject.AddComponent<TextMeshProUGUI>();
            t.font = font;
            t.text = text;
            t.fontSize = size;
            t.color = color;
            t.alignment = align;
            t.raycastTarget = false;
            return t;
        }

        static Button MakeButton(Transform parent, string label, Color fill, float height = 88, float fontSize = 38)
        {
            var rt = MakeBox(parent, label + " Button", fill, fill.a > 0 ? 4 : 0);
            rt.sizeDelta = new Vector2(300, height);
            rt.gameObject.AddComponent<LayoutElement>().preferredHeight = height;
            var button = rt.gameObject.AddComponent<Button>();
            var text = MakeText(rt, label, fontSize, DarkBrown);
            Stretch(text.rectTransform);
            return button;
        }

        static Button MakeCloseButton(RectTransform window)
        {
            var button = MakeButton(window, "X", Cream, 70, 40);
            var rt = (RectTransform)button.transform;
            button.GetComponent<LayoutElement>().ignoreLayout = true;
            rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.one;
            rt.anchoredPosition = new Vector2(-20, -20);
            rt.sizeDelta = new Vector2(70, 70);
            return button;
        }

        static RectTransform MakeRow(Transform parent)
        {
            var row = NewUI("Row", parent);
            row.gameObject.AddComponent<LayoutElement>().preferredHeight = 88;
            var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 24;
            layout.childControlWidth = layout.childControlHeight = true;
            layout.childForceExpandWidth = layout.childForceExpandHeight = true;
            return row;
        }

        // 반투명 배경 + 가운데 창. title이 null이면 제목 없음
        static (GameObject root, RectTransform window) MakePopup(Transform canvas, string name, string title,
            Vector2 size, Color windowColor, Color titleColor)
        {
            var overlay = NewUI(name, canvas);
            Stretch(overlay);
            overlay.gameObject.AddComponent<Image>().color = new Color(0, 0, 0, 0.5f); // 뒤쪽 메뉴 클릭 차단

            var window = MakeBox(overlay, "Window", windowColor, 8);
            window.sizeDelta = size;
            var layout = window.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(60, 60, 40, 50);
            layout.spacing = 24;
            layout.childControlWidth = layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            if (title != null) MakeText(window, title, 56, titleColor).name = "Title";
            return (overlay.gameObject, window);
        }

        static TMP_InputField MakeInputField(Transform parent, string placeholder)
        {
            var go = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());
            go.transform.SetParent(parent, false);
            go.AddComponent<LayoutElement>().preferredHeight = 88;
            var bgImage = go.GetComponent<Image>();
            bgImage.sprite = rounded;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = Cream;
            var outline = go.AddComponent<Outline>();
            outline.effectColor = Wood;
            outline.effectDistance = new Vector2(4, 4);
            var input = go.GetComponent<TMP_InputField>();
            input.fontAsset = font;
            input.pointSize = 38;
            input.textComponent.color = DarkBrown;
            input.textComponent.alignment = TextAlignmentOptions.Center;
            var ph = (TMP_Text)input.placeholder;
            ph.text = placeholder;
            ph.alignment = TextAlignmentOptions.Center;
            return input;
        }

    }
}
