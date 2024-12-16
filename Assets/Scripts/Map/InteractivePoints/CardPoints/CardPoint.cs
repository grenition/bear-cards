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
            MapCompositionRoot.Instance.ShowCardGiver("easy");
            await UniTask.WaitForSeconds(1);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
