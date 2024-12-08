using Project;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class HillPoint : InteractivePoint
    {
        private HillDescriptionConfig _config;
        public HillPoint()
        {
            PointEntity.Key = "Hill";
            _config = Resources.Load<HillDescriptionConfig>("Map/DescConfig/HillDescriptionConfig");
        }

        public override void OnBeginInteract()
        {
            var hillPanel = (HillUI)MapCompositionRoot.Instance.MapUI.ActiveUIByKey("hill");
            hillPanel.Apper(() => MapCompositionRoot.Instance.MapController.ComplitePoint(),
                _config.Icon, _config.Description, _config.HPModificator, _config.Name);
        }

        public override void OnEndInteract()
        {
        }
    }
}