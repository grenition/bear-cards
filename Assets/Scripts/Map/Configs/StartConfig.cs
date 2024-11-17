using UnityEngine;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "HillPointConfig", menuName = "Map/Point/StartPointConfig")]
    internal class StartConfig : InterestingPointConfig
    {
        public override InteractivePoint CreateInteractivePoint()
        {
            var point = new StartPoint();
            point.View = View;
            return point;
        }
    }
}
