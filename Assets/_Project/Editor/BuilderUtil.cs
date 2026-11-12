using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace CafeTime.EditorTools
{
    // 씬 빌더들이 함께 쓰는 UI 생성 도우미 (ADR-0006). 좌표는 1920x1080 기준
    public static class BuilderUtil
    {
        public const string FontAssetPath = "Assets/_Project/Art/Fonts/Jua SDF.asset";
        public static readonly Color TextBrown = new Color32(0x5A, 0x35, 0x20, 0xFF);
        public static readonly Color DarkCoffee = new Color32(0x38, 0x0F, 0x00, 0xFF);
        public static readonly Color Cream = new Color32(0xFA, 0xEA, 0xC2, 0xFF);
        public static readonly Color Warning = new Color32(0xC0, 0x39, 0x2B, 0xFF);

        static TMP_FontAsset font;

        public static TMP_FontAsset GameFont => font != null ? font : font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);

        // 폴더의 PNG를 도트 스프라이트 설정으로 맞춤 (docs/ART_STYLE.md 6장)
        public static void PrepareSprites(string folder, FilterMode filter = FilterMode.Point)
        {
            if (!Directory.Exists(folder)) return;
            foreach (var path in Directory.GetFiles(folder, "*.png").Select(p => p.Replace('\\', '/')))
            {
                if (!(AssetImporter.GetAtPath(path) is TextureImporter importer)) continue;
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.filterMode = filter;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = true;
                importer.maxTextureSize = 4096; // 큰 패널(2400~3200px)이 줄어들지 않게
                importer.SaveAndReimport();
            }
        }

        public static Sprite LoadSprite(string path)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) Debug.LogWarning($"스프라이트 없음 (기본 박스로 대체): {path}");
            return sprite;
        }

        // 카메라 + EventSystem + 1920x1080 Canvas 가 있는 빈 씬
        public static Transform NewSceneWithCanvas(Color background)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = background;
            camGo.AddComponent<AudioListener>();
            camGo.transform.position = new Vector3(0, 0, -10);

            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            return canvasGo.transform;
        }

        public static void SaveSceneAndAddToBuild(string scenePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(scenePath));
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.All(s => s.path != scenePath))
            {
                scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                EditorBuildSettings.scenes = scenes.ToArray();
            }
            AssetDatabase.SaveAssets();
        }

        // 지금 열린 씬의 저장 안 된 변경을 먼저 물어보고, 대상 파일이 있으면 덮어쓸지 확인
        public static bool ConfirmOverwrite(string path, string what)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return false;
            return Application.isBatchMode || !File.Exists(path) || EditorUtility.DisplayDialog($"{what} 다시 생성",
                $"{path}\n을(를) 새로 만듭니다. 에디터에서 수정한 내용은 사라집니다.", "덮어쓰기", "취소");
        }

        public static RectTransform NewUI(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return (RectTransform)go.transform;
        }

        public static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }

        // 부모의 왼쪽 위 모서리 기준 (x, y는 오른쪽·아래 방향 양수)
        public static RectTransform TopLeft(string name, Transform parent, float x, float y, float width, float height)
        {
            var rt = NewUI(name, parent);
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(x, -y);
            rt.sizeDelta = new Vector2(width, height);
            return rt;
        }

        // 부모 중심 기준 (y는 위 방향 양수)
        public static RectTransform Centered(string name, Transform parent, float x, float y, float width, float height)
        {
            var rt = NewUI(name, parent);
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(x, y);
            rt.sizeDelta = new Vector2(width, height);
            return rt;
        }

        // 스프라이트가 없으면 기본 둥근 박스 + fallback 색
        public static Image AddImage(RectTransform rt, Sprite sprite, Color fallback, bool raycast = false)
        {
            var image = rt.gameObject.AddComponent<Image>();
            if (sprite != null)
            {
                image.sprite = sprite;
                image.color = Color.white;
            }
            else if (fallback == Color.clear)
            {
                // 아이콘 자리: 실행 중에 sprite 를 넣는다. 색을 투명으로 두면 sprite 를 넣어도 보이지 않으므로 흰색
                image.color = Color.white;
            }
            else
            {
                image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                image.type = Image.Type.Sliced;
                image.color = fallback;
            }
            image.raycastTarget = raycast;
            return image;
        }

        // 화면 전체 배경. 비율을 유지하며 화면을 꽉 채움
        public static void AddBackground(Transform canvas, Sprite sprite, Color fallback, float dim)
        {
            var bg = NewUI("Background", canvas);
            bg.anchorMin = bg.anchorMax = bg.pivot = new Vector2(0.5f, 0.5f);
            var image = AddImage(bg, sprite, fallback);
            if (sprite != null)
            {
                var fitter = bg.gameObject.AddComponent<AspectRatioFitter>();
                fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fitter.aspectRatio = sprite.rect.width / sprite.rect.height;
            }
            else Stretch(bg);
            image.type = Image.Type.Simple;

            if (dim <= 0) return;
            var overlay = NewUI("Dim", canvas);
            Stretch(overlay);
            overlay.gameObject.AddComponent<Image>().color = new Color(0, 0, 0, dim);
            overlay.GetComponent<Image>().raycastTarget = false;
        }

        // 글자 이미지(LABEL). Codex가 크기 등급별로 글자 높이를 맞춰 만든 PNG를 기본 절반 크기(scale)로 표시한다.
        // (x, y): 부모 왼쪽 위 기준 위치, pivot: 그 위치에 붙일 이미지의 기준점 (왼쪽 정렬 = (0, 0.5), 가운데 = (0.5, 0.5))
        public static Image Label(Transform parent, string name, Sprite sprite, float x, float y, Vector2 pivot, float scale = 0.5f)
        {
            var rt = NewUI(name, parent);
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
            rt.pivot = pivot;
            rt.anchoredPosition = new Vector2(x, -y);
            rt.sizeDelta = sprite != null ? sprite.rect.size * scale : new Vector2(160, 30);
            var image = rt.gameObject.AddComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;
            image.raycastTarget = false;
            return image;
        }

        public static readonly Vector2 PivotLeft = new Vector2(0, 0.5f);
        public static readonly Vector2 PivotCenter = new Vector2(0.5f, 0.5f);
        public static readonly Vector2 PivotRight = new Vector2(1, 0.5f);

        // 부모 한가운데에 놓는 글자 이미지 (버튼·탭 위 라벨)
        public static Image CenterLabel(Transform parent, string name, Sprite sprite)
        {
            var image = Label(parent, name, sprite, 0, 0, PivotCenter);
            var rt = image.rectTransform;
            rt.anchorMin = rt.anchorMax = PivotCenter;
            rt.anchoredPosition = Vector2.zero;
            return image;
        }

        public static TextMeshProUGUI AddText(RectTransform rt, string text, float size, Color color,
            TextAlignmentOptions align = TextAlignmentOptions.Center)
        {
            var t = rt.gameObject.AddComponent<TextMeshProUGUI>();
            t.font = GameFont;
            t.text = text;
            t.fontSize = size;
            t.color = color;
            t.alignment = align;
            t.textWrappingMode = TextWrappingModes.NoWrap;
            t.raycastTarget = false;
            return t;
        }

        public static TextMeshProUGUI StretchText(Transform parent, string name, string text, float size, Color color,
            TextAlignmentOptions align = TextAlignmentOptions.Center)
        {
            var rt = NewUI(name, parent);
            Stretch(rt);
            return AddText(rt, text, size, color, align);
        }

        public static Button AddButton(RectTransform rt, Sprite sprite, Color fallback)
        {
            AddImage(rt, sprite, fallback, true);
            var button = rt.gameObject.AddComponent<Button>();
            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color32(0xFF, 0xF1, 0xD0, 0xFF);
            colors.pressedColor = new Color32(0xD8, 0xB2, 0x85, 0xFF);
            colors.selectedColor = colors.highlightedColor;
            colors.disabledColor = new Color32(0x9A, 0x8C, 0x7C, 0xB0);
            button.colors = colors;
            return button;
        }

        public static void Set(SerializedObject so, string field, Object value)
        {
            var prop = so.FindProperty(field);
            if (prop == null) Debug.LogError($"필드 없음: {so.targetObject.GetType().Name}.{field}");
            else prop.objectReferenceValue = value;
        }

        public static void SetArray(SerializedObject so, string field, Object[] values)
        {
            var prop = so.FindProperty(field);
            prop.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
                prop.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        }
    }
}
