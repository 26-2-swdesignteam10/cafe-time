using CafeTime.Cafe;
using CafeTime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.EspressoShot
{
    // SFR-031: 원두 분쇄 (그라인더)
    // 그라인더 오브젝트에 붙이고, 분쇄 버튼 OnClick → Grinder.OnClickGrind
    public class Grinder : MonoBehaviour
    {
        public const string BeanId = "bean"; // 오픈 준비에서 구매한 원두 재고 (단위: 회)

        public PortafilterSlot slot;        // 그라인더 슬롯 (type = Grinder)
        public int beansPerGrind = 1;
        public Image progressFill;          // 선택: Image Type = Filled 로 설정
        public GameObject stream;           // 선택: 분쇄 중에 켜는 가루 줄기

        bool grinding;
        public bool Grinding => grinding;
        public float TimeLeft { get; private set; }   // 분쇄 남은 시간(초). LCD 표시용

        public void OnClickGrind()
        {
            if (grinding) return;

            Portafilter pf = slot.Current;
            if (pf == null)
            {
                Toast.Show(ToastMessage.SetPortafilter);
                return;
            }
            if (pf.State != PortafilterState.Empty)
            {
                Toast.Show(ToastMessage.EmptyPortafilterOnly);
                return;
            }
            if (!Pantry.TryUse(BeanId, beansPerGrind)) // 분쇄 1회당 재고 차감
            {
                Toast.Show(ToastMessage.NoBean);
                return;
            }

            grinding = true;
            slot.Locked = true;
            float seconds = GameBalance.Instance.GrindSeconds;
            if (progressFill != null) progressFill.fillAmount = 0f;
            if (stream != null) stream.SetActive(true);

            // 화면을 닫아도 계속 진행되도록 항상 켜진 EspressoMachine 에서 진행
            EspressoMachine.Instance.RunTimed(seconds, left =>
            {
                TimeLeft = left;
                if (progressFill != null) progressFill.fillAmount = 1f - left / seconds;
                pf.ShowFill(PortafilterState.Ground, 1f - left / seconds); // 바스켓에 가루가 차오름
            }, () =>
            {
                if (stream != null) stream.SetActive(false);
                pf.HideFill();
                pf.SetState(PortafilterState.Ground);   // → '원두가루 담김'
                if (progressFill != null) progressFill.fillAmount = 0f;
                slot.Locked = false;
                grinding = false;
            });
        }
    }
}
