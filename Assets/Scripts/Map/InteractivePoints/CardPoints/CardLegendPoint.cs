using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project
{
    public class CardLegendPoint : InteractivePoint
    {
        public CardLegendPoint()
        {
            PointEntity.Key = "CardLegend";
        }
        public override async void OnBeginInteract()
        {
            MapCompositionRoot.Instance.ShowCardGiver("strong");
            await UniTask.WaitForSeconds(1);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
