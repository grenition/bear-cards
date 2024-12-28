using Assets.Scripts.Map;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project
{
    public class ReceptMeadlePoint : InteractivePoint
    {
        public ReceptMeadlePoint()
        {
            //PointEntity.Key = "ReceptMeadle";
        }

        public override void OnBeginInteract()
        {
            var recepts = DialoguesStatic.GetNewRecepts();
            if (recepts.Length == 0)
            {
                MapCompositionRoot.Instance.ReceptUI.SetRecepts(recepts);
                MapCompositionRoot.Instance.ReceptUI.gameObject.SetActive(true);
                return;
            }

            var random = new Random();
            for (int i = recepts.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = recepts[j];
                recepts[j] = recepts[i];
                recepts[i] = temp;
            }

            MapCompositionRoot.Instance.ReceptUI.SetRecepts(new string[1] { recepts[0] });
            MapCompositionRoot.Instance.ReceptUI.gameObject.SetActive(true);
            MapCompositionRoot.Instance.ReceptUI.Apper(ComplitedAction);

            var data = DialoguesStatic.LoadData();
            var receptsCollection = data.Recepts.ToList();
            receptsCollection.Add(recepts[0]);
            data.Recepts = receptsCollection.ToArray();
            DialoguesStatic.SaveRecept(receptsCollection.ToArray());

            data.CountReceptComming++;
            DialoguesStatic.SaveDataAndExecuteDialogue(data);
        }
        private void ComplitedAction() => MapCompositionRoot.Instance.MapController.ComplitePoint();

        public override void OnEndInteract()
        {
        }
    }
}
