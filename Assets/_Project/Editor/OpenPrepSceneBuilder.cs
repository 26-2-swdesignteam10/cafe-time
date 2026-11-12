using System.IO;
using CafeTime.OpenPrep;
using CafeTime.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // CafeTime > Build Open Prep Scene : 오픈 준비 씬(SFR-010)과 재료 카드 프리팹 생성 (ADR-0006, 최초 생성용)
    public static class OpenPrepSceneBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/OpenPrep.unity";
        const string CardPrefabPath = "Assets/_Project/Prefabs/OpenPrep/IngredientCard.prefab";
        const string ConfirmPrefabPath = "Assets/_Project/Prefabs/UI/ConfirmPopup.prefab";
        const string Art = "Assets/_Project/Art/UI/OpenPrep";
        const string BackgroundPath = "Assets/_Project/Art/Title/CafeInterior.png";

        // 탭 글자 이미지 (IngredientCategory 순서)
        static readonly string[] TabLabels = { "TabCoffee", "TabMilk", "TabSyrup", "TabTea", "TabSmoothie", "TabDessert" };
        static readonly Color Amber = new Color32(0xDF, 0x70, 0x15, 0xFF);

        [MenuItem("CafeTime/Build Open Prep Scene")]
        public static void Build()
        {
            if (GameFont == null) { Debug.LogError("폰트 에셋이 없습니다. CafeTime > Setup Fonts 를 먼저 실행하세요."); return; }
            if (!ConfirmOverwrite(ScenePath, "OpenPrep 씬")) return;

            GameBalanceAssetBuilder.CreateOrUpdate();
            PrepareSprites(Art);

            var canvas = NewSceneWithCanvas(new Color32(0xE9, 0xC0, 0x8E, 0xFF));
            // 프리팹은 새 씬을 만든 뒤에 저장한다. 먼저 저장하면 NewScene 이 참조 없는 에셋을 내려 cardPrefab 이 비어 저장된다
            var card = BuildCardPrefab();
            AddBackground(canvas, LoadSprite(BackgroundPath), Cream, 0.35f);
            var screen = canvas.gameObject.AddComponent<OpenPrepScreen>();
            var so = new SerializedObject(screen);

            // 상단 바: "DAY n" + [오픈 준비]    [보유금] + "₩n"   ([ ] = 글자 이미지)
            var top = TopLeft("TopBar", canvas, 40, 20, 1840, 120);
            AddImage(top, LoadSprite($"{Art}/TopBar.png"), new Color32(0x90, 0x47, 0x1B, 0xFF));
            Set(so, "dayText", AddText(TopLeft("DayText", top, 110, 0, 170, 120), "DAY 1", 44, Cream, TextAlignmentOptions.Left));
            Label(top, "OpenPrepLabel", LoadSprite($"{Art}/LabelOpenPrep.png"), 290, 60, PivotLeft);
            Label(top, "MoneyLabel", LoadSprite($"{Art}/LabelMoney.png"), 1420, 60, PivotRight);
            Set(so, "moneyText", AddText(TopLeft("MoneyText", top, 1435, 0, 300, 120), "₩0", 44, Cream, TextAlignmentOptions.Left));

            // 왼쪽: 분류 탭 + 재료 카드
            var catalog = TopLeft("CatalogPanel", canvas, 40, 160, 1200, 860);
            AddImage(catalog, LoadSprite($"{Art}/CatalogPanel.png"), Cream);
            var tabNormal = LoadSprite($"{Art}/TabNormal.png");
            var tabSelected = LoadSprite($"{Art}/TabSelected.png");
            var tabs = new Object[TabLabels.Length];
            for (int i = 0; i < TabLabels.Length; i++)
            {
                var tab = TopLeft(TabLabels[i], catalog, 95 + i * 170, 100, 160, 62); // 패널 안쪽 금색 선(약 y 95) 아래
                tabs[i] = AddButton(tab, tabNormal, Cream);
                CenterLabel(tab, "Label", LoadSprite($"{Art}/{TabLabels[i]}.png"));
            }
            SetArray(so, "categoryTabs", tabs);
            Set(so, "tabNormal", tabNormal);
            Set(so, "tabSelected", tabSelected);

            var grid = TopLeft("Cards", catalog, 60, 190, 1080, 560);
            var layout = grid.gameObject.AddComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(200, 252);
            layout.spacing = new Vector2(12, 24);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = 5;
            layout.childAlignment = TextAnchor.UpperCenter;
            Set(so, "cardContainer", grid);
            Set(so, "cardPrefab", card);

            // 오른쪽: 장바구니. CartPanel.png 의 종이 영역은 x 50~550, y 115~790 ("장바구니" 제목이 y 230까지)
            var cart = TopLeft("CartPanel", canvas, 1280, 160, 600, 860);
            AddImage(cart, LoadSprite($"{Art}/CartPanel.png"), Cream);
            // 장바구니 목록은 재료 이름·수량·금액이 바뀌는 값이라 폰트로 표시
            var cartText = AddText(TopLeft("CartList", cart, 75, 255, 450, 190), "", 26, TextBrown, TextAlignmentOptions.TopLeft);
            cartText.lineSpacing = 20;
            Set(so, "cartText", cartText);
            Set(so, "cartEmptyLabel", Label(cart, "CartEmpty", LoadSprite($"{Art}/CartEmpty.png"), 300, 300, PivotCenter).gameObject);
            // 총액 / 구매 후 잔액 / 오늘 임대료 (목록 아래 ~ 구매 버튼 위)
            Label(cart, "TotalLabel", LoadSprite($"{Art}/LabelTotal.png"), 75, 470, PivotLeft);
            Set(so, "totalText", AddText(TopLeft("Total", cart, 225, 447, 300, 46), "₩0", 34, TextBrown, TextAlignmentOptions.Right));
            Label(cart, "RemainingLabel", LoadSprite($"{Art}/LabelRemaining.png"), 75, 512, PivotLeft, 0.4f);
            Set(so, "remainingText", AddText(TopLeft("Remaining", cart, 225, 492, 300, 40), "₩0", 28, TextBrown, TextAlignmentOptions.Right));
            Label(cart, "RentLabel", LoadSprite($"{Art}/LabelRent.png"), 75, 550, PivotLeft, 0.4f);
            Set(so, "rentText", AddText(TopLeft("Rent", cart, 225, 530, 300, 40), "₩0", 28, TextBrown, TextAlignmentOptions.Right));
            Set(so, "boughtLabel", Label(cart, "Bought", LoadSprite($"{Art}/MessageBought.png"), 300, 588, PivotCenter).gameObject);
            Set(so, "unboughtMessage", LoadSprite($"{Art}/ConfirmUnbought.png"));
            Set(so, "buyButton", AddButton(TopLeft("BuyButton", cart, 90, 608, 420, 89), LoadSprite($"{Art}/BuyButton.png"), Amber));
            Set(so, "startButton", AddButton(TopLeft("StartButton", cart, 90, 700, 420, 89), LoadSprite($"{Art}/StartButton.png"), Cream));

            // 확인 팝업은 공용 프리팹 재사용 (맨 위에 그려지도록 마지막에 추가)
            var confirmPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ConfirmPrefabPath);
            if (confirmPrefab != null)
            {
                var confirm = (GameObject)PrefabUtility.InstantiatePrefab(confirmPrefab, canvas);
                Set(so, "confirmPopup", confirm.GetComponent<ConfirmPopup>());
            }
            else Debug.LogWarning("ConfirmPopup 프리팹이 없습니다: " + ConfirmPrefabPath);

            so.ApplyModifiedPropertiesWithoutUndo();
            SaveSceneAndAddToBuild(ScenePath);
            Debug.Log("OpenPrep 씬 생성 완료: " + ScenePath);
        }

        // 재료 카드 200x252: 아이콘 / 이름 / 재고 / 단가 / - 수량 +. CardFrame.png 의 아이콘 칸은 x 52~148, y 20~113
        static IngredientCard BuildCardPrefab()
        {
            var root = new GameObject("IngredientCard", typeof(RectTransform));
            var rt = (RectTransform)root.transform;
            rt.sizeDelta = new Vector2(200, 252);
            AddImage(rt, LoadSprite($"{Art}/CardFrame.png"), Cream);
            var card = root.AddComponent<IngredientCard>();
            var so = new SerializedObject(card);

            var icon = TopLeft("Icon", rt, 56, 24, 88, 86);
            var iconImage = icon.gameObject.AddComponent<Image>();
            iconImage.preserveAspect = true;
            iconImage.raycastTarget = false;
            Set(so, "iconImage", iconImage);
            Set(so, "nameText", AddText(TopLeft("Name", rt, 10, 118, 180, 28), "재료", 22, DarkCoffee));
            Label(rt, "StockLabel", LoadSprite($"{Art}/LabelStock.png"), 96, 157, PivotRight, 0.3f); // 카드 글씨(17pt)에 맞춰 축소
            Set(so, "stockText", AddText(TopLeft("Stock", rt, 102, 146, 95, 22), "0", 17, TextBrown, TextAlignmentOptions.Left));
            Set(so, "priceText", AddText(TopLeft("Price", rt, 10, 168, 180, 22), "₩0", 17, TextBrown));
            // -/+ 는 길게 누르면 반복 (RepeatButton 이 클릭을 처리하고 Button 은 그림·비활성화 표시만)
            var minus = AddButton(TopLeft("Minus", rt, 30, 198, 36, 36), LoadSprite($"{Art}/MinusButton.png"), Cream);
            Set(so, "minusButton", minus);
            Set(so, "minusRepeat", minus.gameObject.AddComponent<RepeatButton>());
            Set(so, "quantityText", AddText(TopLeft("Quantity", rt, 70, 198, 60, 36), "0", 26, DarkCoffee));
            var plus = AddButton(TopLeft("Plus", rt, 134, 198, 36, 36), LoadSprite($"{Art}/PlusButton.png"), Cream);
            Set(so, "plusRepeat", plus.gameObject.AddComponent<RepeatButton>());
            so.ApplyModifiedPropertiesWithoutUndo();

            Directory.CreateDirectory(Path.GetDirectoryName(CardPrefabPath));
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, CardPrefabPath);
            Object.DestroyImmediate(root);
            return prefab.GetComponent<IngredientCard>();
        }
    }
}
