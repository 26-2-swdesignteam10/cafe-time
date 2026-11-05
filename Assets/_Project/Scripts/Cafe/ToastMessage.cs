namespace CafeTime.Cafe
{
    // Toast 안내 문구 종류. Toast.messages 배열 순서와 같다 (각각 Art/UI/CafeHud/Toast<이름>.png 글자 이미지)
    public enum ToastMessage
    {
        NoCustomer,             // 주문할 손님이 없습니다. (SFR-026)
        HandsFull,              // 인벤토리가 가득 찼습니다. (SFR-021)
        NoStock,                // 재고가 없습니다.
        EntryClosed,            // 손님 입장이 마감되었습니다. (SFR-014)
        NoCleanCup,             // 깨끗한 커피잔이 없습니다. (SFR-028)
        SetPortafilter,         // 포터필터를 먼저 세팅해 주세요. (SFR-031)
        NoBean,                 // 원두가 부족합니다.
        EmptyPortafilterOnly,   // 빈 포터필터만 사용할 수 있습니다.
        TampedOnly,             // 탬핑을 완료한 포터필터만 장착할 수 있습니다. (SFR-033)
        AttachPortafilter,      // 포터필터를 장착해 주세요. (SFR-035)
        PlaceShotGlass,         // 빈 컵을 놓아 주세요.
        ShotAlready,            // 이미 샷이 담긴 컵입니다.
        NoIce,                  // 얼음이 부족합니다. (SFR-039)
        PlaceCup,               // 커피잔을 놓아 주세요. (SFR-043, 055)
        CupFirst,               // 커피잔을 먼저 올려 주세요. (SFR-046~048)
        NoIngredient,           // 재료가 부족합니다. (SFR-049)
        NothingToComplete,      // 완성할 음료가 없습니다. (SFR-051)
        BlenderEmpty,           // 믹서기에 재료를 넣어 주세요. (SFR-054)
        BlendFirst,             // 먼저 믹서기를 돌려 주세요. (SFR-055)
        NoPlate,                // 깨끗한 접시가 없습니다.
        NeedTowel,              // 행주를 들고 와 주세요. (SFR-066)
        ReturnScoop,            // 스쿱을 먼저 반납해 주세요. (SFR-040)
        Extracting,             // 추출 중에는 뺄 수 없습니다. (SFR-033, 034)
    }
}
