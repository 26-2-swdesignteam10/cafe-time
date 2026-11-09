using System.Linq;
using CafeTime.Cafe;
using CafeTime.Core;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // Cafe 씬의 UI: 상시 HUD(SFR-013), 주문표, 양손 칸(SFR-021), 안내 문구, 기기 화면 팝업들. CafeSceneBuilder 가 호출한다
    // 모든 창·버튼·고정 문구는 디자인 에셋 (docs/ART_STYLE.md 4장). 좌표는 1920x1080 기준, 부모 왼쪽 위 기준
    public static class CafeUiBuilder
    {
        const string Hud = "Assets/_Project/Art/UI/CafeHud";
        const string Pos = "Assets/_Project/Art/UI/Pos";
        const string Stations = "Assets/_Project/Art/UI/Stations";
        const string OpenPrep = "Assets/_Project/Art/UI/OpenPrep";
        const string Settlement = "Assets/_Project/Art/UI/Settlement";
        const string CloseIcon = "Assets/_Project/Art/UI/Stations/Close.png"; // 기기 작업 창 공용 닫기 (일러스트)
        const string QuitPanel = "Assets/_Project/Art/UI/Quit/Panel.png";

        public class Screens
        {
            public SpeechPopup Speech;
            public PosScreen Pos;
            public PickupScreen Pickup;
            public DessertShowcaseScreen Dessert;
            public CupShelfScreen CupShelf;
            public CafeTime.UI.Popup Espresso;
            public IceMakerScreen IceMaker;
            public WaterScreen Water;
            public DrinkRoomScreen DrinkRoom;
            public ShakeRoomScreen ShakeRoom;
            public TrayReturnScreen TrayReturn;
            public DishwashScreen Dishwash;
            public PauseMenu Pause;
        }

        public static Screens Build(CafeSceneBuilder.Managers managers)
        {
            foreach (var folder in new[] { Hud, OpenPrep, Settlement }) PrepareSprites(folder);
            // 기기 작업 창은 도트가 아닌 일러스트라 부드럽게 표시한다 (docs/ART_STYLE.md 1장)
            foreach (var folder in new[] { Pos, Stations }) PrepareSprites(folder, FilterMode.Bilinear);
            StationUiBuilder.PrepareAll();
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

            // 기기 화면 아래, HUD(주문표·양손 칸·안내)는 기기 화면 위에 그린다 → 픽업대 화면에서 손 칸을 끌고 주문표를 누를 수 있음
            var main = NewCanvas("Canvas", 0);
            var overlay = NewCanvas("HudCanvas", 10);

            var hud = BuildStatus(main);
            var screens = new Screens
            {
                Speech = BuildSpeech(main),
                CupShelf = StationUiBuilder.BuildCupShelf(main),
                Espresso = StationUiBuilder.BuildEspresso(main, managers.Business.GetComponent<CafeTime.EspressoShot.EspressoMachine>()),
                IceMaker = StationUiBuilder.BuildIceMaker(main),
                Water = StationUiBuilder.BuildWater(main),
                DrinkRoom = StationUiBuilder.BuildDrinkRoom(main),
                ShakeRoom = StationUiBuilder.BuildShakeRoom(main),
                TrayReturn = StationUiBuilder.BuildTrayReturn(main, GameBalance.Instance.TrayReturnSlots),
                Dishwash = StationUiBuilder.BuildDishwash(main),
                Dessert = BuildDessert(main),
                Pickup = BuildPickup(main),
            };
            var receipt = BuildReceipt(main);
            screens.Pos = BuildPos(main, receipt);
            receipt.transform.SetAsLastSibling();

            BuildTickets(overlay, hud);
            BuildHands(overlay);
            BuildToast(overlay);
            var banner = Centered("ClosedBanner", overlay, 0, 100, 900, 220);
            AddImage(banner, LoadSprite($"{Hud}/BannerClosed.png"), Cream);
            screens.Pause = StationUiBuilder.BuildPause(overlay); // HUD 위

            var day = new SerializedObject(managers.Day);
            Set(day, "clock", managers.Clock);
            Set(day, "customers", managers.Customers);
            Set(day, "closedBanner", banner.gameObject);
            day.ApplyModifiedPropertiesWithoutUndo();
            return screens;
        }

        static Transform NewCanvas(string name, int order)
        {
            var go = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = order;
            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            return go.transform;
        }

        // ───────── 상시 HUD (SFR-013) ─────────

        static CafeHud BuildStatus(Transform canvas)
        {
            var hud = canvas.gameObject.AddComponent<CafeHud>();
            var so = new SerializedObject(hud);

            // 1줄: DAY  시각  [보유금] ₩    2줄: [임대료] ₩   [평판] ★★★★★
            var bar = TopLeft("StatusBar", canvas, 20, 20, 760, 120);
            AddImage(bar, LoadSprite($"{Hud}/StatusBar.png"), new Color32(0x90, 0x47, 0x1B, 0xFF));
            Set(so, "dayText", AddText(TopLeft("Day", bar, 40, 20, 140, 52), "DAY 1", 26, Cream, TextAlignmentOptions.Left));
            Set(so, "clockText", AddText(TopLeft("Clock", bar, 176, 20, 130, 52), "09:00", 25, Cream, TextAlignmentOptions.Left));
            Label(bar, "MoneyLabel", LoadSprite($"{OpenPrep}/LabelMoney.png"), 318, 43, PivotLeft, 0.4f);
            Set(so, "moneyText", AddText(TopLeft("Money", bar, 440, 20, 290, 52), "₩0", 26, Cream, TextAlignmentOptions.Left));
            Label(bar, "RentLabel", LoadSprite($"{Hud}/LabelRent.png"), 36, 80, PivotLeft, 0.4f);
            Set(so, "rentText", AddText(TopLeft("Rent", bar, 126, 55, 190, 48), "₩0", 20, Cream, TextAlignmentOptions.Left));
            Label(bar, "ReputationLabel", LoadSprite($"{Hud}/LabelReputation.png"), 318, 80, PivotLeft, 0.4f);
            var starFull = LoadSprite($"{Settlement}/Star.png");
            var stars = new Object[5];
            for (int i = 0; i < stars.Length; i++)
                stars[i] = AddImage(TopLeft($"Star {i + 1}", bar, 385 + i * 34, 65, 30, 30), starFull, Cream);
            SetArray(so, "stars", stars);
            Set(so, "starFull", starFull);
            Set(so, "starEmpty", LoadSprite($"{Settlement}/StarEmpty.png"));

            // 주요 재료 재고 (원두, 우유)
            var stock = TopLeft("StockBar", canvas, 795, 20, 300, 120);
            AddImage(stock, LoadSprite($"{Hud}/StockBar.png"), new Color32(0x90, 0x47, 0x1B, 0xFF));
            var icons = new Object[2];
            var texts = new Object[2];
            for (int i = 0; i < 2; i++)
            {
                icons[i] = AddImage(TopLeft($"StockIcon {i}", stock, 34, 20 + i * 38, 40, 40), null, Color.clear);
                ((Image)icons[i]).preserveAspect = true;
                texts[i] = AddText(TopLeft($"StockText {i}", stock, 90, 20 + i * 38, 190, 44), "0", 24, Cream, TextAlignmentOptions.Left);
            }
            SetArray(so, "stockIcons", icons);
            SetArray(so, "stockTexts", texts);
            so.ApplyModifiedPropertiesWithoutUndo();
            return hud;
        }

        static void BuildTickets(Transform overlay, CafeHud hud)
        {
            var so = new SerializedObject(hud);
            var container = TopLeft("Tickets", overlay, 1115, 12, 790, 150);
            var layout = container.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 8;
            layout.childControlWidth = layout.childControlHeight = false;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;

            var ticket = NewUI("Ticket", container);
            ticket.sizeDelta = new Vector2(190, 146);
            var button = AddButton(ticket, LoadSprite($"{Hud}/Ticket.png"), Cream);
            button.transition = Selectable.Transition.SpriteSwap;
            var swap = button.spriteState;
            swap.highlightedSprite = swap.pressedSprite = LoadSprite($"{Hud}/TicketSelected.png");
            button.spriteState = swap;
            var view = ticket.gameObject.AddComponent<TicketView>();
            var tso = new SerializedObject(view);
            Set(tso, "button", button);
            Set(tso, "numberText", AddText(TopLeft("Number", ticket, 16, 38, 158, 30), "#1", 24, DarkCoffee, TextAlignmentOptions.Left));
            var lines = AddText(TopLeft("Lines", ticket, 16, 68, 160, 70), "", 16, TextBrown, TextAlignmentOptions.TopLeft);
            lines.textWrappingMode = TextWrappingModes.Normal;
            Set(tso, "linesText", lines);
            tso.ApplyModifiedPropertiesWithoutUndo();

            Set(so, "ticketContainer", container);
            Set(so, "ticketTemplate", view);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // 화면 왼쪽 아래 양손 칸 + 우클릭 메뉴 (SFR-021)
        static void BuildHands(Transform overlay)
        {
            var root = TopLeft("Hands", overlay, 20, 915, 320, 150);
            var view = root.gameObject.AddComponent<HandInventoryView>();
            var so = new SerializedObject(view);
            var slots = new Object[2];
            for (int i = 0; i < 2; i++)
            {
                var slot = TopLeft(i == 0 ? "LeftHand" : "RightHand", root, i * 165, 0, 150, 150);
                var frame = AddImage(slot, LoadSprite($"{Hud}/{(i == 0 ? "HandSlotLeft" : "HandSlotRight")}.png"), Cream, true);
                var icon = AddImage(Centered("Icon", slot, 0, 0, 104, 104), null, Color.clear);
                icon.preserveAspect = true;
                var slotView = slot.gameObject.AddComponent<HandSlotView>();
                var sso = new SerializedObject(slotView);
                sso.FindProperty("hand").intValue = i;
                Set(sso, "frame", frame);
                Set(sso, "icon", icon);
                Set(sso, "owner", view);
                sso.ApplyModifiedPropertiesWithoutUndo();
                slots[i] = slotView;
            }
            SetArray(so, "slots", slots);

            var menu = NewUI("ContextMenu", overlay);
            menu.pivot = new Vector2(0, 0);
            menu.sizeDelta = new Vector2(220, 90);
            AddImage(menu, LoadSprite($"{Hud}/ContextMenu.png"), Cream, true);
            Set(so, "contextMenu", menu);
            Set(so, "discardButton", AddButton(Centered("DiscardButton", menu, 0, 0, 190, 62), LoadSprite($"{Hud}/DiscardButton.png"), Cream));

            var dragLayer = NewUI("DragLayer", overlay);
            Stretch(dragLayer);
            Set(so, "dragLayer", dragLayer);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void BuildToast(Transform overlay)
        {
            var root = new GameObject("Toast", typeof(RectTransform)).transform;
            root.SetParent(overlay, false);
            var toast = root.gameObject.AddComponent<Toast>();
            var panel = Centered("Panel", root, 0, 330, 760, 96);
            AddImage(panel, LoadSprite($"{Hud}/Toast.png"), Cream);
            var label = CenterLabel(panel, "Label", null);
            var so = new SerializedObject(toast);
            Set(so, "panel", panel.gameObject);
            Set(so, "label", label);
            // ToastMessage 순서. 파일 이름 = "Toast" + 값 이름
            SetArray(so, "messages", System.Enum.GetNames(typeof(ToastMessage))
                .Select(n => (Object)LoadSprite($"{Hud}/Toast{n}.png")).ToArray());
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // ───────── 팝업 공통 ─────────

        // 반투명 배경 + 가운데 창. 창 크기는 그림 기준 좌표계, scale 로 화면 크기 조절
        internal static (T popup, RectTransform window) Window<T>(Transform canvas, string name, Sprite panel, float width, float height,
            float y = 0, float scale = 1f, float dim = 0.45f) where T : Popup
        {
            var root = NewUI(name, canvas);
            Stretch(root);
            var dimImage = root.gameObject.AddComponent<Image>();
            dimImage.color = new Color(0, 0, 0, dim); // 뒤쪽 클릭 차단
            var popup = root.gameObject.AddComponent<T>();
            var window = Centered("Window", root, 0, y, width, height);
            window.localScale = Vector3.one * scale;
            AddImage(window, panel, Cream, true);
            root.gameObject.SetActive(false);
            return (popup, window);
        }

        internal static Button CloseButton(RectTransform window, float x, float y, float size = 64)
        {
            return AddButton(TopLeft("CloseButton", window, x, y, size, size), LoadSprite(CloseIcon), Cream);
        }

        internal static void SetClose(Popup popup, Button close)
        {
            var so = new SerializedObject(popup);
            Set(so, "closeButton", close);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // 메뉴 버튼 견본 (이름 + 가격). 화면이 메뉴 수만큼 복제한다
        static MenuButtonView MenuTemplate(Transform parent, Sprite frame, Vector2 size, bool withIcon)
        {
            var rt = NewUI("MenuButton", parent);
            rt.sizeDelta = size;
            var button = AddButton(rt, frame, Cream);
            var view = rt.gameObject.AddComponent<MenuButtonView>();
            var so = new SerializedObject(view);
            Set(so, "button", button);
            if (withIcon)
            {
                // 카드형 (디저트 진열장): OpenPrep/CardFrame.png 와 같은 배치
                var icon = AddImage(TopLeft("Icon", rt, 56, 24, 88, 86), null, Color.clear);
                icon.preserveAspect = true;
                Set(so, "icon", icon);
                Set(so, "nameText", AddText(TopLeft("Name", rt, 10, 118, 180, 28), "", 22, DarkCoffee));
                Label(rt, "StockLabel", LoadSprite($"{OpenPrep}/LabelStock.png"), 96, 168, PivotRight, 0.3f);
                Set(so, "detailText", AddText(TopLeft("Detail", rt, 102, 157, 95, 22), "", 17, TextBrown, TextAlignmentOptions.Left));
            }
            else
            {
                float w = size.x - 30;
                Set(so, "nameText", AddText(TopLeft("Name", rt, 15, size.y * 0.14f, w, size.y * 0.42f), "", 24, DarkCoffee));
                Set(so, "detailText", AddText(TopLeft("Price", rt, 15, size.y * 0.54f, w, size.y * 0.32f), "", 20, TextBrown));
            }
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        static RectTransform Grid(RectTransform window, string name, float x, float y, float w, float h, Vector2 cell, int columns)
        {
            var grid = TopLeft(name, window, x, y, w, h);
            var layout = grid.gameObject.AddComponent<GridLayoutGroup>();
            layout.cellSize = cell;
            layout.spacing = new Vector2(14, 14);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = columns;
            return grid;
        }

        static TemperatureSelector TemperatureButtons(RectTransform window, float x, float y)
        {
            var root = TopLeft("Temperature", window, x, y, 260, 56);
            var selector = root.gameObject.AddComponent<TemperatureSelector>();
            var so = new SerializedObject(selector);
            Set(so, "hotButton", AddButton(TopLeft("Hot", root, 0, 0, 120, 56), LoadSprite($"{Pos}/HotNormal.png"), Cream));
            Set(so, "iceButton", AddButton(TopLeft("Ice", root, 136, 0, 120, 56), LoadSprite($"{Pos}/IceNormal.png"), Cream));
            Set(so, "hotNormal", LoadSprite($"{Pos}/HotNormal.png"));
            Set(so, "hotSelected", LoadSprite($"{Pos}/HotSelected.png"));
            Set(so, "iceNormal", LoadSprite($"{Pos}/IceNormal.png"));
            Set(so, "iceSelected", LoadSprite($"{Pos}/IceSelected.png"));
            so.ApplyModifiedPropertiesWithoutUndo();
            return selector;
        }

        // ───────── 기기 화면 ─────────

        // 손님 주문 대사 (SFR-026)
        static SpeechPopup BuildSpeech(Transform canvas)
        {
            var (popup, window) = Window<SpeechPopup>(canvas, "SpeechPopup", LoadSprite($"{Hud}/SpeechPanel.png"), 1200, 200, -300, 1f, 0.2f);
            var text = AddText(TopLeft("Sentence", window, 70, 40, 1060, 110), "", 34, DarkCoffee, TextAlignmentOptions.Left);
            text.textWrappingMode = TextWrappingModes.Normal;
            var so = new SerializedObject(popup);
            Set(so, "sentenceText", text);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // 포스기 (SFR-026). 패널 그림 1600x900 기준 좌표, 화면에는 0.94배 (위쪽 주문표와 겹치지 않게 조금 아래)
        static PosScreen BuildPos(Transform canvas, ReceiptPopup receipt)
        {
            var (popup, window) = Window<PosScreen>(canvas, "PosScreen", LoadSprite($"{Pos}/Panel.png"), 1600, 900, -40, 0.94f);
            var so = new SerializedObject(popup);

            var tabNormal = LoadSprite($"{OpenPrep}/TabNormal.png");
            string[] tabLabels = { "TabCoffee", "TabDrink", "TabSmoothie", "TabDessert" };
            var tabs = new Object[tabLabels.Length];
            for (int i = 0; i < tabLabels.Length; i++)
            {
                var tab = TopLeft(tabLabels[i], window, 90 + i * 215, 172, 205, 62);
                tabs[i] = AddButton(tab, tabNormal, Cream);
                CenterLabel(tab, "Label", LoadSprite($"{Pos}/{tabLabels[i]}.png"));
            }
            SetArray(so, "categoryTabs", tabs);
            Set(so, "tabNormal", tabNormal);
            Set(so, "tabSelected", LoadSprite($"{OpenPrep}/TabSelected.png"));
            Set(so, "temperature", TemperatureButtons(window, 90, 248));

            var grid = Grid(window, "Menus", 90, 318, 870, 490, new Vector2(280, 110), 3);
            Set(so, "menuContainer", grid);
            Set(so, "menuTemplate", MenuTemplate(grid, LoadSprite($"{Pos}/MenuButton.png"), new Vector2(280, 110), false));

            // 오른쪽 주문 목록
            var rows = TopLeft("Rows", window, 1015, 180, 520, 360);
            var layout = rows.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 4;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            Set(so, "rowContainer", rows);
            Set(so, "rowTemplate", RowTemplate(rows));

            Set(so, "minusButton", AddButton(TopLeft("Minus", window, 1015, 556, 56, 56), LoadSprite($"{OpenPrep}/MinusButton.png"), Cream));
            Set(so, "plusButton", AddButton(TopLeft("Plus", window, 1081, 556, 56, 56), LoadSprite($"{OpenPrep}/PlusButton.png"), Cream));
            Set(so, "deleteButton", AddButton(TopLeft("DeleteButton", window, 1155, 550, 180, 63), LoadSprite($"{Pos}/DeleteButton.png"), Cream));
            Set(so, "cancelButton", AddButton(TopLeft("CancelButton", window, 1345, 550, 180, 63), LoadSprite($"{Pos}/CancelButton.png"), Cream));
            Label(window, "TotalLabel", LoadSprite($"{OpenPrep}/LabelTotal.png"), 1020, 660, PivotLeft);
            Set(so, "totalText", AddText(TopLeft("Total", window, 1140, 634, 390, 52), "₩0", 36, DarkCoffee, TextAlignmentOptions.Right));
            Set(so, "payButton", AddButton(TopLeft("PayButton", window, 1075, 705, 400, 91), LoadSprite($"{Pos}/PayButton.png"), Cream));
            Set(so, "receipt", receipt);
            so.ApplyModifiedPropertiesWithoutUndo();
            SetClose(popup, CloseButton(window, 1452, 86));
            return popup;
        }

        static OrderRowView RowTemplate(Transform parent)
        {
            var rt = NewUI("Row", parent);
            rt.sizeDelta = new Vector2(520, 52);
            var button = rt.gameObject.AddComponent<Button>();
            var hit = rt.gameObject.AddComponent<Image>();
            hit.color = Color.clear; // 투명한 클릭 영역
            button.targetGraphic = hit;
            var highlight = NewUI("Selected", rt);
            Stretch(highlight);
            AddImage(highlight, LoadSprite($"{Pos}/RowSelected.png"), Cream);
            var text = AddText(TopLeft("Text", rt, 12, 0, 500, 52), "", 24, TextBrown, TextAlignmentOptions.Left);
            var view = rt.gameObject.AddComponent<OrderRowView>();
            var so = new SerializedObject(view);
            Set(so, "button", button);
            Set(so, "text", text);
            Set(so, "selectedHighlight", highlight.gameObject);
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        // 주문 내역 (SFR-027)
        static ReceiptPopup BuildReceipt(Transform canvas)
        {
            var (popup, window) = Window<ReceiptPopup>(canvas, "ReceiptPopup", LoadSprite($"{Pos}/ReceiptPanel.png"), 640, 860);
            var so = new SerializedObject(popup);
            Label(window, "OrderNoLabel", LoadSprite($"{Pos}/LabelOrderNo.png"), 95, 230, PivotLeft);
            Set(so, "numberText", AddText(TopLeft("Number", window, 300, 206, 245, 48), "#1", 32, DarkCoffee, TextAlignmentOptions.Right));
            var lines = AddText(TopLeft("Lines", window, 95, 275, 450, 330), "", 24, TextBrown, TextAlignmentOptions.TopLeft);
            lines.lineSpacing = 16;
            Set(so, "linesText", lines);
            Label(window, "SumLabel", LoadSprite($"{Pos}/LabelSum.png"), 95, 650, PivotLeft);
            Set(so, "sumText", AddText(TopLeft("Sum", window, 245, 624, 300, 52), "₩0", 34, DarkCoffee, TextAlignmentOptions.Right));
            so.ApplyModifiedPropertiesWithoutUndo();
            SetClose(popup, AddButton(TopLeft("ConfirmButton", window, 170, 710, 300, 84), LoadSprite($"{Pos}/ConfirmButton.png"), Cream));
            return popup;
        }

        // 디저트 진열장 (SFR-056)
        static DessertShowcaseScreen BuildDessert(Transform canvas)
        {
            var (popup, window) = Window<DessertShowcaseScreen>(canvas, "DessertScreen", LoadSprite($"{Stations}/DessertPanel.png"), 1000, 600);
            var so = new SerializedObject(popup);
            var row = TopLeft("Desserts", window, 140, 200, 720, 252);
            var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 60;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = layout.childControlHeight = false;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;
            Set(so, "container", row);
            Set(so, "template", MenuTemplate(row, LoadSprite($"{OpenPrep}/CardFrame.png"), new Vector2(200, 252), true));
            so.ApplyModifiedPropertiesWithoutUndo();
            SetClose(popup, CloseButton(window, 866, 66));
            return popup;
        }

        // 픽업대 (SFR-056): 칸 5개 + 안내
        static PickupScreen BuildPickup(Transform canvas)
        {
            var (popup, window) = Window<PickupScreen>(canvas, "PickupScreen", LoadSprite($"{Stations}/PickupPanel.png"), 1100, 640, 40);
            var so = new SerializedObject(popup);
            var slots = new Object[5];
            for (int i = 0; i < slots.Length; i++)
            {
                var slot = TopLeft($"Slot {i + 1}", window, 75 + i * 195, 200, 180, 180);
                var background = AddImage(slot, LoadSprite($"{Stations}/PickupSlot.png"), Cream, true);
                var icon = AddImage(Centered("Icon", slot, 0, -4, 112, 112), null, Color.clear);
                icon.preserveAspect = true;
                var view = slot.gameObject.AddComponent<PickupSlotView>();
                var sso = new SerializedObject(view);
                Set(sso, "owner", popup);
                Set(sso, "background", background);
                Set(sso, "icon", icon);
                Set(sso, "orderText", AddText(TopLeft("Order", slot, 14, 10, 152, 30), "", 24, DarkCoffee, TextAlignmentOptions.Left));
                Set(sso, "unassignedLabel", Label(slot, "Unassigned", LoadSprite($"{Stations}/LabelUnassigned.png"), 90, 158, PivotCenter, 0.4f).gameObject);
                sso.ApplyModifiedPropertiesWithoutUndo();
                slots[i] = view;
            }
            SetArray(so, "slots", slots);
            so.ApplyModifiedPropertiesWithoutUndo();
            Label(window, "Hint", LoadSprite($"{Stations}/PickupHint.png"), 550, 470, PivotCenter);
            SetClose(popup, CloseButton(window, 966, 66));
            return popup;
        }
    }
}
