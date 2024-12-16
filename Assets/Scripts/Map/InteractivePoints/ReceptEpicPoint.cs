using Assets.Scripts.Map;

namespace Project
{
    public class ReceptEpicPoint : InteractivePoint
    {
        public ReceptEpicPoint()
        {
            PointEntity.Key = "ReceptEpic";
        }
        public override async void OnBeginInteract()
        {
        }

        public override void OnEndInteract()
        {
        }
    }
}
