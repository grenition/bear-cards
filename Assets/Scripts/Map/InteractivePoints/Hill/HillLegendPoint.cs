using Assets.Scripts.Map;
using UnityEngine;

namespace Project
{
    public class HillLegendPoint : InteractivePoint
    {
        private HillDescriptionConfig _config;
        public HillLegendPoint()
        {
            //PointEntity.Key = "HillLegend";
            _config = Resources.Load<HillDescriptionConfig>("Map/DescConfig/HillLegendConfig");
        }

        public override void OnBeginInteract()
        {
            var data = DialoguesStatic.LoadData();
            data.CountHillComming++;
            DialoguesStatic.SaveDataAndExecuteDialogue(data);

            var hillPanel = (HillUI)MapCompositionRoot.Instance.MapUI.ActiveUIByKey("hill");
            hillPanel.Apper(() => MapCompositionRoot.Instance.MapController.ComplitePoint(),
                _config.Icon, _config.LocalizedDescription.GetLocalizedString(), _config.HPModificator, _config.LocalizedName.GetLocalizedString());
        }

        public override void OnEndInteract()
        {
        }
    }
}
