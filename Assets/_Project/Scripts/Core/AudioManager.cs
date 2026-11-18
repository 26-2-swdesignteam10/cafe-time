using UnityEngine;
using System.Collections.Generic;

namespace CafeTime.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        readonly HashSet<UnityEngine.Object> pouringOwners =
            new HashSet<UnityEngine.Object>();

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
        [SerializeField] AudioClip serveClip;
        [SerializeField] AudioClip waterPourClip;
        [SerializeField] AudioClip workCompleteClip;
        [SerializeField] AudioClip iceScoopClip;
        [SerializeField] AudioClip smoothiePourClip;
        [SerializeField] AudioClip blenderClip;
        [SerializeField] AudioClip dishScrubClip;


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

        [Range(0f, 1f)]
        [SerializeField] float serveVolume = 0.3f;

        [Range(0f, 1f)]
        [SerializeField] float waterPourVolume = 0.25f;

        [Range(0f, 1f)]
        [SerializeField] float workCompleteVolume = 0.2f;

        [Range(0f, 1f)]
        [SerializeField] float iceScoopVolume = 0.3f;

        [Range(0f, 1f)]
        [SerializeField] float smoothiePourVolume = 0.3f;

        [Range(0f, 1f)]
        [SerializeField] float blenderVolume = 0.2f;

        [Range(0f, 1f)]
        [SerializeField] float dishScrubVolume = 0.25f;

        AudioSource uiSource;
        AudioSource waterPourSource;
        AudioSource smoothiePourSource;
        AudioSource blenderSource;
        AudioSource dishScrubSource;

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

            // 물 따르기 전용 반복 재생 장치
            waterPourSource = gameObject.AddComponent<AudioSource>();
            waterPourSource.playOnAwake = false;
            waterPourSource.loop = true;
            waterPourSource.spatialBlend = 0f;

            smoothiePourSource = gameObject.AddComponent<AudioSource>();
            smoothiePourSource.playOnAwake = false;
            smoothiePourSource.loop = true;
            smoothiePourSource.spatialBlend = 0f;

            blenderSource = gameObject.AddComponent<AudioSource>();
            blenderSource.playOnAwake = false;
            blenderSource.loop = true;
            blenderSource.spatialBlend = 0f;

            dishScrubSource = gameObject.AddComponent<AudioSource>();
            dishScrubSource.playOnAwake = false;
            dishScrubSource.loop = true;
            dishScrubSource.spatialBlend = 0f;
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

        public void PlayWorkComplete()
        {
            PlaySfx(workCompleteClip, workCompleteVolume);
        }

        public void SetBlenderRunning(bool running)
        {
            if (blenderSource == null) return;

            blenderSource.volume =
                blenderVolume * GameSettings.SfxVolume;

            if (!running || blenderClip == null)
            {
                if (blenderSource.isPlaying)
                    blenderSource.Stop();

                return;
            }

            if (blenderSource.clip != blenderClip)
                blenderSource.clip = blenderClip;

            if (!blenderSource.isPlaying)
                blenderSource.Play();
        }

        public void SetDishScrubbing(bool scrubbing)
        {
            if (dishScrubSource == null) return;

            dishScrubSource.volume =
                dishScrubVolume * GameSettings.SfxVolume;

            if (!scrubbing || dishScrubClip == null)
            {
                if (dishScrubSource.isPlaying)
                    dishScrubSource.Stop();

                return;
            }

            if (dishScrubSource.clip != dishScrubClip)
                dishScrubSource.clip = dishScrubClip;

            if (!dishScrubSource.isPlaying)
                dishScrubSource.Play();
        }

        public void SetSmoothiePouring(bool pouring)
        {
            if (smoothiePourSource == null) return;

            smoothiePourSource.volume =
                smoothiePourVolume * GameSettings.SfxVolume;

            if (!pouring || smoothiePourClip == null)
            {
                if (smoothiePourSource.isPlaying)
                    smoothiePourSource.Stop();

                return;
            }

            if (smoothiePourSource.clip != smoothiePourClip)
                smoothiePourSource.clip = smoothiePourClip;

            if (!smoothiePourSource.isPlaying)
                smoothiePourSource.Play();
        }

        public void PlayIceScoop()
        {
            PlaySfx(iceScoopClip, iceScoopVolume);
        }

        public void SetWaterPouring(
    bool pouring,
    UnityEngine.Object owner = null)
        {
            if (waterPourSource == null) return;

            // 기존 정수기 호출은 owner 없이 그대로 사용할 수 있다.
            if (owner == null)
                owner = this;

            if (pouring)
                pouringOwners.Add(owner);
            else
                pouringOwners.Remove(owner);

            pouringOwners.RemoveWhere(target => target == null);

            waterPourSource.volume =
                waterPourVolume * GameSettings.SfxVolume;

            if (pouringOwners.Count == 0 || waterPourClip == null)
            {
                if (waterPourSource.isPlaying)
                    waterPourSource.Stop();

                return;
            }

            if (waterPourSource.clip != waterPourClip)
                waterPourSource.clip = waterPourClip;

            if (!waterPourSource.isPlaying)
                waterPourSource.Play();
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

        public void PlayServe()
        {
            PlaySfx(serveClip, serveVolume);
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
