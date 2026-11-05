using System.Collections;
using TMPro;
using UnityEngine;

namespace CafeTime.EspressoShot
{
    // 경고 문구 표시. "항상 켜져 있는" 오브젝트(Canvas 바로 아래)에 붙이세요.
    // 샷 화면 패널 안에 넣으면 패널이 꺼진 상태에서 Awake가 안 돌아 Instance가 비어요.
    [RequireComponent(typeof(CanvasGroup))]
    public class EspressoToast : MonoBehaviour
    {
        static EspressoToast instance;

        public TMP_Text label;
        public float showSeconds = 1.6f;

        CanvasGroup group;
        Coroutine running;

        void Awake()
        {
            instance = this;
            group = GetComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }

        public static void Show(string message)
        {
            if (instance == null) { UnityEngine.Debug.Log("[Toast] " + message); return; }
            instance.Display(message);
        }

        void Display(string message)
        {
            label.text = message;
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            group.alpha = 1f;
            yield return new WaitForSeconds(showSeconds);
            for (float t = 0; t < 0.3f; t += Time.deltaTime)
            {
                group.alpha = 1f - t / 0.3f;
                yield return null;
            }
            group.alpha = 0f;
            running = null;
        }
    }
}
