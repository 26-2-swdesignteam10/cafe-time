using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 정수기 화면 (SFR-041~043): 뜨거운 물 / 차가운 물 구역에 커피잔을 놓고 물 받기
    public class WaterScreen : StationScreen
    {
        [Tooltip("뜨거운 물, 차가운 물 순서")]
        [SerializeField] DropZone[] zones;
        [SerializeField] Image[] cupIcons;
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
            for (int i = 0; i < zones.Length; i++) ShowIcon(cupIcons[i], ItemIcons.Of(Water.Cups[i]));
        }

        void Update()
        {
            for (int i = 0; i < zones.Length; i++) ShowProgress(progress[i], Water.ZoneProgress(i));
        }
    }
}
