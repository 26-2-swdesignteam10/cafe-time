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
    //   - 영업 루프: 시계, 손님, 주문 내역, 포스기, 픽업대, 디저트 진열장 (ADR-0013)
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

        // 맵 크기 (타일). 바깥 한 줄 + 위쪽 두 줄은 벽 (배치는 ADR-0016)
        const int MapWidth = 26;
        const int MapHeight = 15;
        const int FrontCounterY = 8;
        const int KitchenRightX = 10; // 주방과 홀 사이 칸막이 벽의 x
        const float BackWallY = 12;
        const float CounterTopY = 8.55f; // 카운터 위에 놓는 기기의 아래쪽 y (상판 위)

        static readonly Color DarkBrown = new Color32(0x3E, 0x27, 0x1A, 0xFF);

        enum Screen { Pos, Pickup, Dessert, CupShelf, Espresso, IceMaker, Water, DrinkRoom, ShakeRoom, TrayReturn, Dishwash, Towel }

        // 기기 배치: 이름, 그림, 가운데 x, 아래쪽 y, 연결할 화면 (SFR-012, ADR-0016). 크기는 그림 크기로 정해짐
        static readonly (string name, string sprite, float x, float y, Screen screen)[] Stations =
        {
            // 뒤쪽 벽 (왼쪽부터)
            ("설거지실", "StationDishwash", 1.75f, BackWallY, Screen.Dishwash),
            ("쉐이크 제조실", "StationShakeBar", 3.125f, BackWallY, Screen.ShakeRoom),
            ("제빙기", "StationIceMaker", 4.25f, BackWallY, Screen.IceMaker),
            ("커피잔 선반", "StationCupShelf", 5.25f, BackWallY, Screen.CupShelf),
            ("음료 제조실", "StationDrinkBar", 6.375f, BackWallY, Screen.DrinkRoom),
            ("정수기", "StationWater", 7.5f, BackWallY, Screen.Water),
            ("에스프레소 머신", "StationEspresso", 9, BackWallY, Screen.Espresso),
            // 앞쪽 카운터 위 (왼쪽부터). 맨 왼쪽 x1 칸은 주방 출입구
            ("행주 걸이", "StationTowel", 2.5f, CounterTopY, Screen.Towel),
            ("디저트 진열장", "StationDessertCase", 3.5f, CounterTopY, Screen.Dessert),
            ("포스기", "StationPos", 4.625f, CounterTopY, Screen.Pos),
            ("픽업대", "StationPickup", 7.625f, CounterTopY, Screen.Pickup),
            // 홀 왼쪽 벽을 따라 세로로 긴 카운터 (예전 받침대 + 퇴식구를 한 그림으로 합침, 2026-11-12)
            ("퇴식구", "StationTrayReturn", 1.5f, 3.2f, Screen.TrayReturn),
        };

        // 테이블 (테이블 수는 추후 결정, SFR-024). 아래쪽 줄의 y 와 손님이 걸어오는 가로 통로 y
        static readonly float[] TableXs = { 13f, 16.4f, 19.8f };
        static readonly (float y, float laneY)[] TableRows = { (8.5f, 10.1f), (5.2f, 6.8f), (1.9f, 3.7f) };

        [MenuItem("CafeTime/Build Cafe Scene")]
        public static void Build()
        {
            if (GameFont == null) { Debug.LogError("폰트 에셋이 없습니다. CafeTime > Setup Fonts 를 먼저 실행하세요."); return; }
            if (!ConfirmOverwrite(ScenePath, "Cafe 씬")) return;

            GameBalanceAssetBuilder.CreateOrUpdate();
            PrepareWorldSprites(MapArtDir, new[] { "Floor", "FloorKitchen", "Wall", "WallPlain", "WallKitchen", "WallTop", "Counter", "Door", "WorkDone", "WorkBubble", "CleanRequest" });
            PrepareWorldSprites(PlayerArtDir, null);
            PrepareWorldSprites(CustomerArtDir, null);
            var worldHud = new[] { "PromptKey", "GaugeFrame", "GaugeFill" }; // 맵 위에 그리는 HUD 그림
            PrepareWorldSprites(HudArt, worldHud, worldHud);
            SetSideBorder($"{HudArt}/GaugeFrame.png", 12); // 9-slice: 캡슐 양끝 반원 (높이 24)
            SetSideBorder($"{HudArt}/GaugeFill.png", 8);   // 높이 16
            PrepareSprites(ItemIconDir);
            PrepareCupIcons();

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("Global Light 2D").AddComponent<Light2D>().lightType = Light2D.LightType.Global;

            var walls = BuildMap();
            var tables = BuildSeating();
            BuildDecor();
            var managers = BuildManagers(tables);
            var ui = CafeUiBuilder.Build(managers);
            BuildStations(ui, managers.Business);
            var player = BuildPlayer();

            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 4; // 세로 8칸이 보임. 기기가 잘 보이도록 확대 (ADR-0016)
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
            var kitchenFloor = LoadTile("FloorKitchen");
            // 위쪽 벽은 정면이 보이는 2줄, 옆벽·아래벽·칸막이는 윗면만 보이는 어두운 띠
            var wall = LoadTile("Wall");
            var wallPlain = LoadTile("WallPlain");
            var wallKitchen = LoadTile("WallKitchen");
            var wallTop = LoadTile("WallTop");
            var counter = LoadTile("Counter");
            var door = LoadTile("Door");

            var grid = new GameObject("Grid", typeof(Grid));
            var floorMap = NewTilemap(grid.transform, "Floor", -20);
            var wallMap = NewTilemap(grid.transform, "Walls", -10);
            wallMap.gameObject.AddComponent<TilemapCollider2D>();

            // 6000.6.4f1에서 SetTile(위치, 타일) 한 칸씩 넣기가 반영되지 않아 SetTiles로 한 번에 넣음
            var floorCells = new List<Vector3Int>();
            var floorTiles = new List<TileBase>();
            var wallCells = new List<Vector3Int>();
            var wallTiles = new List<TileBase>();
            for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
            {
                var cell = new Vector3Int(x, y, 0);
                bool border = x == 0 || x == MapWidth - 1 || y == 0 || y >= MapHeight - 2;
                // 앞쪽 카운터: x2~9. x1 은 주방 출입구
                bool frontCounter = !border && y == FrontCounterY && x >= 2 && x < KitchenRightX;
                bool partition = !border && x == KitchenRightX && y >= FrontCounterY;
                if (border || frontCounter || partition)
                {
                    wallCells.Add(cell);
                    bool facade = y >= MapHeight - 2 && x > 0 && x < MapWidth - 1;
                    wallTiles.Add(frontCounter ? counter
                        : y == 0 && (x == 1 || x == 2) ? door
                        : !facade ? wallTop
                        : y == MapHeight - 1 ? wallPlain
                        : x < KitchenRightX ? wallKitchen : wall);
                }
                if (!border && !partition)
                {
                    floorCells.Add(cell);
                    floorTiles.Add(x < KitchenRightX && y > FrontCounterY ? kitchenFloor : floor);
                }
            }
            floorMap.SetTiles(floorCells.ToArray(), floorTiles.ToArray());
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
                // 의자는 충돌 없음 (ADR-0025): 충돌이 있으면 옆 테이블 의자와의 틈이 0.3칸뿐이라 세로로 지나갈 수 없다
                WorldObject("ChairLeft", table.transform, chairLeft, x - 1.3f, y, Vector2.zero);
                WorldObject("ChairRight", table.transform, chairRight, x + 1.3f, y, Vector2.zero);

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

        // 기능 없는 가구 (ADR-0016): 창문, 빵 진열대, 화분 칸막이, 창가 바, 스툴, 부스 (퇴식구 받침대는 2026-11-12 퇴식구 그림에 합침)
        static void BuildDecor()
        {
            var root = new GameObject("Decor").transform;
            for (int i = 0; i < 6; i++)
                Decor("창문", root, "WindowHall", 12.75f + i * 2.1f, 13.4f, Vector2.one);
            Decor("빵 진열대 왼쪽", root, "BreadShelf", 4.3f, 1.5f, new Vector2(1.9f, 5.06f));
            Decor("빵 진열대 오른쪽", root, "BreadShelf", 7.7f, 1.5f, new Vector2(1.9f, 5.06f));
            foreach (float y in new[] { 1f, 2f, 4.4f, 5f, 6f, 7f }) // 사이 y3~4.4 는 좌석으로 가는 통로
                Decor("화분 칸막이", root, "PlanterSegment", 10.5f, y, Vector2.one);
            for (int x = 11; x < MapWidth - 1; x++)
                Decor("창가 바", root, "BarSegment", x + 0.5f, 12, Vector2.one);
            for (int i = 0; i < 10; i++)
                Decor($"스툴 {i + 1}", root, "Stool", 11.8f + i * 1.03f, 11.1f, new Vector2(0.6f, 0.6f));
            foreach (float y in new[] { 2f, 4.6f, 7.2f, 9.8f })
                Decor("부스", root, "Booth", 23.6f, y, new Vector2(2.56f, 1.6f));
        }

        static void Decor(string name, Transform parent, string sprite, float x, float y, Vector2 collider)
        {
            var go = WorldObject(name, parent, LoadSprite($"{MapArtDir}/{sprite}.png"), x, y, collider);
            go.GetComponent<SpriteRenderer>().sortingOrder = -1; // 캐릭터와 위에 올라가는 기기보다 항상 뒤
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

        // 아래쪽 가운데가 기준점인 그림 + 발밑 충돌 영역 (collider 가 0이면 충돌 없음)
        static GameObject WorldObject(string name, Transform parent, Sprite sprite, float x, float y, Vector2 collider)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.position = new Vector3(x, y, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.spriteSortPoint = SpriteSortPoint.Pivot; // 발밑 기준으로 앞뒤 정렬 (Renderer2D Y축 정렬)
            if (collider == Vector2.zero) return go;
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
                // 카운터 위 기기는 충돌 영역을 상판 안으로 줄여 작업 공간을 막지 않게 함
                float colliderHeight = s.y == CounterTopY ? FrontCounterY + 1 - CounterTopY : Mathf.Min(size.y, 1f);
                float colliderWidth = size.x - 0.1f;
                bool wallCounter = s.screen == Screen.TrayReturn;
                if (wallCounter)
                {
                    // 퇴식구는 벽을 따라 긴 카운터 전체가 막힌다. 폭은 예전 받침대(0.75)와 같게 해 앞 통로 폭을 유지 (ADR-0025)
                    colliderWidth = 0.75f;
                    colliderHeight = size.y - 0.2f;
                }
                var go = WorldObject(s.name, root, sprite, s.x, s.y, new Vector2(colliderWidth, colliderHeight));

                // 다가가면 켜지는 테두리 (SFR-013): 기기 그림 모양을 따라 만든 외곽선 그림. 기기와 같은 앞뒤 정렬이라 앞에 선 플레이어를 가리지 않음
                var highlight = new GameObject("Highlight").AddComponent<SpriteRenderer>();
                highlight.transform.SetParent(go.transform, false);
                highlight.sprite = OutlineSprite(spritePath);
                highlight.spriteSortPoint = SpriteSortPoint.Pivot;
                if (wallCounter)
                {
                    // 벽에 붙은 긴 가구라 예전 받침대처럼 캐릭터보다 항상 뒤에 그린다
                    go.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    highlight.sortingOrder = -1;
                    AddTrayReturnView(go);
                }

                var interactable = go.AddComponent<Interactable>();
                var so = new SerializedObject(interactable);
                // 키 안내는 충돌 영역(발밑) 위가 아니라 그림 위쪽에 뜨도록
                so.FindProperty("promptHeight").floatValue = size.y - colliderHeight + 0.35f;
                Set(so, "highlight", highlight.gameObject);
                so.ApplyModifiedPropertiesWithoutUndo();
                Connect(interactable.OnInteract, s, ui, go, business);
                AddWorkStatus(s.screen, business, go, size.y);
                highlight.gameObject.SetActive(false);
            }
        }

        // 반납된 쟁반 수(0~4)에 따라 퇴식구 그림을 바꾼다 (StationTrayReturn.png, StationTrayReturn1~4.png)
        static void AddTrayReturnView(GameObject station)
        {
            var view = new SerializedObject(station.AddComponent<TrayReturnView>());
            Set(view, "target", station.GetComponent<SpriteRenderer>());
            SetArray(view, "byCount", Enumerable.Range(0, 5)
                .Select(n => (Object)LoadSprite($"{MapArtDir}/StationTrayReturn{(n == 0 ? "" : n.ToString())}.png")).ToArray());
            view.ApplyModifiedPropertiesWithoutUndo();
        }

        static void Connect(UnityEvent onInteract, (string name, string sprite, float x, float y, Screen screen) s,
            CafeUiBuilder.Screens ui, GameObject station, GameObject business)
        {
            switch (s.screen)
            {
                case Screen.Pos:
                    var pos = station.AddComponent<PosStation>();
                    var so = new SerializedObject(pos);
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
            go.transform.position = new Vector3(5.5f, 10.5f, 0);
            var body = go.AddComponent<Rigidbody2D>();
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            // 발밑만 충돌 영역 → 기기 앞까지 자연스럽게 다가감
            // 모서리를 둥글게(edgeRadius) 해서 이어 붙은 벽 타일·화분 사이 이음새에 걸리지 않게 함. 바깥 크기는 0.6x0.3 그대로
            var feet = go.AddComponent<BoxCollider2D>();
            feet.size = new Vector2(0.5f, 0.2f);
            feet.edgeRadius = 0.05f;
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

            // 상호작용 키 안내 (오브젝트 위에 표시, 하나를 옮겨 가며 사용)
            var prompt = new GameObject("InteractPrompt").AddComponent<SpriteRenderer>();
            prompt.sortingOrder = 300;
            prompt.sprite = LoadSprite($"{HudArt}/PromptKey.png");
            prompt.gameObject.SetActive(false);

            var interactor = new SerializedObject(go.AddComponent<PlayerInteractor>());
            Set(interactor, "prompt", prompt);
            interactor.ApplyModifiedPropertiesWithoutUndo();
            return go.transform;
        }

        // 커피잔 아이콘은 도트가 아닌 일러스트라 Bilinear (docs/ART_STYLE.md 1장)
        static void PrepareCupIcons()
        {
            foreach (var path in Directory.GetFiles(ItemIconDir, "*.png").Select(p => p.Replace('\\', '/')))
            {
                string name = Path.GetFileNameWithoutExtension(path);
                if (!name.StartsWith("Cup") && name != "DirtyCeramic" && name != "DirtyPlastic") continue;
                if (!(AssetImporter.GetAtPath(path) is TextureImporter importer)) continue;
                importer.filterMode = FilterMode.Bilinear;
                importer.SaveAndReimport();
            }
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

        // 왼쪽·오른쪽만 늘어나지 않는 9-slice 테두리 (가로로 늘려 쓰는 게이지)
        static void SetSideBorder(string path, float side)
        {
            if (!(AssetImporter.GetAtPath(path) is TextureImporter importer)) return;
            importer.spriteBorder = new Vector4(side, 0, side, 0);
            importer.SaveAndReimport();
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
