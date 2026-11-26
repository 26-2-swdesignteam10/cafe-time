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
        // 메뉴 그림 (포스기 카드 = 손 칸·픽업대의 완성 음료): 음료는 판정 결과 완성품 그림 재사용.
        // hot = HOT 그림, ice = ICE 또는 온도 없는 메뉴 그림 (한쪽만 있으면 null). 포스기에서 HOT/ICE 를 바꾸면 카드 그림도 바뀐다.
        // 표에 없는 메뉴(신규 베이커리 등)는 ItemIcons 기본 아이콘
        const string Result = "Assets/_Project/Art/UI/DrinkResult";
        internal static readonly (string id, string hot, string ice)[] MenuPictures =
        {
            ("americano", $"{Result}/AmericanoHot", $"{Result}/AmericanoIce"),
            ("cafe_latte", $"{Result}/LatteHot", $"{Result}/LatteIce"),
            ("vanilla_latte", $"{Result}/VanillaLatteHot", $"{Result}/VanillaLatteIce"),
            ("hazelnut_latte", $"{Result}/HazelnutLatteHot", $"{Result}/HazelnutLatteIce"),
            ("caramel_macchiato", $"{Result}/CaramelHot", $"{Result}/CaramelIce"),
            ("cafe_mocha", $"{Result}/MochaHot", $"{Result}/MochaIce"),
            ("cappuccino", $"{Result}/CappuccinoHot", $"{Result}/CappuccinoIce"),
            ("cold_brew", $"{Result}/ColdBrewHot", $"{Result}/ColdBrewIce"),
            ("cold_brew_latte", $"{Result}/ColdBrewLatteHot", $"{Result}/ColdBrewLatteIce"),
            ("strawberry_latte", null, $"{Result}/StrawberryLatteIce"), ("chocolate_latte", null, $"{Result}/ChocoLatteIce"),
            ("matcha_latte", null, $"{Result}/MatchaLatteIce"),
            ("grapefruit_ade", null, $"{Result}/AdeGrapefruit"), ("blue_lemon_ade", null, $"{Result}/AdeBlueLemon"),
            ("yuja_tea", $"{Result}/YujaTeaHot", null), ("grapefruit_tea", $"{Result}/GrapefruitTeaHot", null),
            ("chamomile", $"{Result}/TeaHot", null), ("earl_grey", $"{Result}/TeaHot", null),
            ("strawberry_smoothie", null, $"{Result}/SmoothieStrawberry"), ("mango_smoothie", null, $"{Result}/SmoothieMango"),
            ("hallabong_smoothie", null, $"{Result}/SmoothieHallabong"), ("melon_smoothie", null, $"{Result}/SmoothieMelon"),
            ("cookie_shake", null, $"{Result}/CookieShake"), ("milk_shake", null, $"{Result}/MilkShake"),
            // 베이커리는 따로 그림을 두지 않고 재료 아이콘(다른 빵과 같은 화풍, 접시 없음)을 쓴다 (2026-11-17 사용자 요청)
        };

        // 메뉴 그림 한 장 불러오기. 표에서 비운 칸(null)은 경고 없이 null
        internal static Sprite LoadMenuPicture(string path) => path == null ? null : LoadSprite($"{path}.png");

        const string Stations = "Assets/_Project/Art/UI/Stations";
        const string OpenPrep = "Assets/_Project/Art/UI/OpenPrep";
        const string Settlement = "Assets/_Project/Art/UI/Settlement";
        const string CloseIcon = "Assets/_Project/Art/UI/Stations/Close.png"; // 기기 작업 창 공용 닫기 (일러스트)
        const string QuitPanel = "Assets/_Project/Art/UI/Quit/Panel.png";
        const string GroupOrderArt = "Assets/_Project/Art/UI/GroupOrder"; // 단체 주문 이벤트 (ADR-0042)

        public class Screens
        {
            public PosScreen Pos;
            public PickupScreen Pickup;
            public CupShelfScreen CupShelf;
            public CafeTime.UI.Popup Espresso;
            public IceMakerScreen IceMaker;
            public WaterScreen Water;
            public DrinkRoomScreen DrinkRoom;
            public ShakeRoomScreen ShakeRoom;
            public TrayReturnScreen TrayReturn;
            public DishwashScreen Dishwash;
            public PauseMenu Pause;
            public EventDialog Dialog;
        }

        public static Screens Build(CafeSceneBuilder.Managers managers)
        {
            foreach (var folder in new[] { Hud, OpenPrep, Settlement, GroupOrderArt }) PrepareSprites(folder);
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
                TrayReturn = StationUiBuilder.BuildTrayReturn(main, GameBalance.Instance.MaxTrayReturnSlots),
                Dishwash = StationUiBuilder.BuildDishwash(main),
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
            screens.Dialog = BuildEventDialog(overlay); // 영업 중 이벤트 대화창은 기기 화면·HUD 위 (SFR-008)
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

            // SFR-013: 첫 줄은 DAY·시각·소지금, 둘째 줄은 평판. 두 줄을 프레임 안쪽으로 모은다.
            if (AssetImporter.GetAtPath($"{Hud}/StatusBar.png") is TextureImporter statusImporter)
            {
                statusImporter.spriteBorder = new Vector4(80, 64, 80, 64);
                statusImporter.SaveAndReimport();
            }
            var bar = TopLeft("StatusBar", canvas, 20, 20, 640, 120);
            var frame = AddImage(bar, LoadSprite($"{Hud}/StatusBar.png"), new Color32(0x90, 0x47, 0x1B, 0xFF));
            frame.type = Image.Type.Sliced;
            frame.pixelsPerUnitMultiplier = 2f;
            var dayImage = AddImage(TopLeft("Day", bar, 36, 16, 144, 52), null, Color.clear);
            dayImage.preserveAspect = true;
            Set(so, "dayImage", dayImage);
            SetArray(so, "daySprites", Enumerable.Range(1, GameBalance.TotalDays).Select(d => (Object)LoadSprite($"{Hud}/Day{d}.png")).ToArray());
            Set(so, "dayText", AddText(TopLeft("DayText", bar, 40, 16, 140, 52), "DAY 1", 26, Cream, TextAlignmentOptions.Left));
            Set(so, "clockText", AddText(TopLeft("Clock", bar, 190, 16, 130, 52), "09:00", 25, Cream, TextAlignmentOptions.Left));
            AddImage(TopLeft("Coin", bar, 318, 18, 48, 48), LoadSprite($"{Hud}/Coin.png"), Cream).preserveAspect = true;
            Set(so, "moneyText", AddText(TopLeft("Money", bar, 374, 16, 230, 52), "0원", 28, Cream, TextAlignmentOptions.Left));
            Label(bar, "ReputationLabel", LoadSprite($"{Hud}/LabelReputation.png"), 36, 78, PivotLeft, 0.4f);
            var starFull = LoadSprite($"{Settlement}/Star.png");
            var stars = new Object[5];
            for (int i = 0; i < stars.Length; i++)
                stars[i] = AddImage(TopLeft($"Star {i + 1}", bar, 110 + i * 29, 63, 30, 30), starFull, Cream); // 별 간격 34 → 29
            SetArray(so, "stars", stars);
            Set(so, "reputationText", AddText(TopLeft("Reputation", bar, 110 + 5 * 29 + 8, 54, 80, 48), "3.0", 26, Cream, TextAlignmentOptions.Left));
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
            // 주문 내역·레시피 버튼과 배지는 현대적인 부드러운 일러스트라 Bilinear (2026-11-17 사용자 요청, docs/ART_STYLE.md 1장)
            foreach (var file in new[] { "OrdersButton", "RecipeButton", "Badge" })
            {
                if (!(AssetImporter.GetAtPath($"{Book}/{file}.png") is TextureImporter importer) || importer.filterMode == FilterMode.Bilinear) continue;
                importer.filterMode = FilterMode.Bilinear;
                importer.SaveAndReimport();
            }
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

            // 큰 아이콘 + 글자 그림 버튼 (460x248 그림을 절반 크기로, 2026-11-17 사용자 요청)
            var ordersButton = AddButton(TopLeft("OrdersButton", overlay, 1428, 16, 230, 124), B("OrdersButton"), Cream);
            var recipeButton = AddButton(TopLeft("RecipeButton", overlay, 1672, 16, 230, 124), B("RecipeButton"), Cream);
            var badge = TopLeft("Badge", ordersButton.transform, 190, -8, 48, 48);
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

            // 영수증은 가로로 나란히 배치하고, 긴 주문은 두 축으로 끌어 확인한다
            var scroll = TopLeft("Receipts", window, 60, 130, 1280, 620);
            scroll.gameObject.AddComponent<Image>().color = Color.clear; // 빈 곳에서도 휠이 먹도록
            scroll.gameObject.AddComponent<RectMask2D>();
            var content = NewUI("Content", scroll);
            content.anchorMin = content.anchorMax = content.pivot = new Vector2(0, 1);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, 620);
            var row = content.gameObject.AddComponent<HorizontalLayoutGroup>();
            row.spacing = 28;
            row.padding = new RectOffset(0, 0, 8, 0);
            row.childAlignment = TextAnchor.UpperLeft;
            row.childControlWidth = true;
            row.childControlHeight = false; // 각 영수증의 ContentSizeFitter가 높이를 결정한다
            row.childForceExpandWidth = row.childForceExpandHeight = false;
            var fitter = content.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var scrollRect = scroll.gameObject.AddComponent<ScrollRect>();
            scrollRect.content = content;
            scrollRect.viewport = scroll;
            scrollRect.horizontal = true;
            scrollRect.vertical = true; // 긴 주문은 세로로도 끌어 모든 품목을 확인한다
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

        // 흰 감열지 영수증: 결제 품목 전체를 표시하고 높이는 메뉴 줄 수에 맞춰 늘어난다 (SFR-013)
        static TicketView ReceiptTemplate(Transform parent)
        {
            // 위아래 톱니는 고정하고 가운데 종이만 늘린다 (SFR-013)
            var paperPath = $"{Book}/ThermalReceipt.png";
            if (AssetImporter.GetAtPath(paperPath) is TextureImporter importer)
            {
                importer.spriteBorder = new Vector4(32, 40, 32, 40);
                importer.SaveAndReimport();
            }
            var rt = NewUI("Ticket", parent);
            rt.pivot = new Vector2(0.5f, 1);
            rt.sizeDelta = new Vector2(360, 0);
            var paper = AddImage(rt, B("ThermalReceipt"), Cream);
            paper.type = Image.Type.Sliced;
            paper.pixelsPerUnitMultiplier = paper.sprite != null ? paper.sprite.rect.width / 360f : 1;
            rt.gameObject.AddComponent<LayoutElement>().preferredWidth = 360;
            rt.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var column = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            column.padding = new RectOffset(28, 28, 30, 30);
            column.spacing = 12;
            column.childControlWidth = column.childControlHeight = true;
            column.childForceExpandWidth = true;
            column.childForceExpandHeight = false;

            var view = rt.gameObject.AddComponent<TicketView>();
            var so = new SerializedObject(view);
            var shop = NewUI("ShopName", rt);
            AddImage(shop, B("ReceiptShopName"), Cream).preserveAspect = true;
            shop.gameObject.AddComponent<LayoutElement>().preferredHeight = 38;
            Set(so, "numberText", AddText(NewUI("Number", rt), "#1", 26, DarkCoffee));
            GroupRow(so, rt);
            var header = NewUI("Columns", rt);
            AddImage(header, B("ReceiptColumns"), Cream).preserveAspect = true;
            header.gameObject.AddComponent<LayoutElement>().preferredHeight = 26;
            var topDivider = NewUI("TopDivider", rt);
            AddImage(topDivider, B("ReceiptDivider"), TextBrown);
            topDivider.gameObject.AddComponent<LayoutElement>().preferredHeight = 3;
            var lines = AddText(NewUI("Lines", rt), "", 20, TextBrown, TextAlignmentOptions.TopLeft);
            lines.textWrappingMode = TextWrappingModes.Normal;
            lines.richText = true;
            Set(so, "linesText", lines);
            var divider = NewUI("Divider", rt);
            AddImage(divider, B("ReceiptDivider"), TextBrown);
            divider.gameObject.AddComponent<LayoutElement>().preferredHeight = 3;
            Set(so, "totalText", ReceiptRow(rt, "Total", B("LabelTotal")));
            Set(so, "pickupText", ReceiptRow(rt, "Pickup", B("LabelPickup")));
            Set(so, "statusText", AddText(NewUI("Status", rt), "", 20, TextBrown));
            so.ApplyModifiedPropertiesWithoutUndo();
            rt.gameObject.SetActive(false);
            return view;
        }

        // 단체 주문 줄 (ADR-0042): '단체 주문' 글자 이미지 + 마감 시각. 단체 주문일 때만 켜진다
        static void GroupRow(SerializedObject so, Transform parent)
        {
            var deadline = ReceiptRow(parent, "GroupOrder", LoadSprite($"{GroupOrderArt}/LabelGroupOrder.png"));
            var row = deadline.transform.parent.gameObject;
            row.SetActive(false);
            Set(so, "groupRow", row);
            Set(so, "deadlineText", deadline);
        }

        // ───────── 영업 중 이벤트 대화창 (SFR-008, 009, ADR-0042) ─────────
        // Quit/Panel.png 창: 왼쪽 손님 그림, 오른쪽 화자 이름·대사, 마지막 대사 뒤 수락/거절 버튼 (Art/UI/GroupOrder)

        static EventDialog BuildEventDialog(Transform overlay)
        {
            const float w = 1100, h = 710; // Panel.png 1561x1008 비율
            var (popup, window) = Window<EventDialog>(overlay, "EventDialog", LoadSprite(QuitPanel), w, h);
            var so = new SerializedObject(popup);
            var portrait = AddImage(TopLeft("Portrait", window, 100, 230, 200, 300), null, Color.clear);
            portrait.preserveAspect = true;
            Set(so, "portrait", portrait);
            Set(so, "speakerText", AddText(TopLeft("Speaker", window, 330, 290, 660, 44), "", 32, DarkCoffee, TextAlignmentOptions.Left));
            var dialogue = AddText(TopLeft("Dialogue", window, 330, 345, 660, 115), "", 26, TextBrown, TextAlignmentOptions.TopLeft);
            dialogue.textWrappingMode = TextWrappingModes.Normal;
            Set(so, "dialogueText", dialogue);
            var choices = TopLeft("Choices", window, 330, 466, 660, 104);
            Set(so, "choicePanel", choices.gameObject);
            Set(so, "acceptButton", AddButton(TopLeft("AcceptButton", choices, 30, 0, 280, 104), LoadSprite($"{GroupOrderArt}/AcceptButton.png"), Cream));
            Set(so, "declineButton", AddButton(TopLeft("DeclineButton", choices, 350, 0, 280, 104), LoadSprite($"{GroupOrderArt}/DeclineButton.png"), Cream));
            choices.gameObject.SetActive(false);
            so.ApplyModifiedPropertiesWithoutUndo();
            return popup;
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

            // 왼쪽 분류 탭: 긴 스무디·쉐이크 이름에 폭을 더 배정한다 (SFR-051)
            var labels = new[] { "LabelCoffee", "LabelBeverage", "LabelSmoothieShake" };
            var widths = new[] { 82f, 82f, 176f }; // 간격 8px 두 개와 합쳐 목록 폭 356px
            var tabs = new Object[labels.Length];
            float tabX = 60;
            for (int i = 0; i < labels.Length; i++)
            {
                var tab = AddButton(TopLeft($"Tab {labels[i]}", window, tabX, 140, widths[i], 52), B("TabOff"), Cream);
                CenterLabel(tab.transform, "Label", B(labels[i]));
                tabs[i] = tab;
                tabX += widths[i] + 8;
            }
            SetArray(so, "tabs", tabs);
            Set(so, "tabOn", B("TabOn"));
            Set(so, "tabOff", B("TabOff"));

            // 왼쪽 메뉴 목록: 음료 분류 10개도 창 안에 모두 표시한다
            var list = TopLeft("Menus", window, 60, 212, 356, 536);
            var column = list.gameObject.AddComponent<VerticalLayoutGroup>();
            column.spacing = 4;
            column.childControlWidth = column.childControlHeight = true;
            column.childForceExpandWidth = true;
            column.childForceExpandHeight = false;
            var item = NewUI("Menu", list);
            Sliced(item, B("MenuItem"), true);
            item.gameObject.AddComponent<LayoutElement>().preferredHeight = 49;
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

            // 오른쪽: 메뉴 이름·온도 선택, 큰 완성품 그림, 재료·정량 목록 (SFR-051)
            Set(so, "menuNameText", AddText(TopLeft("MenuName", window, 470, 148, 570, 60), "", 36, DarkCoffee, TextAlignmentOptions.Left));
            // HOT/ICE 는 포스기와 같은 불꽃·얼음 아이콘 버튼 그림 (2026-11-18 사용자 요청, 240x112 → 120x56)
            Set(so, "hotToggle", AddButton(TopLeft("HotToggle", window, 1070, 148, 120, 56), LoadSprite($"{Pos}/HotNormal.png"), Cream));
            Set(so, "iceToggle", AddButton(TopLeft("IceToggle", window, 1200, 148, 120, 56), LoadSprite($"{Pos}/IceNormal.png"), Cream));
            Set(so, "hotOn", LoadSprite($"{Pos}/HotSelected.png"));
            Set(so, "hotOff", LoadSprite($"{Pos}/HotNormal.png"));
            Set(so, "iceOn", LoadSprite($"{Pos}/IceSelected.png"));
            Set(so, "iceOff", LoadSprite($"{Pos}/IceNormal.png"));
            Set(so, "card", RecipeCard(window));
            var picture = AddImage(TopLeft("FinishedDrink", window, 470, 260, 350, 410), null, Color.clear);
            picture.preserveAspect = true;
            Set(so, "finishedPicture", picture);
            Set(so, "detailText", AddText(TopLeft("Detail", window, 470, 684, 350, 44), "", 22, TextBrown));
            Set(so, "noRecipe", Label(window, "NoRecipe", B("DessertHint"), 1090, 430, PivotCenter, 0.5f).gameObject);
            // 기존 완성품 표를 재사용하되 양쪽 온도가 있는 커피는 ICE 그림을 따로 연결한다 (디저트 탭은 없음)
            var pictures = MenuPictures;
            var pictureIds = so.FindProperty("pictureIds");
            pictureIds.arraySize = pictures.Length;
            for (int i = 0; i < pictures.Length; i++) pictureIds.GetArrayElementAtIndex(i).stringValue = pictures[i].id;
            // 한쪽 온도 그림만 있는 메뉴는 양쪽에 같은 그림
            SetArray(so, "hotPictures", pictures.Select(p => (Object)LoadMenuPicture(p.hot ?? p.ice)).ToArray());
            SetArray(so, "icePictures", pictures.Select(p => (Object)LoadMenuPicture(p.ice ?? p.hot)).ToArray());

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
            var card = TopLeft("RecipeCard", window, 850, 258, 470, 450);
            Label(card, "Ingredients", B("LabelIngredients"), 20, 16, PivotLeft, 0.5f);

            var icons = new Object[Rows];
            var names = new Object[Rows];
            var amounts = new Object[Rows];
            for (int i = 0; i < Rows; i++)
            {
                float y = 62 + i * 74;
                var icon = AddImage(TopLeft($"Icon {i}", card, 16, y, 48, 48), null, Color.clear);
                icon.preserveAspect = true;
                icons[i] = icon;
                names[i] = AddText(TopLeft($"Name {i}", card, 80, y, 254, 48), "", 24, DarkCoffee, TextAlignmentOptions.Left);
                amounts[i] = AddText(TopLeft($"Amount {i}", card, 344, y, 110, 48), "", 24, TextBrown, TextAlignmentOptions.Right);
            }
            var view = card.gameObject.AddComponent<RecipeCardView>();
            var so = new SerializedObject(view);
            SetArray(so, "icons", icons);
            SetArray(so, "names", names);
            SetArray(so, "amounts", amounts);
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        // 화면 아래 가운데 손 칸 4개 + 우클릭 메뉴 (SFR-021, ADR-0038)
        static RectTransform BuildHands(Transform overlay)
        {
            var root = NewUI("Hands", overlay);
            root.anchorMin = root.anchorMax = root.pivot = new Vector2(0.5f, 0f); // 화면 비율이 달라도 아래 가운데
            root.anchoredPosition = new Vector2(0, 15);
            const float slotSize = 110, gap = 12, iconSize = 76; // 화면을 덜 가리도록 작게 (기존 150·15·104)
            const int count = HandInventory.SlotCount;
            root.sizeDelta = new Vector2(slotSize * count + gap * (count - 1), slotSize);
            var view = root.gameObject.AddComponent<HandInventoryView>();
            var so = new SerializedObject(view);
            var slots = new Object[count];
            for (int i = 0; i < count; i++)
            {
                var slot = TopLeft($"HandSlot{i + 1}", root, i * (slotSize + gap), 0, slotSize, slotSize);
                var frame = AddImage(slot, LoadSprite($"{Hud}/HandSlotLeft.png"), Cream, true); // 칸 그림은 왼·오른 구분 없는 같은 액자
                var icon = AddImage(Centered("Icon", slot, 0, 0, iconSize, iconSize), null, Color.clear);
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

        // 메뉴 카드 견본 (디저트 진열장: 아이콘 + 이름 + 재고). 화면이 메뉴 수만큼 복제한다
        static MenuButtonView MenuTemplate(Transform parent, Sprite frame, Vector2 size)
        {
            var rt = NewUI("MenuButton", parent);
            rt.sizeDelta = size;
            var button = AddButton(rt, frame, Cream);
            var view = rt.gameObject.AddComponent<MenuButtonView>();
            var so = new SerializedObject(view);
            Set(so, "button", button);
            // OpenPrep/CardFrame.png 와 같은 배치
            var icon = AddImage(TopLeft("Icon", rt, 56, 24, 88, 86), null, Color.clear);
            icon.preserveAspect = true;
            Set(so, "icon", icon);
            Set(so, "nameText", AddText(TopLeft("Name", rt, 10, 118, 180, 28), "", 22, DarkCoffee));
            Set(so, "detailText", AddText(TopLeft("Detail", rt, 102, 157, 95, 22), "", 17, TextBrown, TextAlignmentOptions.Left));
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        // 포스기 메뉴 카드 (ADR-0029): 왼쪽 완성품 그림 + 오른쪽 이름·가격. 좌표는 Art/UI/Pos/Generation.md 카드 안 상대 좌표
        static MenuButtonView PosMenuCard(Transform parent)
        {
            var rt = NewUI("MenuButton", parent);
            rt.sizeDelta = new Vector2(280, 150);
            var button = AddButton(rt, LoadSprite($"{Pos}/MenuCard.png"), Cream);
            var view = rt.gameObject.AddComponent<MenuButtonView>();
            var so = new SerializedObject(view);
            Set(so, "button", button);
            var icon = AddImage(TopLeft("Icon", rt, 14, 12, 100, 125), null, Color.clear);
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            Set(so, "icon", icon);
            var name = AddText(TopLeft("Name", rt, 124, 32, 146, 54), "", 22, DarkCoffee, TextAlignmentOptions.Left);
            name.textWrappingMode = TextWrappingModes.Normal;
            name.enableAutoSizing = true; // 긴 이름은 2줄·작게
            name.fontSizeMin = 16;
            name.fontSizeMax = 22;
            Set(so, "nameText", name);
            Set(so, "detailText", AddText(TopLeft("Price", rt, 124, 94, 146, 30), "", 22, TextBrown, TextAlignmentOptions.Left));
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
            // 주문 채팅 줄: 왼쪽에 손님 초상화(정사각형), 오른쪽에 말풍선 (2026-11-19 사용자 요청)
            float chatX = (1920 - w) / 2, chatY = Top + h + 10, chatH = 1080 - Top - h - 20;
            var portrait = AddImage(TopLeft("CustomerPortrait", popup.transform, chatX, chatY, chatH, chatH), null, Color.white);
            portrait.preserveAspect = true;
            portrait.raycastTarget = false;
            Set(so, "portrait", portrait);
            Set(so, "chat", OrderChat(popup.transform, chatX + chatH + 12, chatY, w - chatH - 12, chatH));

            // 좌표는 Art/UI/Pos/Generation.md "배치 (2026-11-12)" 표 (ADR-0029)
            var tabNormal = LoadSprite($"{Pos}/TabNormal.png");
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
            Set(so, "tabSelected", LoadSprite($"{Pos}/TabSelected.png"));
            Set(so, "temperature", TemperatureButtons(window, 90, 248));

            var grid = Grid(window, "Menus", 90, 318, 870, 490, new Vector2(280, 150), 3);
            grid.GetComponent<GridLayoutGroup>().spacing = new Vector2(15, 20);
            Set(so, "menuContainer", grid);
            Set(so, "menuTemplate", PosMenuCard(grid));
            // 메뉴 카드 그림은 MenuPictures 표를 ItemIcons 에 넣어 손 칸 아이콘과 함께 쓴다 (CafeSceneBuilder)

            // 오른쪽 주문 목록
            var rows = TopLeft("Rows", window, 1015, 180, 520, 360);
            var layout = rows.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 4;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            Set(so, "rowContainer", rows);
            Set(so, "rowTemplate", RowTemplate(rows));

            Set(so, "minusButton", AddButton(TopLeft("Minus", window, 1015, 556, 56, 56), LoadSprite($"{Pos}/MinusButton.png"), Cream));
            Set(so, "plusButton", AddButton(TopLeft("Plus", window, 1081, 556, 56, 56), LoadSprite($"{Pos}/PlusButton.png"), Cream));
            Set(so, "deleteButton", AddButton(TopLeft("DeleteButton", window, 1155, 550, 180, 63), LoadSprite($"{Pos}/DeleteButton.png"), Cream));
            Set(so, "cancelButton", AddButton(TopLeft("CancelButton", window, 1345, 550, 180, 63), LoadSprite($"{Pos}/CancelButton.png"), Cream));
            Label(window, "TotalLabel", LoadSprite($"{Pos}/LabelTotal.png"), 1020, 660, PivotLeft);
            Set(so, "totalText", AddText(TopLeft("Total", window, 1140, 634, 390, 52), "0원", 36, DarkCoffee, TextAlignmentOptions.Right));
            Set(so, "payButton", AddButton(TopLeft("PayButton", window, 1075, 705, 400, 90), LoadSprite($"{Pos}/PayButton.png"), Cream));
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

        // 주문 내역 (SFR-027): 실제 감열지 영수증 그림 (고정 문구는 그림에 인쇄, 배치는 Art/UI/Pos/Generation.md 2026-11-17)
        static ReceiptPopup BuildReceipt(Transform canvas)
        {
            var (popup, window) = Window<ReceiptPopup>(canvas, "ReceiptPopup", LoadSprite($"{Pos}/ReceiptPanel.png"), 640, 860, scale: 0.88f);
            var so = new SerializedObject(popup);
            var ink = new Color32(0x61, 0x62, 0x5D, 0xFF); // 감열 잉크색
            Set(so, "numberText", AddText(TopLeft("Number", window, 300, 206, 245, 48), "#1", 30, ink, TextAlignmentOptions.Right));
            // 메뉴 6줄 (행 간격 48): 이름 왼쪽 x95, 수량 가운데 x420, 금액 오른쪽 x545
            Set(so, "linesText", Column("Lines", 95, 300, TextAlignmentOptions.TopLeft));
            Set(so, "quantityText", Column("Quantities", 380, 80, TextAlignmentOptions.Top));
            Set(so, "priceText", Column("Prices", 445, 100, TextAlignmentOptions.TopRight));
            Set(so, "sumText", AddText(TopLeft("Sum", window, 245, 624, 300, 52), "0원", 32, ink, TextAlignmentOptions.Right));
            so.ApplyModifiedPropertiesWithoutUndo();
            var confirm = LoadSprite($"{Pos}/ReceiptConfirmButton.png") ?? LoadSprite($"{Pos}/ConfirmButton.png");
            SetClose(popup, AddButton(TopLeft("ConfirmButton", window, 170, 710, 300, 84), confirm, Cream));
            return popup;

            TextMeshProUGUI Column(string name, float x, float w, TextAlignmentOptions align)
            {
                var text = AddText(TopLeft(name, window, x, 315, w, 288), "", 22, ink, align);
                text.lineSpacing = 100; // 22px 글자 + 줄 사이 → 행 간격 약 48px
                text.overflowMode = TextOverflowModes.Ellipsis;
                return text;
            }
        }

        // 픽업대 (SFR-056, ADR-0018): 대기 주문마다 트레이 1개를 가로로 나열 (넘치면 가로 스크롤) + 안내
        const float PickupWidth = 1240, PickupHeight = 720;
        static readonly Vector4 PickupTrayBorder = new Vector4(48, 64, 48, 64); // PickupTray.png 9-slice (원본 px, 왼·아래·오른·위)
        static readonly Vector4 PickupTicketBorder = new Vector4(32, 32, 32, 64); // PickupTicket.png 9-slice (원본 px, 왼·아래·오른·위)
        static readonly RectOffset PickupTicketPadding = new RectOffset(16, 16, 32, 16); // 주문지 안 글자 여백 (화면 px: 왼·오른·위·아래)

        static PickupScreen BuildPickup(Transform canvas)
        {
            if (AssetImporter.GetAtPath($"{Stations}/PickupTray.png") is TextureImporter trayImporter)
            {
                trayImporter.spriteBorder = PickupTrayBorder;
                trayImporter.SaveAndReimport();
            }
            if (AssetImporter.GetAtPath($"{Stations}/PickupTicket.png") is TextureImporter ticketImporter)
            {
                ticketImporter.spriteBorder = PickupTicketBorder;
                ticketImporter.SaveAndReimport();
            }
            var (popup, window) = Window<PickupScreen>(canvas, "PickupScreen", LoadSprite($"{Stations}/PickupPanel.png"), PickupWidth, PickupHeight, 50);
            var empty = Label(window, "Empty", B("EmptyOrders"), PickupWidth / 2, 370, PivotCenter, 0.6f);

            var scroll = TopLeft("Trays", window, 64, 160, PickupWidth - 128, 496);
            scroll.gameObject.AddComponent<Image>().color = Color.clear; // 빈 곳에서도 휠이 먹도록
            scroll.gameObject.AddComponent<RectMask2D>();
            var content = NewUI("Content", scroll);
            content.anchorMin = content.anchorMax = content.pivot = new Vector2(0, 1);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, 496);
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
            Set(so, "bellButton", AddButton(TopLeft("BellButton", window, 64, 48, 320, 80), LoadSprite($"{Stations}/BellButton.png"), Cream));
            Set(so, "bellScreen", BuildBellCall(canvas, popup));
            so.ApplyModifiedPropertiesWithoutUndo();
            SetClose(popup, CloseButton(window, PickupWidth - 150, 74));
            return popup;
        }

        static BellCallScreen BuildBellCall(Transform canvas, PickupScreen pickup)
        {
            // SFR-056, SFR-058: 입력 번호·상태는 TMP, 고정 키는 패널과 같은 원화에서 잘라 쓴다.
            var (popup, window) = Window<BellCallScreen>(canvas, "BellCallScreen", LoadSprite($"{Stations}/BellPanel.png"), 600, 800);
            var so = new SerializedObject(popup);
            Set(so, "pickup", pickup);
            Set(so, "numberText", AddText(TopLeft("Number", window, 74, 140, 452, 68), "", 54, DarkCoffee, TextAlignmentOptions.Right));
            Set(so, "messageText", AddText(TopLeft("Message", window, 66, 234, 468, 34), "", 22, Warning));
            var digits = new Object[10];
            for (int digit = 1; digit <= 9; digit++)
            {
                int index = digit - 1;
                digits[digit] = Key($"Key{digit}", 58 + index % 3 * 166, 286 + index / 3 * 87, 154, 78);
            }
            digits[0] = Key("Key0", 224, 547, 154, 78);
            SetArray(so, "digitButtons", digits);
            Set(so, "clearButton", Key("ClearKey", 58, 547, 154, 78));
            Set(so, "backButton", Key("BackKey", 390, 547, 154, 78));
            Set(so, "callButton", Key("CallButton", 54, 643, 492, 88));
            so.ApplyModifiedPropertiesWithoutUndo();
            SetClose(popup, CloseButton(window, 520, 30, 48));
            return popup;

            Button Key(string name, float x, float y, float width, float height)
            {
                return AddButton(TopLeft(name, window, x, y, width, height), LoadSprite($"{Stations}/{name}.png"), Cream);
            }
        }

        // 트레이 1개: 주문 번호·호출됨 표시, 메뉴 줄, 음료 자리 격자(2열). 서빙 행 없이 내용 높이에 맞춘다.
        static PickupTrayView TrayTemplate(Transform parent, PickupScreen owner)
        {
            var rt = NewUI("Tray", parent);
            var tray = AddImage(rt, LoadSprite($"{Stations}/PickupTray.png"), Cream, true);
            tray.type = Image.Type.Sliced;
            tray.pixelsPerUnitMultiplier = 2f;
            rt.gameObject.AddComponent<LayoutElement>().preferredWidth = 304;
            var column = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            column.padding = new RectOffset(22, 22, 20, 20);
            column.spacing = 8;
            column.childAlignment = TextAnchor.UpperCenter;
            column.childControlWidth = column.childControlHeight = true;
            column.childForceExpandWidth = true;
            column.childForceExpandHeight = false;

            var view = rt.gameObject.AddComponent<PickupTrayView>();
            var so = new SerializedObject(view);
            Set(so, "owner", owner);
            // 주문 번호·메뉴 줄은 트레이 위 주문지 종이에 쓴다 (트레이 위 글자가 어두워 안 보이던 문제, 2026-11-18 사용자 요청)
            var ticket = NewUI("Ticket", rt);
            var paper = AddImage(ticket, LoadSprite($"{Stations}/PickupTicket.png"), Cream);
            paper.type = Image.Type.Sliced;
            paper.pixelsPerUnitMultiplier = 2f;
            paper.raycastTarget = false;
            var ticketColumn = ticket.gameObject.AddComponent<VerticalLayoutGroup>();
            ticketColumn.padding = PickupTicketPadding;
            ticketColumn.spacing = 4;
            ticketColumn.childControlWidth = ticketColumn.childControlHeight = true;
            ticketColumn.childForceExpandWidth = true;
            ticketColumn.childForceExpandHeight = false;
            var header = NewUI("Header", ticket);
            header.gameObject.AddComponent<LayoutElement>().preferredHeight = 40;
            Set(so, "numberText", AddText(TopLeft("Number", header, 0, 0, 112, 40), "#1", 28, DarkCoffee, TextAlignmentOptions.Left));
            var called = AddImage(TopLeft("CalledMark", header, 100, 2, 120, 36), LoadSprite($"{Stations}/CalledBadge.png"), Cream);
            called.preserveAspect = true;
            called.raycastTarget = false;
            Set(so, "calledMark", called.gameObject);
            called.gameObject.SetActive(false);
            GroupRow(so, ticket);
            var lines = AddText(NewUI("Lines", ticket), "", 18, TextBrown, TextAlignmentOptions.TopLeft);
            lines.textWrappingMode = TextWrappingModes.Normal;
            Set(so, "linesText", lines);

            var slots = NewUI("Slots", rt);
            var grid = slots.gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(112, 112);
            grid.spacing = new Vector2(12, 12);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 2;
            grid.childAlignment = TextAnchor.UpperCenter;
            Set(so, "slotContainer", slots);
            var slot = NewUI("Slot", slots);
            var slotButton = AddButton(slot, LoadSprite($"{Stations}/PickupTraySlot.png"), Cream);
            var icon = AddImage(Centered("Icon", slot, 0, 0, 104, 104), null, Color.clear);
            icon.preserveAspect = true;
            Set(so, "slotTemplate", slotButton);

            so.ApplyModifiedPropertiesWithoutUndo();
            rt.gameObject.SetActive(false);
            return view;
        }
    }
}
