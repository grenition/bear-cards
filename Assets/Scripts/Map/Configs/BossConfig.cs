using UnityEngine;

namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "BossPointConfig", menuName = "Map/Point/BossPointConfig")]
    public class BossConfig : InterestingPointConfig
    {
        public override InteractivePoint CreateInteractivePoint()
        {
            var point = new BossPoint();
            point.View = View;
            return point;
        }
    }
}
