using UnityEngine;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "EnemyPointConfig", menuName = "Map/Point/EnemyPointConfig")]
    public class EnemyPointConfig : InterestingPointConfig
    {
        public override InteractivePoint CreateInteractivePoint()
        {
            var point = new EnemyPoint();
            point.View = View;
            return point;
        }
    }
}