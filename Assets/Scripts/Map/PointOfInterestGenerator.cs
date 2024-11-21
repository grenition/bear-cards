using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class PointOfInterestGenerator
    {
        private bool _enemyLevel;

        private FactoryLevel _factoryLevel;
        private LocationConfigurate _locationConfigurate;
        private InterestingPointConfig _startPoint;
        private InterestingPointConfig _endPoint;
        public PointOfInterestGenerator(string keyLocation,
            InterestingPointConfig startPoint,
            InterestingPointConfig endPoint)
        {
            PointFactory _ = new();
            _locationConfigurate = Resources.Load<LocationConfigurate>("Map/" + keyLocation);

            _factoryLevel = new();
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        public List<List<InteresPointEntity>> Generate()
        {
            List<List<InteresPointEntity>> targetLevel = new();

            targetLevel.Add(new List<InteresPointEntity>() { new()
            {
                Level = 0,
                InteractivePoint = _startPoint.CreateInteractivePoint(),
                ConnectPoints = new()
            }});

            for (int i = 1; i < _locationConfigurate.LocationLevel - 1; i++)
            {
                var points = CreatePoints(targetLevel[i - 1], i);
                targetLevel.Add(points);
                _enemyLevel = !_enemyLevel;
            }

            targetLevel.Add(new List<InteresPointEntity>() { new()
            {
                Level = _locationConfigurate.LocationLevel,
                InteractivePoint = _endPoint.CreateInteractivePoint(),
            }});

            return targetLevel;
        }

        private List<InteresPointEntity> CreatePoints(List<InteresPointEntity> interesPointEntities, int i)
        {
            List<InteresPointEntity> levelPoints = new();
            interesPointEntities.ForEach(lastPoint =>
            {
                List<InteresPointEntity> lastPoints = new() { lastPoint };
                List<InteresPointEntity> newPoints = new();

                List<InterestingPointConfig> set = new();
                if (_enemyLevel)
                    set = _locationConfigurate.EnemyPoint;
                else
                    set = _locationConfigurate.PricePoint;

                newPoints = _factoryLevel.CreateLevel(ref lastPoints, set);
                newPoints.ForEach(point =>
                {
                    levelPoints.Add(point);
                    point.ConnectPoints = new();
                    point.Level = i;
                });
            });

            return levelPoints;
        }

        public class InteresPointEntity
        {
            public int Level;
            public InteractivePoint InteractivePoint;
            public List<InteractivePoint> ConnectPoints;
        }

        public class FactoryLevel
        {
            private Dictionary<int, IPointPatternCreatable> _patternSet = new();
            public FactoryLevel()
            {
                _patternSet.Add(0, new OnePath());
                _patternSet.Add(1, new TwoPath());
            }

            public List<InteresPointEntity> CreateLevel(ref List<InteresPointEntity> lastLevelPoint,
            List<InterestingPointConfig> pointsSet)
            {
                int numberPattern = Random.Range(0, _patternSet.Count);

                return _patternSet[numberPattern].Create(ref lastLevelPoint, pointsSet);
            }
        }
    }
}