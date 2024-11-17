using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class TwoPath : IPointPatternCreatable
    {
        private const int _countPoint = 2;
        public List<PointOfInterestGenerator.InteresPointEntity> Create(
            ref List<PointOfInterestGenerator.InteresPointEntity> lastLevelPoint,
            List<InterestingPointConfig> pointsSet)
        {
            var addedPoints = new List<PointOfInterestGenerator.InteresPointEntity>();

            for (int i = 0; i < _countPoint; i++)
            {
                var index = Random.Range(0, pointsSet.Count);

                var newPoint = new PointOfInterestGenerator.InteresPointEntity()
                {
                    InteractivePoint = pointsSet[index].CreateInteractivePoint(),
                };

                lastLevelPoint.First().ConnectPoints.Add(newPoint.InteractivePoint);
                addedPoints.Add(newPoint);
            }
            return addedPoints;
        }
    }
}