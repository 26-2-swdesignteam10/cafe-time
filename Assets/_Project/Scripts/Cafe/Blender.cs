using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 쉐이크 제조실 믹서기 (SFR-052~055). 갈기 버튼을 누르고 있는 동안만 갈린다 (ADR-0029, 떼면 멈추고 진행은 유지)
    public class Blender : MonoBehaviour, IWorkStatus
    {
        public static Blender Instance { get; private set; }

        public Mixture Jar { get; } = new Mixture();
        public Cup CupSpot { get; private set; }

        float blendTime; // 지금까지 간 시간
        bool blended;
        GameBalance balance;

        public event Action Changed;
        public event Action<Cup> Judged;

        public BlenderState State =>
            blended ? BlenderState.Blended : blendTime > 0 ? BlenderState.Blending : Jar.IsEmpty ? BlenderState.Empty : BlenderState.Filled;

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
            IceMaker.Instance.ReturnFromHand(hand); // 얼음을 부은 빈 스쿱은 제빙기 스쿱 자리로 자동 반납
            HandInventory.Instance.NotifyChanged();
            Changed?.Invoke();
            return true;
        }

        // 누르고 있는 동안 매 프레임 호출 (SFR-054, ADR-0029). false 면 갈 수 없음
        public bool BlendHold(float seconds)
        {
            if (blended) return false;
            if (Jar.IsEmpty) { Toast.Show(ToastMessage.BlenderEmpty); return false; }
            bool started = blendTime <= 0;
            blendTime += seconds;
            if (blendTime >= balance.BlendSeconds)
            {
                blendTime = 0;
                blended = true;
                Changed?.Invoke();
            }
            else if (started) Changed?.Invoke(); // 재료를 더 못 넣는 상태로 바뀜
            return true;
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
            blendTime = 0;
            DrinkJudge.Finish(cup, balance);
            if (HandInventory.Instance.TryAdd(cup)) CupSpot = null;
            else Toast.Show(ToastMessage.HandsFull);
            Judged?.Invoke(cup);
            Changed?.Invoke();
        }

        // 붓기 버튼: 잔이 있고 다 갈렸을 때만 붓는 연출을 한다 (아니면 PourOut 이 안내를 띄움)
        public bool CanPour => CupSpot != null && State == BlenderState.Blended;

        public float Progress => !blended && blendTime > 0 ? blendTime / balance.BlendSeconds : -1f;
        public bool Done => false; // 화면을 연 채로 갈기 때문에 완료 알림이 필요 없다
        public void ClearDone() { }
    }
}
