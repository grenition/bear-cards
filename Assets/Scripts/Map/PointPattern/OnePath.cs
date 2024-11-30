using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class OnePath : IPointPatternCreatable
    {
        public List<InteractivePoint> Create(
            ref List<InteractivePoint> lastLevelPoint,
            List<string> pointsSet)
        {
            var index = Random.Range(0, pointsSet.Count);

            var newPoint = PointFactory.Instance.CreatePoint(pointsSet[index]);
            lastLevelPoint.First().PointEntity.NeighborsID.Add(newPoint.PointEntity.ID);

            Debug.Log(newPoint.PointEntity.ID);
            return new List<InteractivePoint> { newPoint };
        }
    }
}