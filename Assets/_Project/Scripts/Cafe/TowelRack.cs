using UnityEngine;

namespace CafeTime.Cafe
{
    // 행주 걸이 (SFR-066): E 로 행주를 손에 든다 (양손 중 1칸). 행주는 1개
    public class TowelRack : MonoBehaviour
    {
        // Interactable.onInteract 에 연결
        public void Interact()
        {
            if (HandInventory.Instance.Holds<Towel>()) return;
            if (!HandInventory.Instance.TryAdd(new Towel())) Toast.Show(ToastMessage.HandsFull);
        }
    }
}
