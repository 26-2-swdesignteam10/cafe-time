using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 기기 화면 안에서 끌 수 있는 물건 (얼음 스쿱, 쟁반 위 물건 등).
    // 끄는 동안 원래 자리는 비워 보이고 커서를 따라다니는 아이콘을 띄운다. 놓은 곳의 DropZone 이 처리하며,
    // 받아 주지 않으면 아이콘이 원래 자리로 날아 돌아온다 (DragGhost)
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Image image;

        // 끄는 물건이 무엇인지 (화면이 정함: IceScoop, 쟁반 칸·순번 등)
        public object Payload { get; set; }

        // 끌기 시작해도 되는지. 없으면 항상 허용
        public System.Func<bool> CanDrag { get; set; }

        DragGhost ghost;      // 끄는 중
        DragGhost returning;  // 놓은 뒤 판단·복귀 중

        public static DragItem From(PointerEventData eventData) =>
            eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<DragItem>() : null;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Payload == null || (CanDrag != null && !CanDrag())) return;
            if (returning != null) returning.Finish();
            ghost = DragGhost.Create(image, transform.root, eventData.position);
            Destroy(ghost.GetComponent<DragItem>()); // 같은 오브젝트에 붙은 끌기 기능까지 복사되므로 지움
            ((RectTransform)ghost.transform).sizeDelta = image.rectTransform.rect.size;
            image.color = Color.clear; // 원래 자리는 비워 보임 (레이캐스트는 그대로 두어 끌기가 이어지게)
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ghost != null) ghost.Follow(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData) => Release();

        // 놓은 곳이 이 물건을 치우면(스쿱을 손으로 가져감 등) 꺼져서 OnEndDrag 가 오지 않으므로 여기서도 정리
        void OnDisable() => Release();

        // 놓은 곳이 가져가면 화면 갱신으로 이 오브젝트가 꺼지거나 지워진다 → 남아 있으면 아이콘이 돌아옴
        void Release()
        {
            if (ghost == null) return;
            returning = ghost;
            ghost = null;
            returning.Release(transform, () => this != null && isActiveAndEnabled, Land);
        }

        void Land()
        {
            returning = null;
            if (this != null) image.color = Color.white;
        }

        // 놓은 곳에서 원본이 먼저 삭제되면(쓰레기통에 넣어 쟁반 화면이 새로고침될 때 등) OnEndDrag 가 오지 않는다.
        // 삭제될 때 고스트도 같이 지우지 않으면 화면에 남아 떠다닌다
        void OnDestroy()
        {
            if (ghost != null) Destroy(ghost.gameObject);
        }

        // 놓은 곳에서 받아 줄 때 쓰는 값. 끌기가 시작되지 않았으면(CanDrag 거부) null
        public object DroppedPayload => ghost != null ? Payload : null;
    }
}
