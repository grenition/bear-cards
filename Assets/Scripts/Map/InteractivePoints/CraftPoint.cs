using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Map
{
    internal class CraftPoint : InteractivePoint
    {
        public CraftPoint()
        {
            PointEntity.Key = "Craft";
        }
        public override async void OnBeginInteract()
        {
            MapCompositionRoot.Instance.ShowCraftGiver();
            await UniTask.WaitForSeconds(1);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
