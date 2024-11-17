using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class OnePath : IPointPatternCreatable
    {
        public List<PointOfInterestGenerator.InteresPointEntity> Create(
            ref List<PointOfInterestGenerator.InteresPointEntity> lastLevelPoint,
            List<InterestingPointConfig> pointsSet)
        {
            var index = Random.Range(0, pointsSet.Count);
            var newPoint = new PointOfInterestGenerator.InteresPointEntity()
            {
                InteractivePoint = pointsSet[index].CreateInteractivePoint(),
            };

            lastLevelPoint.First().ConnectPoints.Add(newPoint.InteractivePoint);

            return new List<PointOfInterestGenerator.InteresPointEntity>() { newPoint };
        }
    }
}