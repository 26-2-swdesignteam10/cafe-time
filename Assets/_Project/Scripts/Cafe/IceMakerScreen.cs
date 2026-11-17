using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 제빙기 화면 (SFR-037~040): 스쿱을 얼음통에 끌어 얼음을 담고, 손 칸으로 끌어 든다. 빈 스쿱은 스쿱 자리로 반납
    public class IceMakerScreen : StationScreen
    {
        // 얼음 수·제빙 속도 글자는 표시하지 않는다 (2026-11-12 사용자 요청). 얼음이 모자라면 FillScoop 이 토스트로 알린다
        [SerializeField] DropZone bin;
        [SerializeField] DropZone scoopSpot;
        [SerializeField] DragItem scoop;
        [SerializeField] Sprite scoopEmpty;
        [SerializeField] Sprite scoopIce;

        static IceMaker Maker => IceMaker.Instance;

        protected override void Awake()
        {
            base.Awake();
            Maker.Changed += RefreshIfOpen;
            HandInventory.Instance.DroppedOnHand += OnDroppedOnHand;
            scoop.Payload = Maker.Scoop;

            bin.Dropped += e =>
            {
                if (DragItem.From(e)?.DroppedPayload is IceScoop) Maker.FillScoop();
                else if (DropZone.HandOf(e) >= 0 && HandInventory.Instance[DropZone.HandOf(e)] is IceScoop)
                    Toast.Show(ToastMessage.ReturnScoop); // 스쿱을 반납하기 전에는 얼음을 담을 수 없음
            };
            BindSpot(scoopSpot, Maker.ReturnFromHand, () => Maker.ScoopToHand(), () => Maker.ScoopAtSpot);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Maker != null) Maker.Changed -= RefreshIfOpen;
            if (HandInventory.Instance != null) HandInventory.Instance.DroppedOnHand -= OnDroppedOnHand;
        }

        // 스쿱 자리의 스쿱을 손 칸으로 끌어 놓음
        void OnDroppedOnHand(int hand, PointerEventData eventData)
        {
            if (isActiveAndEnabled && DragItem.From(eventData)?.DroppedPayload is IceScoop) Maker.ScoopToHand();
        }

        protected override void Refresh()
        {
            scoop.gameObject.SetActive(Maker.ScoopAtSpot);
            scoop.GetComponent<Image>().sprite = Maker.Scoop.HasIce ? scoopIce : scoopEmpty;
        }
    }
}
