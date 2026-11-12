using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 정수기 화면 (SFR-041~043): 뜨거운 물 / 차가운 물 구역에 커피잔을 놓고 물 받기.
    // 올린 잔은 큰 컵 그림으로, 물을 받는 동안 꼭지에서 물 줄기가 떨어진다
    public class WaterScreen : StationScreen
    {
        [Tooltip("뜨거운 물, 차가운 물 순서")]
        [SerializeField] DropZone[] zones;
        [SerializeField] Image[] cupImages;
        [Tooltip("큰 컵 그림: HOT 빈 잔·물·만드는 중, ICE 빈 잔·물·만드는 중")]
        [SerializeField] Sprite[] cupSprites = new Sprite[6];
        [Tooltip("구역별 물 줄기 (물을 받는 동안만 켬)")]
        [SerializeField] GameObject[] streams;
        [SerializeField] Button[] fillButtons;
        [SerializeField] Image[] progress;

        static WaterDispenser Water => WaterDispenser.Instance;

        protected override IWorkStatus Work => Water;

        protected override void Awake()
        {
            base.Awake();
            Water.Changed += RefreshIfOpen;
            for (int i = 0; i < zones.Length; i++)
            {
                int zone = i;
                BindSpot(zones[i], hand => Water.Place(zone, hand), () => Water.Take(zone), () => Water.Cups[zone] != null);
                fillButtons[i].onClick.AddListener(() => Water.Fill(zone));
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Water != null) Water.Changed -= RefreshIfOpen;
        }

        protected override void Refresh()
        {
            for (int i = 0; i < zones.Length; i++) ShowIcon(cupImages[i], BigCupSprite(Water.Cups[i], cupSprites));
        }

        void Update()
        {
            for (int i = 0; i < zones.Length; i++)
            {
                ShowProgress(progress[i], Water.ZoneProgress(i));
                if (streams[i].activeSelf != Water.Filling(i)) streams[i].SetActive(Water.Filling(i));
            }
        }
    }
}
