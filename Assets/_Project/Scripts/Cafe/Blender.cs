using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 쉐이크 제조실 믹서기 (SFR-052~055). 갈기는 화면을 닫아도 계속된다
    public class Blender : MonoBehaviour, IWorkStatus
    {
        public static Blender Instance { get; private set; }

        public Mixture Jar { get; } = new Mixture();
        public Cup CupSpot { get; private set; }

        float blendLeft;
        bool blended;
        bool done;
        GameBalance balance;

        public event Action Changed;
        public event Action<Cup> Judged;

        public BlenderState State =>
            blendLeft > 0 ? BlenderState.Blending : blended ? BlenderState.Blended : Jar.IsEmpty ? BlenderState.Empty : BlenderState.Filled;

        public float Weight => Jar.Weight(balance);

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // 갈기 중이거나 갈린 스무디 상태에서는 재료를 넣을 수 없다 (SFR-053)
        bool CanAdd => State == BlenderState.Empty || State == BlenderState.Filled;

        public void AddBase(IngredientDefinition ingredient)
        {
            if (!CanAdd) return;
            float amount = balance.AmountPerClick(ingredient);
            if (!Pantry.TryUse(ingredient.Id, amount)) { Toast.Show(ToastMessage.NoIngredient); return; }
            Jar.Add(ingredient.Id, amount);
            Changed?.Invoke();
        }

        public bool Pour(IngredientDefinition ingredient, float seconds)
        {
            if (!CanAdd) return false;
            float amount = balance.PourPerSecond * seconds;
            if (!Pantry.TryUse(ingredient.Id, amount)) { Toast.Show(ToastMessage.NoIngredient); return false; }
            Jar.Add(ingredient.Id, amount);
            Changed?.Invoke();
            return true;
        }

        // 얼음이 담긴 스쿱을 용기로 끌어 놓음
        public bool AddIceFromHand(int hand)
        {
            if (!CanAdd || !(HandInventory.Instance[hand] is IceScoop { HasIce: true } scoop)) return false;
            scoop.HasIce = false;
            Jar.Add(RecipeKeys.Ice, 1);
            HandInventory.Instance.NotifyChanged();
            Changed?.Invoke();
            return true;
        }

        public void Blend()
        {
            if (State == BlenderState.Blending || State == BlenderState.Blended) return;
            if (Jar.IsEmpty) { Toast.Show(ToastMessage.BlenderEmpty); return; }
            blendLeft = balance.BlendSeconds;
            Changed?.Invoke();
        }

        void Update()
        {
            if (blendLeft <= 0) return;
            blendLeft -= GameClock.Delta;
            if (blendLeft > 0) return;
            blendLeft = 0;
            blended = true;
            done = true;
            Changed?.Invoke();
        }

        // 커피잔 자리에는 빈 커피잔만 (SFR-055)
        public bool PlaceCup(int hand)
        {
            if (CupSpot != null || !(HandInventory.Instance[hand] is Cup { IsEmpty: true })) return false;
            CupSpot = (Cup)HandInventory.Instance.Take(hand);
            Changed?.Invoke();
            return true;
        }

        public void TakeCup()
        {
            if (CupSpot == null) return;
            if (!HandInventory.Instance.TryAdd(CupSpot)) { Toast.Show(ToastMessage.HandsFull); return; }
            CupSpot = null;
            Changed?.Invoke();
        }

        // 붓기 (SFR-055): 갈린 스무디를 잔에 붓고 판정. 용기는 바로 빈 용기로
        public void PourOut()
        {
            if (CupSpot == null) { Toast.Show(ToastMessage.PlaceCup); return; }
            if (State != BlenderState.Blended) { Toast.Show(ToastMessage.BlendFirst); return; }
            var cup = CupSpot;
            cup.AddAll(Jar);
            Jar.Clear();
            blended = false;
            DrinkJudge.Finish(cup, balance);
            if (HandInventory.Instance.TryAdd(cup)) CupSpot = null;
            else Toast.Show(ToastMessage.HandsFull);
            Judged?.Invoke(cup);
            Changed?.Invoke();
        }

        public float Progress => blendLeft > 0 ? 1f - blendLeft / balance.BlendSeconds : -1f;
        public bool Done => done;
        public void ClearDone() => done = false;
    }
}
