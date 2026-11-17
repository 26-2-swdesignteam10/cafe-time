using System;
using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 재료에 마우스를 올리면 뜨는 이름·남은 양 (SFR-049)
    public class IngredientTooltip : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] Vector2 offset = new Vector2(0, 80);

        Func<string> describe;

        public void Show(IngredientSlotView slot) => Show(slot.transform, slot.Describe);

        // 선반 칸이 아닌 물건(쉐이크 제조실 원액 통·우유 팩)에도 띄운다
        public void Show(Transform anchor, Func<string> describeTarget)
        {
            describe = describeTarget;
            gameObject.SetActive(true);
            transform.position = anchor.position + (Vector3)offset;
            transform.SetAsLastSibling();
        }

        public void Hide()
        {
            describe = null;
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (describe != null) text.text = describe(); // 붓는 동안 실시간 갱신
        }
    }
}
