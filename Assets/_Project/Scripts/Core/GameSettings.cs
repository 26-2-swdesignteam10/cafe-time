using UnityEngine;

namespace CafeTime.Core
{
    // 사운드 설정 값 저장/불러오기 (SFR-004)
    public static class GameSettings
    {
        const string MasterKey = "Volume.Master";
        const string BgmKey = "Volume.Bgm";
        const string SfxKey = "Volume.Sfx";

        public static float MasterVolume { get; private set; } = PlayerPrefs.GetFloat(MasterKey, 1f);
        public static float BgmVolume { get; private set; } = PlayerPrefs.GetFloat(BgmKey, 1f);
        public static float SfxVolume { get; private set; } = PlayerPrefs.GetFloat(SfxKey, 1f);

        public static void SetVolumes(float master, float bgm, float sfx)
        {
            MasterVolume = master;
            BgmVolume = bgm;
            SfxVolume = sfx;
            PlayerPrefs.SetFloat(MasterKey, master);
            PlayerPrefs.SetFloat(BgmKey, bgm);
            PlayerPrefs.SetFloat(SfxKey, sfx);
            PlayerPrefs.Save();
            Apply();
        }

        // 배경음악/효과음 볼륨은 사운드 기능 구현 시 각 AudioSource에서 BgmVolume/SfxVolume을 사용
        public static void Apply()
        {
            AudioListener.volume = MasterVolume;
        }
    }
}
