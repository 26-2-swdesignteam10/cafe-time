using System.Linq;
using CafeTime.Core;
using CafeTime.OpenPrep;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static CafeTime.EditorTools.BuilderUtil;

namespace CafeTime.EditorTools
{
    // CafeTime > Build Open Prep Scene : 영업 준비 씬 = 장비 업그레이드 / 알바생 고용 화면 (ADR-0006, ADR-0057).
    // 배치는 사용자가 준 시안(2026-11-26)을 따른다. 재료 구매 화면은 없앴다 (ADR-0056)
    public static class OpenPrepSceneBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/OpenPrep.unity";
        const string Art = "Assets/_Project/Art/UI/Upgrade";
        const string StaffArt = "Assets/_Project/Art/Sprites/Staff";
        const string StarFull = "Assets/_Project/Art/UI/Settlement/Star.png";
        const string StarEmpty = "Assets/_Project/Art/UI/Settlement/StarEmpty.png";

        // Equipment 순서
        static readonly string[] EquipmentIcons = { "IconDishwasher", "IconIceMaker", "IconEspresso", "IconPos", "IconTrayReturn", "IconWater" };
        static readonly Color Orange = new Color32(0xD9, 0x6B, 0x1E, 0xFF);
        static readonly Color Muted = new Color32(0x9A, 0x86, 0x70, 0xFF);

        [MenuItem("CafeTime/Build Open Prep Scene")]
        public static void Build()
        {
            if (GameFont == null) { Debug.LogError("폰트 에셋이 없습니다. CafeTime > Setup Fonts 를 먼저 실행하세요."); return; }
            if (!ConfirmOverwrite(ScenePath, "OpenPrep 씬")) return;

            GameBalanceAssetBuilder.CreateOrUpdate();
            PrepareSprites(Art);
            PrepareSprites(StaffArt);

            var canvas = NewSceneWithCanvas(new Color32(0x2A, 0x1A, 0x12, 0xFF));
            AddBackground(canvas, LoadSprite($"{Art}/Background.png"), new Color32(0x2A, 0x1A, 0x12, 0xFF), 0.15f);
            var screen = canvas.gameObject.AddComponent<OpenPrepScreen>();
            var so = new SerializedObject(screen);
            var buttonNormal = LoadSprite($"{Art}/ButtonPrimary.png");
            var buttonDisabled = LoadSprite($"{Art}/ButtonDisabled.png");
            var starFull = LoadSprite(StarFull);
            var starEmpty = LoadSprite(StarEmpty);

            // 상단: [장비 업그레이드 | 알바생 고용] 제목      [보유금] n원 / DAY n
            Set(so, "upgradeTitle", Label(canvas, "TitleUpgrade", LoadSprite($"{Art}/LabelTitleUpgrade.png"), 60, 80, PivotLeft, 0.6f).gameObject);
            Set(so, "staffTitle", Label(canvas, "TitleStaff", LoadSprite($"{Art}/LabelTitleStaff.png"), 60, 80, PivotLeft, 0.6f).gameObject);
            Label(canvas, "MoneyLabel", LoadSprite($"{Art}/LabelMoney.png"), 1230, 75, PivotLeft, 0.6f);
            Set(so, "moneyText", AddText(TopLeft("MoneyText", canvas, 1440, 40, 420, 76), "0원", 60, Cream, TextAlignmentOptions.Right));
            Set(so, "dayText", AddText(TopLeft("DayText", canvas, 1440, 118, 420, 44), "DAY 1", 34, Cream, TextAlignmentOptions.Right));

            // 탭 + 오늘 정산 안내
            var tabOn = LoadSprite($"{Art}/TabOn.png");
            var tabOff = LoadSprite($"{Art}/TabOff.png");
            var upgradeTab = AddButton(TopLeft("UpgradeTab", canvas, 60, 180, 320, 76), tabOn, Orange);
            CenterLabel(upgradeTab.transform, "Label", LoadSprite($"{Art}/LabelTabUpgrade.png"));
            var staffTab = AddButton(TopLeft("StaffTab", canvas, 400, 180, 320, 76), tabOff, Cream);
            CenterLabel(staffTab.transform, "Label", LoadSprite($"{Art}/LabelTabStaff.png"));
            Set(so, "upgradeTab", upgradeTab);
            Set(so, "staffTab", staffTab);
            Set(so, "tabOn", tabOn);
            Set(so, "tabOff", tabOff);
            Set(so, "costText", AddText(TopLeft("CostText", canvas, 760, 196, 1100, 44), "", 28, Cream, TextAlignmentOptions.Right));

            var panel = TopLeft("Panel", canvas, 60, 270, 1800, 680);
            AddImage(panel, LoadSprite($"{Art}/Panel.png"), Cream);
            Set(so, "upgradePage", BuildUpgradePage(panel, so, buttonNormal, buttonDisabled, starFull, starEmpty));
            Set(so, "staffPage", BuildStaffPage(panel, so, buttonNormal, buttonDisabled));

            Set(so, "buttonNormal", buttonNormal);
            Set(so, "buttonDisabled", buttonDisabled);
            Set(so, "starFull", starFull);
            Set(so, "starEmpty", starEmpty);
            Set(so, "startButton", AddButton(TopLeft("StartButton", canvas, 1340, 962, 520, 110), LoadSprite($"{Art}/StartButton.png"), Cream));

            so.ApplyModifiedPropertiesWithoutUndo();
            AudioManagerBuilder.AddToScene(AudioManagerBuilder.TitleBgm); // 클릭음 + 타이틀 BGM (PR #25, ADR-0054)
            SaveSceneAndAddToBuild(ScenePath);
            Debug.Log("OpenPrep 씬 생성 완료: " + ScenePath);
        }

        // ───────── 장비 업그레이드 탭 ─────────
        // 위: 장비 카드 6장 (260x330). 아래: 고른 장비 설명 + 1·2·3단계 상자 + 업그레이드 버튼

        static GameObject BuildUpgradePage(RectTransform panel, SerializedObject so, Sprite buttonNormal, Sprite buttonDisabled, Sprite starFull, Sprite starEmpty)
        {
            var page = NewUI("UpgradePage", panel);
            Stretch(page);
            var cardNormal = LoadSprite($"{Art}/EquipmentCard.png");
            var cardSelected = LoadSprite($"{Art}/EquipmentCardSelected.png");
            var icons = EquipmentIcons.Select(n => (Object)LoadSprite($"{Art}/{n}.png")).ToArray();

            var cards = new Object[EquipmentIcons.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                var rt = TopLeft($"Card {(Equipment)i}", page, 60 + i * 284, 36, 260, 330);
                var button = AddButton(rt, cardNormal, Cream);
                var card = rt.gameObject.AddComponent<EquipmentCard>();
                var c = new SerializedObject(card);
                Set(c, "button", button);
                Set(c, "frame", button.image);
                Set(c, "frameNormal", cardNormal);
                Set(c, "frameSelected", cardSelected);
                var icon = AddImage(TopLeft("Icon", rt, 65, 28, 130, 130), (Sprite)icons[i], Color.clear);
                icon.preserveAspect = true;
                Set(c, "nameText", AddText(TopLeft("Name", rt, 10, 176, 240, 40), "", 30, DarkCoffee));
                SetArray(c, "stars", Stars(rt, 130, 238, 40, 48, starEmpty));
                Set(c, "priceText", AddText(TopLeft("Price", rt, 10, 272, 240, 40), "", 28, Orange));
                c.ApplyModifiedPropertiesWithoutUndo();
                cards[i] = card;
            }
            SetArray(so, "equipmentCards", cards);
            SetArray(so, "equipmentIcons", icons);

            // 설명 영역 (y 400 ~ 660)
            var detailIcon = AddImage(TopLeft("DetailIcon", page, 50, 410, 220, 220), null, Color.clear);
            detailIcon.preserveAspect = true;
            Set(so, "detailIcon", detailIcon);
            Set(so, "detailName", AddText(TopLeft("DetailName", page, 300, 400, 560, 60), "", 46, DarkCoffee, TextAlignmentOptions.Left));
            SetArray(so, "detailStars", Stars(page, 300 + 22, 490, 44, 54, starEmpty, leftAligned: true));
            var description = AddText(TopLeft("DetailDescription", page, 300, 530, 560, 100), "", 27, TextBrown, TextAlignmentOptions.TopLeft);
            description.textWrappingMode = TextWrappingModes.Normal;
            Set(so, "detailDescription", description);

            var boxes = new Object[3];
            var titles = new Object[3];
            var texts = new Object[3];
            for (int i = 0; i < 3; i++)
            {
                var box = TopLeft($"Stage {i + 1}", page, 900 + i * 296, 404, 268, 96);
                boxes[i] = AddImage(box, LoadSprite($"{Art}/StageBox.png"), Cream);
                titles[i] = AddText(TopLeft("Title", box, 0, 10, 268, 38), "", 28, TextBrown);
                texts[i] = AddText(TopLeft("Text", box, 0, 50, 268, 36), "", 24, TextBrown);
                if (i < 2) AddImage(TopLeft($"Arrow {i + 1}", page, 1172 + i * 296, 432, 20, 40), LoadSprite($"{Art}/StageArrow.png"), Orange);
            }
            SetArray(so, "stageBoxes", boxes);
            SetArray(so, "stageTitles", titles);
            SetArray(so, "stageTexts", texts);
            Set(so, "stageTodo", LoadSprite($"{Art}/StageBox.png"));
            Set(so, "stageDone", LoadSprite($"{Art}/StageBoxDone.png"));

            Set(so, "nextText", AddText(TopLeft("NextText", page, 900, 530, 540, 44), "", 28, TextBrown, TextAlignmentOptions.Left));
            var upgrade = AddButton(TopLeft("UpgradeButton", page, 1440, 560, 330, 84), buttonNormal, Orange);
            Set(so, "upgradeButton", upgrade);
            Set(so, "upgradeLabel", StretchText(upgrade.transform, "Label", "", 30, DarkCoffee));
            return page.gameObject;
        }

        // 별 3개. centerX 가 가운데 별의 가운데 (leftAligned 면 첫 별의 가운데)
        static Object[] Stars(Transform parent, float centerX, float centerY, float size, float step, Sprite empty, bool leftAligned = false)
        {
            var stars = new Object[3];
            for (int i = 0; i < 3; i++)
            {
                float x = (leftAligned ? centerX + i * step : centerX + (i - 1) * step) - size / 2;
                var star = AddImage(TopLeft($"Star {i + 1}", parent, x, centerY - size / 2, size, size), empty, Color.clear);
                star.preserveAspect = true;
                stars[i] = star;
            }
            return stars;
        }

        // ───────── 알바생 고용 탭 ─────────
        // 왼쪽: 알바생 카드 3장 (400x620). 오른쪽: 오늘의 알바생 자리 2개

        static GameObject BuildStaffPage(RectTransform panel, SerializedObject so, Sprite buttonNormal, Sprite buttonDisabled)
        {
            var page = NewUI("StaffPage", panel);
            Stretch(page);
            var staff = GameBalance.Instance.Staff;
            var portraits = staff.Select(s => (Object)LoadSprite($"{StaffArt}/Staff{s.Id}Portrait.png")).ToArray();
            SetArray(so, "staffPortraits", portraits);

            var cards = new Object[staff.Count];
            for (int i = 0; i < cards.Length; i++)
            {
                var rt = TopLeft($"StaffCard {staff[i].Id}", page, 40 + i * 430, 30, 400, 620);
                AddImage(rt, LoadSprite($"{Art}/StaffCard.png"), Cream);
                var card = rt.gameObject.AddComponent<StaffCard>();
                var c = new SerializedObject(card);
                // StaffCard.png 에 그려진 초상화 칸 (안쪽 x 101~299, y 58~251)
                var face = AddImage(TopLeft("Portrait", rt, 106, 62, 188, 186), null, Color.clear);
                face.preserveAspect = true;
                Set(c, "portrait", face);
                Set(c, "nameText", AddText(TopLeft("Name", rt, 0, 266, 400, 50), "", 40, DarkCoffee));
                AddText(TopLeft("TaskCaption", rt, 0, 320, 400, 30), "맡는 일", 22, Muted);
                Set(c, "taskText", AddText(TopLeft("Task", rt, 0, 352, 400, 40), "", 28, TextBrown));
                Set(c, "noteText", AddText(TopLeft("Note", rt, 0, 398, 400, 36), "", 26, Warning));
                Set(c, "wageText", AddText(TopLeft("Wage", rt, 0, 440, 400, 46), "", 36, Orange));
                var hire = AddButton(TopLeft("HireButton", rt, 60, 498, 280, 80), buttonNormal, Orange);
                Set(c, "hireButton", hire);
                Set(c, "hireLabel", StretchText(hire.transform, "Label", "고용하기", 30, DarkCoffee));
                Set(c, "buttonNormal", buttonNormal);
                Set(c, "buttonDisabled", buttonDisabled);
                c.ApplyModifiedPropertiesWithoutUndo();
                cards[i] = card;
            }
            SetArray(so, "staffCards", cards);

            var hired = TopLeft("HiredPanel", page, 1330, 30, 430, 620);
            AddImage(hired, LoadSprite($"{Art}/StaffSlotPanel.png"), Cream);
            Set(so, "hiredCountText", AddText(TopLeft("HiredCount", hired, 0, 24, 430, 44), "", 30, DarkCoffee));
            var slots = new Object[2];
            for (int i = 0; i < slots.Length; i++)
            {
                var rt = TopLeft($"HiredSlot {i + 1}", hired, 20, 96 + i * 262, 390, 240);
                var slot = rt.gameObject.AddComponent<HiredSlot>();
                var c = new SerializedObject(slot);

                var filled = TopLeft("Filled", rt, 0, 0, 390, 240);
                var frame = TopLeft("PortraitFrame", filled, 0, 0, 110, 110);
                var face = AddImage(TopLeft("Portrait", frame, 8, 8, 94, 94), null, Color.clear);
                face.preserveAspect = true;
                AddImage(TopLeft("Frame", frame, 0, 0, 110, 110), LoadSprite($"{Art}/PortraitFrame.png"), Color.clear); // 초상화 위에 틀
                Set(c, "portrait", face);
                Set(c, "nameText", AddText(TopLeft("Name", filled, 124, 4, 266, 40), "", 30, DarkCoffee, TextAlignmentOptions.Left));
                Set(c, "taskText", AddText(TopLeft("Task", filled, 124, 46, 266, 32), "", 22, Muted, TextAlignmentOptions.Left));
                Set(c, "wageText", AddText(TopLeft("Wage", filled, 124, 80, 266, 32), "", 24, Orange, TextAlignmentOptions.Left));
                var cancel = AddButton(TopLeft("CancelButton", filled, 95, 136, 200, 64), buttonDisabled, Cream);
                StretchText(cancel.transform, "Label", "고용 취소", 26, DarkCoffee);
                Set(c, "cancelButton", cancel);
                Set(c, "filled", filled.gameObject);

                var empty = TopLeft("Empty", rt, 15, 40, 360, 150);
                AddImage(empty, LoadSprite($"{Art}/StaffSlotEmpty.png"), Cream);
                StretchText(empty, "Label", "빈 자리", 32, Muted);
                Set(c, "empty", empty.gameObject);
                c.ApplyModifiedPropertiesWithoutUndo();
                slots[i] = slot;
            }
            SetArray(so, "hiredSlots", slots);
            page.gameObject.SetActive(false);
            return page.gameObject;
        }
    }
}
