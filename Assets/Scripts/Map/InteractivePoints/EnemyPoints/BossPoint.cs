using Project;

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
            var fightPanel = (FightUI)MapCompositionRoot.Instance.MapUI.ActiveUIByKey("fight");
        }

        public override void OnEndInteract()
        {
        }
    }
}