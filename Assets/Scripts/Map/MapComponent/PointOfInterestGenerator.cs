using System.Collections.Generic;
using System.Linq;
using static Assets.Scripts.Map.LocationProgress;
using Random = System.Random;

namespace Assets.Scripts.Map
{
    public class PointOfInterestGenerator
    {
        private PointFactory _factoryLevel;
        private LocationConfigurate _locationConfigurate;

        public PointOfInterestGenerator(LocationConfigurate locationConfigurate)
        {
            _factoryLevel = new PointFactory();
            _locationConfigurate = locationConfigurate;
        }

        public List<InteractivePoint> Generate()
        {
            var locationPointList = new List<InteractivePoint>();

            foreach (var level in _locationConfigurate.Levels)
            {
                var pointKeys = level.PointKeys.ToList();

                pointKeys = ShuffleListWithOrderBy(pointKeys);
                foreach (var point in level.Points)
                {
                    var newPoint = _factoryLevel.CreatePoint(pointKeys.FirstOrDefault());
                    locationPointList.Add(PointInit(newPoint, point, pointKeys.FirstOrDefault(), level.Number));
                    pointKeys.RemoveAt(0);
                }
            }

            return locationPointList;
        }

        public List<InteractivePoint> Generate(LocationData locationData)
        {
            var locationPointList = new List<InteractivePoint>();

            foreach (var point in locationData.Points)
            {
                var newPoint = _factoryLevel.CreatePoint(point.Key);
                newPoint.PointEntity = point;
                locationPointList.Add(PointInit(newPoint, point, point.Key, point.NumberLevel));
            }

            return locationPointList;
        }

        private InteractivePoint PointInit(InteractivePoint newPoint, PointEntity point, string key, int numberLevel)
        {
            newPoint.PointEntity.ID = point.ID;
            newPoint.PointEntity.EnemyKeys = point.EnemyKeys;
            newPoint.PointEntity.NeighborsID = point.NeighborsID;
            newPoint.PointEntity.IsEnemyPoint = point.IsEnemyPoint;
            newPoint.PointEntity.Key = key;
            newPoint.PointEntity.NumberLevel = numberLevel;

            return newPoint;
        }
        private List<T> ShuffleListWithOrderBy<T>(List<T> list)
        {
            Random random = new Random();
            return list.OrderBy(x => random.Next()).ToList();
        }
    }
}