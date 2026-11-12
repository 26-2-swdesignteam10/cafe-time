using System.Linq;
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
            { "CupShelf", "IceMaker", "Water", "DrinkRoom", "ShakeRoom", "TrayReturn", "Dishwash", "Pause", "ShotRoom", "KnockRoom", "DrinkTools", "DrinkResult" };
        // EspressoMachine 폴더(팀원 그림)는 임포트 설정을 바꾸지 않는다: ShotTest 씬이 Multiple 모드의 하위 스프라이트(tamper_0 등)를 참조함

        static Sprite S(string folder, string name) => LoadSprite($"{Art}/{folder}/{name}.png");

        public static void PrepareAll()
        {
            // 기기 작업 창은 도트가 아닌 일러스트라 부드럽게 표시한다. 일시정지 창만 도트 (docs/ART_STYLE.md 1장)
            foreach (var folder in Folders)
                PrepareSprites($"{Art}/{folder}", folder == "Pause" ? FilterMode.Point : FilterMode.Bilinear);
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

        // 회색조 GaugeFill 에 곱하는 캐러멜색 (맵의 작업 게이지 WorldGauge.Work 와 같음)
        static readonly Color GaugeColor = new Color32(0xFF, 0xA0, 0x32, 0xFF);

        // 진행 게이지: 틀(GaugeFrame) > FillArea > 채움(GaugeFill). 둘 다 9-slice 캡슐이고 채움 너비는 GaugeBar.Show 가 정한다
        static Image Progress(Transform parent, string name, float x, float y, float w)
        {
            var frame = TopLeft(name, parent, x, y - 3, w, 24); // 그림 높이 24 (예전 18 막대와 가운데를 맞춤)
            GaugeImage(frame, "GaugeFrame", new Color32(0x5D, 0x26, 0x08, 0xFF));
            var area = NewUI("FillArea", frame);
            Stretch(area);
            area.offsetMin = new Vector2(4, 4); // 틀 128x24 안쪽에 채움 120x16
            area.offsetMax = new Vector2(-4, -4);
            var fill = NewUI("Fill", area);
            fill.anchorMin = Vector2.zero;
            fill.anchorMax = new Vector2(0, 1);
            fill.offsetMin = fill.offsetMax = Vector2.zero;
            var image = GaugeImage(fill, "GaugeFill", GaugeColor);
            image.color = GaugeColor;
            frame.gameObject.SetActive(false);
            return image;
        }

        static Image GaugeImage(RectTransform rt, string file, Color fallback)
        {
            var sprite = LoadSprite($"Assets/_Project/Art/UI/CafeHud/{file}.png");
            var image = AddImage(rt, sprite, fallback);
            image.type = Image.Type.Sliced;
            // 맵용 PPU(128)로 임포트된 그림이라, 캔버스에서 테두리가 원래 픽셀 크기로 보이게 맞춘다
            if (sprite != null) image.pixelsPerUnitMultiplier = 100f / sprite.pixelsPerUnit;
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
            const string F = "CupShelf";
            // 좌표는 Art/UI/CupShelf/Generation.md "배치 (2026-11-12)" 표 (창 1000x640 왼쪽 위 기준)
            var popup = Screen<CupShelfScreen>(canvas, F, "CupShelfScreen", 1000, 640, out var window);
            var so = new SerializedObject(popup);

            // HOT: 선반 2단에 뒤집어 놓은 잔 8개 (윗단 왼쪽부터). 잔은 HotArea 의 자식이라 잔 위에 놓아도 HotArea 가 드롭을 받는다
            var hotArea = TopLeft("HotArea", window, 60, 120, 540, 245);
            AddImage(hotArea, null, Invisible, true);
            var hotCups = new Object[8];
            var cupSprite = S(F, "HotCupUpside");
            for (int i = 0; i < 8; i++)
            {
                var cup = Btn(hotArea, $"HotCup{i + 1}", cupSprite, 16 + (i % 4) * 132, 4 + (i / 4) * 130, 110, 100);
                cup.image.preserveAspect = true;
                hotCups[i] = cup;
            }
            SetArray(so, "hotCups", hotCups);
            Set(so, "hotCount", AddText(TopLeft("HotCount", window, 270, 540, 120, 36), "0개", 26, DarkCoffee));

            // ICE: 맨 아래 컵 위로 겹친 컵 테두리를 step 18 씩 쌓는다 (최대 10개). 더미 영역 전체가 클릭·드롭 자리
            var iceArea = TopLeft("IceArea", window, 700, 90, 190, 365);
            AddImage(iceArea, null, Invisible, true);
            var iceStack = iceArea.gameObject.AddComponent<Button>();
            iceStack.transition = Selectable.Transition.None;
            var layers = new Object[10];
            layers[0] = Layer("IceBottom", "IceCupStackBottom", 259, 186);
            for (int i = 1; i < 10; i++) // 위 컵 띠를 아래 컵보다 나중에 그려 옆면이 아래 입구와 이어지게 (Generation.md)
                layers[i] = Layer($"IceRim{i + 1}", "IceCupStackRim", 259 - i * 18, 72);
            Set(so, "iceStack", iceStack);
            SetArray(so, "iceLayers", layers);
            Set(so, "iceCount", AddText(TopLeft("IceCount", window, 735, 540, 120, 36), "0개", 26, DarkCoffee));

            GameObject Layer(string name, string file, float y, float h)
            {
                var image = AddImage(TopLeft(name, iceArea, 725 - 700, y - 90, 140, h), S(F, file), Color.clear);
                image.raycastTarget = false;
                image.preserveAspect = true;
                return image.gameObject;
            }

            // 손 칸의 빈 커피잔을 HOT/ICE 영역에 끌어다 놓아 되돌리기 (클릭은 잔·더미 Button 이 꺼내기로 처리)
            SetArray(so, "returnZones", new Object[] { hotArea.gameObject.AddComponent<DropZone>(), iceArea.gameObject.AddComponent<DropZone>() });
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

            PortafilterSlot Slot(string name, SlotType type, float x, float y, float portafilterOffsetY = 0)
            {
                var rt = Centered(name, window, x, y, 184, 72);
                AddImage(rt, null, Invisible, true); // 드롭 인식용
                var slot = rt.gameObject.AddComponent<PortafilterSlot>();
                slot.type = type;
                slot.portafilterOffsetY = portafilterOffsetY;
                return slot;
            }

            // 줄여 표시하면 Point 필터에서 픽셀이 빠지므로 표시 크기로 만든 라벨을 1:1 로 쓴다
            void StepLabel(string name, float x, float y) => Label(window, name, S(F, name), W / 2 + x, H / 2 - y, PivotCenter, 1f);

            StepLabel("LabelGrind", -570, -153);
            StepLabel("LabelTamp", -320, -153);
            StepLabel("LabelExtract", 150, -153);
            StepLabel("LabelKnock", 600, -153);
            StepLabel("LabelRack", 0, -370);

            // 앞모습은 184x88 이라 위끝을 슬롯 위끝에 맞추려고 8 내린다. 장착 그림은 그룹헤드 아래에 물리게 56 내린다
            // 탬핑 자리는 바스켓이 매트 위에 앉도록 22 더 내린다 (탬퍼도 그만큼 더 내려가야 닿음)
            const float FrontOffset = -8, TampOffset = -30, MountedOffset = -56;
            var grinderSlot = Slot("Slot_Grinder", SlotType.Grinder, -570, -30, FrontOffset);
            var tampSlot = Slot("Slot_Tamp", SlotType.Tamp, -320, -30, TampOffset); // 탬핑 매트 가운데
            var knockSlot = Slot("Slot_Knockbox", SlotType.Knockbox, 620, -40);
            knockSlot.knockScreen = BuildKnock(canvas);
            var racks = new[] { Slot("Slot_Rack1", SlotType.Rack, -145, -270), Slot("Slot_Rack2", SlotType.Rack, 145, -270) };
            var heads = new[] { Slot("Slot_GroupL", SlotType.GroupHead, -20, 90, MountedOffset), Slot("Slot_GroupR", SlotType.GroupHead, 320, 90, MountedOffset) };

            // 포터필터 2개 (받침대에서 시작)
            var states = new[] { S(F, "PortafilterEmpty"), S(F, "PortafilterGround"), S(F, "PortafilterTamped"), S(F, "PortafilterUsed") };
            var frontStates = new[] { S(F, "PortafilterFrontEmpty"), S(F, "PortafilterFrontGround"), S(F, "PortafilterFrontTamped"), S(F, "PortafilterFrontUsed") };
            for (int i = 0; i < 2; i++)
            {
                var rt = Centered($"Portafilter{i + 1}", racks[i].transform, 0, 0, 184, 72);
                AddImage(rt, states[0], Cream, true).preserveAspect = true;
                rt.gameObject.AddComponent<CanvasGroup>();
                var pf = rt.gameObject.AddComponent<Portafilter>();
                pf.stateSprites = states;
                pf.frontStateSprites = frontStates;
                pf.mountedSprite = S(F, "PortafilterMounted");
                racks[i].startingPortafilter = pf;
            }

            // 탬퍼: 아래로 끌어 누름 (SFR-032)
            var tamperRt = Centered("Tamper", window, -320, 130, 92, 138);
            AddImage(tamperRt, S(F, "Tamper"), Cream, true).preserveAspect = true;
            var tamper = tamperRt.gameObject.AddComponent<TamperHandle>();
            tamper.tampSlot = tampSlot;
            tamper.travel = 88; // 내린 포터필터의 가루 윗면까지
            tamper.progress = Progress(window, "TamperProgress", W / 2 - 320 - 80, H / 2 - 225, 160); // 탬퍼 위끝(199)보다 조금 위 (SFR-032)

            // 그라인더 (SFR-031)
            var grinder = NewUI("Grinder", window).gameObject.AddComponent<Grinder>();
            grinder.slot = grinderSlot;
            var grind = AddButton(Centered("GrindButton", window, -570, 70, 170, 60), S(F, "GrindButton"), Cream);
            UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grind.onClick, grinder.OnClickGrind);
            var grinderLcd = AddText(Centered("LCD_Grinder", window, -570, 150, 190, 60), "", 24, lcdColor);
            var gd = grinderLcd.gameObject.AddComponent<GrinderDisplay>();
            gd.grinder = grinder;
            gd.label = grinderLcd;
            // 분쇄 중 가루 줄기 (위치는 Art/UI/ShotRoom/Generation.md)
            grinder.stream = Stream(window, "GrindStream", GrindStreamPos).gameObject;

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
                heads[g].extractStream = Stream(window, g == 0 ? "ExtractStream_L" : "ExtractStream_R",
                    new Vector2(x, ExtractStreamPos.y)).gameObject;
                heads[g].transform.SetAsLastSibling(); // 장착한 포터필터 손잡이가 컵 앞에 보이게
            }

            machine.screenPanel = popup.gameObject;
            EditorUtility.SetDirty(machine);
            return popup;

            // 꺼진 채로 두었다가 작업 중에만 켜는 반복 그림 (GrindStream0~, ExtractStream0~ 있는 만큼)
            SpriteLoop Stream(RectTransform parent, string name, Vector2 pos)
            {
                string prefix = $"{Art}/{F}/{name.Split('_')[0]}";
                var frames = Enumerable.Range(0, 16).Select(i => AssetDatabase.LoadAssetAtPath<Sprite>($"{prefix}{i}.png"))
                    .TakeWhile(sprite => sprite != null).ToArray();
                if (frames.Length == 0) frames = new Sprite[] { null };
                var size = frames[0] != null ? frames[0].rect.size : new Vector2(40, 80);
                var rt = Centered(name, parent, pos.x, pos.y, size.x, size.y);
                AddImage(rt, frames[0], Invisible);
                var loop = rt.gameObject.AddComponent<SpriteLoop>();
                loop.frames = frames;
                rt.gameObject.SetActive(false);
                return loop;
            }
        }

        // 분쇄·추출 줄기 위치 (창 가운데 기준). 추출은 x 를 그룹헤드마다 바꿔 쓴다
        static readonly Vector2 GrindStreamPos = new Vector2(-570, 8);
        static readonly Vector2 ExtractStreamPos = new Vector2(-20, 12);

        // 찌꺼기 털기 창 (SFR-036). 넉박스에 사용한 포터필터를 대면 샷 창 위에 열린다
        static KnockScreen BuildKnock(Transform canvas)
        {
            const string F = "KnockRoom";
            const float W = 1536, H = 864;
            var (popup, window) = Window<KnockScreen>(canvas, "KnockScreen", S(F, "Panel"), W, H);
            SetClose(popup, CloseButton(window, W - 100, 36));

            RectTransform Part(string name, Sprite sprite, Vector2 pos, Vector2 size, bool raycast)
            {
                var rt = Centered(name, window, pos.x, pos.y, size.x, size.y);
                AddImage(rt, sprite, Cream, raycast).preserveAspect = true;
                return rt;
            }

            popup.knockSprite = S(F, "PortafilterKnock");
            popup.knockEmptySprite = S(F, "PortafilterKnockEmpty");
            popup.brushSprite = S(F, "PortafilterBrush");
            popup.portafilter = Part("Portafilter", popup.knockSprite, popup.knockStart, popup.knockSize, true).GetComponent<Image>();
            popup.residue = Part("Residue", S(F, "Residue"), popup.brushPosition, popup.brushViewSize, false).GetComponent<Image>();
            popup.residue.transform.SetParent(popup.portafilter.transform, false); // 포터필터 가운데에 겹쳐 같이 움직임
            popup.residue.rectTransform.anchoredPosition = Vector2.zero;
            popup.puck = Part("Puck", S(F, "Puck"), Vector2.zero, new Vector2(200, 130), false); // 원기둥 모양 덩어리 (Puck.png 400x260)
            popup.brush = Part("Brush", S(F, "Brush"), popup.brushHome, new Vector2(300, 150), true); // brushTip·brushGrip 은 이 크기 기준
            popup.hintKnock = S(F, "HintKnock");
            popup.hintHarder = S(F, "HintHarder");
            popup.hintBrush = S(F, "HintBrush");
            popup.hint = Part("Hint", popup.hintKnock, new Vector2(0, 340),
                popup.hintKnock != null ? popup.hintKnock.rect.size : new Vector2(600, 48), false).GetComponent<Image>();
            EditorUtility.SetDirty(popup);
            return popup;
        }

        // ───────── 제빙기 (SFR-037~040) ─────────

        public static IceMakerScreen BuildIceMaker(Transform canvas)
        {
            const string F = "IceMaker";
            var popup = Screen<IceMakerScreen>(canvas, F, "IceMakerScreen", 1100, 700, out var window);
            var so = new SerializedObject(popup);
            Set(so, "bin", Zone(window, "IceBin", 90, 190, 520, 340, S(F, "IceBin"), Cream));
            Label(window, "LabelIce", S(F, "LabelIce"), 110, 550, PivotLeft);
            Set(so, "iceText", AddText(TopLeft("Ice", window, 220, 526, 260, 48), "0 / 0", 26, DarkCoffee, TextAlignmentOptions.Left));
            Label(window, "LabelSpeed", S(F, "LabelSpeed"), 380, 550, PivotLeft);
            Set(so, "speedText", AddText(TopLeft("Speed", window, 550, 528, 300, 48), "", 26, TextBrown, TextAlignmentOptions.Left));

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
            // 좌표는 Art/UI/Water/Generation.md "배치 (2026-11-12)" 표 (창 1100x700 왼쪽 위 기준)
            var popup = Screen<WaterScreen>(canvas, F, "WaterScreen", 1100, 700, out var window);
            var so = new SerializedObject(popup);
            var zones = new Object[2];
            var cups = new Object[2];
            var streams = new Object[2];
            var buttons = new Object[2];
            var bars = new Object[2];
            var frames = Enumerable.Range(0, 16).Select(n => S(F, $"WaterStream{n}")).TakeWhile(sprite => sprite != null).ToArray();
            for (int i = 0; i < 2; i++)
            {
                bool hot = i == 0;
                float x = hot ? 135 : 585;
                var zone = Zone(window, hot ? "HotZone" : "ColdZone", x, 155, 380, 380, S(F, hot ? "HotZone" : "ColdZone"), Cream);
                zones[i] = zone;
                // 큰 컵 192x240: 바닥이 정수기 받침 윗면(창 y 475)에 닿게
                var cup = AddImage(TopLeft("Cup", zone.transform, hot ? 100 : 94, 83, 192, 240), null, Color.clear);
                cup.preserveAspect = true;
                cup.enabled = false;
                cups[i] = cup;
                // 물 줄기: 꼭지 출수구에서 컵 안까지. 물을 받는 동안만 켠다 (컵보다 앞에 그림)
                var stream = TopLeft("Stream", zone.transform, 172, 137, 36, hot ? 84 : 60);
                AddImage(stream, frames.Length > 0 ? frames[0] : null, Invisible);
                stream.gameObject.AddComponent<SpriteLoop>().frames = frames.Length > 0 ? frames : new Sprite[] { null };
                stream.gameObject.SetActive(false);
                streams[i] = stream.gameObject;
                Label(window, hot ? "LabelHot" : "LabelCold", S(F, hot ? "LabelHot" : "LabelCold"), hot ? 325 : 775, 100, PivotCenter);
                buttons[i] = Btn(window, hot ? "HotButton" : "ColdButton", S(F, "WaterButton"), hot ? 215 : 665, 555, 220, 72);
                bars[i] = Progress(window, hot ? "HotProgress" : "ColdProgress", hot ? 215 : 665, 644, 220); // 틀 위끝 641 (Progress 가 3 올림)
            }
            SetArray(so, "zones", zones);
            SetArray(so, "cupImages", cups);
            SetArray(so, "cupSprites", new Object[]
            {
                S("DrinkRoom", "BigCupCeramic"), S("DrinkRoom", "BigCupCeramicWater"), S("DrinkRoom", "BigCupCeramicMixing"),
                S("DrinkRoom", "BigCupPlastic"), S("DrinkRoom", "BigCupPlasticWater"), S("DrinkRoom", "BigCupPlasticMixing"),
            });
            SetArray(so, "streams", streams);
            SetArray(so, "fillButtons", buttons);
            SetArray(so, "progress", bars);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 음료 제조실 (SFR-044~051) ─────────

        // 재료 칸 크기. 음료 제조실 선반 그림의 줄 간격(160) 안에 들어가는 최대 크기에 가깝게
        const float SlotSize = 136, SlotIconSize = 128;

        static IngredientSlotView SlotTemplate(Transform parent, string folder)
        {
            var rt = NewUI("Ingredient", parent);
            rt.sizeDelta = new Vector2(SlotSize, SlotSize);
            var button = AddButton(rt, S(folder, "ShelfSlot"), Cream);
            var hover = S(folder, "ShelfSlotHover");
            if (hover != null)
            {
                button.transition = Selectable.Transition.SpriteSwap;
                var swap = button.spriteState;
                swap.highlightedSprite = swap.pressedSprite = hover;
                button.spriteState = swap;
            }
            var icon = AddImage(Centered("Icon", rt, 0, 0, SlotIconSize, SlotIconSize), null, Color.clear);
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

        // 판정 결과 (ADR-0027): 패널 없이 완성품 그림 + 완성도 도장 + 메뉴 이름. 제조실 창(1600x900)을 살짝 어둡게 덮는다.
        // 위치는 Art/UI/DrinkResult/Generation.md 배치 표 (창 가운데 기준, 위쪽 +y)
        static JudgeResultView Result(RectTransform window)
        {
            const string F = "DrinkResult";
            var rt = NewUI("Result", window);
            Stretch(rt);
            AddImage(rt, null, new Color(0, 0, 0, 0.45f), true); // 뒤쪽 클릭 차단
            var view = rt.gameObject.AddComponent<JudgeResultView>();
            var so = new SerializedObject(view);

            var drink = Centered("Drink", rt, -40, 40, 320, 400);
            var drinkImage = AddImage(drink, null, Color.clear);
            drinkImage.preserveAspect = true;
            Set(so, "drink", drink);
            Set(so, "drinkImage", drinkImage);
            var stamp = Centered("Stamp", rt, 110, -80, 200, 200);
            var stampImage = AddImage(stamp, null, Color.clear);
            stampImage.preserveAspect = true;
            Set(so, "stamp", stamp);
            Set(so, "stampImage", stampImage);
            Set(so, "menuText", AddText(Centered("Menu", rt, 0, -225, 640, 56), "", 36, Cream));
            SetArray(so, "stamps", new Object[] { S(F, "StampPerfect"), S(F, "StampGood"), S(F, "StampBad"), S(F, "StampFailed") });

            // 메뉴 → 완성품 그림 (HOT, ICE). 비슷한 메뉴는 같은 그림을 쓴다
            var table = new (string id, string hot, string ice)[]
            {
                ("americano", "AmericanoHot", "AmericanoIce"),
                ("cafe_latte", "LatteHot", "LatteIce"),
                ("vanilla_latte", "LatteHot", "LatteIce"),
                ("hazelnut_latte", "LatteHot", "LatteIce"),
                ("condensed_latte", "LatteHot", "LatteIce"),
                ("caramel_macchiato", "CaramelHot", "CaramelIce"),
                ("cafe_mocha", "MochaHot", "MochaIce"),
                ("strawberry_latte", null, "StrawberryLatteIce"),
                ("grapefruit_ade", null, "AdeGrapefruit"),
                ("yuja_ade", null, "AdeYuja"),
                ("strawberry_ade", null, "AdeStrawberry"),
                ("blue_lemon_ade", null, "AdeBlueLemon"),
                ("yuja_tea", "YujaTeaHot", null),
                ("grapefruit_tea", "GrapefruitTeaHot", null),
                ("chamomile", "TeaHot", "TeaIce"),
                ("earl_grey", "TeaHot", "TeaIce"),
                ("strawberry_smoothie", null, "SmoothieStrawberry"),
                ("mango_smoothie", null, "SmoothieMango"),
                ("hallabong_smoothie", null, "SmoothieHallabong"),
                ("melon_smoothie", null, "SmoothieMelon"),
                ("cookie_shake", null, "CookieShake"),
            };
            var ids = so.FindProperty("menuIds");
            ids.arraySize = table.Length;
            for (int i = 0; i < table.Length; i++) ids.GetArrayElementAtIndex(i).stringValue = table[i].id;
            SetArray(so, "hotDrinks", table.Select(r => (Object)(r.hot != null ? S(F, r.hot) : null)).ToArray());
            SetArray(so, "iceDrinks", table.Select(r => (Object)(r.ice != null ? S(F, r.ice) : null)).ToArray());
            Set(so, "failedHot", S(F, "FailedHot"));
            Set(so, "failedIce", S(F, "FailedIce"));
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        public static DrinkRoomScreen BuildDrinkRoom(Transform canvas)
        {
            const string F = "DrinkRoom";
            var popup = Screen<DrinkRoomScreen>(canvas, F, "DrinkRoomScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);

            // 재료 선반: 선반 판 윗면 y = 150, 310, 470, 630 → 칸 136 + 줄 간격 24, 한 줄 5칸 (재료 14개 = 3줄)
            var shelf = TopLeft("Shelf", window, 100, 200, 920, 660);
            AddImage(shelf, S(F, "Shelf"), new Color32(0x6B, 0x45, 0x2A, 0xFF));
            var grid = shelf.gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(SlotSize, SlotSize);
            grid.spacing = new Vector2(28, 160 - SlotSize);
            grid.padding = new RectOffset(64, 64, 150 - (int)SlotSize, 0); // 가로: (920 - 5칸 - 간격 4개) / 2
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 5;
            Set(so, "shelf", shelf);
            Set(so, "slotTemplate", SlotTemplate(shelf, F));

            // 전자저울 + 큰 컵 (배치는 Art/UI/DrinkRoom/Generation.md 2026-11-11 표). 놓는 자리는 컵과 저울을 함께 덮는다
            // 저울 그림 창 기준 x1110 y460 (판 윗면 y500), 큰 컵 x1162 y184 256x320 (바닥이 판 윗면에 닿음), LCD 글자 x1200 y580
            var scale = Zone(window, "Scale", 1110, 184, 360, 486, null, Invisible);
            AddImage(TopLeft("ScaleImage", scale.transform, 0, 276, 360, 210), S(F, "Scale"), Cream);
            Set(so, "scale", scale);
            var cup = AddImage(TopLeft("Cup", scale.transform, 52, 0, 256, 320), null, Color.clear);
            cup.preserveAspect = true;
            cup.enabled = false; // 잔이 없으면 숨김
            Set(so, "cupImage", cup);
            SetArray(so, "cupSprites", new Object[]
            {
                S(F, "BigCupCeramic"), S(F, "BigCupCeramicWater"), S(F, "BigCupCeramicMixing"),
                S(F, "BigCupPlastic"), S(F, "BigCupPlasticWater"), S(F, "BigCupPlasticMixing"),
            });
            Set(so, "weightText", AddText(TopLeft("Weight", scale.transform, 90, 396, 180, 60), "0g", 34,
                new Color32(0xB8, 0xF5, 0x9A, 0xFF)));

            // 빈 버튼 그림 위에 글자 그림을 가운데 정렬
            var empty = Btn(window, "EmptyButton", S(F, "EmptyButton"), 1050, 712, 220, 80);
            CenterLabel(empty.transform, "Label", S(F, "LabelEmpty"));
            Set(so, "emptyButton", empty);
            var complete = Btn(window, "CompleteButton", S(F, "CompleteButton"), 1290, 707, 260, 90);
            CenterLabel(complete.transform, "Label", S(F, "LabelComplete"));
            Set(so, "completeButton", complete);
            Set(so, "result", Result(window));
            Set(so, "tooltip", Tooltip(window));
            Set(so, "toolScreen", BuildIngredientTool(canvas));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // 재료 작업 창 (ADR-0024): 음료 제조실 위에 겹쳐 열린다. 좌표는 Art/UI/DrinkTools/Generation.md "배치 (2026-11-12)" 표
        static IngredientToolScreen BuildIngredientTool(Transform canvas)
        {
            const string F = "DrinkTools";
            // 그림 좌표(1100x700)는 그대로 두고 창 전체를 1.3배로 키운다 (ADR-0025)
            var (popup, window) = Window<IngredientToolScreen>(canvas, "IngredientToolScreen", S(F, "Panel"), 1100, 700, scale: 1.3f);
            SetClose(popup, CloseButton(window, 1000, 36));
            var so = new SerializedObject(popup);

            var tool = TopLeft("Tool", window, 260, 84, 500, 400);
            var toolImage = AddImage(tool, S(F, "PumpBottle"), Color.clear, true);
            toolImage.preserveAspect = true;
            var button = tool.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None; // 눌린 모습은 그림 교체로 보여 준다
            Set(so, "toolButton", button);
            Set(so, "toolImage", toolImage);
            Set(so, "toolHold", tool.gameObject.AddComponent<HoldButton>());
            Set(so, "nameText", AddText(TopLeft("Name", window, 260, 40, 220, 38), "", 26, DarkCoffee));

            AddImage(TopLeft("WeightPlate", window, 594, 445, 300, 105), S(F, "WeightPlate"), Color.clear).raycastTarget = false;
            Set(so, "weightText", AddText(TopLeft("Weight", window, 690, 508, 108, 22), "0g", 20, new Color32(0x3B, 0x2A, 0x20, 0xFF)));

            var cup = AddImage(TopLeft("Cup", window, 616, 164, 256, 320), null, Color.clear);
            cup.preserveAspect = true;
            cup.raycastTarget = false;
            Set(so, "cupImage", cup);
            SetArray(so, "cupSprites", new Object[]
            {
                S("DrinkRoom", "BigCupCeramic"), S("DrinkRoom", "BigCupCeramicWater"), S("DrinkRoom", "BigCupCeramicMixing"),
                S("DrinkRoom", "BigCupPlastic"), S("DrinkRoom", "BigCupPlasticWater"), S("DrinkRoom", "BigCupPlasticMixing"),
            });
            // 티백 담그기 덧그림 (컵과 같은 영역, 1회 재생)
            var steep = AddImage(TopLeft("TeaSteep", window, 616, 164, 256, 320), null, Color.clear);
            steep.preserveAspect = true;
            steep.raycastTarget = false;
            steep.enabled = false;
            Set(so, "steepImage", steep);
            SetArray(so, "steepCeramic", Frames("TeaSteepCeramic"));
            SetArray(so, "steepPlastic", Frames("TeaSteepPlastic"));

            var teaBag = AddImage(TopLeft("TeaBag", window, 616, 164, 256, 320), null, Color.clear);
            teaBag.preserveAspect = true;
            teaBag.raycastTarget = false;
            teaBag.enabled = false;
            Set(so, "teaBagImage", teaBag);
            SetArray(so, "teaBagSprites", new Object[] { S(F, "TeaBagInCupCeramic"), S(F, "TeaBagInCupPlastic") });

            // 순서: Pump, Cheong, TeaBag, Milk, Soda
            SetArray(so, "idleSprites", new Object[] { S(F, "PumpBottle"), S(F, "CheongJar"), S(F, "TeaBox"), S(F, "MilkCarton"), S(F, "SodaBottle") });
            SetArray(so, "activeSprites", new Object[] { S(F, "PumpBottlePressed"), S(F, "CheongJarOpen"), null, S(F, "MilkCartonPour"), S(F, "SodaBottlePour") });
            SetArray(so, "streams", new Object[]
            {
                Stream("PumpStream", "SyrupDrip", 710, 180, 160),
                Stream("CheongStream", "SyrupDrip", 714, 231, 109), // 숟가락 붓기 아래. 컵 종류별 위치는 cheongStreamRects
                null,
                Stream("MilkStream", "MilkStream", 692, 254, 47),
                Stream("SodaStream", "SodaStream", 712, 234, 67),
            });

            // 청: 컵 위에서 숟가락을 기울여 붓는 그림, 줄기 위치(도자기 잔, 플라스틱 컵)
            var spoonPour = AddImage(TopLeft("SpoonPour", window, 428, 78, 320, 160), S(F, "SpoonPour"), Color.clear);
            spoonPour.raycastTarget = false;
            spoonPour.enabled = false;
            Set(so, "spoonPourImage", spoonPour);
            var rects = so.FindProperty("cheongStreamRects");
            rects.arraySize = 2;
            rects.GetArrayElementAtIndex(0).rectValue = new Rect(714, 231, 36, 109);
            rects.GetArrayElementAtIndex(1).rectValue = new Rect(714, 231, 36, 70);

            // 끌기 잡는 자리 (도구 그림 위): 청 병의 숟가락 손잡이, 티백 상자
            Set(so, "cheongGrab", Grab("CheongGrab", 446, 104, 56, 86));
            Set(so, "teaGrab", Grab("TeaGrab", 378, 188, 98, 72));
            PointerDragRelay Grab(string name, float x, float y, float w, float h)
            {
                var rt = TopLeft(name, window, x, y, w, h);
                AddImage(rt, null, Invisible, true);
                rt.gameObject.SetActive(false); // 해당 도구일 때만 켬
                return rt.gameObject.AddComponent<PointerDragRelay>();
            }

            // 마우스를 따라오는 숟가락·티백: 창 가운데 기준, 피벗 = 잡는 점 (Generation.md 배치 2차)
            var drag = AddImage(Centered("Drag", window, 0, 0, 320, 80), null, Color.clear);
            drag.raycastTarget = false;
            drag.enabled = false;
            Set(so, "dragImage", drag);
            Set(so, "spoonFull", S(F, "SpoonFull"));
            Set(so, "teaBag", S(F, "TeaBag"));
            so.FindProperty("spoonSize").vector2Value = new Vector2(320, 80);
            so.FindProperty("spoonPivot").vector2Value = new Vector2(22f / 320, 1 - 40f / 80);
            so.FindProperty("teaBagSize").vector2Value = new Vector2(128, 160);
            so.FindProperty("teaBagPivot").vector2Value = new Vector2(112f / 128, 1 - 71f / 160);

            Object[] Frames(string file) => Enumerable.Range(0, 16).Select(n => S(F, $"{file}{n}")).TakeWhile(sprite => sprite != null).ToArray();

            GameObject Stream(string name, string file, float x, float y, float h)
            {
                var frames = Enumerable.Range(0, 16).Select(n => S(F, $"{file}{n}")).TakeWhile(sprite => sprite != null).ToArray();
                var rt = TopLeft(name, window, x, y, 36, h);
                AddImage(rt, frames.Length > 0 ? frames[0] : null, Invisible).raycastTarget = false;
                rt.gameObject.AddComponent<SpriteLoop>().frames = frames.Length > 0 ? frames : new Sprite[] { null };
                rt.gameObject.SetActive(false);
                return rt.gameObject;
            }

            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 쉐이크 제조실 (SFR-052~055) ─────────

        public static ShakeRoomScreen BuildShakeRoom(Transform canvas)
        {
            const string F = "ShakeRoom";
            var popup = Screen<ShakeRoomScreen>(canvas, F, "ShakeRoomScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);

            Label(window, "LabelBase", S(F, "LabelBase"), 110, 210, PivotLeft);
            // 원액 5칸이 믹서기 영역(x 800~)에 겹치지 않게: 90 + 136×5 + 6×4 = 794
            var shelf = TopLeft("Bases", window, 90, 240, 704, SlotSize);
            var row = shelf.gameObject.AddComponent<HorizontalLayoutGroup>();
            row.spacing = 6;
            row.childControlWidth = row.childControlHeight = false;
            row.childForceExpandWidth = row.childForceExpandHeight = false;
            Set(so, "shelf", shelf);
            Set(so, "slotTemplate", SlotTemplate(shelf, "DrinkRoom"));
            Label(window, "LabelMilk", S(F, "LabelMilk"), 110, 420, PivotLeft);
            var milk = SlotTemplate(window, "DrinkRoom");
            var milkRt = (RectTransform)milk.transform;
            milkRt.anchorMin = milkRt.anchorMax = milkRt.pivot = new Vector2(0, 1);
            milkRt.anchoredPosition = new Vector2(90, -450);
            Set(so, "milkSlot", milk);

            // 믹서기 (가운데)
            var blender = Zone(window, "Blender", 800, 190, 280, 420, S(F, "BlenderEmpty"), Cream);
            Set(so, "blenderZone", blender);
            Set(so, "blenderImage", blender.GetComponent<Image>());
            SetArray(so, "blenderSprites", new Object[] { S(F, "BlenderEmpty"), S(F, "BlenderFilled"), S(F, "BlenderBlended") });
            Set(so, "weightText", AddText(TopLeft("Weight", window, 800, 580, 280, 50), "0g", 32, DarkCoffee));
            Set(so, "blendButton", Btn(window, "BlendButton", S(F, "BlendButton"), 830, 650, 220, 80));
            Set(so, "blendProgress", Progress(window, "BlendProgress", 830, 770, 220));

            // 커피잔 자리 (오른쪽)
            var cup = Zone(window, "CupSpot", 1180, 400, 260, 220, null, Invisible);
            AddImage(TopLeft("Coaster", cup.transform, 20, 120, 220, 70), S(F, "CupSpot"), Spot);
            Set(so, "cupSpot", cup);
            Set(so, "cupIcon", Icon(cup.transform, "Cup", 66, 30, 128));
            Label(window, "LabelCupSpot", S(F, "LabelCupSpot"), 1310, 625, PivotCenter);
            Set(so, "pourButton", Btn(window, "PourButton", S(F, "PourButton"), 1200, 650, 220, 80));

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

            AddImage(TopLeft("WaitingCrate", window, 100, 200, 300, 240), S(F, "WaitingCrate"), Cream);
            Label(window, "LabelWaiting", S(F, "LabelWaiting"), 250, 470, PivotCenter);
            Set(so, "waitingText", AddText(TopLeft("Waiting", window, 150, 495, 200, 48), "0개", 32, DarkCoffee));

            // 싱크대: 가운데에 잔 1개, 그 위에 얼룩. 수세미는 좌우로만 조금 따라 움직임
            var sink = TopLeft("Sink", window, 420, 250, 640, 380);
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
