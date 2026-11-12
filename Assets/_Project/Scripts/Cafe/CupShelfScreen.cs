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
        [Header("HOT 컵: 뒤집힌 잔 (앞에서부터 보이고, 가져가면 뒤쪽부터 사라짐)")]
        [SerializeField] Button[] hotCups;
        [SerializeField] TMP_Text hotCount;

        [Header("ICE 컵: 맨 아래 컵 + 위로 겹친 컵 테두리")]
        [SerializeField] Button iceStack;
        [SerializeField] GameObject[] iceLayers; // 0 = 맨 아래 컵, 1~ = 그 위로 겹친 컵
        [SerializeField] TMP_Text iceCount;

        [SerializeField] DropZone[] returnZones;

        protected override void Awake()
        {
            base.Awake();
            foreach (var cup in hotCups) cup.onClick.AddListener(() => CupShelf.Instance.TakeCup(CupType.Ceramic));
            iceStack.onClick.AddListener(() => CupShelf.Instance.TakeCup(CupType.Plastic));
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

        protected override void Refresh()
        {
            int hot = CupShelf.Instance.Count(CupType.Ceramic);
            int ice = CupShelf.Instance.Count(CupType.Plastic);
            for (int i = 0; i < hotCups.Length; i++) hotCups[i].gameObject.SetActive(i < hot);
            for (int i = 0; i < iceLayers.Length; i++) iceLayers[i].SetActive(i < ice);
            hotCount.text = $"{hot}개";
            iceCount.text = $"{ice}개";
        }
    }
}
