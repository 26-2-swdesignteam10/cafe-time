using UnityEngine;
using UnityEngine.SceneManagement;

namespace CafeTime.Core
{
    // 씬 이름은 여기서만 관리 (문자열 오타 방지). 새 씬을 만들면 상수 추가 + Build Profiles 씬 목록에 등록
    public static class SceneNames
    {
        public const string Title = "Title";
        public const string Story = "Story";        // SFR-007 스토리 화면
        public const string OpenPrep = "OpenPrep";  // SFR-010 오픈 준비
        public const string Cafe = "Cafe";          // SFR-012 영업 (카페 맵)
        public const string Settlement = "Settlement"; // SFR-071 하루 정산
        public const string Ending = "Ending";      // SFR-073 엔딩
    }

    public static class SceneLoader
    {
        public static bool Exists(string sceneName) => Application.CanStreamedLevelBeLoaded(sceneName);

        public static void Load(string sceneName)
        {
            if (Exists(sceneName))
                SceneManager.LoadScene(sceneName);
            else
                Debug.LogWarning($"'{sceneName}' 씬이 아직 없습니다. (Build Profiles의 씬 목록에 추가 필요)");
        }

        // 아직 다른 브랜치에서 개발 중인 씬이 있을 때, 있는 씬 중 첫 번째로 이동 (ADR-0011)
        public static void LoadFirstAvailable(params string[] sceneNames)
        {
            foreach (var name in sceneNames)
            {
                if (!Exists(name)) continue;
                if (name != sceneNames[0])
                    Debug.Log($"'{sceneNames[0]}' 씬이 없어 '{name}' 씬으로 이동합니다.");
                SceneManager.LoadScene(name);
                return;
            }
            Debug.LogWarning($"이동할 씬이 없습니다: {string.Join(", ", sceneNames)}");
        }
    }
}
