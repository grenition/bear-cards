using System;

namespace Assets.Scripts.Map
{
    internal class CardPoint : InteractivePoint
    {
        public CardPoint()
        {
            Key = "Card";
        }
        public override void OnBeginInteract()
        {
            throw new NotImplementedException();
        }

        public override void OnEndInteract()
        {
            throw new NotImplementedException();
        }
    }
}
