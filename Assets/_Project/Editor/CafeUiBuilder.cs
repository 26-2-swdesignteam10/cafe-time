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
    // Cafe 씬의 UI: 상시 HUD(SFR-013), 주문 내역·레시피북, 양손 칸(SFR-021), 안내 문구, 기기 화면 팝업들. CafeSceneBuilder 가 호출한다
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
            PrepareBook(); // 주문 내역·레시피북 그림 (픽업대도 '주문이 없습니다' 라벨을 같이 씀)
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

            // 기기 화면 아래, HUD(주문 내역 버튼·양손 칸·안내)는 기기 화면 위에 그린다 → 픽업대 화면에서 손 칸을 끌고 주문 내역을 열 수 있음
            var main = NewCanvas("Canvas", 0);
            var overlay = NewCanvas("HudCanvas", 10);

            var hud = BuildStatus(main);
            var screens = new Screens
            {
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

            var hands = BuildHands(overlay);
            var posSo = new SerializedObject(screens.Pos);
            SetArray(posSo, "hideWhileOpen", new Object[] { hands.gameObject }); // 주문 채팅이 양손 칸 자리를 쓴다
            posSo.ApplyModifiedPropertiesWithoutUndo();
            BuildOrderBook(overlay, hud, screens.Pickup); // 모달 창이 양손 칸 위에 뜨도록 그 뒤에 만든다
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

            // 주요 재료 재고(원두, 우유)는 HUD 에 표시하지 않는다 (ADR-0023)
            so.ApplyModifiedPropertiesWithoutUndo();
            return hud;
        }

        // ───────── 주문 내역·레시피북 (SFR-013, SFR-051, SFR-056) ─────────
        // HUD 오른쪽 위 버튼 2개 + 각각 여는 모달 창. 그림은 Art/UI/OrderBook (도트, Generation.md)

        const string Book = "Assets/_Project/Art/UI/OrderBook";
        const string ItemIconFolder = "Assets/_Project/Art/Icons/Items";

        // 9-slice 테두리 (원본 px, 왼·아래·오른·위). 크기가 바뀌는 그림만
        static readonly (string file, Vector4 border)[] BookBorders =
        {
            ("Receipt", new Vector4(16, 60, 16, 90)),
            ("MenuItem", new Vector4(48, 0, 24, 0)),
            ("MenuItemSelected", new Vector4(48, 0, 24, 0)),
            ("RecipeCard", new Vector4(64, 64, 64, 64)),
        };

        static Sprite B(string name) => LoadSprite($"{Book}/{name}.png");

        static void PrepareBook()
        {
            PrepareSprites(Book);
            foreach (var (file, border) in BookBorders)
            {
                if (!(AssetImporter.GetAtPath($"{Book}/{file}.png") is TextureImporter importer)) continue;
                importer.spriteBorder = border;
                importer.SaveAndReimport();
            }
        }

        // 2배 크기로 그린 9-slice 그림을 화면에 절반 크기 테두리로 표시
        static Image Sliced(RectTransform rt, Sprite sprite, bool raycast = false)
        {
            var image = AddImage(rt, sprite, Cream, raycast);
            image.type = Image.Type.Sliced;
            if (sprite != null) image.pixelsPerUnitMultiplier = 2f;
            return image;
        }

        static void BuildOrderBook(Transform overlay, CafeHud hud, PickupScreen pickup)
        {
            var so = new SerializedObject(hud);

            // HUD 버튼 (380x220 그림을 절반 크기로)
            var ordersButton = AddButton(TopLeft("OrdersButton", overlay, 1520, 22, 190, 110), B("OrdersButton"), Cream);
            var recipeButton = AddButton(TopLeft("RecipeButton", overlay, 1720, 22, 190, 110), B("RecipeButton"), Cream);
            var badge = TopLeft("Badge", ordersButton.transform, 150, -8, 48, 48);
            AddImage(badge, B("Badge"), DarkCoffee);
            Set(so, "orderCountText", StretchText(badge, "Count", "0", 24, Cream));
            Set(so, "orderBadge", badge.gameObject);
            Set(so, "ordersButton", ordersButton);
            Set(so, "recipeButton", recipeButton);
            Set(so, "ordersScreen", BuildOrdersScreen(overlay, pickup));
            Set(so, "recipeScreen", BuildRecipeBook(overlay));
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static Button BookClose(RectTransform window) =>
            AddButton(TopLeft("CloseButton", window, 1400 - 100, 36, 64, 64), B("CloseButton"), Cream);

        static OrdersScreen BuildOrdersScreen(Transform overlay, PickupScreen pickup)
        {
            var (popup, window) = Window<OrdersScreen>(overlay, "OrdersScreen", B("ModalPanel"), 1400, 800);
            SetClose(popup, BookClose(window));
            Label(window, "Title", B("TitleOrders"), 700, 72, PivotCenter, 0.7f);
            var empty = Label(window, "Empty", B("EmptyOrders"), 700, 420, PivotCenter, 0.6f);

            // 영수증은 가로로 나란히, 넘치면 마우스 휠로 가로 스크롤
            var scroll = TopLeft("Receipts", window, 60, 130, 1280, 620);
            scroll.gameObject.AddComponent<Image>().color = Color.clear; // 빈 곳에서도 휠이 먹도록
            scroll.gameObject.AddComponent<RectMask2D>();
            var content = NewUI("Content", scroll);
            content.anchorMin = content.anchorMax = content.pivot = new Vector2(0, 1);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, 620);
            var row = content.gameObject.AddComponent<HorizontalLayoutGroup>();
            row.spacing = 24;
            row.padding = new RectOffset(0, 0, 8, 0);
            row.childAlignment = TextAnchor.UpperLeft;
            row.childControlWidth = row.childControlHeight = true;
            row.childForceExpandWidth = row.childForceExpandHeight = false;
            content.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            var scrollRect = scroll.gameObject.AddComponent<ScrollRect>();
            scrollRect.content = content;
            scrollRect.viewport = scroll;
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.inertia = false;
            scrollRect.scrollSensitivity = 40;

            var so = new SerializedObject(popup);
            Set(so, "ticketContainer", content);
            Set(so, "ticketTemplate", ReceiptTemplate(content));
            Set(so, "emptyLabel", empty.gameObject);
            Set(so, "pickup", pickup);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // 영수증 1장: 높이는 메뉴 줄 수에 맞춰 늘어난다 (Receipt.png 9-slice)
        static TicketView ReceiptTemplate(Transform parent)
        {
            var rt = NewUI("Ticket", parent);
            Sliced(rt, B("Receipt"));
            rt.gameObject.AddComponent<LayoutElement>().preferredWidth = 300;
            var column = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            column.padding = new RectOffset(28, 28, 52, 40);
            column.spacing = 8;
            column.childControlWidth = column.childControlHeight = true;
            column.childForceExpandWidth = true;
            column.childForceExpandHeight = false;

            var view = rt.gameObject.AddComponent<TicketView>();
            var so = new SerializedObject(view);
            Set(so, "numberText", AddText(NewUI("Number", rt), "#1", 30, DarkCoffee, TextAlignmentOptions.Left));
            var lines = AddText(NewUI("Lines", rt), "", 20, TextBrown, TextAlignmentOptions.TopLeft);
            lines.textWrappingMode = TextWrappingModes.Normal;
            Set(so, "linesText", lines);
            var divider = NewUI("Divider", rt);
            AddImage(divider, B("ReceiptDivider"), TextBrown);
            divider.gameObject.AddComponent<LayoutElement>().preferredHeight = 6;
            Set(so, "totalText", ReceiptRow(rt, "Total", B("LabelTotal")));
            Set(so, "pickupText", ReceiptRow(rt, "Pickup", B("LabelPickup")));
            so.ApplyModifiedPropertiesWithoutUndo();
            rt.gameObject.SetActive(false);
            return view;
        }

        // 영수증 아래 줄: 왼쪽 글자 이미지(합계·픽업대) + 오른쪽 값
        static TextMeshProUGUI ReceiptRow(Transform receipt, string name, Sprite label)
        {
            var row = NewUI(name, receipt);
            row.gameObject.AddComponent<LayoutElement>().preferredHeight = 30;
            var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = layout.childControlHeight = true;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;

            var labelRt = NewUI("Label", row);
            var image = labelRt.gameObject.AddComponent<Image>();
            image.sprite = label;
            image.preserveAspect = true;
            image.raycastTarget = false;
            var size = labelRt.gameObject.AddComponent<LayoutElement>();
            size.preferredHeight = 26;
            size.preferredWidth = label != null ? label.rect.width * 26 / label.rect.height : 60;

            var value = AddText(NewUI("Value", row), "", 22, DarkCoffee, TextAlignmentOptions.Right);
            value.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
            return value;
        }

        static RecipeBookScreen BuildRecipeBook(Transform overlay)
        {
            var (popup, window) = Window<RecipeBookScreen>(overlay, "RecipeBookScreen", B("ModalPanel"), 1400, 800);
            SetClose(popup, BookClose(window));
            Label(window, "Title", B("TitleRecipe"), 700, 72, PivotCenter, 0.7f);
            var so = new SerializedObject(popup);

            // 분류 탭 (RecipeBookScreen.Categories 순서: 커피, 음료, 스무디)
            var labels = new[] { "LabelCoffee", "LabelBeverage", "LabelSmoothie" };
            var tabs = new Object[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                var tab = AddButton(TopLeft($"Tab {labels[i]}", window, 60 + i * 190, 112, 180, 64), B("TabOff"), Cream);
                CenterLabel(tab.transform, "Label", B(labels[i]));
                tabs[i] = tab;
            }
            SetArray(so, "tabs", tabs);
            Set(so, "tabOn", B("TabOn"));
            Set(so, "tabOff", B("TabOff"));

            // 왼쪽 메뉴 목록 (한 분류 최대 9개)
            var list = TopLeft("Menus", window, 60, 186, 340, 570);
            var column = list.gameObject.AddComponent<VerticalLayoutGroup>();
            column.spacing = 4;
            column.childControlWidth = column.childControlHeight = true;
            column.childForceExpandWidth = true;
            column.childForceExpandHeight = false;
            var item = NewUI("Menu", list);
            Sliced(item, B("MenuItem"), true);
            item.gameObject.AddComponent<LayoutElement>().preferredHeight = 58;
            var itemButton = item.gameObject.AddComponent<Button>();
            var itemText = NewUI("Name", item);
            Stretch(itemText);
            itemText.offsetMin = new Vector2(44, 0); // 선택 화살표 자리
            itemText.offsetMax = new Vector2(-12, 0);
            AddText(itemText, "", 22, DarkCoffee, TextAlignmentOptions.Left);
            item.gameObject.SetActive(false);
            Set(so, "menuContainer", list);
            Set(so, "menuTemplate", itemButton);
            Set(so, "menuItem", B("MenuItem"));
            Set(so, "menuItemSelected", B("MenuItemSelected"));

            // 오른쪽: 메뉴 이름 + HOT/ICE 선택 버튼 (320x128 그림을 절반 크기로) + 큰 레시피 카드 1장
            Set(so, "menuNameText", AddText(TopLeft("MenuName", window, 440, 186, 540, 64), "", 38, DarkCoffee, TextAlignmentOptions.Left));
            Set(so, "hotToggle", AddButton(TopLeft("HotToggle", window, 1000, 186, 160, 64), B("ToggleHotOff"), Cream));
            Set(so, "iceToggle", AddButton(TopLeft("IceToggle", window, 1170, 186, 160, 64), B("ToggleIceOff"), Cream));
            Set(so, "hotOn", B("ToggleHotOn"));
            Set(so, "hotOff", B("ToggleHotOff"));
            Set(so, "iceOn", B("ToggleIceOn"));
            Set(so, "iceOff", B("ToggleIceOff"));
            Set(so, "card", RecipeCard(window));

            Set(so, "shotIcon", LoadSprite($"{ItemIconFolder}/ShotGlassFull.png"));
            Set(so, "iceIcon", LoadSprite($"{ItemIconFolder}/ScoopIce.png"));
            Set(so, "hotWaterIcon", LoadSprite($"{ItemIconFolder}/WaterHot.png"));
            Set(so, "coldWaterIcon", LoadSprite($"{ItemIconFolder}/WaterCold.png"));
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
        }

        // 레시피 카드 1장: 재료 줄 5개 (레시피 재료는 최대 4개). HOT/ICE 는 위쪽 선택 버튼이 보여 준다
        static RecipeCardView RecipeCard(RectTransform window)
        {
            const int Rows = 5;
            var card = TopLeft("RecipeCard", window, 440, 270, 890, 480);
            Sliced(card, B("RecipeCard"));

            var icons = new Object[Rows];
            var names = new Object[Rows];
            var amounts = new Object[Rows];
            for (int i = 0; i < Rows; i++)
            {
                float y = 36 + i * 86;
                var icon = AddImage(TopLeft($"Icon {i}", card, 48, y, 76, 76), null, Color.clear);
                icon.preserveAspect = true;
                icons[i] = icon;
                names[i] = AddText(TopLeft($"Name {i}", card, 148, y, 460, 76), "", 30, DarkCoffee, TextAlignmentOptions.Left);
                amounts[i] = AddText(TopLeft($"Amount {i}", card, 610, y, 232, 76), "", 30, TextBrown, TextAlignmentOptions.Right);
            }
            var view = card.gameObject.AddComponent<RecipeCardView>();
            var so = new SerializedObject(view);
            SetArray(so, "icons", icons);
            SetArray(so, "names", names);
            SetArray(so, "amounts", amounts);
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        // 화면 아래 가운데 양손 칸 + 우클릭 메뉴 (SFR-021)
        static RectTransform BuildHands(Transform overlay)
        {
            var root = NewUI("Hands", overlay);
            root.anchorMin = root.anchorMax = root.pivot = new Vector2(0.5f, 0f); // 화면 비율이 달라도 아래 가운데
            root.anchoredPosition = new Vector2(0, 15);
            root.sizeDelta = new Vector2(315, 150); // 150 칸 2개 + 간격 15
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
            return root;
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

        // 포스기 (SFR-026). 패널 그림 1600x900 기준 좌표, 화면에는 0.78배로 위쪽(상태 바·주문 내역 버튼 바로 아래)에 두고
        // 그 아래에 손님 주문 채팅을 함께 띄운다
        static PosScreen BuildPos(Transform canvas, ReceiptPopup receipt)
        {
            const float Scale = 0.78f, Top = 172; // 상태 바(y 20~140)·주문 내역 버튼(y 22~132) 아래
            float w = 1600 * Scale, h = 900 * Scale;
            var (popup, window) = Window<PosScreen>(canvas, "PosScreen", LoadSprite($"{Pos}/Panel.png"), 1600, 900, 540 - Top - h / 2, Scale);
            var so = new SerializedObject(popup);
            Set(so, "chat", OrderChat(popup.transform, (1920 - w) / 2, Top + h + 10, w, 1080 - Top - h - 20));

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

        // 손님 주문 채팅: 메뉴 한 줄마다 말풍선 하나, 왼쪽 위부터 4개씩 채운다 (SFR-026)
        static OrderChatView OrderChat(Transform parent, float x, float y, float width, float height)
        {
            var root = TopLeft("OrderChat", parent, x, y, width, height);
            const int Columns = 4;
            const float Gap = 12, BubbleH = 86;
            float bubbleW = (width - Gap * (Columns - 1)) / Columns;
            var layout = root.gameObject.AddComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(bubbleW, BubbleH);
            layout.spacing = new Vector2(Gap, 10);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = Columns;

            // SpeechPanel.png(2400x400)는 아래 왼쪽에 꼬리가 있어 글자는 위쪽 80% 안에 둔다
            var bubble = NewUI("Bubble", root);
            bubble.sizeDelta = new Vector2(bubbleW, BubbleH);
            AddImage(bubble, LoadSprite($"{Hud}/SpeechPanel.png"), Cream);
            var text = AddText(TopLeft("Sentence", bubble, 22, 8, bubbleW - 40, 60), "", 22, DarkCoffee, TextAlignmentOptions.Left);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.enableAutoSizing = true; // 긴 메뉴 이름은 줄여서 한 말풍선 안에
            text.fontSizeMin = 16;
            text.fontSizeMax = 22;

            var view = root.gameObject.AddComponent<OrderChatView>();
            var so = new SerializedObject(view);
            Set(so, "container", root);
            Set(so, "bubbleTemplate", text);
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
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

        // 픽업대 (SFR-056, ADR-0018): 대기 주문마다 트레이 1개를 가로로 나열 (넘치면 가로 스크롤) + 안내
        const float PickupWidth = 1240, PickupHeight = 720;
        static readonly Vector4 PickupTrayBorder = new Vector4(48, 64, 48, 64); // PickupTray.png 9-slice (원본 px, 왼·아래·오른·위)

        static PickupScreen BuildPickup(Transform canvas)
        {
            if (AssetImporter.GetAtPath($"{Stations}/PickupTray.png") is TextureImporter trayImporter)
            {
                trayImporter.spriteBorder = PickupTrayBorder;
                trayImporter.SaveAndReimport();
            }
            var (popup, window) = Window<PickupScreen>(canvas, "PickupScreen", LoadSprite($"{Stations}/PickupPanel.png"), PickupWidth, PickupHeight, 50);
            var empty = Label(window, "Empty", B("EmptyOrders"), PickupWidth / 2, 370, PivotCenter, 0.6f);

            var scroll = TopLeft("Trays", window, 64, 150, PickupWidth - 128, 440);
            scroll.gameObject.AddComponent<Image>().color = Color.clear; // 빈 곳에서도 휠이 먹도록
            scroll.gameObject.AddComponent<RectMask2D>();
            var content = NewUI("Content", scroll);
            content.anchorMin = content.anchorMax = content.pivot = new Vector2(0, 1);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, 440);
            var row = content.gameObject.AddComponent<HorizontalLayoutGroup>();
            row.spacing = 20;
            row.childAlignment = TextAnchor.UpperLeft;
            row.childControlWidth = row.childControlHeight = true;
            row.childForceExpandWidth = row.childForceExpandHeight = false;
            content.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            var scrollRect = scroll.gameObject.AddComponent<ScrollRect>();
            scrollRect.content = content;
            scrollRect.viewport = scroll;
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.inertia = false;
            scrollRect.scrollSensitivity = 40;

            var so = new SerializedObject(popup);
            Set(so, "trayContainer", content);
            Set(so, "trayTemplate", TrayTemplate(content, popup));
            Set(so, "emptyLabel", empty.gameObject);
            so.ApplyModifiedPropertiesWithoutUndo();
            SetClose(popup, CloseButton(window, PickupWidth - 150, 74));
            return popup;
        }

        // 트레이 1개: 주문 번호, 메뉴 줄, 음료 자리 격자(3열), 서빙 버튼. 높이는 줄·자리 수에 맞춰 늘어난다 (PickupTray.png 9-slice)
        static PickupTrayView TrayTemplate(Transform parent, PickupScreen owner)
        {
            var rt = NewUI("Tray", parent);
            var tray = AddImage(rt, LoadSprite($"{Stations}/PickupTray.png"), Cream, true);
            tray.type = Image.Type.Sliced;
            tray.pixelsPerUnitMultiplier = 2f;
            rt.gameObject.AddComponent<LayoutElement>().preferredWidth = 260;
            var column = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            column.padding = new RectOffset(22, 22, 28, 26);
            column.spacing = 10;
            column.childAlignment = TextAnchor.UpperCenter;
            column.childControlWidth = column.childControlHeight = true;
            column.childForceExpandWidth = true;
            column.childForceExpandHeight = false;

            var view = rt.gameObject.AddComponent<PickupTrayView>();
            var so = new SerializedObject(view);
            Set(so, "owner", owner);
            Set(so, "numberText", AddText(NewUI("Number", rt), "#1", 28, DarkCoffee, TextAlignmentOptions.Left));
            var lines = AddText(NewUI("Lines", rt), "", 18, TextBrown, TextAlignmentOptions.TopLeft);
            lines.textWrappingMode = TextWrappingModes.Normal;
            Set(so, "linesText", lines);

            var slots = NewUI("Slots", rt);
            var grid = slots.gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(64, 64);
            grid.spacing = new Vector2(8, 8);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 3;
            grid.childAlignment = TextAnchor.UpperCenter;
            Set(so, "slotContainer", slots);
            var slot = NewUI("Slot", slots);
            var slotButton = AddButton(slot, LoadSprite($"{Stations}/PickupTraySlot.png"), Cream);
            var icon = AddImage(Centered("Icon", slot, 0, 0, 52, 52), null, Color.clear);
            icon.preserveAspect = true;
            Set(so, "slotTemplate", slotButton);

            var serveRow = NewUI("ServeRow", rt);
            serveRow.gameObject.AddComponent<LayoutElement>().preferredHeight = 56;
            var serve = AddButton(Centered("ServeButton", serveRow, 0, 0, 160, 56), LoadSprite($"{Stations}/ServeButton.png"), Cream);
            Set(so, "serveButton", serve);
            so.ApplyModifiedPropertiesWithoutUndo();
            rt.gameObject.SetActive(false);
            return view;
        }
    }
}
