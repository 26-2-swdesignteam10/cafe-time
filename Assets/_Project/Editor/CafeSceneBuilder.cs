using System.Collections.Generic;
using System.IO;
using System.Linq;
using CafeTime.Cafe;
using CafeTime.Player;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace CafeTime.EditorTools
{
    // 영업 씬(카페 맵) 초기 생성 도구 (처음 한 번만 사용). CafeTime > Build Cafe Scene
    //   - 임시 도형 그림(바닥/벽/카운터 타일, 캐릭터 프레임)을 PNG로 생성
    //   - 타일맵 맵 + 기기 배치 (SFR-012), 플레이어 (SFR-015~020), 카메라 (SFR-019)
    // 실제 도트 그림이 나오면 PNG를 같은 경로에 덮어쓰거나 Inspector에서 스프라이트를 교체한다.
    // 다시 실행하면 씬을 덮어씀 (생성된 PNG는 이미 있으면 다시 만들지 않음)
    public static class CafeSceneBuilder
    {
        const string FontAssetPath = "Assets/_Project/Art/Fonts/Galmuri11 SDF.asset";
        const string ScenePath = "Assets/_Project/Scenes/Cafe.unity";
        const string MapArtDir = "Assets/_Project/Art/Sprites/Cafe";
        const string PlayerArtDir = "Assets/_Project/Art/Sprites/Player";
        const int PixelsPerUnit = 16;

        // 맵 크기 (타일). 바깥 한 줄 + 위쪽 두 줄은 벽
        const int MapWidth = 26;
        const int MapHeight = 16;
        const int FrontCounterY = 8;

        static readonly Color32 Outline = new Color32(0x2B, 0x1A, 0x12, 0xFF);
        static readonly Color Paper = new Color32(0xF6, 0xE3, 0xC3, 0xFF);
        static readonly Color Cream = new Color32(0xFB, 0xEF, 0xD9, 0xFF);
        static readonly Color DarkBrown = new Color32(0x3E, 0x27, 0x1A, 0xFF);
        static readonly Color Highlight = new Color32(0xFF, 0xE2, 0x7A, 0xFF);

        // 기기 배치: 이름, 안내 문구, 가운데 x, 아래쪽 y, 너비, 높이, 색 (SFR-012)
        static readonly (string name, string action, float x, float y, float w, float h, Color32 color)[] Stations =
        {
            // 뒤쪽 작업대 (위쪽 벽 앞)
            ("커피잔 선반", "열기", 2.2f, 12, 1.6f, 1.5f, new Color32(0xB0, 0x8B, 0x6A, 0xFF)),
            ("에스프레소 머신", "사용", 5, 12, 2, 1.5f, new Color32(0x7A, 0x7F, 0x88, 0xFF)),
            ("제빙기", "사용", 7.6f, 12, 1.4f, 1.5f, new Color32(0xA8, 0xD8, 0xE8, 0xFF)),
            ("정수기", "사용", 9.6f, 12, 1.2f, 1.5f, new Color32(0x9C, 0xC5, 0xE0, 0xFF)),
            ("음료 제조실", "열기", 12, 12, 2.2f, 1.5f, new Color32(0xD9, 0xA0, 0x6B, 0xFF)),
            ("쉐이크 제조실", "열기", 14.8f, 12, 2.2f, 1.5f, new Color32(0xE8, 0x9C, 0xB0, 0xFF)),
            ("설거지실", "열기", 19.5f, 12, 2.6f, 1.5f, new Color32(0x8F, 0xB8, 0xA8, 0xFF)),
            ("행주 걸이", "사용", 22.6f, 12, 1.4f, 1.2f, new Color32(0xE0, 0xD8, 0xC0, 0xFF)),
            // 앞쪽 카운터 위 (작업 공간 쪽에서 상호작용)
            ("포스기", "사용", 3, FrontCounterY, 1.2f, 1.1f, new Color32(0x4A, 0x55, 0x60, 0xFF)),
            ("픽업대", "사용", 8, FrontCounterY, 2.4f, 1.1f, new Color32(0xF0, 0xC0, 0x70, 0xFF)),
            ("디저트 진열장", "열기", 12.2f, FrontCounterY, 2.2f, 1.1f, new Color32(0xF5, 0xD5, 0xD5, 0xFF)),
            ("퇴식구", "열기", 20.5f, FrontCounterY, 2.2f, 1.1f, new Color32(0xA0, 0x90, 0x80, 0xFF)),
        };

        static TMP_FontAsset font;
        static Sprite square;

        [MenuItem("CafeTime/Build Cafe Scene")]
        public static void Build()
        {
            font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);
            if (font == null)
            {
                Debug.LogError("폰트 에셋이 없습니다. 먼저 CafeTime > Setup Fonts 를 실행하세요.");
                return;
            }
            if (File.Exists(ScenePath) && !Application.isBatchMode && !EditorUtility.DisplayDialog("Cafe 씬 다시 생성",
                    "Cafe 씬을 새로 만듭니다.\n에디터에서 수정한 내용은 사라집니다.", "덮어쓰기", "취소"))
                return;

            Directory.CreateDirectory(MapArtDir);
            Directory.CreateDirectory(PlayerArtDir);
            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));

            square = MakeSprite($"{MapArtDir}/Square.png", 16, 16, new Vector2(0.5f, 0), (x, y) => Color.white);
            var floor = MakeTile("Floor", FloorPixel);
            var wall = MakeTile("Wall", WallPixel);
            var counter = MakeTile("Counter", CounterPixel);
            var door = MakeTile("Door", DoorPixel);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var light = new GameObject("Global Light 2D").AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;

            var walls = BuildMap(floor, wall, counter, door);
            BuildSeating();
            var screen = BuildUI();
            BuildStations(screen);
            var player = BuildPlayer();

            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = DarkBrown;
            camGo.AddComponent<AudioListener>();
            camGo.transform.position = new Vector3(player.position.x, player.position.y, -10);
            var follow = new SerializedObject(camGo.AddComponent<CameraFollow>());
            Set(follow, "target", player);
            Set(follow, "mapBounds", walls);
            follow.ApplyModifiedPropertiesWithoutUndo();

            new GameObject("CafeSceneInput", typeof(CafeSceneInput));

            EditorSceneManager.SaveScene(scene, ScenePath);

            // 빌드 씬 목록에 등록 (Title이 맨 앞인 순서는 유지)
            if (EditorBuildSettings.scenes.All(s => s.path != ScenePath))
                EditorBuildSettings.scenes = EditorBuildSettings.scenes
                    .Append(new EditorBuildSettingsScene(ScenePath, true)).ToArray();

            AssetDatabase.SaveAssets();
            Debug.Log("Cafe 씬 생성 완료: " + ScenePath);
        }

        // ───────── 맵 (SFR-012) ─────────

        // 바닥 / 벽·카운터(충돌) 타일맵. 벽 타일맵이 맵 전체 범위를 덮으므로 카메라 경계로도 사용
        static Tilemap BuildMap(TileBase floor, TileBase wall, TileBase counter, TileBase door)
        {
            var grid = new GameObject("Grid", typeof(Grid));
            var floorMap = NewTilemap(grid.transform, "Floor", -20);
            var wallMap = NewTilemap(grid.transform, "Walls", -10);
            wallMap.gameObject.AddComponent<TilemapCollider2D>();

            // 6000.6.4f1에서 SetTile(위치, 타일) 한 칸씩 넣기가 반영되지 않아 SetTiles로 한 번에 넣음
            var floorCells = new List<Vector3Int>();
            var wallCells = new List<Vector3Int>();
            var wallTiles = new List<TileBase>();
            for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
            {
                var cell = new Vector3Int(x, y, 0);
                bool border = x == 0 || x == MapWidth - 1 || y == 0 || y >= MapHeight - 2;
                // 앞쪽 카운터: 왼쪽 x1~14, 오른쪽(퇴식구) x18~24, 사이 x15~17은 통로
                bool frontCounter = !border && y == FrontCounterY && (x < 15 || x > 17);
                if (border || frontCounter)
                {
                    wallCells.Add(cell);
                    wallTiles.Add(frontCounter ? counter : y == 0 && (x == 12 || x == 13) ? door : wall);
                }
                if (!border) floorCells.Add(cell);
            }
            floorMap.SetTiles(floorCells.ToArray(), floorCells.Select(_ => floor).ToArray());
            wallMap.SetTiles(wallCells.ToArray(), wallTiles.ToArray());

            return wallMap;
        }

        static Tilemap NewTilemap(Transform grid, string name, int order)
        {
            // Tilemap은 Grid 아래에 붙인 뒤 추가해야 타일이 정상 저장됨
            var go = new GameObject(name);
            go.transform.SetParent(grid, false);
            var tilemap = go.AddComponent<Tilemap>();
            go.AddComponent<TilemapRenderer>().sortingOrder = order;
            return tilemap;
        }

        // 테이블 및 의자 (테이블 수는 추후 결정, SFR-024)
        static void BuildSeating()
        {
            var root = new GameObject("Seating").transform;
            var table = new Color32(0x9A, 0x62, 0x3C, 0xFF);
            var chair = new Color32(0x6E, 0x44, 0x2A, 0xFF);
            int index = 1;
            foreach (float y in new[] { 5f, 2f })
            foreach (float x in new[] { 4f, 9f, 14f, 20f })
            {
                MakeBlock($"Table {index++}", root, x, y, 1.4f, 0.9f, table);
                MakeBlock("Chair", root, x - 1.1f, y + 0.1f, 0.6f, 0.6f, chair);
                MakeBlock("Chair", root, x + 1.1f, y + 0.1f, 0.6f, 0.6f, chair);
            }
        }

        // 상호작용 기기: 아직 기기 화면이 없으므로 임시 창을 열도록 연결 (SFR-020)
        static void BuildStations(StationScreenPlaceholder screen)
        {
            var root = new GameObject("Stations").transform;
            foreach (var s in Stations)
            {
                var go = MakeBlock(s.name, root, s.x, s.y, s.w, s.h, s.color);
                MakeLabel(go.transform, s.name, s.w, s.h);

                var outline = new GameObject("Highlight").AddComponent<SpriteRenderer>();
                outline.transform.SetParent(go.transform, false);
                outline.transform.localPosition = new Vector3(0, -0.08f, 0);
                outline.sprite = square;
                outline.drawMode = SpriteDrawMode.Sliced;
                outline.size = new Vector2(s.w + 0.16f, s.h + 0.16f);
                outline.color = Highlight;
                outline.sortingOrder = -1;

                var interactable = go.AddComponent<Interactable>();
                var so = new SerializedObject(interactable);
                so.FindProperty("actionLabel").stringValue = s.action;
                Set(so, "highlight", outline.gameObject);
                so.ApplyModifiedPropertiesWithoutUndo();
                UnityEventTools.AddStringPersistentListener(interactable.OnInteract, screen.Open, s.name);
                outline.gameObject.SetActive(false);
            }
        }

        // 아래쪽 가운데가 기준점인 색 사각형 + 같은 크기의 충돌 영역
        static GameObject MakeBlock(string name, Transform parent, float x, float y, float w, float h, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.position = new Vector3(x, y, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = square;
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = new Vector2(w, h);
            sr.color = color;
            var box = go.AddComponent<BoxCollider2D>();
            box.size = new Vector2(w, h);
            box.offset = new Vector2(0, h / 2);
            return go;
        }

        static void MakeLabel(Transform parent, string text, float w, float h)
        {
            var label = new GameObject("Label").AddComponent<TextMeshPro>();
            label.transform.SetParent(parent, false);
            label.transform.localPosition = new Vector3(0, h / 2, 0);
            label.rectTransform.sizeDelta = new Vector2(w, h);
            label.font = font;
            label.text = text;
            label.fontSize = 2.4f;
            label.color = DarkBrown;
            label.alignment = TextAlignmentOptions.Center;
            label.sortingOrder = 1;
        }

        // ───────── 플레이어 (SFR-015~020) ─────────

        static Transform BuildPlayer()
        {
            var go = new GameObject("Player");
            go.transform.position = new Vector3(8, 10.2f, 0);
            var body = go.AddComponent<Rigidbody2D>();
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            // 발밑만 충돌 영역 → 기기 앞까지 자연스럽게 다가감
            var feet = go.AddComponent<BoxCollider2D>();
            feet.size = new Vector2(0.6f, 0.3f);
            feet.offset = new Vector2(0, 0.15f);
            var sr = go.AddComponent<SpriteRenderer>();
            var movement = go.AddComponent<PlayerMovement>();

            var anim = new SerializedObject(go.AddComponent<PlayerAnimator>());
            Set(anim, "movement", movement);
            foreach (var dir in new[] { "down", "up", "left", "right" })
            {
                var idle = MakePlayerFrame(dir, 0);
                var step1 = MakePlayerFrame(dir, 1);
                var step2 = MakePlayerFrame(dir, 2);
                Set(anim, dir + ".idle", idle);
                var walk = anim.FindProperty(dir + ".walk");
                var frames = new[] { step1, idle, step2, idle };
                walk.arraySize = frames.Length;
                for (int i = 0; i < frames.Length; i++)
                    walk.GetArrayElementAtIndex(i).objectReferenceValue = frames[i];
                if (dir == "down") sr.sprite = idle;
            }
            anim.ApplyModifiedPropertiesWithoutUndo();

            // 상호작용 키 안내 (오브젝트 위에 표시, 하나를 옮겨 가며 사용)
            var prompt = new GameObject("InteractPrompt");
            var border = new GameObject("Border").AddComponent<SpriteRenderer>();
            border.transform.SetParent(prompt.transform, false);
            border.transform.localPosition = new Vector3(0, -0.05f, 0);
            border.sprite = square;
            border.drawMode = SpriteDrawMode.Sliced;
            border.size = new Vector2(1.5f, 0.55f);
            border.color = DarkBrown;
            border.sortingOrder = 100;
            var box = new GameObject("Box").AddComponent<SpriteRenderer>();
            box.transform.SetParent(prompt.transform, false);
            box.sprite = square;
            box.drawMode = SpriteDrawMode.Sliced;
            box.size = new Vector2(1.4f, 0.45f);
            box.color = Cream;
            box.sortingOrder = 101;
            var text = new GameObject("Text").AddComponent<TextMeshPro>();
            text.transform.SetParent(prompt.transform, false);
            text.transform.localPosition = new Vector3(0, 0.225f, 0);
            text.rectTransform.sizeDelta = new Vector2(1.4f, 0.45f);
            text.font = font;
            text.text = "E 사용";
            text.fontSize = 3;
            text.color = DarkBrown;
            text.alignment = TextAlignmentOptions.Center;
            text.sortingOrder = 102;
            prompt.SetActive(false);

            var interactor = new SerializedObject(go.AddComponent<PlayerInteractor>());
            Set(interactor, "prompt", prompt);
            Set(interactor, "promptText", text);
            interactor.ApplyModifiedPropertiesWithoutUndo();
            return go.transform;
        }

        // ───────── UI ─────────

        static StationScreenPlaceholder BuildUI()
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            var overlay = NewUI("StationScreenPlaceholder", canvasGo.transform);
            Stretch(overlay);
            overlay.gameObject.AddComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            var window = NewUI("Window", overlay);
            window.sizeDelta = new Vector2(900, 500);
            window.gameObject.AddComponent<Image>().color = Paper;

            var title = NewUI("Title", window).gameObject.AddComponent<TextMeshProUGUI>();
            Stretch(title.rectTransform);
            title.font = font;
            title.fontSize = 64;
            title.color = DarkBrown;
            title.alignment = TextAlignmentOptions.Center;
            title.raycastTarget = false;

            var close = NewUI("Close Button", window);
            close.anchorMin = close.anchorMax = close.pivot = Vector2.one;
            close.anchoredPosition = new Vector2(-20, -20);
            close.sizeDelta = new Vector2(70, 70);
            close.gameObject.AddComponent<Image>().color = Cream;
            var closeButton = close.gameObject.AddComponent<Button>();
            var x = NewUI("Text", close).gameObject.AddComponent<TextMeshProUGUI>();
            Stretch(x.rectTransform);
            x.font = font;
            x.text = "X";
            x.fontSize = 40;
            x.color = DarkBrown;
            x.alignment = TextAlignmentOptions.Center;

            var screen = overlay.gameObject.AddComponent<StationScreenPlaceholder>();
            var so = new SerializedObject(screen);
            Set(so, "titleText", title);
            Set(so, "closeButton", closeButton);
            so.ApplyModifiedPropertiesWithoutUndo();
            overlay.gameObject.SetActive(false);
            return screen;
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

        static void Set(SerializedObject so, string field, Object value)
        {
            var prop = so.FindProperty(field);
            if (prop == null) Debug.LogError($"필드 없음: {so.targetObject.GetType().Name}.{field}");
            else prop.objectReferenceValue = value;
        }

        // ───────── 임시 도트 그림 ─────────

        static Tile MakeTile(string name, System.Func<int, int, Color32> pixel)
        {
            var tilePath = $"{MapArtDir}/{name}Tile.asset";
            var existing = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
            if (existing != null) return existing;
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = MakeSprite($"{MapArtDir}/{name}.png", 16, 16, new Vector2(0.5f, 0.5f), pixel);
            AssetDatabase.CreateAsset(tile, tilePath);
            return tile;
        }

        static Color32 FloorPixel(int x, int y)
        {
            bool seam = y % 4 == 0 || (x == ((y / 4) % 2 == 0 ? 3 : 11));
            return seam ? new Color32(0xA8, 0x7A, 0x50, 0xFF) : new Color32(0xC9, 0x9A, 0x6B, 0xFF);
        }

        static Color32 WallPixel(int x, int y)
        {
            if (y >= 13) return new Color32(0x8B, 0x6A, 0x55, 0xFF);
            bool mortar = y % 6 == 0 || x == ((y / 6) % 2 == 0 ? 0 : 8);
            return mortar ? new Color32(0x55, 0x3A, 0x2C, 0xFF) : new Color32(0x6B, 0x4A, 0x3A, 0xFF);
        }

        static Color32 CounterPixel(int x, int y)
        {
            if (y >= 12) return new Color32(0xC2, 0x6A, 0x3E, 0xFF);
            return y == 11 ? Outline : new Color32(0x8B, 0x45, 0x2A, 0xFF);
        }

        static Color32 DoorPixel(int x, int y)
        {
            bool frame = x == 0 || x == 15 || y == 15;
            return frame ? Outline : new Color32(0x4A, 0x6B, 0x5A, 0xFF);
        }

        // 16x24 캐릭터 프레임. step 0 = 대기, 1·2 = 한쪽 발을 든 걷기 프레임
        static Sprite MakePlayerFrame(string dir, int step)
        {
            const int w = 16, h = 24;
            var skin = new Color32(0xF2, 0xC9, 0xA0, 0xFF);
            var hair = new Color32(0x5A, 0x34, 0x20, 0xFF);
            var shirt = new Color32(0xFF, 0xF4, 0xE0, 0xFF);
            var apron = new Color32(0x8B, 0x45, 0x2A, 0xFF);
            var pants = new Color32(0x3E, 0x3A, 0x4A, 0xFF);
            var px = new Color32[w * h];

            void Rect(int x0, int y0, int x1, int y1, Color32 c)
            {
                for (int y = y0; y <= y1; y++)
                for (int x = x0; x <= x1; x++)
                    px[y * w + x] = c;
            }

            Rect(5, step == 1 ? 2 : 1, 7, 5, pants);
            Rect(8, step == 2 ? 2 : 1, 10, 5, pants);
            Rect(4, 6, 11, 13, shirt);
            Rect(3, 7, 3, 12, shirt);
            Rect(12, 7, 12, 12, shirt);
            if (dir == "down") Rect(5, 6, 10, 12, apron);
            if (dir == "left") Rect(4, 6, 8, 12, apron);
            if (dir == "right") Rect(7, 6, 11, 12, apron);
            Rect(4, 14, 11, 21, skin);
            Rect(4, 19, 11, 22, hair);
            if (dir == "up") Rect(4, 14, 11, 22, hair);
            if (dir == "left") Rect(9, 14, 11, 21, hair);
            if (dir == "right") Rect(4, 14, 6, 21, hair);
            if (dir == "down") { px[17 * w + 6] = Outline; px[17 * w + 9] = Outline; }
            if (dir == "left") px[17 * w + 5] = Outline;
            if (dir == "right") px[17 * w + 10] = Outline;

            // 1픽셀 외곽선
            var outlined = (Color32[])px.Clone();
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                if (px[y * w + x].a != 0) continue;
                bool edge = (x > 0 && px[y * w + x - 1].a != 0) || (x < w - 1 && px[y * w + x + 1].a != 0) ||
                            (y > 0 && px[(y - 1) * w + x].a != 0) || (y < h - 1 && px[(y + 1) * w + x].a != 0);
                if (edge) outlined[y * w + x] = Outline;
            }

            var name = char.ToUpper(dir[0]) + dir.Substring(1);
            return MakeSprite($"{PlayerArtDir}/Player{name}{step}.png", w, h, new Vector2(0.5f, 0),
                (x, y) => outlined[y * w + x]);
        }

        // PNG가 없으면 생성하고, 도트 그림 설정(Point 필터, 16 PPU)으로 임포트
        static Sprite MakeSprite(string path, int w, int h, Vector2 pivot, System.Func<int, int, Color32> pixel)
        {
            if (!File.Exists(path))
            {
                var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
                var pixels = new Color32[w * h];
                for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    pixels[y * w + x] = pixel(x, y);
                tex.SetPixels32(pixels);
                File.WriteAllBytes(path, tex.EncodeToPNG());
                Object.DestroyImmediate(tex);
                AssetDatabase.ImportAsset(path);

                var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = PixelsPerUnit;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                var settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Custom;
                settings.spritePivot = pivot;
                settings.spriteMeshType = SpriteMeshType.FullRect; // Sliced 그리기 모드로 크기 조절
                importer.SetTextureSettings(settings);
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
    }
}
