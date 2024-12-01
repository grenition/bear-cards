using Cysharp.Threading.Tasks;
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
        public override async void OnBeginInteract()
        {
            MapCompositionRoot.Instance.ShowCardGiver();
            await UniTask.WaitForSeconds(1);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
