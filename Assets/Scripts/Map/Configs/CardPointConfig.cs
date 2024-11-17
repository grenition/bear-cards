using UnityEngine;

namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "CardPointConfig", menuName = "Map/Point/CardPointConfig")]
    internal class CardPointConfig : InterestingPointConfig
    {
        public override InteractivePoint CreateInteractivePoint()
        {
            var point = new CardPoint();
            point.View = View;
            return point;
        }
    }
}
