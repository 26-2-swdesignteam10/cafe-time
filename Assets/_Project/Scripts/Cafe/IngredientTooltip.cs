using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 재료에 마우스를 올리면 뜨는 이름·남은 양 (SFR-049)
    public class IngredientTooltip : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] Vector2 offset = new Vector2(0, 80);

        IngredientSlotView target;

        public void Show(IngredientSlotView slot)
        {
            target = slot;
            gameObject.SetActive(true);
            transform.position = slot.transform.position + (Vector3)offset;
            transform.SetAsLastSibling();
        }

        public void Hide()
        {
            target = null;
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (target != null) text.text = target.Describe(); // 붓는 동안 실시간 갱신
        }
    }
}
