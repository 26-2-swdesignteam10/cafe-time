using CafeTime.Core;
using UnityEditor;
using UnityEngine;

namespace CafeTime.EditorTools
{
    // CafeTime > Build Audio Manager : AudioManager 프리팹(Prefabs/AudioManager.prefab)을 코드로 다시 만든다 (ADR-0006, ADR-0054).
    // 음원·볼륨은 PR #19, #25 작성자가 맞춘 값. 씬 빌더(Cafe, OpenPrep, Title)는 AddToScene 으로 이 프리팹을 넣는다.
    // 씬에서 직접 음원을 연결하면 씬을 다시 빌드할 때 사라지므로, 새 음원은 여기 목록에 추가한다
    public static class AudioManagerBuilder
    {
        public const string PrefabPath = "Assets/_Project/Prefabs/AudioManager.prefab";
        const string AudioDir = "Assets/_Project/Audio";

        public const string CafeBgm = "BGM/cafebgm.mp3";
        public const string TitleBgm = "BGM/titlebgm.mp3"; // 타이틀·오픈 준비 화면 (PR #25)

        // AudioManager 필드 이름, 음원 파일(Audio 기준), 볼륨 필드 이름, 볼륨
        static readonly (string clip, string file, string volumeField, float volume)[] Clips =
        {
            ("cafeBgm", CafeBgm, "bgmVolume", 0.1f),
            ("paymentClip", "SFX/cashsound.mp3", "paymentVolume", 0.3f),
            ("buttonClickClip", "SFX/clicksound.ogg", "buttonClickVolume", 1f),
            ("customerEnterClip", "SFX/doorbellsound.mp3", "customerEnterVolume", 0.3f),
            ("interactClip", "SFX/eclicksound.ogg", "interactVolume", 0.7f),
            ("warningClip", "SFX/errorsound.mp3", "warningVolume", 0.25f),
            ("serveClip", "SFX/bellsound.mp3", "serveVolume", 0.3f),
            ("waterPourClip", "SFX/waterpoursound.mp3", "waterPourVolume", 0.25f),
            ("workCompleteClip", "SFX/alarmsound.mp3", "workCompleteVolume", 0.2f),
            ("iceScoopClip", "SFX/icesound.mp3", "iceScoopVolume", 0.15f),
            ("smoothiePourClip", "SFX/smoothiesound.mp3", "smoothiePourVolume", 0.3f),
            ("blenderClip", "SFX/mixersound.mp3", "blenderVolume", 0.2f),
            ("dishScrubClip", "SFX/scrubsound.mp3", "dishScrubVolume", 0.25f),
            // PR #25 커피 제조 효과음 (SFR-031, 035, 036, 046)
            ("grinderClip", "SFX/coffeegrindersound.mp3", "grinderVolume", 0.2f),
            ("espressoClip", "SFX/brewingsound.mp3", "espressoVolume", 0.2f),
            ("knockClip", "SFX/knockboxsound.mp3", "knockVolume", 0.3f),
            ("brushClip", "SFX/brushsound.mp3", "brushVolume", 0.5f),
            ("steamClip", "SFX/steamsound.mp3", "steamVolume", 0.2f),
            ("pumpClip", "SFX/pumpsound.mp3", "pumpVolume", 0.25f),
            ("cheongClip", "SFX/cheongsound.mp3", "cheongVolume", 1f),
        };

        [MenuItem("CafeTime/Build Audio Manager")]
        public static void BuildPrefab()
        {
            var go = new GameObject("AudioManager");
            try
            {
                AudioSource Source(string name)
                {
                    var child = new GameObject(name);
                    child.transform.SetParent(go.transform, false);
                    return child.AddComponent<AudioSource>();
                }
                var so = new SerializedObject(go.AddComponent<AudioManager>());
                so.FindProperty("bgmSource").objectReferenceValue = Source("BGMSource");
                so.FindProperty("sfxSource").objectReferenceValue = Source("SFXSource");
                foreach (var c in Clips)
                {
                    so.FindProperty(c.clip).objectReferenceValue = Load(c.file);
                    so.FindProperty(c.volumeField).floatValue = c.volume;
                }
                so.ApplyModifiedPropertiesWithoutUndo();
                PrefabUtility.SaveAsPrefabAsset(go, PrefabPath); // 같은 경로에 덮어써도 GUID 는 그대로
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        // 지금 열린 씬에 AudioManager 프리팹을 넣는다. bgm 은 이 씬의 배경음악 (Audio 기준 경로)
        public static void AddToScene(string bgm)
        {
            BuildPrefab(); // 프리팹을 늘 최신 목록으로 맞춘 뒤 넣는다
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (bgm == CafeBgm) return;
            var so = new SerializedObject(instance.GetComponent<AudioManager>());
            so.FindProperty("cafeBgm").objectReferenceValue = Load(bgm);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static AudioClip Load(string file)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>($"{AudioDir}/{file}");
            if (clip == null) Debug.LogWarning($"음원 없음: {AudioDir}/{file}");
            return clip;
        }
    }
}
