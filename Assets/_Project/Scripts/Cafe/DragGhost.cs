using System;
using UnityEngine;
using UnityEngine.UI;

namespace CafeTime.Cafe
{
    // 끄는 동안 커서를 따라다니는 아이콘 (HandSlotView, DragItem 공용). 끄는 동안 원래 자리는 비워 보인다.
    // 놓은 다음 프레임에 원래 자리에 물건이 그대로 있으면(받아 준 곳이 없음) 그 자리로 날아 돌아가고,
    // 없어졌으면(놓은 곳이 가져감) 그냥 사라진다. 어느 쪽이든 끝나면 done 을 불러 원래 자리를 다시 보이게 한다
    public class DragGhost : MonoBehaviour
    {
        const float ReturnSeconds = 0.15f;

        Transform home;          // 돌아갈 자리
        Func<bool> stillThere;   // 원래 자리에 물건이 그대로 있는지
        Action done;
        bool released, judged;
        Vector3 from;
        float elapsed;

        public static DragGhost Create(Image original, Transform layer, Vector2 position)
        {
            var image = Instantiate(original, layer);
            foreach (var graphic in image.GetComponentsInChildren<Graphic>()) graphic.raycastTarget = false; // 놓을 곳을 가리지 않게
            image.enabled = true;
            image.transform.position = position;
            return image.gameObject.AddComponent<DragGhost>();
        }

        public void Follow(Vector2 position)
        {
            if (!released) transform.position = position;
        }

        public void Release(Transform homeSpot, Func<bool> isStillThere, Action onDone)
        {
            home = homeSpot;
            stillThere = isStillThere;
            done = onDone;
            released = true;
        }

        // 날아가는 중에 다시 끌기 시작하면 바로 끝낸다
        public void Finish()
        {
            if (this == null) return;
            var callback = done;
            done = null;
            Destroy(gameObject);
            callback?.Invoke();
        }

        void Update()
        {
            if (!released) return;
            if (!judged)
            {
                judged = true; // 놓은 곳의 처리(물건 이동, 화면 갱신·삭제)가 끝난 다음 프레임에 판단
                if (!HomeVisible || !stillThere()) { Finish(); return; }
                from = transform.position;
            }
            if (!HomeVisible) { Finish(); return; }
            elapsed += Time.unscaledDeltaTime; // 일시정지(timeScale 0) 중에도 돌아가게
            float t = Mathf.Clamp01(elapsed / ReturnSeconds);
            transform.position = Vector3.Lerp(from, home.position, 1f - (1f - t) * (1f - t)); // 끝에서 느려짐
            if (t >= 1f) Finish();
        }

        bool HomeVisible => home != null && home.gameObject.activeInHierarchy;
    }
}
