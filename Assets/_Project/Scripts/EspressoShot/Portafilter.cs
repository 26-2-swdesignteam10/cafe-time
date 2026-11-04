using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [Header("상태 글자 (선택)")]
    public TMP_Text stateLabel;

    public PortafilterState State { get; private set; } = PortafilterState.Empty;
    public PortafilterSlot Slot { get; private set; }

    Image image;
    CanvasGroup group;
    RectTransform rect;
    Canvas rootCanvas;
    bool dragging;

    void Awake()
    {
        image = GetComponent<Image>();
        group = GetComponent<CanvasGroup>();
        rect = (RectTransform)transform;
        rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
        Refresh();
    }

    public void SetState(PortafilterState s)
    {
        State = s;
        Refresh();
    }

    void Refresh()
    {
        if (image == null) return;
        int i = (int)State;
        bool hasSprite = stateSprites != null && i < stateSprites.Length && stateSprites[i] != null;
        if (hasSprite) { image.sprite = stateSprites[i]; image.color = Color.white; }
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

    // 슬롯에 붙이기 (슬롯 쪽에서 호출)
    public void AttachTo(PortafilterSlot slot)
    {
        if (Slot != null && Slot != slot) Slot.Release(this);
        Slot = slot;
        slot.Place(this);
        SnapToSlot();
    }

    void SnapToSlot()
    {
        transform.SetParent(Slot.transform, false);
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
    }

    // ---------- 드래그 ----------
    public void OnBeginDrag(PointerEventData e)
    {
        if (Slot != null && Slot.Locked)   // 추출 중 / 분쇄 중에는 분리 불가
        {
            EspressoToast.Show("작업 중에는 분리할 수 없습니다.");
            dragging = false;
            return;
        }
        dragging = true;
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

        // 놓을 수 없는 곳 / 거절된 곳 → 원래 위치로 (SFR-030)
        if (target == null || target == Slot || !target.TryAccept(this))
            SnapToSlot();
    }
}
