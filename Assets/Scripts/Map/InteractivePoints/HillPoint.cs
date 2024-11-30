using UnityEngine;

namespace Assets.Scripts.Map
{
    public class HillPoint : InteractivePoint
    {
        public HillPoint()
        {
            PointEntity.Key = "Hill";
        }
        public override void OnBeginInteract()
        {
            MapCompositionRoot.Instance.MapUI.ActiveUIByKey("hill", () =>
            {
                Debug.Log("You hill hit point");
                MapCompositionRoot.Instance.MapController.ComplitePoint();
            });
        }

        public override void OnEndInteract()
        {
        }
    }
}