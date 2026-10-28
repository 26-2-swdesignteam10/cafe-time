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
    }

    public static class SceneLoader
    {
        public static void Load(string sceneName)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
                SceneManager.LoadScene(sceneName);
            else
                Debug.LogWarning($"'{sceneName}' 씬이 아직 없습니다. (Build Profiles의 씬 목록에 추가 필요)");
        }
    }
}
