using System.Linq;
using CafeTime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 쉐이크 제조실 화면 (SFR-052~055): 원액·우유·얼음을 믹서기에 넣고 갈아서 커피잔에 붓는다
    public class ShakeRoomScreen : StationScreen
    {
        [SerializeField] Transform shelf;
        [SerializeField] IngredientSlotView slotTemplate;
        [SerializeField] IngredientSlotView milkSlot;
        [SerializeField] IngredientTooltip tooltip;
        [SerializeField] DropZone blenderZone;
        [SerializeField] Image blenderImage;
        [Tooltip("빈 용기, 재료 담김, 갈린 스무디")]
        [SerializeField] Sprite[] blenderSprites;
        [SerializeField] TMP_Text weightText;
        [SerializeField] Button blendButton;
        [SerializeField] Image blendProgress;
        [SerializeField] DropZone cupSpot;
        [SerializeField] Image cupIcon;
        [SerializeField] Button pourButton;
        [SerializeField] JudgeResultView result;

        static Blender Mixer => Blender.Instance;

        protected override IWorkStatus Work => Mixer;

        protected override void Awake()
        {
            base.Awake();
            Mixer.Changed += RefreshIfOpen;
            Mixer.Judged += result.Show;

            slotTemplate.gameObject.SetActive(false);
            var balance = GameBalance.Instance;
            foreach (var ingredient in balance.Ingredients.Where(i => i.Category == IngredientCategory.Smoothie))
                Instantiate(slotTemplate, shelf).Setup(ingredient, Mixer.AddBase, null, tooltip.Show, tooltip.Hide);
            milkSlot.Setup(balance.FindIngredient("milk"), null, Mixer.Pour, tooltip.Show, tooltip.Hide);
            tooltip.Hide();

            blenderZone.Dropped += e =>
            {
                int hand = DropZone.HandOf(e);
                if (hand >= 0) Mixer.AddIceFromHand(hand);
            };
            blendButton.onClick.AddListener(Mixer.Blend);
            BindSpot(cupSpot, Mixer.PlaceCup, Mixer.TakeCup, () => Mixer.CupSpot != null);
            pourButton.onClick.AddListener(Mixer.PourOut);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Mixer == null) return;
            Mixer.Changed -= RefreshIfOpen;
            Mixer.Judged -= result.Show;
        }

        protected override void Refresh()
        {
            var state = Mixer.State;
            blenderImage.sprite = blenderSprites[state == BlenderState.Empty ? 0 : state == BlenderState.Blended ? 2 : 1];
            weightText.text = $"{Mixer.Weight:0}g";
            ShowIcon(cupIcon, ItemIcons.Of(Mixer.CupSpot));
        }

        void Update() => ShowProgress(blendProgress, Mixer.Progress);
    }
}
