namespace CafeTime.Cafe
{
    // 알바생 한 명의 그림 묶음 (Art/Sprites/Staff, ADR-0057). 손님과 같은 걷기 그림 구조를 쓴다 (앉는 그림은 없음)
    [System.Serializable]
    public class StaffLook
    {
        public string id; // StaffDefinition.Id
        public CustomerLook look;
    }
}
