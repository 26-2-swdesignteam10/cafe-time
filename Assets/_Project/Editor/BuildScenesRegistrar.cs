using System.Collections.Generic;
using System.IO;
using System.Linq;
using CafeTime.Core;
using UnityEditor;
using UnityEngine;

namespace CafeTime.EditorTools
{
    // 게임 흐름에 쓰는 씬을 Build Settings 에 순서대로 등록한다 (첫 씬 = Title).
    // 씬 파일이 아직 없으면 건너뛴다. 목록에 없는 씬(테스트 씬 등)은 빼고 다시 쓴다.
    public static class BuildScenesRegistrar
    {
        static readonly string[] Order =
        {
            SceneNames.Title, SceneNames.Story, SceneNames.OpenPrep, SceneNames.Cafe,
            SceneNames.Score, SceneNames.Settlement, SceneNames.Ending,
        };

        [MenuItem("CafeTime/Register Build Scenes")]
        public static void Register()
        {
            var scenes = new List<EditorBuildSettingsScene>();
            var missing = new List<string>();
            foreach (var name in Order)
            {
                string path = $"Assets/_Project/Scenes/{name}.unity";
                if (File.Exists(path)) scenes.Add(new EditorBuildSettingsScene(path, true));
                else missing.Add(name);
            }
            EditorBuildSettings.scenes = scenes.ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("Build Settings 씬 등록: " + string.Join(" → ", scenes.Select(s => Path.GetFileNameWithoutExtension(s.path))) +
                      (missing.Count > 0 ? $" (없어서 건너뜀: {string.Join(", ", missing)})" : ""));
        }
    }
}
