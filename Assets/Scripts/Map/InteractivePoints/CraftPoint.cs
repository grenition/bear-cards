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
            PointEntity.Key = "CraftMeadle";
        }
        public override  void OnBeginInteract()
        {
            MapCompositionRoot.Instance.ShowCraftGiver();
        }

        public override void OnEndInteract()
        {
        }
    }
}
