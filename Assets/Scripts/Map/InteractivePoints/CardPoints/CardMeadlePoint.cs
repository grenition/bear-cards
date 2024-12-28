using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;

namespace Project
{
    public class CardMeadlePoint : InteractivePoint
    {
        public CardMeadlePoint()
        {
            //PointEntity.Key = "CardMeadle";
        }
        public override async void OnBeginInteract()
        {
            var panel = MapCompositionRoot.Instance.ShowCardGiver("meadle");
            await UniTask.WaitUntil(() => panel.activeSelf == false);
            MapCompositionRoot.Instance.MapController.ComplitePoint();
        }

        public override void OnEndInteract()
        {
        }
    }
}
