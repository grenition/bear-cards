namespace Assets.Scripts.Map
{
    public class BossPoint : InteractivePoint
    {
        public BossPoint()
        {
            PointEntity.Key = "Boss";
        }

        public override void OnBeginInteract()
        {
        }

        public override void OnEndInteract()
        {
        }
    }
}