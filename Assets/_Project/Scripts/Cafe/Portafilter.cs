namespace CafeTime.Cafe
{
    // 포터필터 1개 (SFR-030). 에스프레소 머신에 2개가 있다
    public class Portafilter
    {
        public PortafilterState State { get; set; }
        public PortafilterSpot Spot { get; set; }

        public Portafilter(PortafilterSpot spot) => Spot = spot;
    }
}
