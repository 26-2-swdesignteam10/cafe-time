using CafeTime.Cafe;
using CafeTime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CafeTime.Player
{
    // 오브젝트 상호작용 (SFR-020), 접근 시 테두리 + "E 사용" 안내 (SFR-013)
    // 범위 안의 Interactable 중 가장 가까운 1개만 대상으로 함
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] float range = 0.8f; // 상호작용 거리 (추후 결정). 오브젝트 충돌 영역 가장자리까지의 거리
        [SerializeField] GameObject prompt;  // 오브젝트 위에 띄우는 키 안내
        [SerializeField] TMP_Text promptText;

        Interactable target;

        void Update()
        {
            // UI가 열려 있는 동안에는 대상 없음 → 안내 숨김, E 입력 무시
            var next = Popup.AnyOpen ? null : FindNearest();
            if (next != target)
            {
                if (target != null) target.SetHighlighted(false);
                target = next;
                if (target != null) target.SetHighlighted(true);
            }

            prompt.SetActive(target != null);
            if (target == null) return;

            prompt.transform.position = target.PromptPosition;
            promptText.text = "E " + target.ActionLabel;

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                target.Interact();
        }

        Interactable FindNearest()
        {
            Vector2 position = transform.position;
            Interactable nearest = null;
            float nearestDistance = range;
            foreach (var interactable in Interactable.All)
            {
                float distance = interactable.DistanceTo(position);
                if (distance > nearestDistance) continue;
                nearest = interactable;
                nearestDistance = distance;
            }
            return nearest;
        }
    }
}
