using System;
using System.Linq;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 에스프레소 머신 (SFR-029~036)의 상태와 동작. 화면을 닫아도 분쇄·추출은 계속된다 (GameClock.Delta)
    public class EspressoBar : MonoBehaviour, IWorkStatus
    {
        public const int Groups = 2;

        public static EspressoBar Instance { get; private set; }

        public Portafilter[] Portafilters { get; } = { new Portafilter(PortafilterSpot.Rest0), new Portafilter(PortafilterSpot.Rest1) };

        // 그룹헤드 아래 컵 자리의 샷 컵 (없으면 null)
        public ShotGlass[] CupSpots { get; } = { new ShotGlass(), new ShotGlass() };

        readonly float[] extractLeft = new float[Groups];
        float grindLeft;
        bool done;
        GameBalance balance;

        public event Action Changed;

        public bool Grinding => grindLeft > 0;
        public bool Extracting(int group) => extractLeft[group] > 0;

        void Awake()
        {
            Instance = this;
            balance = GameBalance.Instance;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public Portafilter At(PortafilterSpot spot) => Portafilters.FirstOrDefault(p => p.Spot == spot);

        static PortafilterSpot GroupSpot(int group) => group == 0 ? PortafilterSpot.Group0 : PortafilterSpot.Group1;

        // ───────── 포터필터 이동 (SFR-030, 033) ─────────

        // 움직일 수 있는지 (분쇄·추출 중이면 불가)
        public bool CanPick(Portafilter p)
        {
            if (p.Spot == PortafilterSpot.Grinder && Grinding) return false;
            for (int g = 0; g < Groups; g++)
            {
                if (p.Spot != GroupSpot(g) || !Extracting(g)) continue;
                Toast.Show(ToastMessage.Extracting);
                return false;
            }
            return true;
        }

        // 놓을 수 없는 자리면 false → 원래 자리로 되돌아감
        public bool Move(Portafilter p, PortafilterSpot target)
        {
            if (p.Spot == target || At(target) != null || !CanPick(p)) return false;
            if ((target == PortafilterSpot.Group0 || target == PortafilterSpot.Group1) && p.State != PortafilterState.Tamped)
            {
                Toast.Show(ToastMessage.TampedOnly);
                return false;
            }
            p.Spot = target;
            Changed?.Invoke();
            return true;
        }

        // 넉박스에 끌어 놓기 (SFR-036): 찌꺼기를 비우고 빈 받침대 자리로
        public bool Knock(Portafilter p)
        {
            if (p.State != PortafilterState.Used || !CanPick(p)) return false;
            p.State = PortafilterState.Empty;
            if (p.Spot != PortafilterSpot.Rest0 && p.Spot != PortafilterSpot.Rest1)
                p.Spot = At(PortafilterSpot.Rest0) == null ? PortafilterSpot.Rest0 : PortafilterSpot.Rest1;
            Changed?.Invoke();
            return true;
        }

        // ───────── 분쇄 (SFR-031) / 탬핑 (SFR-032) ─────────

        public void Grind()
        {
            if (Grinding) return;
            var p = At(PortafilterSpot.Grinder);
            if (p == null) { Toast.Show(ToastMessage.SetPortafilter); return; }
            if (p.State != PortafilterState.Empty) { Toast.Show(ToastMessage.EmptyPortafilterOnly); return; }
            if (!Pantry.TryUse("bean", 1)) { Toast.Show(ToastMessage.NoBean); return; } // 분쇄 1회당 원두 1회분
            grindLeft = balance.GrindSeconds;
            Changed?.Invoke();
        }

        // 탬퍼를 아래로 끌면 호출. 원두가루가 담긴 포터필터일 때만 동작
        public bool Tamp()
        {
            var p = At(PortafilterSpot.Tamp);
            if (p == null || p.State != PortafilterState.Ground) return false;
            p.State = PortafilterState.Tamped;
            Changed?.Invoke();
            return true;
        }

        // ───────── 컵 (SFR-034) / 추출 (SFR-035) ─────────

        public bool PlaceGlass(int group, int hand)
        {
            if (CupSpots[group] != null || !(HandInventory.Instance[hand] is ShotGlass)) return false;
            CupSpots[group] = (ShotGlass)HandInventory.Instance.Take(hand);
            Changed?.Invoke();
            return true;
        }

        public void TakeGlass(int group)
        {
            var glass = CupSpots[group];
            if (glass == null) return;
            if (Extracting(group)) { Toast.Show(ToastMessage.Extracting); return; }
            if (!HandInventory.Instance.TryAdd(glass)) { Toast.Show(ToastMessage.HandsFull); return; }
            CupSpots[group] = null;
            Changed?.Invoke();
        }

        // 손에서 내려놓은 빈 샷 컵은 비어 있는 컵 자리로 돌아간다
        public void ReturnGlass(ShotGlass glass)
        {
            int free = Array.IndexOf(CupSpots, null);
            if (free >= 0) CupSpots[free] = glass;
            Changed?.Invoke();
        }

        public void Extract(int group)
        {
            if (Extracting(group)) return;
            var p = At(GroupSpot(group));
            if (p == null || p.State != PortafilterState.Tamped) { Toast.Show(ToastMessage.AttachPortafilter); return; }
            var glass = CupSpots[group];
            if (glass == null) { Toast.Show(ToastMessage.PlaceShotGlass); return; }
            if (glass.HasShot) { Toast.Show(ToastMessage.ShotAlready); return; }
            extractLeft[group] = balance.ExtractSeconds;
            Changed?.Invoke();
        }

        void Update()
        {
            float dt = GameClock.Delta;
            if (Grinding)
            {
                grindLeft -= dt;
                if (grindLeft <= 0)
                {
                    grindLeft = 0;
                    var p = At(PortafilterSpot.Grinder);
                    if (p != null) p.State = PortafilterState.Ground;
                    Finish();
                }
            }
            for (int g = 0; g < Groups; g++)
            {
                if (!Extracting(g)) continue;
                extractLeft[g] -= dt;
                if (extractLeft[g] > 0) continue;
                extractLeft[g] = 0;
                var p = At(GroupSpot(g));
                if (p != null) p.State = PortafilterState.Used;
                if (CupSpots[g] != null) CupSpots[g].HasShot = true;
                Finish();
            }
        }

        void Finish()
        {
            done = true;
            Changed?.Invoke();
        }

        // ───────── 작업 상태 표시 (SFR-022) ─────────

        public float Progress
        {
            get
            {
                float p = -1f;
                if (Grinding) p = Mathf.Max(p, 1f - grindLeft / balance.GrindSeconds);
                for (int g = 0; g < Groups; g++)
                    if (Extracting(g)) p = Mathf.Max(p, 1f - extractLeft[g] / balance.ExtractSeconds);
                return p;
            }
        }

        public bool Done => done;

        public void ClearDone() => done = false;

        public float GrindProgress => Grinding ? 1f - grindLeft / balance.GrindSeconds : -1f;

        public float ExtractProgress(int group) => Extracting(group) ? 1f - extractLeft[group] / balance.ExtractSeconds : -1f;
    }
}
