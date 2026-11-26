namespace CafeTime.Core
{
    // 업그레이드할 수 있는 장비 (ADR-0057). 순서 = 업그레이드 화면 카드 순서, 저장 파일의 단계 배열 순서이므로 끝에만 추가한다
    public enum Equipment
    {
        Dishwasher, // 식기세척기 (설거지실)
        IceMaker,   // 제빙기
        Espresso,   // 에스프레소 머신 (그라인더 포함)
        Pos,        // 포스기
        TrayReturn, // 퇴식구
        Water,      // 정수기
    }
}
