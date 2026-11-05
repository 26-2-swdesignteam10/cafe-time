using System;
using CafeTime.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 기기 화면 공통 (SFR-029, 037, 041, 044, 052, 059, 063): 열려 있는 동안 기기·손 상태가 바뀌면 다시 그리고,
    // 열면 맵의 완료 아이콘(SFR-022)을 지운다. 기기 상태는 화면이 아니라 기기 오브젝트에 있어서 닫아도 유지된다
    public abstract class StationScreen : Popup
    {
        // 완료 아이콘을 지울 기기 (없으면 null)
        protected virtual IWorkStatus Work => null;

        protected abstract void Refresh();

        protected override void Awake()
        {
            base.Awake();
            HandInventory.Instance.Changed += RefreshIfOpen;
        }

        protected virtual void OnDestroy()
        {
            if (HandInventory.Instance != null) HandInventory.Instance.Changed -= RefreshIfOpen;
        }

        public override void Open()
        {
            base.Open();
            Work?.ClearDone();
            Refresh();
        }

        protected void RefreshIfOpen()
        {
            if (isActiveAndEnabled) Refresh();
        }

        // 손 칸에서 끌어다 놓거나, 손 칸을 선택한 뒤 자리를 클릭해 놓기. 자리에 물건이 있으면 클릭으로 손에 회수
        protected static void BindSpot(DropZone zone, Func<int, bool> placeFromHand, Action take, Func<bool> occupied)
        {
            zone.Dropped += e =>
            {
                int hand = DropZone.HandOf(e);
                if (hand >= 0) placeFromHand(hand);
            };
            zone.Clicked += () =>
            {
                if (occupied()) take();
                else if (HandInventory.Instance.Selected >= 0) placeFromHand(HandInventory.Instance.Selected);
            };
        }

        // 진행 게이지 (Image Filled). 작업이 없으면 숨김
        protected static void ShowProgress(Image bar, float progress)
        {
            if (bar == null) return;
            bar.transform.parent.gameObject.SetActive(progress >= 0);
            bar.fillAmount = Mathf.Clamp01(progress);
        }

        protected static void ShowIcon(Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.enabled = sprite != null;
        }
    }
}
