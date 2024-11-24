using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class PointOfInterestGenerator
    {
        private bool _enemyLevel;

        private FactoryLevel _factoryLevel;
        private LocationConfigurate _locationConfigurate;
        private ViewPoint _startPoint;
        private ViewPoint _endPoint;
        public PointOfInterestGenerator(string keyLocation,
            ViewPoint startPoint,
            ViewPoint endPoint)
        {
            PointFactory _ = new();
            _locationConfigurate = Resources.Load<LocationConfigurate>("Map/" + keyLocation);

            _factoryLevel = new();
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        public List<List<InteractivePoint>> Generate()
        {
            List<List<InteractivePoint>> targetLevel = new();

            var start = PointFactory.Instance.CreatePoint("Start");
            start.ConnectPoints = new List<InteractivePoint>();
            start.Pass();

            targetLevel.Add(new List<InteractivePoint>() { start });

            for (int i = 1; i < _locationConfigurate.LocationLevel - 1; i++)
            {
                var points = CreatePoints(targetLevel[i - 1], i);
                targetLevel.Add(points);
                _enemyLevel = !_enemyLevel;
            }

            var boss = PointFactory.Instance.CreatePoint("Start");
            boss.ConnectPoints = new List<InteractivePoint>();
            boss.Level = _locationConfigurate.LocationLevel;
            targetLevel.Add(new List<InteractivePoint>() { boss });

            return targetLevel;
        }

        private List<InteractivePoint> CreatePoints(List<InteractivePoint> interesPointEntities, int level)
        {
            List<InteractivePoint> levelPoints = new();
            interesPointEntities.ForEach(lastPoint =>
            {
                List<InteractivePoint> lastPoints = new() { lastPoint };
                List<InteractivePoint> newPoints = new();

                List<string> set = new();
                if (_enemyLevel)
                    set = _locationConfigurate.KeysEnemy;
                else
                    set = _locationConfigurate.KeysPrice;

                newPoints = _factoryLevel.CreateLevel(ref lastPoints, set);
                newPoints.ForEach(point =>
                {
                    levelPoints.Add(point);
                    point.ConnectPoints = new();
                    point.Level = level;
                });
            });

            return levelPoints;
        }


        public class FactoryLevel
        {
            private Dictionary<int, IPointPatternCreatable> _patternSet = new();
            public FactoryLevel()
            {
                _patternSet.Add(0, new OnePath());
                _patternSet.Add(1, new TwoPath());
            }

            public List<InteractivePoint> CreateLevel(
                ref List<InteractivePoint> lastLevelPoint,
                List<string> pointsSet)
            {
                int numberPattern = Random.Range(0, _patternSet.Count);
                return _patternSet[numberPattern].Create(ref lastLevelPoint, pointsSet);
            }
        }
    }
}