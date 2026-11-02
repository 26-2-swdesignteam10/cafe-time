using UnityEngine;
using UnityEngine.Tilemaps;

namespace CafeTime.Player
{
    // 카메라 따라가기 (SFR-019)
    // 캐릭터를 화면 중앙에 두되, 맵 가장자리에서는 맵 바깥이 보이지 않도록 멈춤
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Tilemap mapBounds; // 맵 전체를 덮는 타일맵 (벽 타일맵)

        Camera cam;
        Bounds bounds;

        void Awake()
        {
            cam = GetComponent<Camera>();
            mapBounds.CompressBounds();
            var local = mapBounds.localBounds;
            bounds = new Bounds(mapBounds.transform.TransformPoint(local.center), local.size);
        }

        void LateUpdate()
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            var p = target.position;
            transform.position = new Vector3(
                ClampAxis(p.x, bounds.min.x, bounds.max.x, halfWidth),
                ClampAxis(p.y, bounds.min.y, bounds.max.y, halfHeight),
                transform.position.z);
        }

        // 맵이 화면보다 작은 축은 맵 가운데에 고정
        static float ClampAxis(float value, float min, float max, float halfView)
        {
            if (max - min <= halfView * 2) return (min + max) / 2;
            return Mathf.Clamp(value, min + halfView, max - halfView);
        }
    }
}
