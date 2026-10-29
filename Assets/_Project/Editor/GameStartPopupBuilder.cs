using System.Collections.Generic;
using System.Linq;
using CafeTime.Title;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.EditorTools
{
    public static class GameStartPopupBuilder
    {
        const string Art = "Assets/_Project/Art/UI/GameStart/";
        const string PrefabPath = "Assets/_Project/Prefabs/Title/GameStartPopup.prefab";
        static Dictionary<string, Sprite> sprites;

        [MenuItem("CafeTime/Build Game Start Popup")]
        public static void Rebuild()
        {
            LoadAssets();
            var root = PrefabUtility.LoadPrefabContents(PrefabPath);
            try
            {
                while (root.transform.childCount > 0)
                    Object.DestroyImmediate(root.transform.GetChild(0).gameObject);
                Populate(root);
                PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
            Debug.Log("Game start chalkboard popup rebuilt.");
        }

        public static GameObject Create(Transform canvas)
        {
            LoadAssets();
            var root = new GameObject("GameStartPopup", typeof(RectTransform), typeof(Image), typeof(GameStartPopup));
            root.transform.SetParent(canvas, false);
            var rect = (RectTransform)root.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            Populate(root);
            return root;
        }

        static void Populate(GameObject root)
        {
            root.GetComponent<Image>().color = new Color(0, 0, 0, 0.18f);
            var window = At(root.transform, "Window", 0, 0, 720, 650);
            window.anchorMin = window.anchorMax = new Vector2(0.5f, 350f / 1080f);
            window.pivot = new Vector2(0.5f, 0.5f);
            window.anchoredPosition = Vector2.zero;
            window.gameObject.AddComponent<Image>().sprite = sprites["Board"];
            var so = new SerializedObject(root.GetComponent<GameStartPopup>());
            var start = MakeButton(window, "NewGameButton", sprites["NewGame"], 50, 180, 620, 140);
            var resume = MakeButton(window, "ContinueButton", sprites["Continue"], 50, 365, 620, 140);
            var info = At(resume.transform, "ContinueInfo", 155, 83, 445, 35).gameObject.AddComponent<TextMeshProUGUI>();
            info.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/_Project/Art/Fonts/Galmuri11 SDF.asset");
            info.text = "저장된 데이터 없음";
            info.fontSize = 24;
            info.alignment = TextAlignmentOptions.Center;
            info.textWrappingMode = TextWrappingModes.NoWrap;
            info.color = new Color32(0x5A, 0x35, 0x20, 0xFF);
            info.raycastTarget = false;
            // The back caption is painted on the board; this transparent button supplies its hit area.
            var back = MakeButton(window, "BackButton", null, 130, 535, 460, 95);
            back.image.color = Color.clear;
            Set(so, "newGameButton", start);
            Set(so, "continueButton", resume);
            Set(so, "continueInfoText", info);
            Set(so, "closeButton", back);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static RectTransform At(Transform parent, string name, float x, float y, float width, float height)
        {
            var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(x, -y);
            rect.sizeDelta = new Vector2(width, height);
            return rect;
        }

        static Button MakeButton(Transform parent, string name, Sprite sprite, float x, float y, float width, float height)
        {
            var image = At(parent, name, x, y, width, height).gameObject.AddComponent<Image>();
            image.sprite = sprite;
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color32(255, 241, 215, 255);
            colors.pressedColor = new Color32(216, 178, 133, 255);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;
            return button;
        }

        static void Set(SerializedObject so, string name, Object value)
        {
            so.FindProperty(name).objectReferenceValue = value;
        }

        static void LoadAssets()
        {
            sprites = new Dictionary<string, Sprite>();
            foreach (var name in new[] { "Board", "NewGame", "Continue" })
                sprites[name] = AssetDatabase.LoadAllAssetsAtPath(Art + name + ".png").OfType<Sprite>().Single();
        }
    }
}