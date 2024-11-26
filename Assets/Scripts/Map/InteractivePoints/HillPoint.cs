namespace Assets.Scripts.Map
{
    public class HillPoint : InteractivePoint
    {
        public HillPoint()
        {
            PointEntity.Key = "Hill";
        }
        public override void OnBeginInteract()
        { 
        }

        public override void OnEndInteract()
        {
        }
    }
}