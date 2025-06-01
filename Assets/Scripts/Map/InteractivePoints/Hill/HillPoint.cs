using Project;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class HillPoint : InteractivePoint
    {
        private HillDescriptionConfig _config;
        public HillPoint()
        {
            //PointEntity.Key = "HillEasy";
            _config = Resources.Load<HillDescriptionConfig>("Map/DescConfig/HillDescriptionConfig");
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