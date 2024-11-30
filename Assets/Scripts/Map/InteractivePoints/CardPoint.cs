using System;
using UnityEngine;

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
            MapCompositionRoot.Instance.MapUI.ActiveUIByKey("giveCard", () =>
            {
                Debug.Log("You get card!");
                MapCompositionRoot.Instance.MapController.ComplitePoint();
            });
        }

        public override void OnEndInteract()
        {
        }
    }
}
