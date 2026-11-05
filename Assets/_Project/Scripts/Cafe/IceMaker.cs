using System;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 제빙기 (SFR-037~040): 일정 시간마다 얼음 1개 자동 생성, 스쿱 1개. 화면을 닫아도 제빙은 계속된다
    public class IceMaker : MonoBehaviour
    {
        public static IceMaker Instance { get; private set; }

        public IceScoop Scoop { get; } = new IceScoop();
        public bool ScoopAtSpot { get; private set; } = true; // 제빙기 옆 스쿱 자리에 있는지

        float ice;
        GameBalance balance;

        public event Action Changed;

        public int Ice => Mathf.FloorToInt(ice);
        public int Max => balance.IceMax;
        public float SecondsPerCube => balance.IceSecondsPerCube;

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
            ice = balance.IceMax; // 영업 시작 때는 가득 차 있음
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (ice >= balance.IceMax) return; // 최대 보유량이면 제빙을 멈춤
            int before = Ice;
            ice = Mathf.Min(balance.IceMax, ice + GameClock.Delta / balance.IceSecondsPerCube);
            if (Ice != before) Changed?.Invoke();
        }

        // 스쿱 자리의 빈 스쿱을 얼음통에 끌어 놓음 (SFR-039)
        public bool FillScoop()
        {
            if (!ScoopAtSpot || Scoop.HasIce) return false;
            if (ice < balance.IcePerScoop)
            {
                Toast.Show(ToastMessage.NoIce);
                return false;
            }
            ice -= balance.IcePerScoop;
            Scoop.HasIce = true;
            Changed?.Invoke();
            return true;
        }

        // 스쿱을 손 칸으로 끌어 놓음 (SFR-040)
        public bool ScoopToHand()
        {
            if (!ScoopAtSpot) return false;
            if (!HandInventory.Instance.TryAdd(Scoop))
            {
                Toast.Show(ToastMessage.HandsFull);
                return false;
            }
            ScoopAtSpot = false;
            Changed?.Invoke();
            return true;
        }

        // 손의 스쿱을 스쿱 자리로 반납 (SFR-040)
        public bool ReturnFromHand(int hand)
        {
            if (ScoopAtSpot || !(HandInventory.Instance[hand] is IceScoop)) return false;
            HandInventory.Instance.Take(hand);
            ScoopAtSpot = true;
            Changed?.Invoke();
            return true;
        }

        // 우클릭 메뉴로 빈 스쿱을 내려놓으면 스쿱 자리로
        public void ReturnScoop()
        {
            ScoopAtSpot = true;
            Changed?.Invoke();
        }
    }
}
