using System.Collections.Generic;
using System.Linq;
using CafeTime.Core;

namespace CafeTime.Cafe
{
    // 커피잔·믹서기 용기에 담긴 내용물 (재료 key → 양). key 는 재료 id 또는 RecipeKeys (SFR-045, 053)
    public class Mixture
    {
        readonly Dictionary<string, float> amounts = new Dictionary<string, float>();

        public IReadOnlyDictionary<string, float> Amounts => amounts;
        public bool IsEmpty => amounts.Count == 0;
        public IEnumerable<string> Keys => amounts.Keys;

        public float Get(string key) => amounts.TryGetValue(key, out var v) ? v : 0f;

        public void Add(string key, float amount)
        {
            if (amount <= 0) return;
            amounts[key] = Get(key) + amount;
        }

        public void AddAll(Mixture other)
        {
            foreach (var pair in other.amounts) Add(pair.Key, pair.Value);
        }

        public void Clear() => amounts.Clear();

        public bool OnlyWater => amounts.Count > 0 && amounts.Keys.All(k => k == RecipeKeys.HotWater || k == RecipeKeys.ColdWater);

        public bool HasTeabag(GameBalance balance) =>
            amounts.Keys.Any(k => balance.FindIngredient(k) is { Category: IngredientCategory.Tea });

        // 전자저울 표시 무게 (g, 잔·용기 무게 제외, SFR-045)
        public float Weight(GameBalance balance)
        {
            float total = 0;
            foreach (var pair in amounts)
            {
                if (pair.Key == RecipeKeys.Shot) total += pair.Value * balance.ShotWeight;
                else if (pair.Key == RecipeKeys.Ice) total += pair.Value * balance.IceScoopWeight;
                else if (balance.FindIngredient(pair.Key) is { Category: IngredientCategory.Tea }) total += pair.Value * balance.TeabagWeight;
                else total += pair.Value;
            }
            return total;
        }
    }
}
