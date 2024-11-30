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
            MapCompositionRoot.Instance.MapUI.ActiveUIByKey("fight", null);
        }

        public override void OnEndInteract()
        {
        }
    }
}