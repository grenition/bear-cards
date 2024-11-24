using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class PointOfInterestGenerator
    {
        private FactoryLevel _factoryLevel;
        private LocationConfigurate _locationConfigurate;
        private ViewPoint _startPoint;
        private ViewPoint _endPoint;

        private bool _enemyLevel;
        private int _countFork;
        public PointOfInterestGenerator(
            ViewPoint startPoint,
            ViewPoint endPoint,
            LocationConfigurate locationConfigurate)
        {
            PointFactory _ = new();
            _locationConfigurate = locationConfigurate;
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

            var boss = PointFactory.Instance.CreatePoint("Boss");
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

                 newPoints = FactoryPath(ref lastPoints, set);

                if (lastPoint.ConnectPoints.Count > 1)
                    _countFork++;

                newPoints.ForEach(point =>
                {
                    levelPoints.Add(point);
                    point.ConnectPoints = new();
                    point.Level = level;
                });
            });

            return levelPoints;
        }

        private List<InteractivePoint> FactoryPath(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet)
        {
            if ((_countFork <= _locationConfigurate.MinimumFork))
            {
                return _factoryLevel.CreateRandomPoint(ref lastLevelPoint, pointsSet);
            }
            else if (_countFork < _locationConfigurate.MaximumFork)
            {
                return _factoryLevel.CreateOnePath(ref lastLevelPoint, pointsSet);
            }

            return _factoryLevel.CreateTwoPath(ref lastLevelPoint, pointsSet);
        }

        public class FactoryLevel
        {
            private Dictionary<int, IPointPatternCreatable> _patternSet = new();
            public FactoryLevel()
            {
                _patternSet.Add(0, new OnePath());
                _patternSet.Add(1, new TwoPath());
            }

            public List<InteractivePoint> CreateOnePath(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet) =>
                     _patternSet[0].Create(ref lastLevelPoint, pointsSet);

            public List<InteractivePoint> CreateTwoPath(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet) =>
                     _patternSet[1].Create(ref lastLevelPoint, pointsSet);

            public List<InteractivePoint> CreateRandomPoint(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet)
            {
                int numberPattern = Random.Range(0, _patternSet.Count);
                return _patternSet[numberPattern].Create(ref lastLevelPoint, pointsSet);
            }
        }
    }
}