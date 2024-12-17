using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Map
{
    internal class CardPoint : InteractivePoint
    {
        public CardPoint()
        {
            PointEntity.Key = "CardEasy";
        }

        public override async void OnBeginInteract()
        {
            var panel = MapCompositionRoot.Instance.ShowCardGiver("easy");
            await UniTask.WaitUntil(() => panel.activeSelf == false);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
