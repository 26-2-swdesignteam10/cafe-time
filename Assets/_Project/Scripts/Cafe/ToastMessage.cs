namespace CafeTime.Cafe
{
    // Toast 안내 문구 종류. Toast.messages 배열 순서와 같다 (각각 Art/UI/CafeHud 의 글자 이미지)
    public enum ToastMessage
    {
        NoCustomer,   // 주문할 손님이 없습니다. (SFR-026)
        HandsFull,    // 인벤토리가 가득 찼습니다. (SFR-021)
        NoStock,      // 재고가 없습니다.
        EntryClosed,  // 손님 입장이 마감되었습니다. (SFR-014)
    }
}
