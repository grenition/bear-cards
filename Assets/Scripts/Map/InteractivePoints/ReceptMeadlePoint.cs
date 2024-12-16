using Assets.Scripts.Map;
using System;
using System.Collections.Generic;

namespace Project
{
    public class ReceptMeadlePoint : InteractivePoint
    {
        public ReceptMeadlePoint()
        {
            PointEntity.Key = "Start";
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

            string newRecept = recepts[0];//Give
        }

        public override void OnEndInteract()
        {
        }
    }
}
