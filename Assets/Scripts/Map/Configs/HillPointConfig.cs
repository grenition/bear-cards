using UnityEngine;

namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "HillPointConfig", menuName = "Map/Point/HillPointConfig")]
    internal class HillPointConfig : InterestingPointConfig
    {
        public override InteractivePoint CreateInteractivePoint()
        {
            var point = new HillPoint();
            point.View = View;
            return point;
        }
    }
}
