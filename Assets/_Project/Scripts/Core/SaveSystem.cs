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

        // 씬을 에디터에서 바로 Play 해서 만든 임시 데이터인지. 임시 데이터는 파일에 저장하지 않는다
        public static bool IsPreview { get; private set; }

        public static bool HasSave => File.Exists(SavePath);

        public static SaveData Load()
        {
            if (!HasSave) return null;
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
        }

        // 하루 정산 화면에서 호출 (SFR-071)
        public static void SaveCurrent()
        {
            if (IsPreview)
            {
                Debug.Log("미리보기 데이터라 저장하지 않습니다. (Title 씬에서 시작하면 저장됨)");
                return;
            }
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
            Current = CreateNewData(nickname);
            IsPreview = false;
        }

        // 이어하기: 마지막 저장 시점부터 진행 (SFR-002)
        public static bool ContinueGame()
        {
            Current = Load();
            IsPreview = false;
            return Current != null;
        }

        // Title을 거치지 않고 씬을 바로 Play 했을 때 쓸 임시 데이터를 만든다 (저장 파일은 건드리지 않음)
        public static void EnsureCurrentForPreview()
        {
            if (Current != null) return;
            Current = CreateNewData("미리보기");
            IsPreview = true;
        }

        static SaveData CreateNewData(string nickname)
        {
            var data = new SaveData { nickname = nickname };
            var balance = GameBalance.Instance;
            if (balance != null)
            {
                data.money = balance.StartingMoney;
                data.reputation = balance.InitialReputation;
            }
            return data;
        }
    }
}
