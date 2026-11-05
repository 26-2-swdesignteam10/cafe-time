using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 샷 내리기 화면 (SFR-029~036): 포터필터를 끌어 그라인더 → 탬핑 → 그룹헤드 → 넉박스로 옮기고, 샷 컵에 샷을 받는다
    public class EspressoScreen : StationScreen
    {
        [Header("포터필터")]
        [Tooltip("PortafilterSpot 순서대로 (받침대 왼쪽·오른쪽, 그라인더, 탬핑, 그룹헤드 1·2)")]
        [SerializeField] DropZone[] spots;
        [SerializeField] DropZone knockBox;
        [SerializeField] DragItem[] portafilters;
        [Tooltip("PortafilterState 순서대로 (빈, 원두가루, 탬핑 완료, 찌꺼기)")]
        [SerializeField] Sprite[] portafilterSprites;

        [Header("그라인더·탬퍼")]
        [SerializeField] Button grindButton;
        [SerializeField] Image grindProgress;
        [SerializeField] DragMotion tamper;
        [SerializeField] float tampDistance = 50f; // 아래로 이만큼 끌면 탬핑 완료

        [Header("그룹헤드 (1번, 2번)")]
        [SerializeField] Button[] extractButtons;
        [SerializeField] Image[] extractProgress;
        [SerializeField] DropZone[] cupSpots;
        [SerializeField] Image[] cupIcons;
        [SerializeField] Sprite shotEmpty;
        [SerializeField] Sprite shotFull;

        float tampPushed;

        static EspressoBar Bar => EspressoBar.Instance;

        protected override IWorkStatus Work => Bar;

        protected override void Awake()
        {
            base.Awake();
            Bar.Changed += RefreshIfOpen;

            for (int i = 0; i < spots.Length; i++)
            {
                var spot = (PortafilterSpot)i;
                spots[i].Dropped += e =>
                {
                    if (DragItem.From(e)?.DroppedPayload is Portafilter p) Bar.Move(p, spot);
                };
            }
            knockBox.Dropped += e =>
            {
                if (DragItem.From(e)?.DroppedPayload is Portafilter p) Bar.Knock(p);
            };
            for (int i = 0; i < portafilters.Length; i++)
            {
                var p = Bar.Portafilters[i];
                portafilters[i].Payload = p;
                portafilters[i].CanDrag = () => Bar.CanPick(p);
            }

            grindButton.onClick.AddListener(Bar.Grind);
            tamper.Moved += delta =>
            {
                tampPushed -= delta.y; // 아래로 끌면 y 감소
                if (tampPushed >= tampDistance && Bar.Tamp()) tampPushed = float.MinValue; // 한 번 누르면 끝
            };
            tamper.Released += () => tampPushed = 0;

            for (int g = 0; g < EspressoBar.Groups; g++)
            {
                int group = g;
                extractButtons[g].onClick.AddListener(() => Bar.Extract(group));
                BindSpot(cupSpots[g], hand => Bar.PlaceGlass(group, hand), () => Bar.TakeGlass(group), () => Bar.CupSpots[group] != null);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Bar != null) Bar.Changed -= RefreshIfOpen;
        }

        protected override void Refresh()
        {
            for (int i = 0; i < portafilters.Length; i++)
            {
                var p = Bar.Portafilters[i];
                var view = portafilters[i];
                view.transform.SetParent(spots[(int)p.Spot].transform, false);
                ((RectTransform)view.transform).anchoredPosition = Vector2.zero; // 자리 한가운데
                view.GetComponent<Image>().sprite = portafilterSprites[(int)p.State];
            }
            for (int g = 0; g < EspressoBar.Groups; g++)
            {
                var glass = Bar.CupSpots[g];
                ShowIcon(cupIcons[g], glass == null ? null : glass.HasShot ? shotFull : shotEmpty);
            }
        }

        void Update()
        {
            ShowProgress(grindProgress, Bar.GrindProgress);
            for (int g = 0; g < EspressoBar.Groups; g++) ShowProgress(extractProgress[g], Bar.ExtractProgress(g));
        }
    }
}
