using UnityEngine;

namespace CafeTime.Cafe
{
    // 청소 요청이 있는 테이블 (SFR-066): 행주를 든 상태로 E 를 누르면 청소 완료. 요청이 없으면 꺼져 있다
    public class TableCleanSpot : Interactable
    {
        [SerializeField] CafeTable table;

        public override void Interact()
        {
            if (!HandInventory.Instance.Holds<Towel>())
            {
                Toast.Show(ToastMessage.NeedTowel);
                return;
            }
            table.Clean(); // 행주는 청소 후에도 손에 남는다
        }
    }
}
