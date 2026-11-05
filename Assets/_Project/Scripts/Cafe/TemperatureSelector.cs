using CafeTime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // HOT / ICE 선택 버튼 한 쌍 (SFR-026). 메뉴를 누르기 전에 고른 온도가 적용된다
    public class TemperatureSelector : MonoBehaviour
    {
        [SerializeField] Button hotButton;
        [SerializeField] Button iceButton;
        [SerializeField] Sprite hotNormal;
        [SerializeField] Sprite hotSelected;
        [SerializeField] Sprite iceNormal;
        [SerializeField] Sprite iceSelected;

        public Temperature Current { get; private set; } = Temperature.Ice;

        void Awake()
        {
            hotButton.onClick.AddListener(() => Select(Temperature.Hot));
            iceButton.onClick.AddListener(() => Select(Temperature.Ice));
            Select(Current);
        }

        public void Select(Temperature temperature)
        {
            Current = temperature;
            hotButton.image.sprite = temperature == Temperature.Hot ? hotSelected : hotNormal;
            iceButton.image.sprite = temperature == Temperature.Ice ? iceSelected : iceNormal;
        }

        // 메뉴에 실제로 적용할 온도: 한쪽만 되는 메뉴는 그 온도, 디저트는 None
        public Temperature For(MenuDefinition menu) =>
            menu.IsDessert ? Temperature.None : menu.OnlyTemperature != Temperature.None ? menu.OnlyTemperature : Current;
    }
}
