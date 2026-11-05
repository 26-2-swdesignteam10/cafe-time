namespace CafeTime.Cafe
{
    // 시간이 걸리는 작업을 하는 기기 (SFR-022). 맵의 WorkStatusIcon 이 읽어 게이지·완료 아이콘을 띄운다
    public interface IWorkStatus
    {
        // 진행 중인 작업의 진행도 0~1. 작업이 없으면 음수
        float Progress { get; }

        // 작업이 끝났고 플레이어가 아직 기기 화면을 열어 보지 않음
        bool Done { get; }

        // 기기 화면을 열면 호출 → 완료 아이콘이 사라짐
        void ClearDone();
    }
}
