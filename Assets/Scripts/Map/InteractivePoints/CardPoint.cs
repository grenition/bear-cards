using System;

namespace Assets.Scripts.Map
{
    internal class CardPoint : InteractivePoint
    {
        public CardPoint()
        {
            PointEntity.Key = "Card";
        }
        public override void OnBeginInteract()
        {
        }

        public override void OnEndInteract()
        {
        }
    }
}
