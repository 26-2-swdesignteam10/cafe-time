using TMPro;
using UnityEngine;

// 그룹헤드 위 LCD 글자. 그룹헤드 하나당 하나씩 (L, R) 붙이세요.
// 글자(TMP_Text) 오브젝트에 이 스크립트를 붙이고 Head / Label 을 연결합니다.
public class ExtractDisplay : MonoBehaviour
{
    public PortafilterSlot head;   // Slot_GroupL 또는 Slot_GroupR
    public TMP_Text label;         // 같은 오브젝트의 TMP 글자

    void Reset() { label = GetComponent<TMP_Text>(); }

    void Update()
    {
        if (head == null || label == null) return;

        if (head.Locked && head.TimeLeft > 0f)
        {
            label.text = "추출 중…\n" + Mathf.CeilToInt(head.TimeLeft) + "초 남음";
            return;
        }

        Portafilter pf = head.Current;
        if (pf == null) label.text = "대기";
        else if (pf.State == PortafilterState.Used) label.text = "추출\n완료";
        else label.text = "준비\n완료";
    }
}
