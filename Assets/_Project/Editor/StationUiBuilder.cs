using CafeTime.Cafe;
using CafeTime.EspressoShot;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static CafeTime.EditorTools.BuilderUtil;
using static CafeTime.EditorTools.CafeUiBuilder;

namespace CafeTime.EditorTools
{
    // Cafe 씬의 기기 화면 (SFR-028~066)과 일시정지 메뉴 (SFR-023). CafeUiBuilder 가 호출한다
    // 좌표는 각 Panel.png 의 표시 크기 기준, 창 왼쪽 위 기준 (그림 규격은 Art/UI/<기기>/Generation.md)
    public static class StationUiBuilder
    {
        const string Art = "Assets/_Project/Art/UI";
        const string SharedPrefabs = "Assets/_Project/Prefabs/UI";

        static readonly Color Spot = new Color(0, 0, 0, 0.12f);   // 그림이 없는 놓는 자리
        static readonly Color Invisible = new Color(1, 1, 1, 0);  // 그림 위에 겹친 놓는 자리 (클릭·드롭만 받음)

        public static readonly string[] Folders =
            { "CupShelf", "IceMaker", "Water", "DrinkRoom", "ShakeRoom", "TrayReturn", "Dishwash", "Pause", "ShotRoom" };
        // EspressoMachine 폴더(팀원 그림)는 임포트 설정을 바꾸지 않는다: ShotTest 씬이 Multiple 모드의 하위 스프라이트(tamper_0 등)를 참조함

        static Sprite S(string folder, string name) => LoadSprite($"{Art}/{folder}/{name}.png");

        public static void PrepareAll()
        {
            foreach (var folder in Folders) PrepareSprites($"{Art}/{folder}");
        }

        // ───────── 공통 ─────────

        static DropZone Zone(Transform parent, string name, float x, float y, float w, float h, Sprite sprite, Color fallback)
        {
            var rt = TopLeft(name, parent, x, y, w, h);
            AddImage(rt, sprite, fallback, true);
            return rt.gameObject.AddComponent<DropZone>();
        }

        static Image Icon(Transform parent, string name, float x, float y, float size)
        {
            var image = AddImage(TopLeft(name, parent, x, y, size, size), null, Color.clear);
            image.preserveAspect = true;
            image.enabled = false; // 물건이 없으면 숨김 (화면이 Refresh 에서 켬)
            return image;
        }

        // 진행 게이지 (GaugeFrame 안에 Filled GaugeFill)
        static Image Progress(Transform parent, string name, float x, float y, float w)
        {
            var frame = TopLeft(name, parent, x, y, w, 18);
            AddImage(frame, LoadSprite("Assets/_Project/Art/UI/CafeHud/GaugeFrame.png"), new Color32(0x3E, 0x27, 0x1A, 0xFF));
            var fill = NewUI("Fill", frame);
            Stretch(fill);
            fill.offsetMin = new Vector2(3, 3);
            fill.offsetMax = new Vector2(-3, -3);
            var image = AddImage(fill, LoadSprite("Assets/_Project/Art/UI/CafeHud/GaugeFill.png"), new Color32(0x7C, 0xC4, 0x5A, 0xFF));
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillAmount = 0;
            frame.gameObject.SetActive(false);
            return image;
        }

        static Button Btn(Transform parent, string name, Sprite sprite, float x, float y, float w, float h) =>
            AddButton(TopLeft(name, parent, x, y, w, h), sprite, Cream);

        static DragItem Draggable(Transform parent, string name, Sprite sprite, float w, float h)
        {
            var rt = Centered(name, parent, 0, 0, w, h);
            var image = AddImage(rt, sprite, Color.clear, true);
            image.preserveAspect = true;
            var item = rt.gameObject.AddComponent<DragItem>();
            var so = new SerializedObject(item);
            Set(so, "image", image);
            so.ApplyModifiedPropertiesWithoutUndo();
            return item;
        }

        static DragMotion Tool(Transform parent, string name, Sprite sprite, float x, float y, float w, float h, Vector2 range)
        {
            var rt = TopLeft(name, parent, x, y, w, h);
            AddImage(rt, sprite, Cream, true).preserveAspect = true;
            var motion = rt.gameObject.AddComponent<DragMotion>();
            var so = new SerializedObject(motion);
            so.FindProperty("followRange").vector2Value = range;
            so.ApplyModifiedPropertiesWithoutUndo();
            return motion;
        }

        static T Screen<T>(Transform canvas, string folder, string name, float w, float h, out RectTransform window) where T : Popup
        {
            var (popup, win) = Window<T>(canvas, name, S(folder, "Panel"), w, h);
            window = win;
            SetClose(popup, CloseButton(window, w - 100, 36));
            return popup;
        }

        // ───────── 커피잔 선반 (SFR-028) ─────────

        public static CupShelfScreen BuildCupShelf(Transform canvas)
        {
            var popup = Screen<CupShelfScreen>(canvas, "CupShelf", "CupShelfScreen", 900, 560, out var window);
            var so = new SerializedObject(popup);
            var ceramic = Btn(window, "Ceramic", S("CupShelf", "CeramicButton"), 130, 170, 300, 300);
            var plastic = Btn(window, "Plastic", S("CupShelf", "PlasticButton"), 470, 170, 300, 300);
            Set(so, "ceramicButton", ceramic);
            Set(so, "plasticButton", plastic);
            Set(so, "ceramicCount", AddText(TopLeft("Count", ceramic.transform, 0, 250, 300, 46), "0개", 26, DarkCoffee));
            Set(so, "plasticCount", AddText(TopLeft("Count", plastic.transform, 0, 250, 300, 46), "0개", 26, DarkCoffee));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 에스프레소 머신 (SFR-029~036) ─────────
        // 팀원의 샷 내리기 스크립트(Scripts/EspressoShot) 위에 새 화면을 만든다 (ADR-0015)
        // 좌표는 창 가운데 기준이며 Panel.png 에 그려진 자리와 맞춘다 (Art/UI/ShotRoom/Generation.md)

        public static Popup BuildEspresso(Transform canvas, EspressoMachine machine)
        {
            const string F = "ShotRoom";
            const float W = 1536, H = 864;
            var lcdColor = new Color32(0xE4, 0xF1, 0xE0, 0xFF); // 어두운 LCD 위 글자

            var (popup, window) = Window<Popup>(canvas, "EspressoScreen", S(F, "Panel"), W, H);
            SetClose(popup, CloseButton(window, W - 100, 36));

            PortafilterSlot Slot(string name, SlotType type, float x, float y)
            {
                var rt = Centered(name, window, x, y, 184, 72);
                AddImage(rt, null, Invisible, true); // 드롭 인식용
                var slot = rt.gameObject.AddComponent<PortafilterSlot>();
                slot.type = type;
                return slot;
            }

            void StepLabel(string name, float x, float y) => Label(window, name, S(F, name), W / 2 + x, H / 2 - y, PivotCenter);

            StepLabel("LabelGrind", -570, -153);
            StepLabel("LabelTamp", -280, -153);
            StepLabel("LabelExtract", 150, -153);
            StepLabel("LabelKnock", 600, -153);
            StepLabel("LabelRack", 145, -360);

            var grinderSlot = Slot("Slot_Grinder", SlotType.Grinder, -570, -30);
            var tampSlot = Slot("Slot_Tamp", SlotType.Tamp, -280, -30);
            Slot("Slot_Knockbox", SlotType.Knockbox, 620, -40);
            var racks = new[] { Slot("Slot_Rack1", SlotType.Rack, 0, -270), Slot("Slot_Rack2", SlotType.Rack, 290, -270) };
            var heads = new[] { Slot("Slot_GroupL", SlotType.GroupHead, -20, 90), Slot("Slot_GroupR", SlotType.GroupHead, 320, 90) };

            // 포터필터 2개 (받침대에서 시작)
            var states = new[] { S(F, "PortafilterEmpty"), S(F, "PortafilterGround"), S(F, "PortafilterTamped"), S(F, "PortafilterUsed") };
            for (int i = 0; i < 2; i++)
            {
                var rt = Centered($"Portafilter{i + 1}", racks[i].transform, 0, 0, 184, 72);
                AddImage(rt, states[0], Cream, true).preserveAspect = true;
                rt.gameObject.AddComponent<CanvasGroup>();
                var pf = rt.gameObject.AddComponent<Portafilter>();
                pf.stateSprites = states;
                racks[i].startingPortafilter = pf;
            }

            // 탬퍼: 아래로 끌어 누름 (SFR-032)
            var tamperRt = Centered("Tamper", window, -280, 130, 92, 138);
            AddImage(tamperRt, S(F, "Tamper"), Cream, true).preserveAspect = true;
            tamperRt.gameObject.AddComponent<TamperHandle>().tampSlot = tampSlot;

            // 그라인더 (SFR-031)
            var grinder = NewUI("Grinder", window).gameObject.AddComponent<Grinder>();
            grinder.slot = grinderSlot;
            var grind = AddButton(Centered("GrindButton", window, -570, 70, 170, 60), S(F, "GrindButton"), Cream);
            UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grind.onClick, grinder.OnClickGrind);
            var grinderLcd = AddText(Centered("LCD_Grinder", window, -570, 150, 190, 60), "", 24, lcdColor);
            var gd = grinderLcd.gameObject.AddComponent<GrinderDisplay>();
            gd.grinder = grinder;
            gd.label = grinderLcd;

            // 그룹헤드별 추출 버튼·LCD (SFR-035) + 아래 샷 컵 자리 (SFR-034)
            for (int g = 0; g < 2; g++)
            {
                float x = g == 0 ? -20 : 320;
                var extract = AddButton(Centered(g == 0 ? "ExtractButton_L" : "ExtractButton_R", window, x, 225, 140, 56), S(F, "ExtractButton"), Cream);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(extract.onClick, heads[g].OnClickExtract);
                var lcd = AddText(Centered(g == 0 ? "LCD_ExtractL" : "LCD_ExtractR", window, x, 300, 130, 56), "", 20, lcdColor);
                var ed = lcd.gameObject.AddComponent<ExtractDisplay>();
                ed.head = heads[g];
                ed.label = lcd;

                var cupRt = Centered(g == 0 ? "Cup_L" : "Cup_R", window, x, -40, 130, 130);
                AddImage(cupRt, null, Invisible, true); // 클릭·드롭 인식용
                var icon = AddImage(Centered("Glass", cupRt, 0, 0, 100, 110), null, Color.clear);
                icon.preserveAspect = true;
                var cup = cupRt.gameObject.AddComponent<ShotCupSlot>();
                cup.head = heads[g];
                cup.glassIcon = icon;
                heads[g].cup = cup;
            }

            machine.screenPanel = popup.gameObject;
            EditorUtility.SetDirty(machine);
            return popup;
        }

        // ───────── 제빙기 (SFR-037~040) ─────────

        public static IceMakerScreen BuildIceMaker(Transform canvas)
        {
            const string F = "IceMaker";
            var popup = Screen<IceMakerScreen>(canvas, F, "IceMakerScreen", 1100, 700, out var window);
            var so = new SerializedObject(popup);
            Label(window, "Hint", S(F, "Hint"), 550, 135, PivotCenter);
            Set(so, "bin", Zone(window, "IceBin", 90, 190, 520, 340, S(F, "IceBin"), Cream));
            Label(window, "LabelIce", S(F, "LabelIce"), 110, 580, PivotLeft);
            Set(so, "iceText", AddText(TopLeft("Ice", window, 220, 556, 260, 48), "0 / 0", 32, DarkCoffee, TextAlignmentOptions.Left));
            Label(window, "LabelSpeed", S(F, "LabelSpeed"), 110, 635, PivotLeft);
            Set(so, "speedText", AddText(TopLeft("Speed", window, 260, 611, 300, 48), "", 28, TextBrown, TextAlignmentOptions.Left));

            var spot = Zone(window, "ScoopSpot", 760, 260, 200, 170, S(F, "ScoopSpot"), Cream);
            Label(window, "LabelScoop", S(F, "LabelScoop"), 860, 455, PivotCenter);
            Set(so, "scoopSpot", spot);
            Set(so, "scoop", Draggable(spot.transform, "Scoop", S(F, "ScoopEmpty"), 170, 110));
            Set(so, "scoopEmpty", S(F, "ScoopEmpty"));
            Set(so, "scoopIce", S(F, "ScoopIce"));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 정수기 (SFR-041~043) ─────────

        public static WaterScreen BuildWater(Transform canvas)
        {
            const string F = "Water";
            var popup = Screen<WaterScreen>(canvas, F, "WaterScreen", 1100, 700, out var window);
            var so = new SerializedObject(popup);
            Label(window, "Hint", S(F, "Hint"), 550, 130, PivotCenter);
            var zones = new Object[2];
            var icons = new Object[2];
            var buttons = new Object[2];
            var bars = new Object[2];
            for (int i = 0; i < 2; i++)
            {
                float x = i == 0 ? 130 : 590;
                var zone = Zone(window, i == 0 ? "HotZone" : "ColdZone", x, 160, 380, 380, S(F, i == 0 ? "HotZone" : "ColdZone"), Cream);
                icons[i] = Icon(zone.transform, "Cup", 126, 202, 128); // 받침 위 (y 330)
                zones[i] = zone;
                Label(window, i == 0 ? "LabelHot" : "LabelCold", S(F, i == 0 ? "LabelHot" : "LabelCold"), x + 190, 562, PivotCenter);
                buttons[i] = Btn(window, i == 0 ? "HotButton" : "ColdButton", S(F, "WaterButton"), x + 80, 590, 220, 72);
                bars[i] = Progress(window, i == 0 ? "HotProgress" : "ColdProgress", x + 80, 670, 220);
            }
            SetArray(so, "zones", zones);
            SetArray(so, "cupIcons", icons);
            SetArray(so, "fillButtons", buttons);
            SetArray(so, "progress", bars);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 음료 제조실 (SFR-044~051) ─────────

        static IngredientSlotView SlotTemplate(Transform parent, string folder)
        {
            var rt = NewUI("Ingredient", parent);
            rt.sizeDelta = new Vector2(120, 120);
            var button = AddButton(rt, S(folder, "ShelfSlot"), Cream);
            var hover = S(folder, "ShelfSlotHover");
            if (hover != null)
            {
                button.transition = Selectable.Transition.SpriteSwap;
                var swap = button.spriteState;
                swap.highlightedSprite = swap.pressedSprite = hover;
                button.spriteState = swap;
            }
            var icon = AddImage(Centered("Icon", rt, 0, 0, 96, 96), null, Color.clear);
            icon.preserveAspect = true;
            var hold = rt.gameObject.AddComponent<HoldButton>();
            var view = rt.gameObject.AddComponent<IngredientSlotView>();
            var so = new SerializedObject(view);
            Set(so, "button", button);
            Set(so, "hold", hold);
            Set(so, "icon", icon);
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        static IngredientTooltip Tooltip(RectTransform window)
        {
            var rt = Centered("Tooltip", window, 0, 0, 340, 96);
            AddImage(rt, S("DrinkRoom", "Tooltip"), new Color32(0x3E, 0x27, 0x1A, 0xF0));
            var tooltip = rt.gameObject.AddComponent<IngredientTooltip>();
            var so = new SerializedObject(tooltip);
            Set(so, "text", StretchText(rt, "Text", "", 26, Cream));
            so.ApplyModifiedPropertiesWithoutUndo();
            return tooltip;
        }

        static JudgeResultView Result(RectTransform window)
        {
            var rt = Centered("Result", window, 0, 0, 640, 380);
            AddImage(rt, S("DrinkRoom", "ResultPanel"), Cream, true);
            var view = rt.gameObject.AddComponent<JudgeResultView>();
            var so = new SerializedObject(view);
            Set(so, "menuText", AddText(TopLeft("Menu", rt, 40, 130, 560, 60), "", 36, DarkCoffee));
            var grade = AddImage(Centered("Grade", rt, 0, -80, 300, 80), null, Color.clear);
            grade.preserveAspect = true;
            Set(so, "gradeImage", grade);
            SetArray(so, "grades", new Object[]
            {
                S("DrinkRoom", "GradePerfect"), S("DrinkRoom", "GradeGood"), S("DrinkRoom", "GradeBad"), S("DrinkRoom", "GradeFailed"),
            });
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        public static DrinkRoomScreen BuildDrinkRoom(Transform canvas)
        {
            const string F = "DrinkRoom";
            var popup = Screen<DrinkRoomScreen>(canvas, F, "DrinkRoomScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);

            // 재료 선반: 선반 판 윗면 y = 150, 310, 470, 630 → 칸 120 + 줄 간격 40
            var shelf = TopLeft("Shelf", window, 70, 160, 920, 660);
            AddImage(shelf, S(F, "Shelf"), new Color32(0x6B, 0x45, 0x2A, 0xFF));
            var grid = shelf.gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(120, 120);
            grid.spacing = new Vector2(20, 40);
            grid.padding = new RectOffset(50, 50, 30, 0);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 6;
            Set(so, "shelf", shelf);
            Set(so, "slotTemplate", SlotTemplate(shelf, F));
            Label(window, "PourHint", S(F, "PourHint"), 530, 850, PivotCenter);

            // 전자저울: 판 윗면 y = 40 (저울 그림 기준), LCD x 90~270, y 120~180
            var scale = Zone(window, "Scale", 1100, 300, 360, 340, null, Invisible);
            AddImage(TopLeft("ScaleImage", scale.transform, 0, 130, 360, 210), S(F, "Scale"), Cream);
            Set(so, "scale", scale);
            Set(so, "cupIcon", Icon(scale.transform, "Cup", 116, 42, 128));
            Set(so, "weightText", AddText(TopLeft("Weight", scale.transform, 90, 250, 180, 60), "0g", 34,
                new Color32(0xB8, 0xF5, 0x9A, 0xFF)));

            Set(so, "emptyButton", Btn(window, "EmptyButton", S(F, "EmptyButton"), 1070, 700, 220, 80));
            Set(so, "completeButton", Btn(window, "CompleteButton", S(F, "CompleteButton"), 1310, 695, 260, 90));
            Set(so, "result", Result(window));
            Set(so, "tooltip", Tooltip(window));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 쉐이크 제조실 (SFR-052~055) ─────────

        public static ShakeRoomScreen BuildShakeRoom(Transform canvas)
        {
            const string F = "ShakeRoom";
            var popup = Screen<ShakeRoomScreen>(canvas, F, "ShakeRoomScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);
            Label(window, "Hint", S(F, "Hint"), 800, 135, PivotCenter);

            Label(window, "LabelBase", S(F, "LabelBase"), 110, 210, PivotLeft);
            var shelf = TopLeft("Bases", window, 100, 240, 640, 130);
            var row = shelf.gameObject.AddComponent<HorizontalLayoutGroup>();
            row.spacing = 16;
            row.childControlWidth = row.childControlHeight = false;
            row.childForceExpandWidth = row.childForceExpandHeight = false;
            Set(so, "shelf", shelf);
            Set(so, "slotTemplate", SlotTemplate(shelf, "DrinkRoom"));
            Label(window, "LabelMilk", S(F, "LabelMilk"), 110, 420, PivotLeft);
            var milk = SlotTemplate(window, "DrinkRoom");
            var milkRt = (RectTransform)milk.transform;
            milkRt.anchorMin = milkRt.anchorMax = milkRt.pivot = new Vector2(0, 1);
            milkRt.anchoredPosition = new Vector2(100, -450);
            Set(so, "milkSlot", milk);

            // 믹서기 (가운데)
            var blender = Zone(window, "Blender", 800, 190, 280, 420, S(F, "BlenderEmpty"), Cream);
            Set(so, "blenderZone", blender);
            Set(so, "blenderImage", blender.GetComponent<Image>());
            SetArray(so, "blenderSprites", new Object[] { S(F, "BlenderEmpty"), S(F, "BlenderFilled"), S(F, "BlenderBlended") });
            Set(so, "weightText", AddText(TopLeft("Weight", window, 800, 620, 280, 50), "0g", 32, DarkCoffee));
            Set(so, "blendButton", Btn(window, "BlendButton", S(F, "BlendButton"), 830, 680, 220, 80));
            Set(so, "blendProgress", Progress(window, "BlendProgress", 830, 770, 220));

            // 커피잔 자리 (오른쪽)
            var cup = Zone(window, "CupSpot", 1180, 400, 260, 220, null, Invisible);
            AddImage(TopLeft("Coaster", cup.transform, 20, 150, 220, 70), S(F, "CupSpot"), Spot);
            Set(so, "cupSpot", cup);
            Set(so, "cupIcon", Icon(cup.transform, "Cup", 66, 30, 128));
            Label(window, "LabelCupSpot", S(F, "LabelCupSpot"), 1310, 645, PivotCenter);
            Set(so, "pourButton", Btn(window, "PourButton", S(F, "PourButton"), 1200, 680, 220, 80));

            Set(so, "result", Result(window));
            Set(so, "tooltip", Tooltip(window));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 퇴식구 (SFR-059~062) ─────────

        public static TrayReturnScreen BuildTrayReturn(Transform canvas, int slots)
        {
            const string F = "TrayReturn";
            var popup = Screen<TrayReturnScreen>(canvas, F, "TrayReturnScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);
            var trays = new Object[slots];
            var containers = new Object[slots];
            DragItem template = null;
            for (int i = 0; i < slots; i++)
            {
                var slot = TopLeft($"TraySlot {i + 1}", window, 80 + (i % 2) * 360, 160 + (i / 2) * 270, 330, 240);
                AddImage(slot, S(F, "TraySlot"), Spot);
                var tray = Centered("Tray", slot, 0, 0, 300, 210);
                AddImage(tray, S(F, "Tray"), new Color32(0x8B, 0x5A, 0x3C, 0xFF));
                var items = Centered("Items", tray, 0, 0, 270, 180);
                var grid = items.gameObject.AddComponent<GridLayoutGroup>();
                grid.cellSize = new Vector2(80, 80);
                grid.spacing = new Vector2(8, 8);
                grid.childAlignment = TextAnchor.MiddleCenter;
                grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                grid.constraintCount = 3;
                trays[i] = tray.gameObject;
                containers[i] = items;
                if (template == null) template = Draggable(items, "Item", null, 80, 80);
            }
            SetArray(so, "trays", trays);
            SetArray(so, "itemContainers", containers);
            Set(so, "itemTemplate", template);

            Set(so, "trashBin", Zone(window, "TrashBin", 880, 300, 260, 300, S(F, "TrashBin"), Cream));
            Label(window, "LabelTrash", S(F, "LabelTrash"), 1010, 630, PivotCenter);
            Set(so, "dishBin", Zone(window, "DishBin", 1200, 340, 320, 260, S(F, "DishBin"), Cream));
            Label(window, "LabelDishBin", S(F, "LabelDishBin"), 1360, 630, PivotCenter);
            Label(window, "Hint", S(F, "Hint"), 800, 840, PivotCenter);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 설거지실 (SFR-063~065) ─────────

        public static DishwashScreen BuildDishwash(Transform canvas)
        {
            const string F = "Dishwash";
            var popup = Screen<DishwashScreen>(canvas, F, "DishwashScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);
            Label(window, "Hint", S(F, "Hint"), 760, 600, PivotCenter);

            AddImage(TopLeft("WaitingCrate", window, 70, 200, 300, 240), S(F, "WaitingCrate"), Cream);
            Label(window, "LabelWaiting", S(F, "LabelWaiting"), 220, 470, PivotCenter);
            Set(so, "waitingText", AddText(TopLeft("Waiting", window, 120, 495, 200, 48), "0개", 32, DarkCoffee));

            // 싱크대: 가운데에 잔 1개, 그 위에 얼룩. 수세미는 좌우로만 조금 따라 움직임
            var sink = TopLeft("Sink", window, 420, 180, 640, 380);
            AddImage(sink, S(F, "Sink"), new Color32(0xB8, 0xC0, 0xC4, 0xFF));
            var item = Icon(sink, "Item", 240, 120, 160);
            Set(so, "sinkItem", item);
            var stain = AddImage(Centered("Stain", item.transform, 0, 0, 160, 160), S(F, "Stain"), new Color32(0x6B, 0x45, 0x2A, 0xB0));
            stain.enabled = false;
            Set(so, "stain", stain);
            Set(so, "sponge", Tool(sink, "Sponge", S(F, "Sponge"), 245, 250, 150, 100, new Vector2(160, 20)));

            // 건조대: 칸 중심 x = 60, 148, ... 500, 잔 바닥 y = 230 (건조대 그림 기준)
            var rack = TopLeft("DryingRack", window, 1000, 220, 560, 300);
            AddImage(rack, S(F, "DryingRack"), Cream);
            var cups = new Object[6];
            for (int i = 0; i < cups.Length; i++) cups[i] = Icon(rack, $"Slot {i + 1}", 60 + i * 88 - 40, 150, 80);
            SetArray(so, "rack", cups);
            Set(so, "dryingLabel", Label(window, "LabelDrying", S(F, "LabelDrying"), 1280, 545, PivotCenter).gameObject);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 일시정지 (SFR-023) ─────────

        public static PauseMenu BuildPause(Transform overlay)
        {
            const string F = "Pause";
            var (popup, window) = Window<PauseMenu>(overlay, "PauseMenu", S(F, "Panel"), 700, 640, 0, 1f, 0.6f);
            var so = new SerializedObject(popup);
            Set(so, "continueButton", Btn(window, "ContinueButton", S(F, "ContinueButton"), 140, 200, 420, 96));
            Set(so, "settingsButton", Btn(window, "SettingsButton", S(F, "SettingsButton"), 140, 320, 420, 96));
            Set(so, "mainMenuButton", Btn(window, "MainMenuButton", S(F, "MainMenuButton"), 140, 440, 420, 96));
            Set(so, "confirmMessage", S(F, "ConfirmMainMenu"));

            // 설정·확인 창은 타이틀과 같은 프리팹 (일시정지 창 위에 뜨도록 뒤에 둔다)
            Set(so, "settings", SharedPopup<SettingsPopup>(overlay, "SettingsPopup"));
            Set(so, "confirm", SharedPopup<ConfirmPopup>(overlay, "ConfirmPopup"));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        static T SharedPopup<T>(Transform parent, string name) where T : Component
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{SharedPrefabs}/{name}.prefab");
            if (prefab == null)
            {
                Debug.LogWarning($"{name} 프리팹이 없습니다. CafeTime > Build Title Scene 을 먼저 실행하세요.");
                return null;
            }
            var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            go.SetActive(false);
            return go.GetComponent<T>();
        }
    }
}
