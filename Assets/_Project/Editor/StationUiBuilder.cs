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
            { "CupShelf", "IceMaker", "Water", "DrinkRoom", "ShakeRoom", "TrayReturn", "Dishwash", "Pause", "ShotRoom", "KnockRoom", "DrinkTools", "DrinkResult", "SteamRoom" };
        // EspressoMachine 폴더(팀원 그림)는 임포트 설정을 바꾸지 않는다: ShotTest 씬이 Multiple 모드의 하위 스프라이트(tamper_0 등)를 참조함

        static Sprite S(string folder, string name) => LoadSprite($"{Art}/{folder}/{name}.png");

        // 큰 컵 내용물 층 색 (ADR-0037). 값은 docs/ART_STYLE.md 9장 표
        static readonly (string key, string hex)[] LiquidColors =
        {
            ("shot", "#60351FEF"), ("water_hot", "#E7F4FC24"), ("water_cold", "#DDF3FF30"), ("milk", "#FFF4DEFA"), (CafeTime.Core.RecipeKeys.SteamedMilk, "#FFE8C4FA"),
            ("soda", "#D9F5FF38"), ("condensed_milk", "#FFE2A8F5"), ("syrup_vanilla", "#F5D69796"),
            ("syrup_caramel", "#C47A32DC"), ("syrup_hazelnut", "#A56B3ACB"), ("syrup_chocolate", "#4C2C25F5"), ("syrup_blue", "#268FCAD0"),
            ("cheong_strawberry", "#CB3F52EB"), ("cheong_grapefruit", "#EF8655DC"), ("cheong_yuja", "#EFC24BD9"), ("cheong_lemon", "#F3E25AD9"),
            ("tea_chamomile", "#E5BD57B8"), ("tea_earlgrey", "#AD682ED4"), ("smoothie_strawberry", "#ED8D9AFF"),
            ("smoothie_mango", "#FFC656FF"), ("smoothie_hallabong", "#FFA23FFF"), ("smoothie_melon", "#B8D59AFF"), ("smoothie_cookie", "#D6C8B6FF"),
            // 2026-11-17 새 재료 (ADR-0038)
            ("cold_brew", "#3A2014E6"), ("matcha", "#8DB255F0"), ("cinnamon", "#9A5530C8"), ("vanilla_ice_cream", "#FFF6DDFF"),
        };

        // 큰 컵 Image 와 같은 자리·크기에 내용물 층을 겹친다 (ADR-0037). 컵 Image 바로 다음에 만들어야 컵 위에 그려진다.
        // 캔버스 좌표는 Art/UI/DrinkRoom/Generation.md 2026-11-17 "정렬·계량" 표
        static CupContentsView CupContents(Transform parent, float x, float y, float w, float h)
        {
            const string F = "DrinkRoom";
            if (AssetImporter.GetAtPath($"{Art}/{F}/BubblesTile.png") is TextureImporter bubbles && bubbles.wrapMode != TextureWrapMode.Repeat)
            {
                bubbles.wrapMode = TextureWrapMode.Repeat; // Tiled 로 반복
                bubbles.SaveAndReimport();
            }
            var view = TopLeft("CupContents", parent, x, y, w, h).gameObject.AddComponent<CupContentsView>();
            var so = new SerializedObject(view);
            Set(so, "plasticMask", S(F, "CupPlasticLiquidMask"));
            Set(so, "plasticFront", S(F, "CupPlasticFront"));
            Set(so, "liquidShade", S(F, "LiquidShade"));
            Set(so, "liquidSurface", S(F, "LiquidSurface"));
            Set(so, "bubblesTile", S(F, "BubblesTile"));
            Set(so, "iceFill", S(F, "IceFill"));
            SetArray(so, "teaBagSprites", new Object[] { S("DrinkTools", "TeaBagInCupCeramic"), S("DrinkTools", "TeaBagInCupPlastic") });
            Set(so, "ceramicSurface", S(F, "CupCeramicSurface"));
            Set(so, "ceramicFoam", S(F, "CupCeramicFoam"));
            so.FindProperty("plasticBottomY").floatValue = 604;
            so.FindProperty("plasticTopY").floatValue = 280;
            so.FindProperty("plasticBottomWidth").floatValue = 153;
            so.FindProperty("plasticTopWidth").floatValue = 297;
            so.FindProperty("plasticCenterX").floatValue = 254;
            so.FindProperty("surfaceHeight").floatValue = 64;
            var keys = so.FindProperty("colorKeys");
            var colors = so.FindProperty("colors");
            keys.arraySize = colors.arraySize = LiquidColors.Length;
            for (int i = 0; i < LiquidColors.Length; i++)
            {
                keys.GetArrayElementAtIndex(i).stringValue = LiquidColors[i].key;
                ColorUtility.TryParseHtmlString(LiquidColors[i].hex, out var color);
                colors.GetArrayElementAtIndex(i).colorValue = color;
            }
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

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
            // 좌표는 Art/UI/CupShelf/Generation.md "배치 (2026-11-18)" 표 (창 1000x640 왼쪽 위 기준)
            var popup = Screen<CupShelfScreen>(canvas, F, "CupShelfScreen", 1000, 640, out var window);
            var so = new SerializedObject(popup);

            // HOT: 선반 4단에 뒤집힌 잔 16개. 빈 선반에도 반환할 수 있게 드롭 영역은 유지한다 (SFR-028)
            var hotArea = TopLeft("HotArea", window, 48, 98, 540, 400);
            AddImage(hotArea, null, Invisible, true);
            var hotCups = new Object[16];
            var cupSprite = S(F, "HotCupUpside");
            float[] hotRows = { 0, 96, 192, 289 };
            for (int i = 0; i < hotCups.Length; i++)
            {
                var cup = Btn(hotArea, $"HotCup{i + 1}", cupSprite, 16 + (i % 4) * 132, hotRows[i / 4], 110, 100);
                cup.image.preserveAspect = true;
                hotCups[i] = cup;
            }
            SetArray(so, "hotCups", hotCups);
            Set(so, "hotCount", AddText(TopLeft("HotCount", window, 270, 540, 120, 36), "0개", 26, DarkCoffee));

            // ICE: 입구가 아래인 컵을 16px씩 포개어 두 탑에 10개씩 놓는다. 왼쪽 탑부터 채운다 (SFR-028)
            var iceStacks = new Object[2];
            var layers = new Object[20];
            var zones = new Object[3];
            zones[0] = hotArea.gameObject.AddComponent<DropZone>();
            for (int tower = 0; tower < iceStacks.Length; tower++)
            {
                var area = TopLeft($"IceArea{tower + 1}", window, 624 + tower * 168, 108, 164, 346);
                AddImage(area, null, Invisible, true);
                var stack = area.gameObject.AddComponent<Button>();
                stack.transition = Selectable.Transition.None;
                iceStacks[tower] = stack;
                zones[tower + 1] = area.gameObject.AddComponent<DropZone>();
                for (int i = 0; i < 10; i++)
                {
                    // 윗컵 몸체가 아래 컵을 덮어 연속된 옆면과 입구 테두리가 함께 보인다
                    var image = AddImage(TopLeft($"IceCup{tower * 10 + i + 1}", area, 12, 153 - i * 16, 140, 186), S(F, "IceCupInverted"), Color.clear);
                    image.raycastTarget = false;
                    image.preserveAspect = true;
                    layers[tower * 10 + i] = image.gameObject;
                }
            }
            SetArray(so, "iceStacks", iceStacks);
            SetArray(so, "iceLayers", layers);
            Set(so, "iceCount", AddText(TopLeft("IceCount", window, 735, 540, 120, 36), "0개", 26, DarkCoffee));

            // 손 칸의 빈 커피잔을 HOT/ICE 영역에 끌어다 놓아 되돌리기 (클릭은 잔·더미 Button 이 꺼내기로 처리)
            SetArray(so, "returnZones", zones);
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

            // 앞모습은 184x88 이라 위끝을 슬롯 위끝에 맞추려고 8 내린다. 장착 그림의 플랜지는 그룹헤드 아래에 겹친다
            // 탬핑 자리는 바스켓이 매트 위에 앉도록 22 더 내린다 (탬퍼도 그만큼 더 내려가야 닿음)
            const float FrontOffset = -8, TampOffset = -30, MountedOffset = -23;
            var grinderSlot = Slot("Slot_Grinder", SlotType.Grinder, -570, -30, FrontOffset);
            var tampSlot = Slot("Slot_Tamp", SlotType.Tamp, -320, -30, TampOffset); // 탬핑 매트 가운데
            var knockSlot = Slot("Slot_Knockbox", SlotType.Knockbox, 620, -40);
            knockSlot.knockScreen = BuildKnock(canvas);
            // 손잡이가 살짝 처지게 4° 기울이고, 바스켓 발이 받침대 매트에 닿도록 18 내린다 (2026-11-19 사용자 선택, 14°는 떠 보였음). 슬롯 전체가 드롭 영역이다 (SFR-030)
            const float RackOffset = -18;
            var racks = new[] { Slot("Slot_Rack1", SlotType.Rack, -145, -258, RackOffset), Slot("Slot_Rack2", SlotType.Rack, 145, -258, RackOffset) };
            foreach (var rack in racks) rack.transform.localEulerAngles = new Vector3(0, 0, 4);
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

                // 256px 원화의 잔 몸통 중심 x=110, 입구 y=110, 바닥 y=235. 손잡이·투명 여백을 보정한다
                const float glassSize = 144, glassScale = glassSize / 256f, trayY = 496;
                float glassTop = trayY - 235 * glassScale;
                float rimY = glassTop + 110 * glassScale;
                var cupRt = Centered(g == 0 ? "Cup_L" : "Cup_R", window, x, H / 2 - (glassTop + glassSize / 2), 144, 144);
                AddImage(cupRt, null, Invisible, true); // 클릭·드롭 인식용
                var icon = AddImage(Centered("Glass", cupRt, (128 - 110) * glassScale, 0, glassSize, glassSize), null, Color.clear);
                icon.preserveAspect = true;
                var cup = cupRt.gameObject.AddComponent<ShotCupSlot>();
                cup.head = heads[g];
                cup.glassIcon = icon;
                heads[g].cup = cup;
                // 줄기 원화의 투명 여백(y=10..61)을 제외한 끝을 추출구 아래와 잔 입구 안쪽에 맞춘다 (SFR-035)
                float spoutY = H / 2 - (90 + MountedOffset) + 32; // 장착 그림 140×64의 아래끝
                float streamHeight = (rimY - spoutY) * 64 / 51;
                var stream = Stream(window, g == 0 ? "ExtractStream_L" : "ExtractStream_R", Vector2.zero);
                var streamRt = (RectTransform)stream.transform;
                streamRt.sizeDelta = new Vector2(48, streamHeight);
                streamRt.anchoredPosition = new Vector2(x, H / 2 - (spoutY - 10 * streamHeight / 64 + streamHeight / 2));
                heads[g].extractStream = stream.gameObject;
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

        // 분쇄 줄기 위치 (창 가운데 기준). 추출 줄기는 잔 입구에서 계산한다
        static readonly Vector2 GrindStreamPos = new Vector2(-570, 8);

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
            // 좌표는 Art/UI/IceMaker/Generation.md "배치 (2026-11-12)" 표 (창 1100x700 왼쪽 위 기준)
            // 제빙기와 스쿱 트레이는 작업대 윗면(창 y 448~494)에 선다. 얼음 수·제빙 속도·스쿱 글자는 표시하지 않는다 (2026-11-12 사용자 요청)
            var popup = Screen<IceMakerScreen>(canvas, F, "IceMakerScreen", 1100, 700, out var window);
            var so = new SerializedObject(popup);
            Set(so, "bin", Zone(window, "IceBin", 90, 46, 500, 443, S(F, "IceBin"), Cream));

            // 스쿱 자리: 아래쪽에 트레이가 그려진 3:2 그림. 스쿱은 가운데에 놓여 트레이 안에 담긴 것처럼 보인다
            var spot = Zone(window, "ScoopSpot", 660, 264, 340, 227, S(F, "ScoopSpot"), Cream);
            Set(so, "scoopSpot", spot);
            Set(so, "scoop", Draggable(spot.transform, "Scoop", S(F, "ScoopEmpty"), 240, 156));
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
            var contents = new Object[2];
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
                contents[i] = CupContents(zone.transform, hot ? 100 : 94, 83, 192, 240);
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
            SetArray(so, "cupContents", contents);
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
            // 아이콘을 조금 올리고 칸 아래쪽에 재료 이름 (2026-11-17 사용자 요청)
            var icon = AddImage(Centered("Icon", rt, 0, 12, SlotIconSize - 26, SlotIconSize - 26), null, Color.clear);
            icon.preserveAspect = true;
            var nameText = AddText(TopLeft("Name", rt, 2, SlotSize - 34, SlotSize - 4, 30), "", 19, DarkCoffee);
            nameText.enableAutoSizing = true;
            nameText.fontSizeMin = 13;
            nameText.fontSizeMax = 19;
            nameText.raycastTarget = false;
            var hold = rt.gameObject.AddComponent<HoldButton>();
            var view = rt.gameObject.AddComponent<IngredientSlotView>();
            var so = new SerializedObject(view);
            Set(so, "button", button);
            Set(so, "hold", hold);
            Set(so, "icon", icon);
            Set(so, "nameText", nameText);
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
                ("vanilla_latte", "VanillaLatteHot", "VanillaLatteIce"),
                ("hazelnut_latte", "HazelnutLatteHot", "HazelnutLatteIce"),
                ("caramel_macchiato", "CaramelHot", "CaramelIce"),
                ("cafe_mocha", "MochaHot", "MochaIce"),
                ("cappuccino", "CappuccinoHot", "CappuccinoIce"),
                ("cold_brew", "ColdBrewHot", "ColdBrewIce"),
                ("cold_brew_latte", "ColdBrewLatteHot", "ColdBrewLatteIce"),
                ("strawberry_latte", null, "StrawberryLatteIce"),
                ("chocolate_latte", null, "ChocoLatteIce"),
                ("matcha_latte", null, "MatchaLatteIce"),
                ("grapefruit_ade", null, "AdeGrapefruit"),
                ("blue_lemon_ade", null, "AdeBlueLemon"),
                ("yuja_tea", "YujaTeaHot", null),
                ("grapefruit_tea", "GrapefruitTeaHot", null),
                ("chamomile", "TeaHot", null),
                ("earl_grey", "TeaHot", null),
                ("strawberry_smoothie", null, "SmoothieStrawberry"),
                ("mango_smoothie", null, "SmoothieMango"),
                ("hallabong_smoothie", null, "SmoothieHallabong"),
                ("melon_smoothie", null, "SmoothieMelon"),
                ("cookie_shake", null, "CookieShake"),
                ("milk_shake", null, "MilkShake"),
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

            // 재료 선반: 선반 판 윗면 y = 150, 310, 470, 630 → 칸 136 + 줄 간격 24, 한 줄 5칸 (재료 17개 = 4줄)
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
            Set(so, "cupContents", CupContents(scale.transform, 52, 0, 256, 320));
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
            // 우유 스티밍 창 열기 (ADR-0046, Art/UI/SteamRoom/Generation.md "음료 제조실 진입 버튼")
            var steam = Btn(window, "SteamButton", S(F, "SteamButton"), 1080, 80, 330, 90);
            CenterLabel(steam.transform, "Label", S(F, "LabelSteam"));
            Set(so, "steamButton", steam);
            Set(so, "steamScreen", BuildSteam(canvas));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // 우유 스티밍 창 (ADR-0046): 음료 제조실 위에 겹쳐 열린다. 좌표는 Art/UI/SteamRoom/Generation.md "스팀 창 배치표" (1100x700, 1.3배 표시)
        static SteamScreen BuildSteam(Transform canvas)
        {
            const string F = "SteamRoom";
            var (popup, window) = Window<SteamScreen>(canvas, "SteamScreen", S(F, "Panel"), 1100, 700, scale: 1.3f);
            SetClose(popup, CloseButton(window, 1000, 36));
            var so = new SerializedObject(popup);

            Image Picture(string name, Sprite sprite, float x, float y, float w, float h)
            {
                var image = AddImage(TopLeft(name, window, x, y, w, h), sprite, Color.clear);
                image.preserveAspect = true;
                image.raycastTarget = false;
                return image;
            }
            GameObject Loop(string name, string file, float x, float y, float w, float h, string folder = F)
            {
                var frames = Enumerable.Range(0, 16).Select(n => S(folder, $"{file}{n}")).TakeWhile(sprite => sprite != null).ToArray();
                var image = Picture(name, frames.Length > 0 ? frames[0] : null, x, y, w, h);
                image.preserveAspect = false;
                image.gameObject.AddComponent<SpriteLoop>().frames = frames.Length > 0 ? frames : new Sprite[] { null };
                image.gameObject.SetActive(false);
                return image.gameObject;
            }

            Label(window, "Hint", S(F, "Hint"), 80, 84, PivotLeft);

            // 잔과 무게 판 (재료 작업 창과 같은 자리)
            AddImage(TopLeft("WeightPlate", window, 594, 445, 300, 105), S("DrinkTools", "WeightPlate"), Color.clear).raycastTarget = false;
            Set(so, "weightText", AddText(TopLeft("Weight", window, 690, 508, 108, 22), "0g", 20, new Color32(0x3B, 0x2A, 0x20, 0xFF)));
            Set(so, "cupImage", Picture("Cup", null, 616, 164, 256, 320));
            Set(so, "cupContents", CupContents(window, 616, 164, 256, 320));
            SetArray(so, "cupSprites", new Object[]
            {
                S("DrinkRoom", "BigCupCeramic"), S("DrinkRoom", "BigCupCeramicWater"), S("DrinkRoom", "BigCupCeramicMixing"),
                S("DrinkRoom", "BigCupPlastic"), S("DrinkRoom", "BigCupPlasticWater"), S("DrinkRoom", "BigCupPlasticMixing"),
            });

            // 우유팩: 세운 그림과 붓는 그림은 자리가 달라 따로 둔다
            Set(so, "carton", Picture("MilkCarton", S("DrinkTools", "MilkCarton"), 55, 210, 360, 288).gameObject);
            var cartonPour = Picture("MilkCartonPour", S("DrinkTools", "MilkCartonPour"), 20, 112, 360, 288).gameObject;
            cartonPour.SetActive(false);
            Set(so, "cartonPour", cartonPour);
            Set(so, "milkStream", Loop("MilkStream", "MilkStream", 326, 234, 36, 78, "DrinkTools"));

            // 피처 (원위치), 스팀 완드, 증기. 붓는 중에는 원위치 피처를 숨기고 컵 위의 붓는 피처를 보여 준다
            Set(so, "pitcherImage", Picture("Pitcher", S(F, "Pitcher"), 250, 248, 248, 248));
            Set(so, "pitcherEmpty", S(F, "Pitcher"));
            Set(so, "pitcherMilk", S(F, "PitcherMilk"));
            var pitcherPour = Picture("PitcherPour", S(F, "PitcherPour"), 610, 100, 160, 160).gameObject;
            pitcherPour.SetActive(false);
            Set(so, "pitcherPour", pitcherPour);
            Set(so, "wandImage", Picture("SteamWand", S(F, "SteamWand"), 330, 100, 152, 240));
            Set(so, "wandOff", S(F, "SteamWand"));
            Set(so, "wandOn", S(F, "SteamWandOn"));
            Set(so, "steamFx", Loop("Steam", "Steam", 296, 112, 192, 230));
            Set(so, "pitcherStream", Loop("PitcherStream", "SteamedMilkStream", 743.25f, 190.625f, 36, 155.375f));
            so.FindProperty("pitcherStreamHeights").GetArrayElementAtIndex(0).floatValue = 155.375f; // HOT 잔 입구 안쪽까지
            so.FindProperty("pitcherStreamHeights").GetArrayElementAtIndex(1).floatValue = 116f;     // ICE 컵은 입구가 더 낮다 (재료 작업 창 우유 줄기 비율)

            // 온도계: 채움은 아래(0℃ y472)부터 위(90℃ y202)로
            var fill = AddImage(TopLeft("ThermoFill", window, 540, 202, 9, 270), S(F, "ThermoFill"), new Color32(0xC8, 0x4B, 0x31, 0xFF));
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Vertical;
            fill.fillOrigin = (int)Image.OriginVertical.Bottom;
            fill.raycastTarget = false;
            Set(so, "thermoFill", fill);
            Picture("ThermoFrame", S(F, "ThermoFrame"), 512, 170, 96, 336);

            Label(window, "LabelPitcher", S(F, "LabelPitcher"), 270, 517, PivotLeft);
            Set(so, "pitcherText", AddText(TopLeft("PitcherAmount", window, 325, 492, 165, 38), "0g", 28, TextBrown));
            Label(window, "LabelTemperature", S(F, "LabelTemperature"), 510, 530, PivotLeft);
            Set(so, "temperatureText", AddText(TopLeft("Temperature", window, 510, 552, 100, 36), "-", 28, TextBrown));

            // 꾹 누르는 버튼 3개, 비우기
            HoldButton Hold(string name, float x)
            {
                var rt = TopLeft(name, window, x, 598, 240, 64);
                AddImage(rt, S(F, name), Cream, true);
                return rt.gameObject.AddComponent<HoldButton>();
            }
            Set(so, "milkHold", Hold("MilkButton", 64));
            Set(so, "steamHold", Hold("SteamButton", 344));
            Set(so, "pourHold", Hold("PourButton", 624));
            Set(so, "emptyButton", Btn(window, "EmptyButton", S(F, "EmptyButton"), 330, 538, 150, 44));

            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // 재료 작업 창 (ADR-0024): 음료 제조실 위에 겹쳐 열린다. 좌표는 Art/UI/DrinkTools/Generation.md "배치 (2026-11-12)" 표
        static IngredientToolScreen BuildIngredientTool(Transform canvas)
        {
            const string F = "DrinkTools";
            // 그림 좌표(1100x700)는 그대로 두고 창 전체를 1.3배로 키운다 (ADR-0028)
            var (popup, window) = Window<IngredientToolScreen>(canvas, "IngredientToolScreen", S(F, "Panel"), 1100, 700, scale: 1.3f);
            SetClose(popup, CloseButton(window, 1000, 36));
            var so = new SerializedObject(popup);

            var tool = TopLeft("Tool", window, 260, 84, 500, 400);
            var toolImage = AddImage(tool, S(F, "PumpBottle"), Color.clear, true);
            toolImage.preserveAspect = true;
            Set(so, "toolImage", toolImage);
            Set(so, "toolHold", tool.gameObject.AddComponent<HoldButton>());
            Set(so, "nameText", AddText(TopLeft("Name", window, 260, 40, 220, 38), "", 26, DarkCoffee));

            AddImage(TopLeft("WeightPlate", window, 594, 445, 300, 105), S(F, "WeightPlate"), Color.clear).raycastTarget = false;
            Set(so, "weightText", AddText(TopLeft("Weight", window, 690, 508, 108, 22), "0g", 20, new Color32(0x3B, 0x2A, 0x20, 0xFF)));

            var cup = AddImage(TopLeft("Cup", window, 616, 164, 256, 320), null, Color.clear);
            cup.preserveAspect = true;
            cup.raycastTarget = false;
            Set(so, "cupImage", cup);
            Set(so, "cupContents", CupContents(window, 616, 164, 256, 320));
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

            // 도구 공용 그림. 순서: Pump, Spoon, TeaBag, Milk, Soda, ColdBrew
            SetArray(so, "idleSprites", new Object[] { S(F, "PumpBottle"), S(F, "CheongJar"), S(F, "TeaBox"), S(F, "MilkCarton"), S(F, "SodaBottle"), S(F, "ColdBrewBottle") });
            SetArray(so, "activeSprites", new Object[] { S(F, "PumpBottlePressed"), S(F, "CheongJarOpen"), null, S(F, "MilkCartonPour"), S(F, "SodaBottlePour"), S(F, "ColdBrewBottlePour") });
            // 펌프 방울·숟가락 낙하는 중립색 그림(없으면 기존 꿀색)에 재료 색을 곱한다
            string drip = S(F, "SyrupDripNeutral0") != null ? "SyrupDripNeutral" : "SyrupDrip";
            SetArray(so, "streams", new Object[]
            {
                Stream("PumpStream", drip, 710, 180, 160),
                Stream("SpoonStream", drip, 714, 231, 109), // 숟가락 붓기 아래. 컵 종류별 위치는 cheongStreamRects
                null,
                Stream("MilkStream", "MilkStream", 692, 254, 47),
                Stream("SodaStream", "SodaStream", 712, 234, 67),
                Stream("ColdBrewStream", S(F, "ColdBrewStream0") != null ? "ColdBrewStream" : "SodaStream", 712, 234, 67), // 탄산수 병과 같은 기하
            });
            Set(so, "powderStream", Stream("PowderStream", "PowderFall", 714, 231, 109));
            ToolArts(so, F);

            // 청: 컵 위에서 숟가락을 기울여 붓는 그림, 줄기 위치(도자기 잔, 플라스틱 컵)
            var spoonPour = AddImage(TopLeft("SpoonPour", window, 428, 78, 320, 160), S(F, "SpoonPour"), Color.clear);
            spoonPour.raycastTarget = false;
            spoonPour.enabled = false;
            Set(so, "spoonPourImage", spoonPour);
            Set(so, "spoonPour", S(F, "SpoonPour"));
            var rects = so.FindProperty("cheongStreamRects");
            rects.arraySize = 2;
            rects.GetArrayElementAtIndex(0).rectValue = new Rect(714, 231, 36, 109);
            rects.GetArrayElementAtIndex(1).rectValue = new Rect(714, 231, 36, 70);

            // 끌기 잡는 자리 (도구 그림 위): 펌프 머리(원본 x130~325 y12~85 → 창 x325~422 y90~127, 여유 포함), 청 병의 숟가락 손잡이, 티백 상자
            Set(so, "pumpGrab", Grab("PumpGrab", 315, 80, 120, 70));
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

        // 재료 전용 작업 창 그림 (ADR-0038, Art/UI/DrinkTools/Generation.md 2026-11-17). 파일이 없으면 그 칸은 공용 그림
        static void ToolArts(SerializedObject so, string F)
        {
            Color C(string hex) => ColorUtility.TryParseHtmlString(hex, out var c) ? c : Color.white;
            var table = new (string id, string idle, string active, string spoon, string spoonPour, bool powder, Color color)[]
            {
                ("syrup_vanilla", "PumpBottleSyrupVanilla", "PumpBottleSyrupVanillaPressed", null, null, false, C("#F5D697")),
                ("syrup_caramel", "PumpBottleSyrupCaramel", "PumpBottleSyrupCaramelPressed", null, null, false, C("#C47A32")),
                ("syrup_hazelnut", "PumpBottleSyrupHazelnut", "PumpBottleSyrupHazelnutPressed", null, null, false, C("#A56B3A")),
                ("syrup_chocolate", "PumpBottleSyrupChocolate", "PumpBottleSyrupChocolatePressed", null, null, false, C("#4C2C25")),
                ("syrup_blue", "PumpBottleSyrupBlue", "PumpBottleSyrupBluePressed", null, null, false, C("#268FCA")),
                ("condensed_milk", "PumpBottleCondensedMilk", "PumpBottleCondensedMilkPressed", null, null, false, C("#FFE2A8")),
                ("cheong_strawberry", "CheongJarCheongStrawberry", "CheongJarCheongStrawberryOpen", "SpoonCheongStrawberryFull", "SpoonCheongStrawberryPour", false, C("#CB3F52")),
                ("cheong_grapefruit", "CheongJarCheongGrapefruit", "CheongJarCheongGrapefruitOpen", "SpoonCheongGrapefruitFull", "SpoonCheongGrapefruitPour", false, C("#EF8655")),
                ("cheong_yuja", "CheongJarCheongYuja", "CheongJarCheongYujaOpen", "SpoonCheongYujaFull", "SpoonCheongYujaPour", false, C("#EFC24B")),
                ("cheong_lemon", "CheongJarCheongLemon", "CheongJarCheongLemonOpen", "SpoonCheongLemonFull", "SpoonCheongLemonPour", false, C("#F3E25A")),
                ("matcha", "MatchaTin", "MatchaTinOpen", "SpoonMatchaFull", "SpoonMatchaPour", true, C("#8DB255")),
                ("cinnamon", "CinnamonJar", "CinnamonJarOpen", "SpoonCinnamonFull", "SpoonCinnamonPour", true, C("#9A5530")),
                ("tea_chamomile", "TeaBoxChamomile", null, null, null, false, Color.white),
                ("tea_earlgrey", "TeaBoxEarlgrey", null, null, null, false, Color.white),
            };
            var arts = so.FindProperty("arts");
            arts.arraySize = table.Length;
            for (int i = 0; i < table.Length; i++)
            {
                var row = table[i];
                var element = arts.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("ingredientId").stringValue = row.id;
                element.FindPropertyRelative("idle").objectReferenceValue = Optional(row.idle);
                element.FindPropertyRelative("active").objectReferenceValue = Optional(row.active);
                element.FindPropertyRelative("spoon").objectReferenceValue = Optional(row.spoon);
                element.FindPropertyRelative("spoonPour").objectReferenceValue = Optional(row.spoonPour);
                element.FindPropertyRelative("powder").boolValue = row.powder;
                element.FindPropertyRelative("streamColor").colorValue = row.color;
            }

            Sprite Optional(string file) => file != null ? S(F, file) : null;
        }

        // ───────── 쉐이크 제조실 (SFR-052~055) ─────────

        // 좌표는 Art/UI/ShakeRoom/Generation.md "배치 (2026-11-12)" 표 (창 1600x900 왼쪽 위 기준, ADR-0029)
        public static ShakeRoomScreen BuildShakeRoom(Transform canvas)
        {
            const string F = "ShakeRoom";
            var popup = Screen<ShakeRoomScreen>(canvas, F, "ShakeRoomScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);
            Label(window, "Hint", S(F, "Hint"), 800, 74.5f, PivotCenter);
            // '원액'·'우유'·'커피잔 자리' 글자는 빼고 뚜껑 안내만 둔다 (2026-11-17 사용자 요청)
            Label(window, "LabelLid", S(F, "LabelLid"), 613.5f, 709.5f, PivotCenter);

            // 컵 자리
            AddImage(TopLeft("Coaster", window, 1178, 660, 280, 70), S(F, "CupCoaster"), Spot);
            var cupImage = AddImage(TopLeft("Cup", window, 1190, 370, 256, 320), null, Color.clear);
            cupImage.enabled = false;
            Set(so, "cupImage", cupImage);
            Set(so, "cupContents", CupContents(window, 1190, 370, 256, 320));
            SetArray(so, "cupSprites", new Object[]
            {
                S("DrinkRoom", "BigCupCeramic"), S("DrinkRoom", "BigCupCeramicWater"), S("DrinkRoom", "BigCupCeramicMixing"),
                S("DrinkRoom", "BigCupPlastic"), S("DrinkRoom", "BigCupPlasticWater"), S("DrinkRoom", "BigCupPlasticMixing"),
            });
            Set(so, "cupSpot", Zone(window, "CupSpot", 1178, 370, 280, 360, null, Invisible));

            // 원액 통 + 꽂힌 국자. 6통을 위아래 선반 2단에 3통씩 (2026-11-18 사용자 요청). 선반 윗면 y 는 Panel.png 기준
            string[] flavors = { "Strawberry", "Mango", "Hallabong", "Melon", "Cookie", "Vanilla" };
            string[] baseIds = { "smoothie_strawberry", "smoothie_mango", "smoothie_hallabong", "smoothie_melon", "smoothie_cookie", "vanilla_ice_cream" };
            const float jarW = 120, jarH = 160, jarStep = 200, jarLeft = 64; // 선반 x24~624 에 3통 고르게
            float[] shelfTops = { 265, 440 }; // 윗단, 아랫단 선반 윗면 = 통 바닥 (Panel.png Generation.md)
            var ids = so.FindProperty("baseIds");
            ids.arraySize = flavors.Length;
            var jars = new Object[flavors.Length];
            var scoops = new Object[flavors.Length];
            for (int i = 0; i < flavors.Length; i++)
            {
                ids.GetArrayElementAtIndex(i).stringValue = baseIds[i];
                var jar = TopLeft($"Jar{flavors[i]}", window, jarLeft + i % 3 * jarStep, shelfTops[i / 3] - jarH, jarW, jarH);
                AddImage(jar, S(F, $"Jar{flavors[i]}"), Cream, true);
                jars[i] = jar.gameObject.AddComponent<PointerDragRelay>();
                var scoop = TopLeft("Scoop", jar, 0, 0, jarW, jarH);
                AddImage(scoop, S(F, "ScoopInJar"), Color.clear);
                scoops[i] = scoop.gameObject;
            }
            SetArray(so, "baseJars", jars);
            SetArray(so, "jarScoops", scoops);
            SetArray(so, "scoopSprites", flavors.Select(f => (Object)S(F, $"Scoop{f}")).ToArray());
            so.FindProperty("scoopSize").vector2Value = new Vector2(280, 120);
            so.FindProperty("scoopPivot").vector2Value = new Vector2(30f / 560, 1 - 100f / 240);

            // 우유 팩 (DrinkTools 그림 재사용, 1000x800 → 220x176). 잡는 영역은 팩 몸통만
            // 원액 통과 어울리는 크기로 카운터 상판 위에 둔다 (Art/UI/ShakeRoom/Generation.md "우유 팩 배치 수정 (2026-11-17)")
            var milk = TopLeft("Milk", window, 240, 506, 220, 176);
            Set(so, "milkImage", AddImage(milk, S("DrinkTools", "MilkCarton"), Cream));
            var milkGrab = TopLeft("Grab", milk, 15.4f, 52.8f, 86.46f, 121.44f);
            AddImage(milkGrab, null, Color.clear, true).color = Color.clear; // 투명한 잡기 영역 (AddImage 는 sprite 없는 clear 를 아이콘 자리로 보고 흰색으로 칠함)
            Set(so, "milkCarton", milkGrab.gameObject.AddComponent<PointerDragRelay>());
            Set(so, "milkIdle", S("DrinkTools", "MilkCarton"));
            Set(so, "milkPour", S("DrinkTools", "MilkCartonPour"));
            so.FindProperty("milkSize").vector2Value = new Vector2(220, 176);
            so.FindProperty("milkPivot").vector2Value = new Vector2(107f / 400, 1 - 170f / 320);
            // 붓는 팩도 같은 배율. 출구(원본 900,340 → 0.22배 198,74.8)를 기존 (840,221)에 고정해 우유 줄기 위치는 그대로
            so.FindProperty("milkPourRect").rectValue = new Rect(642, 146.2f, 220, 176);

            // 붓는 줄기: 컵보다 앞, 용기보다 뒤. 회색 그림을 맛 색으로 물들인다
            Set(so, "pourStream", Loop(window, "PourStream", "SmoothieStream", 1312, 337, 48, 180));

            // 믹서기: 흔들리는 묶음(본체 + 용기). 기준점은 아래 가운데
            var shaker = TopLeft("Shaker", window, 750, 205, 360, 535);
            shaker.pivot = new Vector2(0.5f, 0);
            shaker.anchoredPosition = new Vector2(750 + 180, -(205 + 535));
            Set(so, "shaker", shaker);
            AddImage(TopLeft("BlenderBase", shaker, 15, 355, 320, 180), S(F, "BlenderBase"), Cream);
            Set(so, "weightText", AddText(TopLeft("Weight", shaker, 97, 421, 164, 39), "0g", 28, DarkCoffee));
            // 용기는 본체 소켓에 끼워지도록 창 (780,229) — 기존 (750,205)에서 오른쪽 30, 아래 24 (Generation.md "장착 위치·뚜껑 단계")
            const float jarX = 30, jarY = 24;
            var jarRt = TopLeft("Jar", shaker, jarX, jarY, 360, 390);
            jarRt.pivot = new Vector2(0.45f, 0.12f); // 붓기 회전 기준점
            jarRt.anchoredPosition = new Vector2(jarX + 0.45f * 360, -(jarY + (1 - 0.12f) * 390));
            Set(so, "jar", jarRt);
            AddImage(jarRt, S(F, "BlenderJar"), Cream);
            Set(so, "chunkyImage", Contents("Chunky"));
            Set(so, "smoothImage", Contents("Smooth"));
            var whirl = Loop(jarRt, "Whirl", "BlendWhirl", 55, 108, 195, 237);
            Fill(whirl);
            Set(so, "whirl", whirl);
            AddImage(TopLeft("JarFront", jarRt, 0, 0, 360, 390), S(F, "BlenderJarFront"), Color.clear);
            var lid = TopLeft("Lid", jarRt, 0, 0, 360, 390);
            AddImage(lid, S(F, "BlenderLid"), Color.clear);
            lid.gameObject.SetActive(false);
            Set(so, "lid", lid.gameObject);
            SetArray(so, "chunkySprites", flavors.Append("Plain").Select(f => (Object)S(F, $"Chunky{f}")).ToArray());
            SetArray(so, "smoothSprites", flavors.Append("Plain").Select(f => (Object)S(F, $"Smooth{f}")).ToArray());
            var colors = so.FindProperty("flavorColors");
            var palette = new Color[]
            {
                new Color32(0xED, 0x84, 0x94, 0xFF), new Color32(0xF8, 0xB9, 0x3A, 0xFF), new Color32(0xF3, 0x91, 0x3A, 0xFF),
                new Color32(0xA9, 0xD3, 0x6E, 0xFF), new Color32(0xD9, 0xD2, 0xC8, 0xFF), new Color32(0xF3, 0xE5, 0xCD, 0xFF), // 바닐라 (ShakeRoom/Generation.md 권장색)
                new Color32(0xF1, 0xE6, 0xD2, 0xFF),
            };
            colors.arraySize = palette.Length;
            for (int i = 0; i < palette.Length; i++) colors.GetArrayElementAtIndex(i).colorValue = palette[i];
            // 붓는 자세: 용기 기준점 창 (1030,390) → Shaker 왼쪽 위 기준, 오른쪽으로 60° 기울임
            so.FindProperty("pourPosition").vector2Value = new Vector2(1030 - 750, -(390 - 205));
            so.FindProperty("pourAngle").floatValue = -60;

            // 우유 줄기 (출구 고정, 길이는 화면이 수면에 맞춤)
            Set(so, "milkStream", Loop(window, "MilkStream", "MilkStream", 822, 221, 36, 200, "DrinkTools").rectTransform);
            so.FindProperty("milkStreamFit").vector3Value = new Vector3(221, 337, 237); // 출구, 내용물 위끝(용기를 24 내림), 내용물 높이

            // 국자·우유 판정 영역 (용기 + 그 위), 손의 얼음 스쿱을 놓는 곳 (용기)
            Set(so, "blenderArea", TopLeft("BlenderArea", window, 770, 60, 380, 559));
            Set(so, "blenderZone", Zone(window, "BlenderZone", 780, 229, 360, 390, null, Invisible));

            // 뚜껑 (ADR-0036): 카운터의 뚜껑 자리 ↔ 용기 위. 덮인 뚜껑 잡는 영역은 얼음 놓는 곳보다 위
            var lidRest = TopLeft("LidRest", window, 470, 610, 287, 72);
            Set(so, "lidRestImage", AddImage(lidRest, S(F, "LidLoose"), Cream, true));
            Set(so, "lidRestGrab", lidRest.gameObject.AddComponent<PointerDragRelay>());
            var lidOnJar = TopLeft("LidGrabOnJar", window, 786, 229, 279, 64);
            AddImage(lidOnJar, null, Invisible, true);
            Set(so, "lidJarGrab", lidOnJar.gameObject.AddComponent<PointerDragRelay>());
            Set(so, "lidDropArea", TopLeft("LidDrop", window, 760, 205, 335, 130));
            Set(so, "lidLoose", S(F, "LidLoose"));
            so.FindProperty("lidSize").vector2Value = new Vector2(287, 72);
            so.FindProperty("lidPivot").vector2Value = new Vector2(0.480836f, 0.694444f);

            var blend = Btn(window, "BlendButton", S(F, "BlendButton"), 695, 758, 220, 80);
            blend.transition = Selectable.Transition.SpriteSwap;
            blend.spriteState = new SpriteState { pressedSprite = S(F, "BlendButtonPressed") };
            Set(so, "blendHold", blend.gameObject.AddComponent<HoldButton>());
            Set(so, "pourButton", Btn(window, "PourButton", S(F, "PourButton"), 945, 758, 220, 80));
            Set(so, "blendProgress", Progress(window, "BlendProgress", 780, 160, 360));

            Set(so, "result", Result(window));
            Set(so, "tooltip", Tooltip(window));
            var drag = AddImage(TopLeft("DragImage", window, 0, 0, 100, 100), null, Color.clear);
            drag.raycastTarget = false;
            drag.enabled = false;
            Set(so, "dragImage", drag);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;

            Image Contents(string kind)
            {
                var image = AddImage(TopLeft(kind, jarRt, 55, 108, 195, 237), S(F, $"{kind}Plain"), Color.clear);
                Fill(image);
                return image;
            }

            static void Fill(Image image)
            {
                image.type = Image.Type.Filled;
                image.fillMethod = Image.FillMethod.Vertical;
                image.fillOrigin = (int)Image.OriginVertical.Bottom;
                image.raycastTarget = false;
            }

            Image Loop(RectTransform parent, string name, string file, float x, float y, float w, float h, string folder = F)
            {
                var frames = Enumerable.Range(0, 16).Select(n => S(folder, $"{file}{n}")).TakeWhile(sprite => sprite != null).ToArray();
                var rt = TopLeft(name, parent, x, y, w, h);
                var image = AddImage(rt, frames.Length > 0 ? frames[0] : null, Invisible);
                image.raycastTarget = false;
                var loop = rt.gameObject.AddComponent<SpriteLoop>();
                loop.frames = frames.Length > 0 ? frames : new Sprite[] { null };
                loop.framesPerSecond = 10;
                rt.gameObject.SetActive(false);
                return image;
            }
        }

        // ───────── 퇴식구 (SFR-059~062) ─────────

        public static TrayReturnScreen BuildTrayReturn(Transform canvas, int slots)
        {
            const string F = "TrayReturn";
            // 좌표는 Art/UI/TrayReturn/Generation.md "배치 (2026-11-12)" 표 (창 1600x900 왼쪽 위 기준)
            // 정면에서 본 수납장: 오크 상판(윗면 y 474~516) 위에 쟁반, 아래 왼쪽 칸 쓰레기통.
            // 오른쪽 칸의 설거지통은 없앴다: 잔·접시는 손으로 가져가 설거지실 대기 바구니에 넣는다 (ADR-0052)
            var popup = Screen<TrayReturnScreen>(canvas, F, "TrayReturnScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);
            Label(window, "LabelTitle", S(F, "LabelTitle"), 800, 62, PivotCenter); // 퇴식구 제목은 남긴다 (ADR-0019)
            Label(window, "Hint", S(F, "Hint"), 800, 112, PivotCenter);

            // 쟁반 칸: 상판 위에 가로로 나란히 (4칸 기준 x = 75, 445, 815, 1185). 쟁반 아래에 늘 보이는 자리 매트.
            // 퇴식구 업그레이드 최대 칸 수만큼 만들고, 화면이 그날 칸 수에 맞춰 다시 배치한다 (ADR-0057)
            const float trayW = 340, trayH = 44, trayBottom = 512;
            float step = slots > 1 ? (1525 - 75 - trayW) / (slots - 1) : 0;
            var trays = new Object[slots];
            var mats = new Object[slots];
            var containers = new Object[slots];
            DragItem template = null;
            for (int i = 0; i < slots; i++)
            {
                float x = slots > 1 ? 75 + i * step : 630;
                var mat = TopLeft($"TraySlot {i + 1}", window, x - 10, trayBottom - 14, trayW + 20, 22);
                AddImage(mat, S(F, "TraySlot"), Spot);
                mats[i] = mat;
                var tray = TopLeft($"Tray {i + 1}", window, x, trayBottom - trayH, trayW, trayH);
                AddImage(tray, S(F, "Tray"), new Color32(0x8B, 0x5A, 0x3C, 0xFF));
                // 물건 컨테이너: 아래 가장자리가 쟁반 윗면(쟁반 위끝 + 14). 물건은 화면이 줄 단위로 배치한다
                var items = TopLeft("Items", tray, 0, 14 - 300, trayW, 300);
                trays[i] = tray.gameObject;
                containers[i] = items;
                if (template == null) template = Draggable(items, "Item", null, 96, 96);
            }
            SetArray(so, "trays", trays);
            SetArray(so, "slotMats", mats);
            SetArray(so, "itemContainers", containers);
            Set(so, "itemTemplate", template);

            // 쓰레기통 칸 (왼쪽 아래 열린 칸 전체가 놓는 자리)
            var trash = Zone(window, "TrashBin", 62, 556, 426, 296, null, Invisible);
            AddImage(TopLeft("Bin", trash.transform, 155 - 62, 600 - 556, 240, 250), S(F, "TrashBin"), Cream);
            Label(trash.transform, "LabelTrash", S(F, "LabelTrash"), 275 - 62, 590 - 556, PivotCenter);
            Set(so, "trashBin", trash);

            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // ───────── 설거지실 (SFR-063~065) ─────────

        public static DishwashScreen BuildDishwash(Transform canvas)
        {
            const string F = "Dishwash";
            // 좌표는 Art/UI/Dishwash/Generation.md "배치 (2026-11-12)" 표 (창 1600x900 왼쪽 위 기준)
            var popup = Screen<DishwashScreen>(canvas, F, "DishwashScreen", 1600, 900, out var window);
            var so = new SerializedObject(popup);

            // 설거지 대기 바구니: 뒤판 → 대기 잔 3개(조금씩 기울여 담음) → 앞판. 손 칸의 더러운 잔·접시를 놓는 자리 (ADR-0052)
            var crate = TopLeft("WaitingCrate", window, 60, 385, 340, 230);
            AddImage(crate, S(F, "WaitingCrate"), Cream, true);
            Set(so, "waitingZone", crate.gameObject.AddComponent<DropZone>());
            var crateItems = new Object[3];
            float[] tilt = { 8, -6, 10 };
            for (int i = 0; i < crateItems.Length; i++) crateItems[i] = CenterIcon(crate, $"Waiting {i + 1}", 85 + i * 85, 85, 130, tilt[i]);
            SetArray(so, "crateItems", crateItems);
            Overlay(crate, "Front", S(F, "WaitingCrateFront"), 0, 0, 340, 230);
            Label(window, "LabelWaiting", S(F, "LabelWaiting"), 230, 690, PivotCenter);
            Set(so, "waitingText", AddText(TopLeft("Waiting", window, 130, 712, 200, 48), "0개", 32, DarkCoffee));

            // 싱크대: 뒤판(수전·개수대) → 잔(깨끗한 그림 + 더러운 그림 + 거품) → 헹굼 물줄기 → 앞 테두리 → 방울 층 → 수세미
            var sink = TopLeft("Sink", window, 450, 300, 680, 560);
            AddImage(sink, S(F, "Sink"), new Color32(0xB8, 0xC0, 0xC4, 0xFF));
            var item = CenterIcon(sink, "Item", 340, 312, 220, 0); // 잔 바닥이 앞 테두리(y 398) 뒤에 살짝 잠긴다
            Set(so, "sinkItem", item);
            var stain = AddImage(Centered("Stain", item.transform, 0, 0, 220, 220), null, Color.clear);
            stain.preserveAspect = true;
            stain.enabled = false;
            Set(so, "stain", stain);
            var suds = AddImage(Centered("Suds", item.transform, 0, -42, 190, 123), S(F, "Suds"), Color.clear);
            suds.enabled = false;
            Set(so, "suds", suds);

            var frames = Enumerable.Range(0, 16).Select(n => S("Water", $"WaterStream{n}")).TakeWhile(sprite => sprite != null).ToArray();
            var rinse = TopLeft("Rinse", sink, 322, 116, 36, 190); // 수전 끝(340, 116)에서 잔 안까지
            AddImage(rinse, frames.Length > 0 ? frames[0] : null, Invisible);
            rinse.gameObject.AddComponent<SpriteLoop>().frames = frames.Length > 0 ? frames : new Sprite[] { null };
            rinse.gameObject.SetActive(false);
            Set(so, "rinse", rinse.gameObject);
            Overlay(sink, "SinkFront", S(F, "SinkFront"), 0, 0, 680, 560);

            var fx = NewUI("Fx", sink);
            Stretch(fx);
            Set(so, "fxLayer", fx);
            var bubble = AddImage(Centered("Bubble", fx, 0, 0, 32, 32), S(F, "Bubble"), Color.clear);
            bubble.gameObject.SetActive(false);
            Set(so, "bubbleTemplate", bubble);
            var sparkles = new Object[3];
            for (int i = 0; i < sparkles.Length; i++)
            {
                var sparkle = AddImage(Centered($"Sparkle {i + 1}", fx, 0, 0, 52, 52), S(F, "Sparkle"), Color.clear);
                sparkle.gameObject.SetActive(false);
                sparkles[i] = sparkle;
            }
            SetArray(so, "sparkles", sparkles);

            // 수세미: 잔 아래쪽에 놓고 좌우로 끈다. 문지를 때 가운데를 기준으로 기울고 눌린다
            var sponge = Tool(sink, "Sponge", S(F, "Sponge"), 325, 340, 150, 97, new Vector2(170, 24));
            var spongeRt = (RectTransform)sponge.transform;
            spongeRt.pivot = PivotCenter;
            spongeRt.anchoredPosition += new Vector2(75, -48.5f);
            Set(so, "sponge", sponge);

            // 건조대 2단 3칸: 칸 가운데 x = 70, 190, 310, 잔은 거꾸로 엎어 레일(y 176, 356)에 걸친다
            var rack = TopLeft("DryingRack", window, 1160, 215, 380, 400);
            AddImage(rack, S(F, "DryingRack"), Cream);
            var cups = new Object[6];
            for (int i = 0; i < cups.Length; i++)
                cups[i] = CenterIcon(rack, $"Slot {i + 1}", 70 + (i % 3) * 120, (i < 3 ? 176 : 356) - 21, 110, 180);
            SetArray(so, "rack", cups);
            Set(so, "dryingLabel", Label(window, "LabelDrying", S(F, "LabelDrying"), 1350, 690, PivotCenter).gameObject);

            // 다 닦은 잔이 건조대로 날아가는 그림 (창 맨 위에 그림)
            var flyer = CenterIcon(window, "Flyer", 0, 0, 220, 0);
            var flyerSuds = AddImage(Centered("Suds", flyer.transform, 0, -42, 190, 123), S(F, "Suds"), Color.clear);
            flyer.gameObject.SetActive(false);
            Set(so, "flyer", flyer);
            Set(so, "flyerSuds", flyerSuds);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // 가운데 기준 아이콘 (cx, cy 는 부모 왼쪽 위 기준). 기울이거나 뒤집어도 가운데를 축으로 돈다
        static Image CenterIcon(Transform parent, string name, float cx, float cy, float size, float angle)
        {
            var rt = TopLeft(name, parent, cx, cy, size, size);
            rt.pivot = PivotCenter;
            rt.localRotation = Quaternion.Euler(0, 0, angle);
            var image = AddImage(rt, null, Color.clear);
            image.preserveAspect = true;
            image.enabled = false; // 물건이 없으면 숨김 (화면이 Refresh 에서 켬)
            return image;
        }

        // 물건 앞에 겹치는 덮개 그림 (클릭·드롭을 가리지 않음)
        static void Overlay(Transform parent, string name, Sprite sprite, float x, float y, float w, float h) =>
            AddImage(TopLeft(name, parent, x, y, w, h), sprite, Color.clear).raycastTarget = false;

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
