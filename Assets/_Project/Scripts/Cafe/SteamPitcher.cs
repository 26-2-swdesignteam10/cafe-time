using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 스팀 피처 (ADR-0044): 우유를 붓고 스팀으로 데운 뒤 저울 위 잔에 붓는다. 음료 제조실에 1개
    public class SteamPitcher
    {
        public float Milk { get; private set; }        // g
        public float Temperature { get; private set; } // ℃
        public bool IsEmpty => Milk <= 0.01f;

        public bool RightTemperature(GameBalance balance) =>
            Temperature >= balance.SteamMinTemperature && Temperature <= balance.SteamMaxTemperature;

        // 차가운 우유를 더하면 온도는 양에 비례해 섞인다
        public void AddMilk(float amount, GameBalance balance)
        {
            Temperature = IsEmpty ? balance.ColdMilkTemperature
                : (Temperature * Milk + balance.ColdMilkTemperature * amount) / (Milk + amount);
            Milk += amount;
        }

        public void Heat(float seconds, GameBalance balance) =>
            Temperature = Mathf.Min(balance.MaxMilkTemperature, Temperature + balance.SteamDegreesPerSecond * seconds);

        public float Take(float amount)
        {
            amount = Mathf.Min(amount, Milk);
            Milk -= amount;
            if (IsEmpty) Clear();
            return amount;
        }

        public void Clear()
        {
            Milk = 0f;
            Temperature = 0f;
        }
    }
}
