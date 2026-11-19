namespace CafeTime.Core
{
    // 단체 주문 한 줄: 메뉴 id + HOT/ICE + 잔 수 (SFR-009, ADR-0042)
    [System.Serializable]
    public class GroupOrderEntry
    {
        public string menuId;
        public Temperature temperature;
        public int quantity;

        public GroupOrderEntry(string menuId, Temperature temperature, int quantity)
        {
            this.menuId = menuId;
            this.temperature = temperature;
            this.quantity = quantity;
        }
    }
}
