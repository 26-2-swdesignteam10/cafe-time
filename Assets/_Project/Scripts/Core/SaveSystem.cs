using System.IO;
using UnityEngine;

namespace CafeTime.Core
{
    // 저장 파일 읽기/쓰기 + 현재 플레이 중인 게임 데이터 보관
    public static class SaveSystem
    {
        static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

        // 현재 플레이 중인 게임 데이터 (새 게임 또는 이어하기로 설정됨). 다른 씬에서 이 값을 사용
        public static SaveData Current { get; private set; }

        public static bool HasSave => File.Exists(SavePath);

        public static SaveData Load()
        {
            if (!HasSave) return null;
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
        }

        // 하루 정산 화면에서 호출 (SFR-071)
        public static void SaveCurrent()
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(Current, true));
        }

        public static void Delete()
        {
            if (HasSave) File.Delete(SavePath);
        }

        // 처음부터 시작: 기존 저장 데이터를 지우고 DAY1 데이터로 시작 (SFR-002)
        public static void StartNewGame(string nickname)
        {
            Delete();
            Current = new SaveData { nickname = nickname };
        }

        // 이어하기: 마지막 저장 시점부터 진행 (SFR-002)
        public static bool ContinueGame()
        {
            Current = Load();
            return Current != null;
        }
    }
}
