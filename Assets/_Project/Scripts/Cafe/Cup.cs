using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 커피잔 1개 (SFR-028): 빈 잔 → 물·샷·재료가 담긴 잔 → 완성 판정을 받은 음료 (SFR-051)
    public class Cup : HeldItem
    {
        public CupType Type { get; }
        public Mixture Contents { get; } = new Mixture();
        public bool IsFinished { get; private set; }

        MenuDefinition menu;
        Temperature temperature;
        DrinkGrade grade;

        public Cup(CupType type) => Type = type;

        public bool IsEmpty => Contents.IsEmpty;

        public override bool Servable => IsFinished;
        public override MenuDefinition Menu => IsFinished ? menu : null;
        public override Temperature Temperature => IsFinished ? temperature : Temperature.None;
        public override DrinkGrade Grade => IsFinished ? grade : DrinkGrade.Failed;

        // 재료를 더 넣으면 다시 만드는 중인 잔이 된다
        public void Add(string key, float amount)
        {
            Contents.Add(key, amount);
            IsFinished = false;
        }

        public void AddAll(Mixture mixture)
        {
            Contents.AddAll(mixture);
            IsFinished = false;
        }

        // 비우기 (SFR-050): 내용물을 모두 버리고 빈 잔으로
        public void Empty()
        {
            Contents.Clear();
            IsFinished = false;
        }

        public void Finish(MenuDefinition judgedMenu, Temperature judgedTemperature, DrinkGrade judgedGrade)
        {
            menu = judgedMenu;
            temperature = judgedTemperature;
            grade = judgedGrade;
            IsFinished = true;
        }

        public override Sprite Icon
        {
            get
            {
                bool ceramic = Type == CupType.Ceramic;
                if (IsFinished)
                    return menu != null ? ItemIcons.ForMenu(menu, temperature) : ItemIcons.Get(ItemSprite.FailedDrink);
                if (IsEmpty) return ItemIcons.Get(ceramic ? ItemSprite.CupCeramic : ItemSprite.CupPlastic);
                if (Contents.OnlyWater) return ItemIcons.Get(ceramic ? ItemSprite.CupCeramicWater : ItemSprite.CupPlasticWater);
                return ItemIcons.Get(ceramic ? ItemSprite.CupCeramicMixing : ItemSprite.CupPlasticMixing);
            }
        }
    }
}
