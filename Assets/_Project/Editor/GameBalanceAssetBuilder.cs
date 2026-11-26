using System.IO;
using System.Linq;
using CafeTime.Core;
using UnityEditor;
using UnityEngine;

namespace CafeTime.EditorTools
{
    // CafeTime > Create Game Balance : Resources/GameBalance.asset 생성 (ADR-0009)
    // 이미 있으면 기존 수치를 보존하고 메뉴 구성 동기화(ADR-0038)·재료 아이콘 연결을 한다.
    public static class GameBalanceAssetBuilder
    {
        const string AssetPath = "Assets/_Project/Resources/GameBalance.asset";
        const string IconFolder = "Assets/_Project/Art/Icons/Ingredients";

        [MenuItem("CafeTime/Create Game Balance")]
        public static void CreateOrUpdate()
        {
            var balance = AssetDatabase.LoadAssetAtPath<GameBalance>(AssetPath);
            if (balance == null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(AssetPath));
                balance = ScriptableObject.CreateInstance<GameBalance>(); // 기본값 = docs/GAME_DATA.md 임시 값
                AssetDatabase.CreateAsset(balance, AssetPath);
                Debug.Log("GameBalance 에셋 생성: " + AssetPath);
            }

            balance.EnsureCatalog();
            RaiseCupCounts(balance);
            MigrateBusinessHours(balance);
            RaiseWaterSeconds(balance);
            MigrateRent(balance);
            EditorUtility.SetDirty(balance);
            AssignIcons(balance);
            AssetDatabase.SaveAssets();
            Selection.activeObject = balance;
        }

        // 기존 에셋의 컵 개수가 2026-11-18 늘린 값(HOT 16, ICE 20)보다 적으면 올린다. 더 많이 바꿔 둔 값은 그대로
        static void RaiseCupCounts(GameBalance balance)
        {
            var so = new SerializedObject(balance);
            var ceramic = so.FindProperty("ceramicCups");
            var plastic = so.FindProperty("plasticCups");
            ceramic.intValue = Mathf.Max(ceramic.intValue, 16);
            plastic.intValue = Mathf.Max(plastic.intValue, 20);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // 기존 에셋의 영업 시간이 예전 값이면 지금 값으로 바꾼다. 따로 고친 값은 그대로
        //  - 1시간 120초 → 35초 (ADR-0021)
        //  - 입장 마감 17:30·종료 18시(ADR-0044) → 입장 마감 20시·마감 21시 (ADR-0055)
        static void MigrateBusinessHours(GameBalance balance)
        {
            var so = new SerializedObject(balance);
            var seconds = so.FindProperty("secondsPerGameHour");
            var entry = so.FindProperty("entryCloseHour");
            var close = so.FindProperty("closeHour");
            if (Mathf.Approximately(seconds.floatValue, 120f)) seconds.floatValue = 35f;
            if (Mathf.Approximately(entry.floatValue, 17.5f)) entry.floatValue = 20f;
            if (close.intValue == 18) close.intValue = 21;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // 물 받는 시간이 2026-11-24 사용자 요청 값(7초)보다 짧으면 올린다. 에셋에 3초가 남아 있던 문제도 같이 고친다. 더 길게 바꿔 둔 값은 그대로
        static void RaiseWaterSeconds(GameBalance balance)
        {
            var so = new SerializedObject(balance);
            var water = so.FindProperty("waterSeconds");
            water.floatValue = Mathf.Max(water.floatValue, 7f);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // 임대료가 예전 값(매일 ₩60,000)이면 날마다 오르는 값으로 바꾼다 (ADR-0056). 따로 고친 값은 그대로
        static void MigrateRent(GameBalance balance)
        {
            var so = new SerializedObject(balance);
            var rent = so.FindProperty("rentByDay");
            bool old = rent.arraySize > 0;
            for (int i = 0; i < rent.arraySize; i++) old &= rent.GetArrayElementAtIndex(i).intValue == 60000;
            if (!old) return;
            var values = GameBalance.DefaultRent();
            rent.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++) rent.GetArrayElementAtIndex(i).intValue = values[i];
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // 아이콘 파일 이름 = 재료 id 의 PascalCase (bean → Bean.png, syrup_vanilla → SyrupVanilla.png)
        static void AssignIcons(GameBalance balance)
        {
            BuilderUtil.PrepareSprites(IconFolder);
            var so = new SerializedObject(balance);
            var list = so.FindProperty("ingredients");
            int missing = 0;
            for (int i = 0; i < list.arraySize; i++)
            {
                var element = list.GetArrayElementAtIndex(i);
                string id = element.FindPropertyRelative("id").stringValue;
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{IconFolder}/{ToPascal(id)}.png");
                if (sprite == null) missing++;
                element.FindPropertyRelative("icon").objectReferenceValue = sprite;
            }
            so.ApplyModifiedPropertiesWithoutUndo();
            if (missing > 0) Debug.LogWarning($"재료 아이콘 {missing}개가 없습니다. ({IconFolder})");
        }

        static string ToPascal(string id) =>
            string.Concat(id.Split('_').Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
    }
}
