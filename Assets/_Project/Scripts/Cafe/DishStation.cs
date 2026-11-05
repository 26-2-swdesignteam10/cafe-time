using System;
using System.Collections.Generic;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 설거지실 (SFR-063~065): 설거지 대기 → 싱크대에서 수세미로 얼룩 닦기 → 건조대 → 커피잔 선반 재고 복구.
    // 건조는 화면을 닫아도 계속된다
    public class DishStation : MonoBehaviour, IWorkStatus
    {
        public class Drying
        {
            public TrayItemKind Kind;
            public float Left;
        }

        public static DishStation Instance { get; private set; }

        public List<TrayItemKind> Waiting { get; } = new List<TrayItemKind>();
        public TrayItemKind? InSink { get; private set; }
        public float Stain { get; private set; } // 1 = 얼룩 가득, 0 = 깨끗
        public List<Drying> Rack { get; } = new List<Drying>();

        bool done;
        GameBalance balance;

        public event Action Changed;

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // 퇴식구 설거지통에 넣거나, 손에서 버린 잔·접시 (SFR-021, 064)
        public void Enqueue(TrayItemKind dish)
        {
            if (Tray.IsTrash(dish)) return;
            Waiting.Add(dish);
            LoadNext();
            Changed?.Invoke();
        }

        // 대기 중인 잔 1개가 자동으로 싱크대에 올라옴
        void LoadNext()
        {
            if (InSink != null || Waiting.Count == 0) return;
            InSink = Waiting[0];
            Waiting.RemoveAt(0);
            Stain = 1f;
        }

        // 수세미를 좌우로 끈 거리(px)만큼 얼룩이 줄어든다
        public void Scrub(float distance)
        {
            if (InSink == null) return; // 싱크대가 비어 있으면 수세미가 작동하지 않음
            Stain -= distance / balance.ScrubDistance;
            if (Stain <= 0)
            {
                Rack.Add(new Drying { Kind = InSink.Value, Left = balance.DryingSeconds }); // 설거지 완료 → 건조대
                InSink = null;
                LoadNext();
            }
            Changed?.Invoke();
        }

        void Update()
        {
            if (Rack.Count == 0) return;
            float dt = GameClock.Delta;
            for (int i = Rack.Count - 1; i >= 0; i--)
            {
                Rack[i].Left -= dt;
                if (Rack[i].Left > 0) continue;
                CupShelf.Instance.Return(Rack[i].Kind); // 건조 끝 → 선반으로
                Rack.RemoveAt(i);
                done = true;
                Changed?.Invoke();
            }
        }

        public float Progress => Rack.Count > 0 ? 1f - Rack[0].Left / balance.DryingSeconds : -1f;
        public bool Done => done;
        public void ClearDone() => done = false;
    }
}
