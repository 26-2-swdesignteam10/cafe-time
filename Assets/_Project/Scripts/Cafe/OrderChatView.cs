using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 포스기 화면 옆 손님 주문 채팅 (SFR-026). 메뉴 한 줄마다 말풍선 하나
    public class OrderChatView : MonoBehaviour
    {
        [SerializeField] Transform container;
        [SerializeField] TMP_Text bubbleTemplate;   // 말풍선 그림 안의 글자. 부모(말풍선)를 복제한다

        readonly List<GameObject> bubbles = new List<GameObject>();

        void Awake() => bubbleTemplate.transform.parent.gameObject.SetActive(false);

        public void Show(IReadOnlyList<string> sentences)
        {
            foreach (var bubble in bubbles) { bubble.SetActive(false); Destroy(bubble); } // Destroy 는 프레임 끝이라 먼저 숨김
            bubbles.Clear();
            var template = bubbleTemplate.transform.parent.gameObject;
            foreach (string sentence in sentences)
            {
                var bubble = Instantiate(template, container);
                bubble.SetActive(true);
                bubble.GetComponentInChildren<TMP_Text>().text = sentence;
                bubbles.Add(bubble);
            }
        }
    }
}
