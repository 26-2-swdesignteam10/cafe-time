using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 커피잔 선반 (SFR-028): 선반에 뒤집어 놓은 HOT 컵과 세로로 쌓인 ICE 컵 더미. 남은 개수만큼 그림이 보이고,
    // 잔·더미를 클릭하면 빈 커피잔 1개를 손에 든다. 손 칸의 빈 커피잔을 HOT/ICE 영역에 끌어다 놓으면 되돌려 놓는다
    // (놓은 영역과 상관없이 잔 종류대로)
    public class CupShelfScreen : StationScreen
    {
        [Header("HOT 컵: 뒤집힌 잔 (클릭한 잔이 사라지고, 되돌려 놓으면 앞쪽 빈자리부터 채움)")]
        [SerializeField] Button[] hotCups;
        [SerializeField] TMP_Text hotCount;

        [Header("ICE 컵: 입구가 아래인 컵 두 탑 (각 10개)")]
        [SerializeField] Button[] iceStacks;
        [SerializeField] GameObject[] iceLayers; // 0~9 = 왼쪽 탑, 10~19 = 오른쪽 탑. 아래 컵부터 그린다
        [SerializeField] TMP_Text iceCount;

        [SerializeField] DropZone[] returnZones;
        [Tooltip("꺼낸 잔이 손 칸으로 날아가는 시간 (2026-11-18 사용자 요청)")]
        [SerializeField] float flySeconds = 0.4f;

        bool[] hotFilled; // 잔 자리마다 잔이 있는지
        bool taking;

        protected override void Awake()
        {
            base.Awake();
            hotFilled = new bool[hotCups.Length];
            for (int i = 0; i < hotCups.Length; i++)
            {
                int slot = i;
                hotCups[i].onClick.AddListener(() => TakeHotCup(slot));
            }
            foreach (var stack in iceStacks) stack.onClick.AddListener(TakeIceCup);
            CupShelf.Instance.Changed += RefreshIfOpen;
            foreach (var zone in returnZones)
                zone.Dropped += e =>
                {
                    int hand = DropZone.HandOf(e);
                    if (hand >= 0) CupShelf.Instance.ReturnCup(hand);
                };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (CupShelf.Instance != null) CupShelf.Instance.Changed -= RefreshIfOpen;
        }

        // 클릭한 자리의 잔을 가져간다 (2026-11-18 사용자 요청). TakeCup 은 손 칸 알림을 개수 차감보다 먼저 보내므로
        // 도중의 Refresh 는 건너뛰고, 끝난 뒤 가져갔으면 그 자리만 비운다
        void TakeHotCup(int slot)
        {
            int before = CupShelf.Instance.Count(CupType.Ceramic);
            var hands = HandInventoryView.Snapshot();
            taking = true;
            CupShelf.Instance.TakeCup(CupType.Ceramic);
            taking = false;
            if (CupShelf.Instance.Count(CupType.Ceramic) < before)
            {
                hotFilled[slot] = false;
                FlyToHand(hands, hotCups[slot].image);
            }
            Refresh();
        }

        // ICE 탑을 누르면 맨 위 컵을 가져간다
        void TakeIceCup()
        {
            int before = CupShelf.Instance.Count(CupType.Plastic);
            var hands = HandInventoryView.Snapshot();
            CupShelf.Instance.TakeCup(CupType.Plastic);
            int after = CupShelf.Instance.Count(CupType.Plastic);
            if (after < before && after < iceLayers.Length) FlyToHand(hands, iceLayers[after].GetComponent<Image>());
        }

        void FlyToHand(HeldItem[] before, Image source) => HandInventoryView.FlyToNewSlot(before, source, flySeconds);

        protected override void Refresh()
        {
            if (taking) return;
            int hot = CupShelf.Instance.Count(CupType.Ceramic);
            int ice = CupShelf.Instance.Count(CupType.Plastic);
            // 보이는 잔 수를 남은 개수에 맞춘다: 모자라면 앞쪽 빈자리부터 채우고, 많으면 뒤쪽부터 지운다
            int target = Mathf.Min(hot, hotCups.Length);
            int filled = System.Array.FindAll(hotFilled, f => f).Length;
            for (int i = 0; i < hotFilled.Length && filled < target; i++)
                if (!hotFilled[i]) { hotFilled[i] = true; filled++; }
            for (int i = hotFilled.Length - 1; i >= 0 && filled > target; i--)
                if (hotFilled[i]) { hotFilled[i] = false; filled--; }
            for (int i = 0; i < hotCups.Length; i++) hotCups[i].gameObject.SetActive(hotFilled[i]);
            for (int i = 0; i < iceLayers.Length; i++) iceLayers[i].SetActive(i < ice);
            hotCount.text = $"{hot}개";
            iceCount.text = $"{ice}개";
        }
    }
}
