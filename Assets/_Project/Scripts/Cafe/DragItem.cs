using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 기기 화면 안에서 끌 수 있는 물건 (포터필터, 스쿱, 쟁반 위 물건 등).
    // 끄는 동안 커서를 따라다니는 아이콘을 띄우고, 놓은 곳의 DropZone 이 처리한다. 받아 주지 않으면 원래 자리에 그대로 남는다
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Image image;

        // 끄는 물건이 무엇인지 (화면이 정함: Portafilter, IceScoop, 쟁반 칸·순번 등)
        public object Payload { get; set; }

        // 끌기 시작해도 되는지 (추출 중인 포터필터 등). 없으면 항상 허용
        public System.Func<bool> CanDrag { get; set; }

        Image ghost;

        public static DragItem From(PointerEventData eventData) =>
            eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<DragItem>() : null;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Payload == null || (CanDrag != null && !CanDrag())) return;
            ghost = Instantiate(image, transform.root);
            ghost.raycastTarget = false;
            ghost.rectTransform.sizeDelta = image.rectTransform.rect.size;
            ghost.transform.position = eventData.position;
            image.color = new Color(1, 1, 1, 0.4f); // 원래 자리는 흐리게
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ghost != null) ghost.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (ghost != null) Destroy(ghost.gameObject);
            ghost = null;
            image.color = Color.white;
        }

        // 놓은 곳에서 받아 줄 때 쓰는 값. 끌기가 시작되지 않았으면(CanDrag 거부) null
        public object DroppedPayload => ghost != null ? Payload : null;
    }
}
