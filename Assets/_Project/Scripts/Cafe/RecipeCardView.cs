using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 레시피북의 레시피 카드 (고른 온도 1개): 재료 줄(아이콘, 이름, 양). 쓰지 않는 줄은 숨긴다
    public class RecipeCardView : MonoBehaviour
    {
        [SerializeField] Image[] icons;
        [SerializeField] TMP_Text[] names;
        [SerializeField] TMP_Text[] amounts;

        public int Rows => icons.Length;

        public void SetRow(int row, Sprite icon, string label, string amount)
        {
            icons[row].sprite = icon;
            icons[row].enabled = icon != null;
            names[row].text = label;
            amounts[row].text = amount;
            ShowRow(row, true);
        }

        public void ShowRow(int row, bool show)
        {
            icons[row].gameObject.SetActive(show);
            names[row].gameObject.SetActive(show);
            amounts[row].gameObject.SetActive(show);
        }
    }
}
