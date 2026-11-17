using UnityEngine;

namespace CafeTime.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("재생 장치")]
        [SerializeField] AudioSource bgmSource;
        [SerializeField] AudioSource sfxSource;

        [Header("음원")]
        [SerializeField] AudioClip cafeBgm;
        [SerializeField] AudioClip paymentClip;
        [SerializeField] AudioClip buttonClickClip;
        [SerializeField] AudioClip customerEnterClip;
        [SerializeField] AudioClip interactClip;
        [SerializeField] AudioClip warningClip;


        [Header("기본 볼륨")]
        [Range(0f, 1f)]
        [SerializeField] float bgmVolume = 0.3f;

        [Range(0f, 1f)]
        [SerializeField] float paymentVolume = 0.3f;

        [Range(0f, 1f)]
        [SerializeField] float buttonClickVolume = 0.2f;

        [Range(0f, 1f)]
        [SerializeField] float interactVolume = 0.2f;

        [Range(0f, 1f)]
        [SerializeField] float customerEnterVolume = 0.3f;

        [Range(0f, 1f)]
        [SerializeField] float warningVolume = 0.25f;

        AudioSource uiSource;

        AudioClip pendingUiClip;
        float pendingUiVolume;

        float warningUntil;
        int warningFrame = -1;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            bgmSource.playOnAwake = false;
            bgmSource.loop = true;
            bgmSource.spatialBlend = 0f;

            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            sfxSource.spatialBlend = 0f;
            sfxSource.volume = 1f;

            // 클릭·E 소리를 경고음과 따로 관리한다.
            uiSource = gameObject.AddComponent<AudioSource>();
            uiSource.playOnAwake = false;
            uiSource.loop = false;
            uiSource.spatialBlend = 0f;
            uiSource.volume = 1f;
        }

        void Start()
        {
            if (cafeBgm == null) return;

            ApplyBgmVolume();
            bgmSource.clip = cafeBgm;
            bgmSource.Play();
        }

        void Update()
        {
            ApplyBgmVolume();
        }

        void ApplyBgmVolume()
        {
            bgmSource.volume =
                bgmVolume * GameSettings.BgmVolume;
        }

        public void PlayPayment()
        {
            PlaySfx(paymentClip, paymentVolume);
        }

        public void PlayWarning()
        {
            if (warningClip == null) return;

            // 예약된 클릭·E 소리 취소
            pendingUiClip = null;

            // 이미 재생 중인 클릭·E 소리도 중지
            if (uiSource != null)
                uiSource.Stop();

            warningFrame = Time.frameCount;

            float pitch = Mathf.Max(
                Mathf.Abs(sfxSource.pitch),
                0.01f
            );

            warningUntil =
                Time.unscaledTime + warningClip.length / pitch;

            PlaySfx(warningClip, warningVolume);
        }

        public void PlayButtonClick()
        {
            QueueUiSound(buttonClickClip, buttonClickVolume);
        }

        public void PlayInteract()
        {
            QueueUiSound(interactClip, interactVolume);
        }

        public void PlayCustomerEnter()
        {
            PlaySfx(customerEnterClip, customerEnterVolume);
        }

        void PlaySfx(AudioClip clip, float volume)
        {
            if (clip == null) return;

            sfxSource.PlayOneShot(
                clip,
                volume * GameSettings.SfxVolume
            );
        }

        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        bool WarningActive =>
    warningFrame == Time.frameCount ||
    Time.unscaledTime < warningUntil;

        void QueueUiSound(AudioClip clip, float volume)
        {
            if (clip == null || WarningActive) return;

            pendingUiClip = clip;
            pendingUiVolume = volume;
        }

        void LateUpdate()
        {
            if (pendingUiClip == null) return;

            var clip = pendingUiClip;
            float volume = pendingUiVolume;
            pendingUiClip = null;

            // 같은 동작에서 경고가 발생했다면 클릭·E 소리를 재생하지 않는다.
            if (WarningActive) return;

            uiSource.PlayOneShot(
                clip,
                volume * GameSettings.SfxVolume
            );
        }
    }
}
