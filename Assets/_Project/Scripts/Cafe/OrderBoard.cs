using System;
using System.Collections.Generic;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 현재 주문 목록 (SFR-013 주문 내역 창, SFR-027 등록, SFR-056 배정)
    public class OrderBoard : MonoBehaviour
    {
        public static OrderBoard Instance { get; private set; }

        readonly List<Order> orders = new List<Order>();
        int nextNumber = 1;

        public IReadOnlyList<Order> Orders => orders;

        public event Action Changed;
        public event Action<Order> Removed;

        void Awake() => Instance = this;

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public Order Register(List<OrderLine> lines, int paid, CustomerParty party)
        {
            var order = new Order(nextNumber++, lines, paid, party);
            orders.Add(order);
            Changed?.Invoke();
            return order;
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
