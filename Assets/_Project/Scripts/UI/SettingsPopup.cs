using CafeTime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.UI
{
    // 설정 창 (SFR-004). 메인 메뉴와 일시정지 메뉴(SFR-023)에서 같은 프리팹을 사용
    // 확인: 변경 내용 저장 / 취소·X·ESC: 변경 내용 취소
    public class SettingsPopup : Popup
    {
        [SerializeField] Button soundTabButton;
        [SerializeField] Button keyTabButton;
        [SerializeField] Sprite soundNormalSprite;
        [SerializeField] Sprite soundSelectedSprite;
        [SerializeField] Sprite keyNormalSprite;
        [SerializeField] Sprite keySelectedSprite;
        [SerializeField] GameObject soundTab;
        [SerializeField] GameObject keyTab;
        [SerializeField] Slider masterSlider;
        [SerializeField] Slider bgmSlider;
        [SerializeField] Slider sfxSlider;
        [SerializeField] Button confirmButton;
        [SerializeField] Button cancelButton;

        protected override void Awake()
        {
            base.Awake();
            soundTabButton.onClick.AddListener(() => ShowTab(true));
            keyTabButton.onClick.AddListener(() => ShowTab(false));
            confirmButton.onClick.AddListener(Confirm);
            cancelButton.onClick.AddListener(Close);
        }

        public override void Open()
        {
            masterSlider.value = GameSettings.MasterVolume;
            bgmSlider.value = GameSettings.BgmVolume;
            sfxSlider.value = GameSettings.SfxVolume;
            ShowTab(true);
            base.Open();
        }

        void Confirm()
        {
            GameSettings.SetVolumes(masterSlider.value, bgmSlider.value, sfxSlider.value);
            Close();
        }

        void ShowTab(bool sound)
        {
            soundTab.SetActive(sound);
            keyTab.SetActive(!sound);
            soundTabButton.image.sprite = sound ? soundSelectedSprite : soundNormalSprite;
            keyTabButton.image.sprite = sound ? keyNormalSprite : keySelectedSprite;
        }
    }
}
