using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 음료 제조실 (SFR-044~051): 전자저울 위 커피잔에 재료를 넣고 완성 판정. 저울 위 잔은 화면을 닫아도 남아 있다
    public class DrinkStation : MonoBehaviour
    {
        public static DrinkStation Instance { get; private set; }

        public Cup OnScale { get; private set; }

        GameBalance balance;

        public event Action Changed;
        public event Action<Cup> Judged; // 완성 판정 결과 (메뉴명, 등급 표시)

        public float Weight => OnScale != null ? OnScale.Contents.Weight(balance) : 0f;

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // 저울에는 커피잔만 올릴 수 있다 (SFR-045)
        public bool PlaceCup(int hand)
        {
            if (OnScale != null || !(HandInventory.Instance[hand] is Cup)) return false;
            OnScale = (Cup)HandInventory.Instance.Take(hand);
            Changed?.Invoke();
            return true;
        }

        public void TakeCup()
        {
            if (OnScale == null) return;
            if (!HandInventory.Instance.TryAdd(OnScale)) { Toast.Show(ToastMessage.HandsFull); return; }
            OnScale = null;
            Changed?.Invoke();
        }

        bool NeedCup()
        {
            if (OnScale != null) return false;
            Toast.Show(ToastMessage.CupFirst);
            return true;
        }

        // 시럽·청·연유·티백 클릭 1회 (SFR-046)
        public void AddIngredient(IngredientDefinition ingredient)
        {
            if (NeedCup()) return;
            float amount = balance.AmountPerClick(ingredient);
            if (!Pantry.TryUse(ingredient.Id, amount)) { Toast.Show(ToastMessage.NoIngredient); return; }
            OnScale.Add(ingredient.Id, amount);
            Changed?.Invoke();
        }

        // 우유·탄산수: 누르고 있는 동안 매 프레임 호출 (SFR-047). 멈춰야 하면 false
        public bool Pour(IngredientDefinition ingredient, float seconds)
        {
            if (NeedCup()) return false;
            float amount = balance.PourPerSecond * seconds;
            if (!Pantry.TryUse(ingredient.Id, amount)) { Toast.Show(ToastMessage.NoIngredient); return false; }
            OnScale.Add(ingredient.Id, amount);
            Changed?.Invoke();
            return true;
        }

        // 손의 샷 컵·얼음 스쿱을 저울 위 잔으로 끌어 놓음 (SFR-048)
        public bool PourFromHand(int hand)
        {
            var item = HandInventory.Instance[hand];
            if (!(item is ShotGlass { HasShot: true }) && !(item is IceScoop { HasIce: true })) return false;
            if (NeedCup()) return false;
            if (item is ShotGlass glass)
            {
                glass.HasShot = false;
                OnScale.Add(RecipeKeys.Shot, 1);
            }
            else
            {
                ((IceScoop)item).HasIce = false;
                OnScale.Add(RecipeKeys.Ice, 1);
            }
            HandInventory.Instance.NotifyChanged();
            Changed?.Invoke();
            return true;
        }

        // 비우기 (SFR-050): 버린 재료는 복구되지 않고 감점 사항에 반영
        public void Empty()
        {
            if (OnScale == null || OnScale.IsEmpty) return;
            OnScale.Empty();
            DayResult.Today.itemsDiscarded++;
            Changed?.Invoke();
        }

        // 완성 (SFR-051): 판정 후 완성 음료를 손에 든다. 손이 가득 차 있으면 저울 위에 남는다
        public void Complete()
        {
            if (OnScale == null || OnScale.IsEmpty) { Toast.Show(ToastMessage.NothingToComplete); return; }
            var cup = OnScale;
            DrinkJudge.Finish(cup, balance);
            if (HandInventory.Instance.TryAdd(cup)) OnScale = null;
            else Toast.Show(ToastMessage.HandsFull);
            Judged?.Invoke(cup);
            Changed?.Invoke();
        }
    }
}
