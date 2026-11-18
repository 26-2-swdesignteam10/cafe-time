using UnityEngine;
using CafeTime.Core;

namespace CafeTime.Cafe
{
    // 맵의 기기 위 작업 상태 표시 (SFR-022): 작업 중이면 진행 게이지, 끝나면 완료 아이콘(기기 화면을 열면 사라짐)
    public class WorkStatusIcon : MonoBehaviour
    {
        [Tooltip("IWorkStatus 를 구현한 기기 (EspressoMachine, WaterDispenser, Blender, DishStation)")]
        [SerializeField] MonoBehaviour source;
        [SerializeField] SpriteRenderer doneIcon;
        [SerializeField] Sprite gaugeFrame;
        [SerializeField] Sprite gaugeFill;

        IWorkStatus status;
        WorldGauge gauge;
        bool wasDoneVisible;

        void Start()
        {
            status = source as IWorkStatus;
            gauge = WorldGauge.Create(transform, gaugeFrame, gaugeFill, warnColors: false);
        }

        void Update()
        {
            if (status == null) return;

            float progress = status.Progress;
            gauge.Show(progress >= 0);

            if (progress >= 0)
                gauge.Set(progress);

            bool showDone = progress < 0 && status.Done;
            doneIcon.gameObject.SetActive(showDone);

            // SFR-022: 완료 체크 표시가 켜지는 순간 한 번만 재생
            if (showDone && !wasDoneVisible)
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlayWorkComplete();
            }

            wasDoneVisible = showDone;
        }
    }
}
