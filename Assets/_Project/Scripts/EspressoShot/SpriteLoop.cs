using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.EspressoShot
{
    // 켜져 있는 동안 프레임 그림을 반복해 보여 준다 (분쇄 가루 줄기, 추출 커피 줄기)
    [RequireComponent(typeof(Image))]
    public class SpriteLoop : MonoBehaviour
    {
        public Sprite[] frames;
        public float framesPerSecond = 12f;

        Image image;
        float time;

        void Awake() => image = GetComponent<Image>();

        void OnEnable()
        {
            time = 0f;
            Apply();
        }

        void Update()
        {
            time += EspressoMachine.Delta; // 일시정지 중에는 멈춤
            Apply();
        }

        void Apply()
        {
            if (image == null || frames == null || frames.Length == 0) return;
            image.sprite = frames[(int)(time * framesPerSecond) % frames.Length];
        }
    }
}
