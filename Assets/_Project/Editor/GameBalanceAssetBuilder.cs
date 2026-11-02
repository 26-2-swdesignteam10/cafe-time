using System.IO;
using System.Linq;
using CafeTime.Core;
using UnityEditor;
using UnityEngine;

namespace CafeTime.EditorTools
{
    // CafeTime > Create Game Balance : Resources/GameBalance.asset 생성 (ADR-0009)
    // 이미 있으면 수치는 그대로 두고 재료 아이콘 연결만 다시 한다.
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

            AssignIcons(balance);
            AssetDatabase.SaveAssets();
            Selection.activeObject = balance;
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
