using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project
{
    public class CardMeadlePoint : InteractivePoint
    {
        public CardMeadlePoint()
        {
            PointEntity.Key = "CardMeadle";
        }
        public override async void OnBeginInteract()
        {
            MapCompositionRoot.Instance.ShowCardGiver("meadle");
            await UniTask.WaitForSeconds(1);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
