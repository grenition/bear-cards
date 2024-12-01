using System;
using System.Collections;
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
            MapCompositionRoot.Instance.ShowCardGiver();
        }

        private IEnumerator Deley()
        {
            yield return new WaitForSeconds(0.6f);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
