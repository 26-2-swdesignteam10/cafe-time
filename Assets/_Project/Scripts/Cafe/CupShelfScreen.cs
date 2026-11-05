using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 커피잔 선반 (SFR-028): 커피잔 종류를 골라 빈 커피잔 1개를 손에 든다
    public class CupShelfScreen : StationScreen
    {
        [SerializeField] Button ceramicButton;
        [SerializeField] Button plasticButton;
        [SerializeField] TMP_Text ceramicCount;
        [SerializeField] TMP_Text plasticCount;

        protected override void Awake()
        {
            base.Awake();
            ceramicButton.onClick.AddListener(() => CupShelf.Instance.TakeCup(CupType.Ceramic));
            plasticButton.onClick.AddListener(() => CupShelf.Instance.TakeCup(CupType.Plastic));
            CupShelf.Instance.Changed += RefreshIfOpen;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (CupShelf.Instance != null) CupShelf.Instance.Changed -= RefreshIfOpen;
        }

        protected override void Refresh()
        {
            ceramicCount.text = $"{CupShelf.Instance.Count(CupType.Ceramic)}개";
            plasticCount.text = $"{CupShelf.Instance.Count(CupType.Plastic)}개";
        }
    }
}
