using Assets.Scripts.Map;
using System;
using System.Collections.Generic;

namespace Project
{
    public class ReceptEpicPoint : InteractivePoint
    {

        public ReceptEpicPoint()
        {
            PointEntity.Key = "ReceptEpic";
        }

        public override void OnBeginInteract()
        {
            var recepts = DialoguesStatic.GetNewRecepts();
            if (recepts.Length == 0)
                return;//Give empty string

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
                if (i < newRecept.Count)
                    newRecept.Add(recepts[i]);
            }

            //give newRecept
        }

        public override void OnEndInteract()
        {
        }
    }
}
