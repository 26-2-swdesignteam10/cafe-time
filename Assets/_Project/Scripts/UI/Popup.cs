using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.UI
{
    // 모든 팝업/기기 화면의 공통 부모.
    // - 열린 순서대로 관리하여 ESC 입력 시 가장 위의 창부터 닫음 (SFR-023, 029 등 "X 또는 ESC로 닫기")
    // - Popup.AnyOpen 으로 UI가 열려 있는지 확인 가능 (SFR-015, 020: UI가 열려 있으면 이동/상호작용 입력 무시)
    public class Popup : MonoBehaviour
    {
        static readonly List<Popup> openPopups = new List<Popup>();

        public static bool AnyOpen => openPopups.Count > 0;

        [SerializeField] Button closeButton; // X / 닫기 버튼 (없으면 비워 둠)

        protected virtual void Awake()
        {
            if (closeButton != null) closeButton.onClick.AddListener(Close);
        }

        public virtual void Open() => gameObject.SetActive(true);

        // ESC, X 버튼 모두 이 메서드로 닫힘. 닫을 때 처리가 필요하면 override
        public virtual void Close() => gameObject.SetActive(false);

        protected virtual void OnEnable() => openPopups.Add(this);

        protected virtual void OnDisable() => openPopups.Remove(this);

        // 가장 위에 열린 팝업을 닫음. 닫을 팝업이 없으면 false
        public static bool CloseTop()
        {
            if (openPopups.Count == 0) return false;
            openPopups[openPopups.Count - 1].Close();
            return true;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => openPopups.Clear();
    }
}
