using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.OpenPrep
{
    // 영업 준비 화면 (ADR-0057): 매일 영업 전에 장비를 업그레이드하고 오늘 일할 알바생을 고용한다.
    // 재료 구매는 없앴다 (재고 없음, ADR-0056). 업그레이드 비용은 바로 내고, 일당은 하루 정산 때 임대료와 함께 낸다
    public class OpenPrepScreen : MonoBehaviour
    {
        // 고정 문구(제목, 탭 이름, 보유금)는 씬의 이미지 라벨이고, 여기서는 바뀌는 값만 다룬다
        [Header("상단")]
        [SerializeField] TMP_Text dayText;    // "DAY 3"
        [SerializeField] TMP_Text moneyText;  // "86,500원"
        [SerializeField] TMP_Text costText;   // "오늘 정산 때 낼 돈: 임대료 48,000원 + 일당 15,000원"
        [SerializeField] Button upgradeTab;
        [SerializeField] Button staffTab;
        [SerializeField] Sprite tabOn;
        [SerializeField] Sprite tabOff;
        [SerializeField] GameObject upgradeTitle;
        [SerializeField] GameObject staffTitle;
        [SerializeField] GameObject upgradePage;
        [SerializeField] GameObject staffPage;

        [Header("장비 업그레이드")]
        [Tooltip("Equipment 순서")]
        [SerializeField] EquipmentCard[] equipmentCards;
        [Tooltip("Equipment 순서")]
        [SerializeField] Sprite[] equipmentIcons;
        [SerializeField] Image detailIcon;
        [SerializeField] TMP_Text detailName;
        [SerializeField] Image[] detailStars;
        [SerializeField] TMP_Text detailDescription;
        [SerializeField] Image[] stageBoxes;
        [SerializeField] TMP_Text[] stageTitles;   // "1단계" / "1단계 완료"
        [SerializeField] TMP_Text[] stageTexts;
        [SerializeField] Sprite stageTodo;
        [SerializeField] Sprite stageDone;
        [SerializeField] TMP_Text nextText;        // "다음 단계: 2단계 (한 번에 2개)"
        [SerializeField] Button upgradeButton;
        [SerializeField] TMP_Text upgradeLabel;    // "업그레이드 18,000원"
        [SerializeField] Sprite buttonNormal;
        [SerializeField] Sprite buttonDisabled;
        [SerializeField] Sprite starFull;
        [SerializeField] Sprite starEmpty;

        [Header("알바생 고용")]
        [Tooltip("GameBalance.Staff 순서")]
        [SerializeField] StaffCard[] staffCards;
        [Tooltip("GameBalance.Staff 순서")]
        [SerializeField] Sprite[] staffPortraits;
        [SerializeField] TMP_Text hiredCountText; // "오늘의 알바생 1 / 2"
        [SerializeField] HiredSlot[] hiredSlots;

        [SerializeField] Button startButton;
        [SerializeField] Color normalTextColor = new Color32(0x5A, 0x35, 0x20, 0xFF);
        [SerializeField] Color priceColor = new Color32(0xD9, 0x6B, 0x1E, 0xFF);
        [SerializeField] Color mutedColor = new Color32(0x9A, 0x86, 0x70, 0xFF);
        [SerializeField] Color warningColor = new Color32(0xC0, 0x39, 0x2B, 0xFF);

        Equipment selected = Equipment.Dishwasher;
        GameBalance balance;

        SaveData Save => SaveSystem.Current;

        void Start()
        {
            SaveSystem.EnsureCurrentForPreview();
            balance = GameBalance.Instance;
            if (balance == null) return;

            for (int i = 0; i < equipmentCards.Length; i++)
            {
                var equipment = (Equipment)i;
                equipmentCards[i].Init(() => { selected = equipment; Refresh(); });
            }
            for (int i = 0; i < staffCards.Length; i++)
            {
                int index = i;
                staffCards[i].Init(() => Hire(balance.Staff[index]));
            }
            for (int i = 0; i < hiredSlots.Length; i++)
            {
                int index = i;
                hiredSlots[i].Init(() => Dismiss(index));
            }
            upgradeTab.onClick.AddListener(() => ShowPage(upgrade: true));
            staffTab.onClick.AddListener(() => ShowPage(upgrade: false));
            upgradeButton.onClick.AddListener(Upgrade);
            startButton.onClick.AddListener(StartBusiness);

            Save.hiredStaff.RemoveAll(id => balance.FindStaff(id) == null);
            dayText.text = $"DAY {Save.day}";
            ShowPage(upgrade: true);
        }

        void ShowPage(bool upgrade)
        {
            upgradePage.SetActive(upgrade);
            staffPage.SetActive(!upgrade);
            upgradeTitle.SetActive(upgrade);
            staffTitle.SetActive(!upgrade);
            upgradeTab.image.sprite = upgrade ? tabOn : tabOff;
            staffTab.image.sprite = upgrade ? tabOff : tabOn;
            Refresh();
        }

        int Wages => Save.hiredStaff.Sum(id => balance.FindStaff(id)?.Wage ?? 0);

        void Refresh()
        {
            int rent = balance.GetRent(Save.day);
            moneyText.text = $"{Save.money:N0}원";
            costText.text = Wages > 0 ? $"오늘 정산: 임대료 {rent:N0}원 + 일당 {Wages:N0}원" : $"오늘 정산: 임대료 {rent:N0}원";
            costText.color = Save.money < rent + Wages ? warningColor : normalTextColor;
            RefreshUpgrade(rent);
            RefreshStaff();
        }

        // ───────── 장비 업그레이드 ─────────

        void RefreshUpgrade(int rent)
        {
            for (int i = 0; i < equipmentCards.Length; i++)
            {
                var definition = balance.FindEquipment((Equipment)i);
                equipmentCards[i].Show(definition, Save.GetLevel((Equipment)i), (Equipment)i == selected, starFull, starEmpty, priceColor, mutedColor);
            }

            var equipment = balance.FindEquipment(selected);
            int level = Save.GetLevel(selected);
            detailIcon.sprite = equipmentIcons[(int)selected];
            detailName.text = equipment.DisplayName;
            detailDescription.text = equipment.Description;
            for (int i = 0; i < detailStars.Length; i++) detailStars[i].sprite = i < level ? starFull : starEmpty;
            for (int i = 0; i < stageBoxes.Length; i++)
            {
                bool done = i < level;
                stageBoxes[i].sprite = done ? stageDone : stageTodo;
                stageTitles[i].text = done ? $"{i + 1}단계 완료" : $"{i + 1}단계";
                stageTitles[i].color = done ? mutedColor : normalTextColor;
                stageTexts[i].text = equipment.StageText(i + 1);
                stageTexts[i].color = done ? mutedColor : normalTextColor;
            }

            bool max = level >= EquipmentDefinition.MaxLevel;
            int price = max ? 0 : equipment.PriceOf(level + 1);
            bool affordable = price <= Save.money;
            nextText.text = max ? "최고 단계예요" : $"다음 단계: {level + 1}단계 ({equipment.StageText(level + 1)})";
            upgradeLabel.text = max ? "최고 단계" : $"업그레이드 {price:N0}원";
            upgradeButton.interactable = !max && affordable;
            upgradeButton.image.sprite = upgradeButton.interactable ? buttonNormal : buttonDisabled;
        }

        void Upgrade()
        {
            int level = Save.GetLevel(selected);
            if (level >= EquipmentDefinition.MaxLevel) return;
            int price = balance.FindEquipment(selected).PriceOf(level + 1);
            if (price > Save.money) return;
            Save.money -= price;
            Save.SetLevel(selected, level + 1);
            // 저장은 하루 정산 때만 한다 (ADR-0005). 영업 도중 종료하면 업그레이드도 취소된다
            Refresh();
        }

        // ───────── 알바생 고용 ─────────

        Sprite Portrait(StaffDefinition definition)
        {
            int index = balance.Staff.ToList().IndexOf(definition);
            return index >= 0 && index < staffPortraits.Length ? staffPortraits[index] : null;
        }

        void RefreshStaff()
        {
            bool full = Save.hiredStaff.Count >= balance.MaxStaffPerDay;
            for (int i = 0; i < staffCards.Length; i++)
            {
                if (i >= balance.Staff.Count) { staffCards[i].gameObject.SetActive(false); continue; }
                var definition = balance.Staff[i];
                var state = Save.hiredStaff.Contains(definition.Id) ? StaffCard.State.Hired
                    : Save.day < definition.UnlockDay ? StaffCard.State.Locked
                    : full ? StaffCard.State.Full
                    : StaffCard.State.Hirable;
                staffCards[i].Show(definition, Portrait(definition), state);
            }
            hiredCountText.text = $"오늘의 알바생 {Save.hiredStaff.Count} / {balance.MaxStaffPerDay}";
            for (int i = 0; i < hiredSlots.Length; i++)
            {
                hiredSlots[i].gameObject.SetActive(i < balance.MaxStaffPerDay);
                var definition = i < Save.hiredStaff.Count ? balance.FindStaff(Save.hiredStaff[i]) : null;
                hiredSlots[i].Show(definition, definition != null ? Portrait(definition) : null);
            }
        }

        void Hire(StaffDefinition definition)
        {
            if (Save.hiredStaff.Contains(definition.Id) || Save.hiredStaff.Count >= balance.MaxStaffPerDay) return;
            if (Save.day < definition.UnlockDay) return;
            Save.hiredStaff.Add(definition.Id);
            Refresh();
        }

        void Dismiss(int index)
        {
            if (index >= Save.hiredStaff.Count) return;
            Save.hiredStaff.RemoveAt(index);
            Refresh();
        }

        static void StartBusiness()
        {
            DayResult.StartNewDay();
            SceneLoader.LoadFirstAvailable(SceneNames.Cafe, SceneNames.Settlement);
        }
    }
}
