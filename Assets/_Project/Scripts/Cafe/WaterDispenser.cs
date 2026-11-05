using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 정수기 (SFR-041~043): 뜨거운 물 / 차가운 물 구역이 각각 독립적으로 물을 받는다. 화면을 닫아도 계속된다
    public class WaterDispenser : MonoBehaviour, IWorkStatus
    {
        public const int Hot = 0;
        public const int Cold = 1;

        public static WaterDispenser Instance { get; private set; }

        public Cup[] Cups { get; } = new Cup[2];

        readonly float[] left = new float[2];
        bool done;
        GameBalance balance;

        public event Action Changed;

        public bool Filling(int zone) => left[zone] > 0;

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // 구역에는 커피잔만 놓을 수 있다 (SFR-042)
        public bool Place(int zone, int hand)
        {
            if (Cups[zone] != null || !(HandInventory.Instance[hand] is Cup)) return false;
            Cups[zone] = (Cup)HandInventory.Instance.Take(hand);
            Changed?.Invoke();
            return true;
        }

        public void Take(int zone)
        {
            if (Cups[zone] == null || Filling(zone)) return; // 물을 받는 중에는 회수 불가
            if (!HandInventory.Instance.TryAdd(Cups[zone])) { Toast.Show(ToastMessage.HandsFull); return; }
            Cups[zone] = null;
            Changed?.Invoke();
        }

        public void Fill(int zone)
        {
            if (Filling(zone)) return;
            if (Cups[zone] == null) { Toast.Show(ToastMessage.PlaceCup); return; }
            left[zone] = balance.WaterSeconds;
            Changed?.Invoke();
        }

        void Update()
        {
            for (int zone = 0; zone < 2; zone++)
            {
                if (!Filling(zone)) continue;
                left[zone] -= GameClock.Delta;
                if (left[zone] > 0) continue;
                left[zone] = 0;
                Cups[zone]?.Add(zone == Hot ? RecipeKeys.HotWater : RecipeKeys.ColdWater, balance.WaterAmount);
                done = true;
                Changed?.Invoke();
            }
        }

        public float ZoneProgress(int zone) => Filling(zone) ? 1f - left[zone] / balance.WaterSeconds : -1f;

        public float Progress => Mathf.Max(ZoneProgress(Hot), ZoneProgress(Cold));
        public bool Done => done;
        public void ClearDone() => done = false;
    }
}
