using System.Collections.Generic;
using System.IO;
using System.Linq;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.EditorTools
{
    public static class SettingsPopupBuilder
    {
        const string Art = "Assets/_Project/Art/UI/Settings/";
        const string PrefabPath = "Assets/_Project/Prefabs/UI/SettingsPopup.prefab";
        static readonly Vector2 WindowSize = new Vector2(1448, 1086);
        static Dictionary<string, Sprite> sprites;

        [MenuItem("CafeTime/Build Settings Popup")]
        public static void Rebuild()
        {
            ImportAssets();
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
            Debug.Log("Settings popup rebuilt with sprite assets.");
        }

        public static GameObject Create(Transform canvas)
        {
            ImportAssets();
            var root = new GameObject("SettingsPopup", typeof(RectTransform), typeof(Image), typeof(SettingsPopup));
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
            root.GetComponent<Image>().color = new Color(0, 0, 0, 0.55f);
            var window = At(root.transform, "Window", 0, 0, WindowSize.x, WindowSize.y);
            window.anchorMin = window.anchorMax = window.pivot = new Vector2(0.5f, 0.5f);
            window.anchoredPosition = Vector2.zero;
            window.localScale = new Vector3(0.8f, 0.8f, 1);
            window.gameObject.AddComponent<Image>().sprite = sprites["Panel"];
            var so = new SerializedObject(root.GetComponent<SettingsPopup>());
            Set(so, "soundTabButton", Button(window, "SoundTabButton", "SoundSelected", 108, 294, 294, 151));
            Set(so, "keyTabButton", Button(window, "KeyTabButton", "KeyNormal", 108, 449, 294, 148));
            Set(so, "soundNormalSprite", sprites["SoundNormal"]);
            Set(so, "soundSelectedSprite", sprites["SoundSelected"]);
            Set(so, "keyNormalSprite", sprites["KeyNormal"]);
            Set(so, "keySelectedSprite", sprites["KeySelected"]);

            var sound = At(window, "SoundTab", 0, 0, WindowSize.x, WindowSize.y);
            Picture(sound, "MasterLabel", 440, 332, 295, 83);
            Picture(sound, "BgmLabel", 440, 474, 270, 82);
            Picture(sound, "SfxLabel", 440, 615, 240, 82);
            Set(so, "soundTab", sound.gameObject);
            Set(so, "masterSlider", VolumeSlider(sound, "MasterSlider", 350));
            Set(so, "bgmSlider", VolumeSlider(sound, "BgmSlider", 484));
            Set(so, "sfxSlider", VolumeSlider(sound, "SfxSlider", 626));

            var keys = At(window, "KeyTab", 470, 330, 810, 400);
            var text = keys.gameObject.AddComponent<TextMeshProUGUI>();
            text.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/_Project/Art/Fonts/Galmuri11 SDF.asset");
            text.text = "이동<pos=42%>W A S D / 방향키\n달리기<pos=42%>Shift (누르고 있기)\n상호작용<pos=42%>E\n일시정지 / 닫기<pos=42%>ESC\n다음 대사<pos=42%>클릭 / 지정 키\n버리기<pos=42%>양손 칸 우클릭";
            text.fontSize = 32;
            text.lineSpacing = 28;
            text.color = new Color32(0x3E, 0x27, 0x1A, 0xFF);
            text.raycastTarget = false;
            keys.gameObject.SetActive(false);
            Set(so, "keyTab", keys.gameObject);
            Set(so, "confirmButton", Button(window, "ConfirmButton", "Confirm", 486, 786, 335, 139));
            Set(so, "cancelButton", Button(window, "CancelButton", "Cancel", 832, 786, 310, 139));
            Set(so, "closeButton", Button(window, "CloseButton", "Close", 1252, 128, 130, 130));
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static Slider VolumeSlider(Transform parent, string name, float y)
        {
            var root = At(parent, name, 766, y, 532, 72);
            var track = Picture(root, "Track", 0, 16, 532, 40);
            var fill = Picture(root, "Fill", 0, 16, 532, 40);
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = 0;
            fill.rectTransform.anchorMin = Vector2.zero;
            fill.rectTransform.anchorMax = Vector2.one;
            fill.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            fill.rectTransform.offsetMin = new Vector2(0, 16);
            fill.rectTransform.offsetMax = new Vector2(0, -16);
            var handleArea = At(root, "HandleArea", 24, 0, 484, 72);
            var handle = Picture(handleArea, "Handle", 0, 3, 58, 66);
            handle.preserveAspect = true;
            handle.raycastTarget = true;
            var handleRect = handle.rectTransform;
            handleRect.pivot = new Vector2(0.5f, 0.5f);
            handleRect.anchorMin = Vector2.zero;
            handleRect.anchorMax = new Vector2(0, 1);
            handleRect.sizeDelta = new Vector2(58, -6);
            handleRect.anchoredPosition = Vector2.zero;
            track.raycastTarget = true;
            var slider = root.gameObject.AddComponent<Slider>();
            slider.targetGraphic = handle;
            slider.fillRect = fill.rectTransform;
            slider.handleRect = handleRect;
            slider.direction = Slider.Direction.LeftToRight;
            slider.value = 1;
            return slider;
        }

        static RectTransform At(Transform parent, string name, float x, float y, float width, float height)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)go.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(x, -y);
            rect.sizeDelta = new Vector2(width, height);
            return rect;
        }

        static Image Picture(Transform parent, string sprite, float x, float y, float width, float height)
        {
            var image = At(parent, sprite, x, y, width, height).gameObject.AddComponent<Image>();
            image.sprite = sprites[sprite];
            image.raycastTarget = false;
            return image;
        }

        static Button Button(Transform parent, string name, string sprite, float x, float y, float width, float height)
        {
            var image = Picture(parent, sprite, x, y, width, height);
            image.name = name;
            image.raycastTarget = true;
            var button = image.gameObject.AddComponent<Button>();
            var colors = button.colors;
            colors.highlightedColor = new Color32(255, 241, 215, 255);
            colors.pressedColor = new Color32(216, 178, 133, 255);
            colors.selectedColor = Color.white;
            button.colors = colors;
            return button;
        }

        static void Set(SerializedObject so, string name, Object value)
        {
            so.FindProperty(name).objectReferenceValue = value;
        }

        static void ImportAssets()
        {
            sprites = new Dictionary<string, Sprite>();
            Import("Reference", new[]
            {
                Slice("MasterLabel", 440, 332, 295, 83),
                Slice("BgmLabel", 440, 474, 270, 82),
                Slice("SfxLabel", 440, 615, 240, 82)
            });
            foreach (var name in new[] { "Panel", "SoundNormal", "SoundSelected", "KeyNormal", "KeySelected", "Confirm", "Cancel", "Track", "Fill", "Handle", "Close" })
                Import(name, null);
        }

        static SpriteRect Slice(string name, float x, float y, float width, float height)
        {
            // Source rectangles use top-left coordinates; Unity uses bottom-left coordinates.
            return new SpriteRect { name = name, rect = new Rect(x, WindowSize.y - y - height, width, height) };
        }

        static void Import(string file, SpriteRect[] rects)
        {
            var path = Art + file + ".png";
            var texture = new Texture2D(2, 2);
            texture.LoadImage(File.ReadAllBytes(path));
            if (rects == null)
            {
                var bounds = new Rect(0, 0, texture.width, texture.height);
                if (file != "Panel")
                {
                    var pixels = texture.GetPixels32();
                    int left = texture.width, bottom = texture.height, right = 0, top = 0;
                    for (int y = 0; y < texture.height; y++)
                    for (int x = 0; x < texture.width; x++)
                    {
                        if (pixels[y * texture.width + x].a < 16) continue;
                        left = Mathf.Min(left, x); right = Mathf.Max(right, x);
                        bottom = Mathf.Min(bottom, y); top = Mathf.Max(top, y);
                    }
                    left = Mathf.Max(0, left - 4); bottom = Mathf.Max(0, bottom - 4);
                    right = Mathf.Min(texture.width - 1, right + 4); top = Mathf.Min(texture.height - 1, top + 4);
                    bounds = new Rect(left, bottom, right - left + 1, top - bottom + 1);
                }
                rects = new[] { new SpriteRect { name = file, rect = bounds } };
            }
            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.maxTextureSize = 2048;
            var factories = new SpriteDataProviderFactories();
            factories.Init();
            var provider = factories.GetSpriteEditorDataProviderFromObject(importer);
            provider.InitSpriteEditorDataProvider();
            var previous = provider.GetSpriteRects();
            foreach (var rect in rects)
            {
                rect.spriteID = previous.FirstOrDefault(r => r.name == rect.name)?.spriteID ?? GUID.Generate();
                rect.alignment = SpriteAlignment.Center;
                rect.pivot = new Vector2(0.5f, 0.5f);
            }
            provider.SetSpriteRects(rects);
            provider.GetDataProvider<ISpriteNameFileIdDataProvider>().SetNameFileIdPairs(
                rects.Select(r => new SpriteNameFileIdPair(r.name, r.spriteID)));
            provider.Apply();
            importer.SaveAndReimport();
            foreach (var sprite in AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
                sprites[sprite.name] = sprite;
        }
    }
}
