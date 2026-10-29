using System.Linq;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.EditorTools
{
    public static class ConfirmPopupBuilder
    {
        const string Art = "Assets/_Project/Art/UI/Quit/";
        const string PrefabPath = "Assets/_Project/Prefabs/UI/ConfirmPopup.prefab";

        [MenuItem("CafeTime/Build Confirm Popup")]
        public static void Rebuild()
        {
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
        }

        public static GameObject Create(Transform canvas)
        {
            var root = new GameObject("ConfirmPopup", typeof(RectTransform), typeof(Image), typeof(ConfirmPopup));
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
            root.GetComponent<Image>().color = new Color(0, 0, 0, 0.3f);
            var window = At(root.transform, "Window", 0, 0, 1000, 650);
            window.anchorMin = window.anchorMax = new Vector2(0.5f, 350f / 1080f);
            window.pivot = new Vector2(0.5f, 0.5f);
            window.anchoredPosition = Vector2.zero;
            window.gameObject.AddComponent<Image>().sprite = Load("Panel");
            var graphic = At(window, "QuitMessage", 100, 260, 800, 110).gameObject.AddComponent<Image>();
            graphic.sprite = Load("Message");
            graphic.preserveAspect = true;
            graphic.raycastTarget = false;
            var message = At(window, "Message", 100, 260, 800, 110).gameObject.AddComponent<TextMeshProUGUI>();
            message.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/_Project/Art/Fonts/Galmuri11 SDF.asset");
            message.fontSize = 38;
            message.alignment = TextAlignmentOptions.Center;
            message.color = new Color32(0x5A, 0x35, 0x20, 0xFF);
            message.raycastTarget = false;
            message.gameObject.SetActive(false);
            var so = new SerializedObject(root.GetComponent<ConfirmPopup>());
            Set(so, "messageText", message);
            Set(so, "quitMessageGraphic", graphic.gameObject);
            Set(so, "yesButton", MakeButton(window, "YesButton", "Yes", 110));
            Set(so, "noButton", MakeButton(window, "NoButton", "No", 530));
            Set(so, "closeButton", null);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static Button MakeButton(Transform parent, string name, string sprite, float x)
        {
            var image = At(parent, name, x, 415, 360, 130).gameObject.AddComponent<Image>();
            image.sprite = Load(sprite);
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

        static RectTransform At(Transform parent, string name, float x, float y, float width, float height)
        {
            var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(x, -y);
            rect.sizeDelta = new Vector2(width, height);
            return rect;
        }

        static Sprite Load(string name) => AssetDatabase.LoadAllAssetsAtPath(Art + name + ".png").OfType<Sprite>().Single();

        static void Set(SerializedObject so, string name, Object value)
        {
            so.FindProperty(name).objectReferenceValue = value;
        }
    }
}
