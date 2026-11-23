using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 현재 주문 목록 (SFR-013 주문 내역 창, SFR-027 등록, SFR-056 배정)
    public class OrderBoard : MonoBehaviour
    {
        public static OrderBoard Instance { get; private set; }

        // 진동벨 번호 (ADR-0045): 1~maxBellNumber 중 대기 중인 주문과 겹치지 않는 번호를 무작위로 (2026-11-19 사용자 요청).
        // 끝난 주문의 번호는 다시 나올 수 있다. 주문이 들어온 순서는 orders 목록 순서다
        [SerializeField] int maxBellNumber = 30;

        readonly List<Order> orders = new List<Order>();

        public IReadOnlyList<Order> Orders => orders;

        public event Action Changed;
        public event Action<Order> Removed;

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public Order Register(List<OrderLine> lines, int paid, CustomerParty party, GroupOrder group = null, InfluencerVisit influencer = null)
        {
            var order = new Order(NextBellNumber(), lines, paid, party, group, influencer);
            orders.Add(order);
            Changed?.Invoke();
            return order;
        }

        int NextBellNumber()
        {
            var free = Enumerable.Range(1, maxBellNumber).Where(n => !orders.Any(o => o.Number == n)).ToList();
            if (free.Count == 0) return maxBellNumber + orders.Count; // 대기 주문이 번호보다 많을 때만 (실제로는 일어나지 않음)
            return free[UnityEngine.Random.Range(0, free.Count)];
        }

        // 서빙 완료, 환불(이탈) 모두 목록에서 뺀다
        public void Remove(Order order)
        {
            if (!orders.Remove(order)) return;
            Removed?.Invoke(order);
            Changed?.Invoke();
        }

        public void NotifyChanged() => Changed?.Invoke();
    }
}
