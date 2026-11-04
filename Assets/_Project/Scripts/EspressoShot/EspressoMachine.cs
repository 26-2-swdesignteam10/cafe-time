using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// SFR-029: 샷 내리기 화면 열기/닫기 + 추출 진행
// "항상 켜져 있는" 오브젝트(예: Canvas 아래 EspressoMachine 빈 오브젝트)에 붙이세요.
// screenPanel 만 껐다 켜는 구조라서, 닫아도 포터필터 위치/상태가 그대로 유지되고
// 추출 코루틴도 이 오브젝트에서 돌기 때문에 화면을 닫아도 계속 진행돼요.
public class EspressoMachine : MonoBehaviour
{
    public static EspressoMachine Instance { get; private set; }

    public GameObject screenPanel;
    public float extractSeconds = 25f;   // 임시값

    void Awake()
    {
        Instance = this;
        if (screenPanel != null) screenPanel.SetActive(false);
    }

    void Update()
    {
        if (screenPanel == null || !screenPanel.activeSelf) return;
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) Close();
#else
        if (Input.GetKeyDown(KeyCode.Escape)) Close();
#endif
    }

    // 맵에서 에스프레소 머신과 상호작용할 때 호출
    public void Open()  { screenPanel.SetActive(true); }

    // X 버튼 OnClick 또는 ESC
    public void Close() { screenPanel.SetActive(false); }

    // ---------- 추출 (그룹헤드 슬롯별 독립 진행) ----------
    public void StartExtraction(PortafilterSlot head)
    {
        if (head.Locked) return;
        Portafilter pf = head.Current;
        if (pf == null)
        {
            EspressoToast.Show("장착된 포터필터가 없습니다.");
            return;
        }
        if (pf.State != PortafilterState.Tamped)
        {
            EspressoToast.Show("탬핑 완료된 포터필터가 필요합니다.");
            return;
        }
        StartCoroutine(ExtractRoutine(head, pf));
    }

    IEnumerator ExtractRoutine(PortafilterSlot head, Portafilter pf)
    {
        head.Locked = true;                 // 추출 중에는 분리 불가
        head.TimeLeft = extractSeconds;
        while (head.TimeLeft > 0f)
        {
            head.TimeLeft -= Time.deltaTime;
            yield return null;
        }
        head.TimeLeft = 0f;
        pf.SetState(PortafilterState.Used); // → '사용 완료(찌꺼기)'
        head.Locked = false;
        // TODO: 완성된 샷을 음료 제작 쪽(샷 개수 +1 등)으로 넘기기
    }
}
