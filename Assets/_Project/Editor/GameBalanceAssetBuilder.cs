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
