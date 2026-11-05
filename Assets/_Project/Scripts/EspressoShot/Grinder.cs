using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.EspressoShot
{
    // SFR-031: 원두 분쇄 (그라인더)
    // 그라인더 오브젝트에 붙이고, 분쇄 버튼 OnClick → Grinder.OnClickGrind
    public class Grinder : MonoBehaviour
    {
        public PortafilterSlot slot;        // 그라인더 슬롯 (type = Grinder)
        public float grindSeconds = 5f;
        public int beansPerGrind = 1;
        public Image progressFill;          // 선택: Image Type = Filled 로 설정

        bool grinding;
        public bool Grinding => grinding;
        public float TimeLeft { get; private set; }   // 분쇄 남은 시간(초). LCD 표시용

        public void OnClickGrind()
        {
            if (grinding) return;

            Portafilter pf = slot.Current;
            if (pf == null)
            {
                EspressoToast.Show("포터필터를 먼저 세팅해 주세요.");
                return;
            }
            if (BeanInventory.Beans < beansPerGrind)
            {
                EspressoToast.Show("원두가 부족합니다.");
                return;
            }
            if (pf.State != PortafilterState.Empty)
            {
                EspressoToast.Show("빈 포터필터만 사용할 수 있습니다.");
                return;
            }

            // 화면을 닫아도 계속 진행되도록 항상 켜진 EspressoMachine 에서 코루틴 실행
            EspressoMachine.Instance.StartCoroutine(GrindRoutine(pf));
        }

        IEnumerator GrindRoutine(Portafilter pf)
        {
            grinding = true;
            slot.Locked = true;
            if (progressFill != null) progressFill.fillAmount = 0f;

            for (float t = 0; t < grindSeconds; t += Time.deltaTime)
            {
                TimeLeft = grindSeconds - t;
                if (progressFill != null) progressFill.fillAmount = t / grindSeconds;
                yield return null;
            }

            TimeLeft = 0f;
            BeanInventory.Beans -= beansPerGrind;   // 분쇄 1회당 재고 차감
            pf.SetState(PortafilterState.Ground);   // → '원두가루 담김'

            if (progressFill != null) progressFill.fillAmount = 0f;
            slot.Locked = false;
            grinding = false;
        }
    }
}
