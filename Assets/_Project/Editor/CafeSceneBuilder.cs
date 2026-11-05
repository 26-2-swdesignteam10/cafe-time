using System.Collections.Generic;
using System.IO;
using System.Linq;
using CafeTime.Cafe;
using CafeTime.Player;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // 영업 씬(카페 맵) 생성 도구 (ADR-0006). CafeTime > Build Cafe Scene
    //   - 타일맵 맵 + 테이블 + 기기 배치 (SFR-012), 플레이어 (SFR-015~021), 카메라 (SFR-019)
    //   - 영업 루프: 시계, 손님, 주문표, 포스기, 픽업대, 디저트 진열장 (ADR-0013)
    //   - 기기: 커피잔 선반, 에스프레소, 제빙기, 정수기, 음료·쉐이크 제조실, 퇴식구, 설거지실, 행주 걸이, 테이블 청소 (ADR-0014)
    //   - UI 는 CafeUiBuilder
    // 맵 그림은 1칸 = 128px (docs/ART_STYLE.md 5장). 다시 실행하면 씬을 덮어씀
    public static class CafeSceneBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Cafe.unity";
        const string MapArtDir = "Assets/_Project/Art/Sprites/Cafe";
        const string OutlineDir = "Assets/_Project/Art/Sprites/Cafe/Outlines";
        const string PlayerArtDir = "Assets/_Project/Art/Sprites/Player";
        const string CustomerArtDir = "Assets/_Project/Art/Sprites/Customers";
        const string HudArt = "Assets/_Project/Art/UI/CafeHud";
        const string ItemIconDir = "Assets/_Project/Art/Icons/Items";
        const int PixelsPerUnit = 128;

        // 맵 크기 (타일). 바깥 한 줄 + 위쪽 두 줄은 벽
        const int MapWidth = 26;
        const int MapHeight = 16;
        const int FrontCounterY = 8;

        static readonly Color DarkBrown = new Color32(0x3E, 0x27, 0x1A, 0xFF);

        enum Screen { Pos, Pickup, Dessert, CupShelf, Espresso, IceMaker, Water, DrinkRoom, ShakeRoom, TrayReturn, Dishwash, Towel }

        // 기기 배치: 이름, 안내 문구, 그림, 가운데 x, 아래쪽 y, 연결할 화면 (SFR-012). 크기는 그림 크기로 정해짐
        static readonly (string name, string action, string sprite, float x, float y, Screen screen)[] Stations =
        {
            // 뒤쪽 작업대 (위쪽 벽 앞)
            ("커피잔 선반", "열기", "StationCupShelf", 2.2f, 12, Screen.CupShelf),
            ("에스프레소 머신", "사용", "StationEspresso", 5, 12, Screen.Espresso),
            ("제빙기", "사용", "StationIceMaker", 7.6f, 12, Screen.IceMaker),
            ("정수기", "사용", "StationWater", 9.6f, 12, Screen.Water),
            ("음료 제조실", "열기", "StationDrinkBar", 12, 12, Screen.DrinkRoom),
            ("쉐이크 제조실", "열기", "StationShakeBar", 14.8f, 12, Screen.ShakeRoom),
            ("설거지실", "열기", "StationDishwash", 19.5f, 12, Screen.Dishwash),
            ("행주 걸이", "사용", "StationTowel", 22.6f, 12, Screen.Towel),
            // 앞쪽 카운터 위 (작업 공간 쪽에서 상호작용)
            ("포스기", "사용", "StationPos", 3, FrontCounterY, Screen.Pos),
            ("픽업대", "열기", "StationPickup", 8, FrontCounterY, Screen.Pickup),
            ("디저트 진열장", "열기", "StationDessertCase", 12.2f, FrontCounterY, Screen.Dessert),
            ("퇴식구", "열기", "StationTrayReturn", 20.5f, FrontCounterY, Screen.TrayReturn),
        };

        // 테이블 (테이블 수는 추후 결정, SFR-024). 아래쪽 줄의 y 와 손님이 걸어오는 가로 통로 y
        static readonly float[] TableXs = { 4f, 9f, 14f, 20f };
        static readonly (float y, float laneY)[] TableRows = { (5f, 6.6f), (2f, 4.25f) };

        [MenuItem("CafeTime/Build Cafe Scene")]
        public static void Build()
        {
            if (GameFont == null) { Debug.LogError("폰트 에셋이 없습니다. CafeTime > Setup Fonts 를 먼저 실행하세요."); return; }
            if (!ConfirmOverwrite(ScenePath, "Cafe 씬")) return;

            GameBalanceAssetBuilder.CreateOrUpdate();
            PrepareWorldSprites(MapArtDir, new[] { "Floor", "Wall", "Counter", "Door", "WorkDone", "WorkBubble", "CleanRequest" });
            PrepareWorldSprites(PlayerArtDir, null);
            PrepareWorldSprites(CustomerArtDir, null);
            var worldHud = new[] { "PromptUse", "PromptOpen", "GaugeFrame", "GaugeFill" }; // 맵 위에 그리는 HUD 그림
            PrepareWorldSprites(HudArt, worldHud, worldHud);
            PrepareSprites(ItemIconDir);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("Global Light 2D").AddComponent<Light2D>().lightType = Light2D.LightType.Global;

            var walls = BuildMap();
            var tables = BuildSeating();
            var managers = BuildManagers(tables);
            var ui = CafeUiBuilder.Build(managers);
            BuildStations(ui, managers.Business);
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

            var input = new SerializedObject(new GameObject("CafeSceneInput").AddComponent<CafeSceneInput>());
            Set(input, "pauseMenu", ui.Pause);
            input.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, ScenePath);
            if (EditorBuildSettings.scenes.All(s => s.path != ScenePath))
                EditorBuildSettings.scenes = EditorBuildSettings.scenes
                    .Append(new EditorBuildSettingsScene(ScenePath, true)).ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("Cafe 씬 생성 완료: " + ScenePath);
        }

        // ───────── 맵 (SFR-012) ─────────

        // 바닥 / 벽·카운터(충돌) 타일맵. 벽 타일맵이 맵 전체 범위를 덮으므로 카메라 경계로도 사용
        static Tilemap BuildMap()
        {
            var floor = LoadTile("Floor");
            var wall = LoadTile("Wall");
            var counter = LoadTile("Counter");
            var door = LoadTile("Door");

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

        static TileBase LoadTile(string name)
        {
            var path = $"{MapArtDir}/{name}Tile.asset";
            var tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile == null)
            {
                tile = ScriptableObject.CreateInstance<Tile>();
                AssetDatabase.CreateAsset(tile, path);
            }
            tile.sprite = LoadSprite($"{MapArtDir}/{name}.png");
            EditorUtility.SetDirty(tile);
            return tile;
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

        // 4인용 테이블 + 양옆 의자. 손님은 의자 2자리 + 테이블 뒤 2자리에 앉는다
        static CafeTable[] BuildSeating()
        {
            var root = new GameObject("Seating").transform;
            var tableSprite = LoadSprite($"{MapArtDir}/Table.png");
            var chairLeft = LoadSprite($"{MapArtDir}/ChairLeft.png");
            var chairRight = LoadSprite($"{MapArtDir}/ChairRight.png");
            var tables = new List<CafeTable>();
            foreach (var (y, laneY) in TableRows)
            foreach (float x in TableXs)
            {
                var table = WorldObject($"Table {tables.Count + 1}", root, tableSprite, x, y, new Vector2(1.8f, 0.7f));
                WorldObject("ChairLeft", table.transform, chairLeft, x - 1.3f, y, new Vector2(0.5f, 0.3f));
                WorldObject("ChairRight", table.transform, chairRight, x + 1.3f, y, new Vector2(0.5f, 0.3f));

                var seats = new[]
                {
                    Point("SeatLeft", table.transform, x - 1.3f, y - 0.02f),
                    Point("SeatRight", table.transform, x + 1.3f, y - 0.02f),
                    Point("SeatBackLeft", table.transform, x - 0.5f, y + 0.95f),
                    Point("SeatBackRight", table.transform, x + 0.5f, y + 0.95f),
                };
                var component = table.AddComponent<CafeTable>();
                var so = new SerializedObject(component);
                SetArray(so, "seats", seats.Cast<Object>().ToArray());
                so.FindProperty("laneY").floatValue = laneY;
                Set(so, "gaugePoint", Point("GaugePoint", table.transform, x, y - 0.3f)); // 테이블 앞쪽 (뒤쪽은 포스기 줄과 겹침)
                // 청소 (SFR-066): 두고 간 쟁반, 청소 요청 아이콘, 행주를 들고 E
                Set(so, "trayOnTable", Decoration("TrayOnTable", table.transform, LoadSprite($"{MapArtDir}/TrayOnTable.png"), x, y + 0.35f, 1));
                Set(so, "cleanIcon", Decoration("CleanRequest", table.transform, LoadSprite($"{MapArtDir}/CleanRequest.png"), x, y + 1.3f, 5));
                var cleanSpot = table.AddComponent<TableCleanSpot>();
                var cso = new SerializedObject(cleanSpot);
                Set(cso, "table", component);
                cso.ApplyModifiedPropertiesWithoutUndo();
                cleanSpot.enabled = false; // 청소 요청이 있을 때만
                Set(so, "cleanSpot", cleanSpot);
                so.ApplyModifiedPropertiesWithoutUndo();
                tables.Add(component);
            }
            return tables.ToArray();
        }

        // 충돌 없는 그림 (테이블 위 쟁반, 머리 위 아이콘). 처음엔 꺼 둔다
        static GameObject Decoration(string name, Transform parent, Sprite sprite, float x, float y, int order)
        {
            var sr = new GameObject(name).AddComponent<SpriteRenderer>();
            sr.transform.SetParent(parent, false);
            sr.transform.position = new Vector3(x, y, 0);
            sr.sprite = sprite;
            sr.sortingOrder = order;
            sr.gameObject.SetActive(false);
            return sr.gameObject;
        }

        static Transform Point(string name, Transform parent, float x, float y)
        {
            var t = new GameObject(name).transform;
            t.SetParent(parent, false);
            t.position = new Vector3(x, y, 0);
            return t;
        }

        // 아래쪽 가운데가 기준점인 그림 + 발밑 충돌 영역
        static GameObject WorldObject(string name, Transform parent, Sprite sprite, float x, float y, Vector2 collider)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.position = new Vector3(x, y, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.spriteSortPoint = SpriteSortPoint.Pivot; // 발밑 기준으로 앞뒤 정렬 (Renderer2D Y축 정렬)
            var box = go.AddComponent<BoxCollider2D>();
            box.size = collider;
            box.offset = new Vector2(0, collider.y / 2);
            return go;
        }

        // ───────── 영업 관리 오브젝트 ─────────

        public class Managers
        {
            public GameClock Clock;
            public CustomerManager Customers;
            public BusinessDay Day;
            public GameObject Business;
        }

        static Managers BuildManagers(CafeTable[] tables)
        {
            var go = new GameObject("Business");
            var managers = new Managers
            {
                Clock = go.AddComponent<GameClock>(),
                Customers = go.AddComponent<CustomerManager>(),
                Day = go.AddComponent<BusinessDay>(),
                Business = go,
            };
            go.AddComponent<OrderBoard>();
            go.AddComponent<HandInventory>();
            // 기기 상태 (기기 화면을 닫아도 유지, ADR-0014)
            go.AddComponent<CupShelf>();
            go.AddComponent<CafeTime.EspressoShot.EspressoMachine>();
            go.AddComponent<IceMaker>();
            go.AddComponent<WaterDispenser>();
            go.AddComponent<DrinkStation>();
            go.AddComponent<Blender>();
            go.AddComponent<TrayReturn>();
            go.AddComponent<DishStation>();
            var cleaning = new SerializedObject(go.AddComponent<CleaningManager>());
            SetArray(cleaning, "tables", tables.Cast<Object>().ToArray());
            cleaning.ApplyModifiedPropertiesWithoutUndo();

            // 물건 아이콘: ItemSprite 순서, 파일 이름 = 값 이름
            var icons = new SerializedObject(go.AddComponent<ItemIcons>());
            SetArray(icons, "sprites", System.Enum.GetNames(typeof(ItemSprite))
                .Select(n => (Object)LoadSprite($"{ItemIconDir}/{n}.png")).ToArray());
            icons.ApplyModifiedPropertiesWithoutUndo();

            var customers = new SerializedObject(managers.Customers);
            var looks = customers.FindProperty("looks");
            looks.arraySize = 4;
            for (int i = 0; i < 4; i++)
            {
                var look = looks.GetArrayElementAtIndex(i);
                foreach (var pose in new[] { "Down", "Up", "Left", "Right", "Sit" })
                    look.FindPropertyRelative(pose.ToLower()).objectReferenceValue = LoadSprite($"{CustomerArtDir}/Customer{i + 1}{pose}.png");
            }
            Set(customers, "gaugeFrame", LoadSprite($"{HudArt}/GaugeFrame.png"));
            Set(customers, "gaugeFill", LoadSprite($"{HudArt}/GaugeFill.png"));
            SetArray(customers, "tables", tables.Cast<Object>().ToArray());
            customers.ApplyModifiedPropertiesWithoutUndo();
            return managers;
        }

        // ───────── 기기 (SFR-012, 020) ─────────

        static void BuildStations(CafeUiBuilder.Screens ui, GameObject business)
        {
            var root = new GameObject("Stations").transform;
            foreach (var s in Stations)
            {
                var spritePath = $"{MapArtDir}/{s.sprite}.png";
                var sprite = LoadSprite(spritePath);
                var size = sprite != null ? (Vector2)sprite.bounds.size : new Vector2(1.5f, 1.5f);
                var go = WorldObject(s.name, root, sprite, s.x, s.y, new Vector2(size.x - 0.1f, Mathf.Min(size.y, 1f)));

                // 다가가면 켜지는 테두리 (SFR-013): 기기 그림 모양을 따라 만든 외곽선 그림
                var highlight = new GameObject("Highlight").AddComponent<SpriteRenderer>();
                highlight.transform.SetParent(go.transform, false);
                highlight.sprite = OutlineSprite(spritePath);
                highlight.spriteSortPoint = SpriteSortPoint.Pivot;
                highlight.sortingOrder = 1;

                var interactable = go.AddComponent<Interactable>();
                var so = new SerializedObject(interactable);
                so.FindProperty("actionLabel").stringValue = s.action;
                // 키 안내는 충돌 영역(발밑) 위가 아니라 그림 위쪽에 뜨도록
                so.FindProperty("promptHeight").floatValue = size.y - Mathf.Min(size.y, 1f) + 0.35f;
                Set(so, "highlight", highlight.gameObject);
                so.ApplyModifiedPropertiesWithoutUndo();
                Connect(interactable.OnInteract, s, ui, go, business);
                AddWorkStatus(s.screen, business, go, size.y);
                highlight.gameObject.SetActive(false);
            }
        }

        static void Connect(UnityEvent onInteract, (string name, string action, string sprite, float x, float y, Screen screen) s,
            CafeUiBuilder.Screens ui, GameObject station, GameObject business)
        {
            switch (s.screen)
            {
                case Screen.Pos:
                    var pos = station.AddComponent<PosStation>();
                    var so = new SerializedObject(pos);
                    Set(so, "speech", ui.Speech);
                    Set(so, "pos", ui.Pos);
                    so.ApplyModifiedPropertiesWithoutUndo();
                    UnityEventTools.AddVoidPersistentListener(onInteract, pos.Interact);
                    break;
                case Screen.Pickup:
                    UnityEventTools.AddVoidPersistentListener(onInteract, ui.Pickup.Open);
                    break;
                case Screen.Dessert:
                    UnityEventTools.AddVoidPersistentListener(onInteract, ui.Dessert.Open);
                    break;
                case Screen.CupShelf: Open(onInteract, ui.CupShelf); break;
                case Screen.Espresso: // 완료 아이콘 지우기(SFR-022)까지 EspressoMachine.Open 이 처리
                    UnityEventTools.AddVoidPersistentListener(onInteract, business.GetComponent<CafeTime.EspressoShot.EspressoMachine>().Open);
                    break;
                case Screen.IceMaker: Open(onInteract, ui.IceMaker); break;
                case Screen.Water: Open(onInteract, ui.Water); break;
                case Screen.DrinkRoom: Open(onInteract, ui.DrinkRoom); break;
                case Screen.ShakeRoom: Open(onInteract, ui.ShakeRoom); break;
                case Screen.TrayReturn: Open(onInteract, ui.TrayReturn); break;
                case Screen.Dishwash: Open(onInteract, ui.Dishwash); break;
                case Screen.Towel:
                    UnityEventTools.AddVoidPersistentListener(onInteract, station.AddComponent<TowelRack>().Interact);
                    break;
            }
        }

        static void Open(UnityEvent onInteract, CafeTime.UI.Popup screen) =>
            UnityEventTools.AddVoidPersistentListener(onInteract, screen.Open);

        // 작업 시간이 있는 기기 위 진행 게이지 / 완료 아이콘 (SFR-022)
        static void AddWorkStatus(Screen screen, GameObject business, GameObject station, float height)
        {
            MonoBehaviour source = screen switch
            {
                Screen.Espresso => business.GetComponent<CafeTime.EspressoShot.EspressoMachine>(),
                Screen.Water => business.GetComponent<WaterDispenser>(),
                Screen.ShakeRoom => business.GetComponent<Blender>(),
                Screen.Dishwash => business.GetComponent<DishStation>(),
                _ => null,
            };
            if (source == null) return;
            var go = new GameObject("WorkStatus");
            go.transform.SetParent(station.transform, false);
            go.transform.localPosition = new Vector3(0, height + 0.2f, 0);
            var done = Decoration("Done", go.transform, LoadSprite($"{MapArtDir}/WorkDone.png"), 0, 0, 20);
            done.transform.localPosition = new Vector3(0, 0.35f, 0);
            var status = new SerializedObject(go.AddComponent<WorkStatusIcon>());
            Set(status, "source", source);
            Set(status, "doneIcon", done.GetComponent<SpriteRenderer>());
            Set(status, "gaugeFrame", LoadSprite($"{HudArt}/GaugeFrame.png"));
            Set(status, "gaugeFill", LoadSprite($"{HudArt}/GaugeFill.png"));
            status.ApplyModifiedPropertiesWithoutUndo();
        }

        // 기기 그림의 투명하지 않은 부분을 몇 픽셀 넓혀 만든 노란 외곽선. 그림이 바뀌면 다시 만든다
        static Sprite OutlineSprite(string sourcePath)
        {
            if (!File.Exists(sourcePath)) return null;
            Directory.CreateDirectory(OutlineDir);
            var outPath = $"{OutlineDir}/{Path.GetFileNameWithoutExtension(sourcePath)}Outline.png";
            const int pad = 8;
            if (!File.Exists(outPath) || File.GetLastWriteTimeUtc(outPath) < File.GetLastWriteTimeUtc(sourcePath))
            {
                var source = new Texture2D(2, 2);
                source.LoadImage(File.ReadAllBytes(sourcePath));
                int w = source.width, h = source.height, W = w + pad * 2, H = h + pad * 2;
                var alpha = source.GetPixels32().Select(p => p.a > 40).ToArray();
                bool Solid(int x, int y) => x >= 0 && y >= 0 && x < w && y < h && alpha[y * w + x];
                var pixels = new Color32[W * H];
                var glow = new Color32(0xFF, 0xE2, 0x7A, 0xFF);
                for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    int sx = x - pad, sy = y - pad;
                    if (Solid(sx, sy)) continue;
                    bool near = false;
                    for (int dy = -6; dy <= 6 && !near; dy += 2)
                    for (int dx = -6; dx <= 6 && !near; dx += 2)
                        near = dx * dx + dy * dy <= 36 && Solid(sx + dx, sy + dy);
                    if (near) pixels[y * W + x] = glow;
                }
                var tex = new Texture2D(W, H, TextureFormat.RGBA32, false);
                tex.SetPixels32(pixels);
                File.WriteAllBytes(outPath, tex.EncodeToPNG());
                Object.DestroyImmediate(tex);
                Object.DestroyImmediate(source);
                AssetDatabase.ImportAsset(outPath);
                ConfigureWorldSprite(outPath, new Vector2(0.5f, (float)pad / H));
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(outPath);
        }

        // ───────── 플레이어 (SFR-015~021) ─────────

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
            sr.spriteSortPoint = SpriteSortPoint.Pivot;
            var movement = go.AddComponent<PlayerMovement>();

            var anim = new SerializedObject(go.AddComponent<PlayerAnimator>());
            Set(anim, "movement", movement);
            foreach (var dir in new[] { "down", "up", "left", "right" })
            {
                string Frame(int step) => $"{PlayerArtDir}/Player{char.ToUpper(dir[0]) + dir.Substring(1)}{step}.png";
                var idle = LoadSprite(Frame(0));
                Set(anim, dir + ".idle", idle);
                var walk = anim.FindProperty(dir + ".walk");
                var frames = new[] { LoadSprite(Frame(1)), idle, LoadSprite(Frame(2)), idle };
                walk.arraySize = frames.Length;
                for (int i = 0; i < frames.Length; i++)
                    walk.GetArrayElementAtIndex(i).objectReferenceValue = frames[i];
                if (dir == "down") sr.sprite = idle;
            }
            anim.ApplyModifiedPropertiesWithoutUndo();

            // 손에 든 물건 (SFR-021)
            var hands = new SerializedObject(go.AddComponent<HandDisplay>());
            Set(hands, "leftHand", HandRenderer(go.transform, "LeftHandItem", -0.32f));
            Set(hands, "rightHand", HandRenderer(go.transform, "RightHandItem", 0.32f));
            hands.ApplyModifiedPropertiesWithoutUndo();

            // 상호작용 키 안내 (오브젝트 위에 표시, 하나를 옮겨 가며 사용)
            var prompt = new GameObject("InteractPrompt").AddComponent<SpriteRenderer>();
            prompt.sortingOrder = 300;
            prompt.sprite = LoadSprite($"{HudArt}/PromptUse.png");
            prompt.gameObject.SetActive(false);

            var interactor = new SerializedObject(go.AddComponent<PlayerInteractor>());
            Set(interactor, "prompt", prompt);
            Set(interactor, "promptUse", LoadSprite($"{HudArt}/PromptUse.png"));
            Set(interactor, "promptOpen", LoadSprite($"{HudArt}/PromptOpen.png"));
            interactor.ApplyModifiedPropertiesWithoutUndo();
            return go.transform;
        }

        static SpriteRenderer HandRenderer(Transform player, string name, float x)
        {
            var sr = new GameObject(name).AddComponent<SpriteRenderer>();
            sr.transform.SetParent(player, false);
            sr.transform.localPosition = new Vector3(x, 0.55f, 0);
            sr.transform.localScale = Vector3.one * 0.15f; // 256px 아이콘 → 약 0.4칸
            sr.sortingOrder = 5;
            return sr;
        }

        // ───────── 맵 그림 임포트 (1칸 = 128px) ─────────

        // centered 에 있는 그림(타일 등)은 가운데, 나머지는 아래쪽 가운데 기준점. only 가 있으면 그 파일만
        static void PrepareWorldSprites(string folder, string[] centered, string[] only = null)
        {
            if (!Directory.Exists(folder)) return;
            foreach (var path in Directory.GetFiles(folder, "*.png").Select(p => p.Replace('\\', '/')))
            {
                var name = Path.GetFileNameWithoutExtension(path);
                if (only != null && !only.Contains(name)) continue;
                bool center = centered != null && centered.Contains(name);
                ConfigureWorldSprite(path, center ? new Vector2(0.5f, 0.5f) : new Vector2(0.5f, 0f));
            }
        }

        static void ConfigureWorldSprite(string path, Vector2 pivot)
        {
            if (!(AssetImporter.GetAtPath(path) is TextureImporter importer)) return;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = PixelsPerUnit;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.Custom;
            settings.spritePivot = pivot;
            settings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);
            importer.SaveAndReimport();
        }
    }
}
