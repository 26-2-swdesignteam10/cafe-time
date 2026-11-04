using CafeTime.UI;
using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 아직 구현되지 않은 기기 화면 대신 띄우는 임시 창 (SFR-029 등 기기 화면 구현 시 각 기기의 화면으로 교체)
    public class StationScreenPlaceholder : Popup
    {
        [SerializeField] TMP_Text titleText;

        public void Open(string stationName)
        {
            titleText.text = stationName; // "기기 화면 준비 중" 은 창의 글자 이미지
            Open();
        }
    }
}
