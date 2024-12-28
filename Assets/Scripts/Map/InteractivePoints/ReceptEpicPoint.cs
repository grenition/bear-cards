using Assets.Scripts.Map;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project
{
    public class ReceptEpicPoint : InteractivePoint
    {

        public ReceptEpicPoint()
        {
            //PointEntity.Key = "ReceptEpic";
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

            List<string> newRecept = new();

            for (int i = 0; i < 3; i++)
            {
                if (i < recepts.Length)
                    newRecept.Add(recepts[i]);
            }

            MapCompositionRoot.Instance.ReceptUI.SetRecepts(newRecept.ToArray());
            MapCompositionRoot.Instance.ReceptUI.gameObject.SetActive(true);
            MapCompositionRoot.Instance.ReceptUI.Apper(ComplitedAction);

            var data = DialoguesStatic.LoadData();
            var receptsCollection = data.Recepts.ToList();
            receptsCollection.AddRange(newRecept);
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
