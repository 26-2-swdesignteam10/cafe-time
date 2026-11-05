using CafeTime.Cafe;
using UnityEngine;

namespace CafeTime.Player
{
    // 맵의 캐릭터 손에 든 물건 표시 (SFR-021)
    public class HandDisplay : MonoBehaviour
    {
        [SerializeField] SpriteRenderer leftHand;
        [SerializeField] SpriteRenderer rightHand;

        void Start()
        {
            HandInventory.Instance.Changed += Refresh;
            Refresh();
        }

        void OnDestroy()
        {
            if (HandInventory.Instance != null) HandInventory.Instance.Changed -= Refresh;
        }

        void Refresh()
        {
            leftHand.sprite = ItemIcons.Of(HandInventory.Instance[HandInventory.Left]);
            rightHand.sprite = ItemIcons.Of(HandInventory.Instance[HandInventory.Right]);
        }
    }
}
