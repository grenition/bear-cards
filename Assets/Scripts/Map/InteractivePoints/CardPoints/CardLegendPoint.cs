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
            var panel = MapCompositionRoot.Instance.ShowCardGiver("strong");
            await UniTask.WaitUntil(() => panel.activeSelf == false);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
