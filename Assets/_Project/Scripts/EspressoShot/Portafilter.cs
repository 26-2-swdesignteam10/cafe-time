using System.Collections.Generic;
using TMPro;
using CafeTime.Cafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CafeTime.EspressoShot
{
    // SFR-030: 포터필터 4가지 상태 (빈 → 원두가루 담김 → 탬핑 완료 → 사용 완료)
    public enum PortafilterState { Empty, Ground, Tamped, Used }

    [RequireComponent(typeof(Image), typeof(CanvasGroup))]
    public class Portafilter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("상태별 임시 색 (스프라이트 없을 때)")]
        public Color emptyColor  = new Color32(0xc9, 0xc2, 0xb8, 255);
        public Color groundColor = new Color32(0xc5, 0x8a, 0x52, 255);
        public Color tampedColor = new Color32(0x8a, 0x55, 0x2e, 255);
        public Color usedColor   = new Color32(0x4a, 0x2a, 0x17, 255);

        [Header("상태별 스프라이트 (선택) 순서: 빈, 원두가루, 탬핑완료, 사용완료")]
        public Sprite[] stateSprites;

        [Header("앞모습 스프라이트 (선택) 그라인더·탬핑·그룹헤드에서 사용. 순서는 위와 같음")]
        public Sprite[] frontStateSprites;

        [Header("그룹헤드에 장착했을 때 스프라이트 (선택). 끼우면 내용물이 보이지 않아 상태와 관계없이 같음")]
        public Sprite mountedSprite;

        [Header("상태 글자 (선택)")]
        public TMP_Text stateLabel;

        public PortafilterState State { get; private set; } = PortafilterState.Empty;
        public PortafilterSlot Slot { get; private set; }

        Image image;
        CanvasGroup group;
        RectTransform rect;
        Canvas rootCanvas;
        bool dragging;
        Vector2 sideSize;
        Image fillOverlay; // 분쇄 중 아래부터 차오르는 다음 상태 그림

        // 놓은 곳이 슬롯 밖이어도 이 거리(캔버스 단위) 안의 빈 슬롯에 끼운다
        const float SnapRadius = 130f;

        void Awake()
        {
            image = GetComponent<Image>();
            group = GetComponent<CanvasGroup>();
            rect = (RectTransform)transform;
            sideSize = rect.sizeDelta;
            rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
            Refresh();
        }

        public void SetState(PortafilterState s)
        {
            State = s;
            Refresh();
        }

        // 그라인더·탬핑·그룹헤드에서는 손잡이가 앞을 향한다
        bool FacesFront => Slot != null &&
            (Slot.type == SlotType.Grinder || Slot.type == SlotType.Tamp || Slot.type == SlotType.GroupHead);

        void Refresh()
        {
            if (image == null) return;
            int i = (int)State;
            bool front = FacesFront && frontStateSprites != null && frontStateSprites.Length > 0;
            Sprite[] sprites = front ? frontStateSprites : stateSprites;
            Sprite sprite = sprites != null && i < sprites.Length ? sprites[i] : null;
            if (!dragging && mountedSprite != null && Slot != null && Slot.type == SlotType.GroupHead)
                sprite = mountedSprite;
            bool hasSprite = sprite != null;
            // 앞모습·장착 그림은 옆모습과 크기가 달라 그림 크기를 그대로 쓴다
            rect.sizeDelta = front && hasSprite ? sprite.rect.size : sideSize;
            if (hasSprite) { image.sprite = sprite; image.color = Color.white; }
            else
            {
                image.color = State switch
                {
                    PortafilterState.Empty  => emptyColor,
                    PortafilterState.Ground => groundColor,
                    PortafilterState.Tamped => tampedColor,
                    _                       => usedColor,
                };
            }
            if (stateLabel != null)
            {
                stateLabel.text = State switch
                {
                    PortafilterState.Empty  => "빈 필터",
                    PortafilterState.Ground => "원두가루",
                    PortafilterState.Tamped => "탬핑 완료",
                    _                       => "사용 완료",
                };
            }
        }

        // 분쇄처럼 서서히 바뀌는 동안, 다음 상태의 앞모습을 아래부터 amount(0~1)만큼 겹쳐 보인다
        public void ShowFill(PortafilterState next, float amount)
        {
            int i = (int)next;
            if (frontStateSprites == null || i >= frontStateSprites.Length || frontStateSprites[i] == null) return;
            if (fillOverlay == null)
            {
                var rt = new GameObject("Fill", typeof(RectTransform)).GetComponent<RectTransform>();
                rt.SetParent(transform, false);
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = rt.offsetMax = Vector2.zero;
                fillOverlay = rt.gameObject.AddComponent<Image>();
                fillOverlay.raycastTarget = false;
                fillOverlay.type = Image.Type.Filled;
                fillOverlay.fillMethod = Image.FillMethod.Vertical;
                fillOverlay.fillOrigin = (int)Image.OriginVertical.Bottom;
            }
            fillOverlay.sprite = frontStateSprites[i];
            fillOverlay.preserveAspect = image.preserveAspect;
            fillOverlay.fillAmount = amount;
            fillOverlay.gameObject.SetActive(true);
        }

        public void HideFill()
        {
            if (fillOverlay != null) fillOverlay.gameObject.SetActive(false);
        }

        // 슬롯에 붙이기 (슬롯 쪽에서 호출)
        public void AttachTo(PortafilterSlot slot)
        {
            if (Slot != null && Slot != slot) Slot.Release(this);
            Slot = slot;
            slot.Place(this);
            SnapToSlot();
            Refresh();
        }

        void SnapToSlot()
        {
            transform.SetParent(Slot.transform, false);
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, Slot.portafilterOffsetY);
        }

        // ---------- 드래그 ----------
        public void OnBeginDrag(PointerEventData e)
        {
            if (Slot != null && Slot.Locked)   // 추출 중 / 분쇄 중에는 분리 불가
            {
                Toast.Show(ToastMessage.Extracting); // SFR-033
                dragging = false;
                return;
            }
            dragging = true;
            Refresh();                         // 그룹헤드에서 빼면 장착 그림 대신 내용물이 보이게
            group.blocksRaycasts = false;      // 드롭 위치의 슬롯을 레이캐스트로 찾기 위해
            transform.SetParent(rootCanvas.transform, true);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData e)
        {
            if (!dragging) return;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    (RectTransform)rootCanvas.transform, e.position, e.pressEventCamera, out Vector3 wp))
                rect.position = wp;
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (!dragging) return;
            dragging = false;
            group.blocksRaycasts = true;

            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(e, hits);
            PortafilterSlot target = null;
            foreach (var h in hits)
            {
                target = h.gameObject.GetComponentInParent<PortafilterSlot>();
                if (target != null) break;
            }
            if (target == null) target = NearestSlot(e);

            if (target == Slot)
            {
                SnapToSlot();
                Slot.Place(this); // 넉박스에서 털다 만 포터필터를 다시 대면 털기 창이 다시 열림
            }
            // 놓을 수 없는 곳 / 거절된 곳 → 원래 위치로 (SFR-030)
            else if (target == null || !target.TryAccept(this))
                SnapToSlot();
            Refresh();
        }

        // 슬롯 위가 아닌 곳에 놓았을 때, 이 거리 안에서 가장 가까운 빈 슬롯 (없으면 null)
        PortafilterSlot NearestSlot(PointerEventData e)
        {
            PortafilterSlot best = null;
            float bestDistance = SnapRadius * rootCanvas.scaleFactor;
            foreach (var slot in rootCanvas.GetComponentsInChildren<PortafilterSlot>())
            {
                if (slot.Current != null && slot.Current != this) continue;
                Vector2 p = RectTransformUtility.WorldToScreenPoint(e.pressEventCamera, slot.transform.position);
                float d = Vector2.Distance(p, e.position);
                if (d < bestDistance)
                {
                    bestDistance = d;
                    best = slot;
                }
            }
            return best;
        }
    }
}
