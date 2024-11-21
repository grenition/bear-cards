using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class OnePath : IPointPatternCreatable
    {
        public List<InteractivePoint> Create(
            ref List<InteractivePoint> lastLevelPoint,
            List<InterestingPointConfig> pointsSet)
        {
            var index = Random.Range(0, pointsSet.Count);

            var newPoint = PointFactory.Instance.CreatePoint(pointsSet[index].Name);
            newPoint.View = pointsSet[index].View;
            lastLevelPoint.First().ConnectPoints.Add(newPoint);

            return new List<InteractivePoint> { newPoint };
        }
    }
}