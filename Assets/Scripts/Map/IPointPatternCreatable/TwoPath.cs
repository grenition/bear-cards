using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class TwoPath : IPointPatternCreatable
    {
        private const int _countPoint = 2;
        public List<InteractivePoint> Create(
            ref List<InteractivePoint> lastLevelPoint,
            List<InterestingPointConfig> pointsSet)
        {
            var addedPoints = new List<InteractivePoint>();

            for (int i = 0; i < _countPoint; i++)
            {
                var index = Random.Range(0, pointsSet.Count);
                var newPoint = PointFactory.Instance.CreatePoint(pointsSet[index].Name);
                newPoint.View = pointsSet[index].View;

                lastLevelPoint.First().ConnectPoints.Add(newPoint);
                addedPoints.Add(newPoint);
            }
            return addedPoints;
        }
    }
}