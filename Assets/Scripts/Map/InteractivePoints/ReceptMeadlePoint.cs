using Assets.Scripts.Map;

namespace Project
{
    public class ReceptMeadlePoint : InteractivePoint
    {
        public ReceptMeadlePoint()
        {
            PointEntity.Key = "Start";
        }

        public override void OnBeginInteract()
        {
        }

        public override void OnEndInteract()
        {
        }
    }
}
