using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 설거지실 화면 (SFR-063~065): 싱크대의 잔을 수세미로 좌우로 문질러 얼룩을 지우면 건조대로
    public class DishwashScreen : StationScreen
    {
        [SerializeField] TMP_Text waitingText;  // "3개"
        [SerializeField] Image sinkItem;
        [SerializeField] Image stain;
        [SerializeField] DragMotion sponge;
        [SerializeField] Image[] rack;          // 건조대 칸
        [SerializeField] GameObject dryingLabel; // "건조 중"

        static DishStation Dish => DishStation.Instance;

        protected override IWorkStatus Work => Dish;

        protected override void Awake()
        {
            base.Awake();
            Dish.Changed += RefreshIfOpen;
            sponge.Moved += delta => Dish.Scrub(Mathf.Abs(delta.x));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Dish != null) Dish.Changed -= RefreshIfOpen;
        }

        // 설거지 중·건조 중인 잔은 깨끗한 잔 그림 + 얼룩
        static Sprite Clean(TrayItemKind kind) => ItemIcons.Get(
            kind == TrayItemKind.DirtyCeramic ? ItemSprite.CupCeramic : kind == TrayItemKind.DirtyPlastic ? ItemSprite.CupPlastic : ItemSprite.Plate);

        protected override void Refresh()
        {
            waitingText.text = $"{Dish.Waiting.Count}개";
            var inSink = Dish.InSink;
            ShowIcon(sinkItem, inSink != null ? Clean(inSink.Value) : null);
            stain.enabled = inSink != null;
            stain.color = new Color(1, 1, 1, Mathf.Clamp01(Dish.Stain));
            for (int i = 0; i < rack.Length; i++)
                ShowIcon(rack[i], i < Dish.Rack.Count ? Clean(Dish.Rack[i].Kind) : null);
            dryingLabel.SetActive(Dish.Rack.Count > 0);
        }
    }
}
