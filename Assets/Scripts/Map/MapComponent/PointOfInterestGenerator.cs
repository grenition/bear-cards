using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Map.LocationProgress;
using Random = System.Random;

namespace Assets.Scripts.Map
{
    public class PointOfInterestGenerator
    {
        private PointFactory _factoryLevel;
        private LocationConfigurate _locationConfigurate;
        //private ViewPoint _startPoint;
        //private ViewPoint _endPoint;

        //private bool _enemyLevel;
        //private int _countFork;

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
            //newPoint.PointEntity.PointComplited = point.PointComplited;
            newPoint.PointEntity.Key = key;
            newPoint.PointEntity.NumberLevel = numberLevel;

            return newPoint;
        }
        private List<T> ShuffleListWithOrderBy<T>(List<T> list)
        {
            Random random = new Random();
            return list.OrderBy(x => random.Next()).ToList();
        }

        //public PointOfInterestGenerator(
        //    ViewPoint startPoint,
        //    ViewPoint endPoint,
        //    LocationConfigurate locationConfigurate)
        //{
        //    PointFactory _ = new();
        //    _locationConfigurate = locationConfigurate;
        //    //_factoryLevel = new();
        //    _startPoint = startPoint;
        //    _endPoint = endPoint;

        //   // _enemyLevel = _locationConfigurate.FirsEnemyPoint;
        //}

        //public List<InteractivePoint> Generate()
        //{
        //    List<InteractivePoint> targetLevel = new();

        //    var start = PointFactory.Instance.CreatePoint("Start");
        //    start.PointEntity.NeighborsID = new List<int>();
        //    start.PointEntity.Level = 0;
        //    start.Complited();
        //    targetLevel.Add(start);

        //    for (int i = 1; i < _locationConfigurate.LocationLevel - 1; i++)
        //    {
        //        List<InteractivePoint> lastLevelPoints = new List<InteractivePoint>();
        //        targetLevel.Where(point => point.PointEntity.Level == i - 1).ForEach(point =>
        //        {
        //            lastLevelPoints.Add(point);
        //        });

        //        var newPoints = CreatePoints(lastLevelPoints, i);
        //        newPoints.ForEach(addedPoint =>
        //        {
        //            targetLevel.Add(addedPoint);
        //        });

        //        _enemyLevel = !_enemyLevel;
        //    }

        //    var boss = PointFactory.Instance.CreatePoint("Boss");
        //    boss.PointEntity.NeighborsID = new List<int>();
        //    boss.PointEntity.Level = _locationConfigurate.LocationLevel - 1;
        //    targetLevel.Add(boss);

        //    return targetLevel;
        //}

        //public List<InteractivePoint> Generate(List<PointEntity> loadPoints)
        //{
        //    List<InteractivePoint> generatedPoints = new();
        //    loadPoints.ForEach(point =>
        //    {
        //        var newPoint = PointFactory.Instance.CreatePoint(point.Key);
        //        newPoint.PointEntity = point;
        //        generatedPoints.Add(newPoint);
        //    });

        //    return generatedPoints;
        //}

        //private List<InteractivePoint> CreatePoints(List<InteractivePoint> interesPointEntities, int level)
        //{
        //    List<InteractivePoint> levelPoints = new();
        //    interesPointEntities.ForEach(lastPoint =>
        //    {
        //        List<InteractivePoint> lastPoints = new() { lastPoint };
        //        List<InteractivePoint> newPoints = new();
        //        List<string> set = new();

        //        if (_enemyLevel)
        //            set = _locationConfigurate.KeysEnemy;
        //        else
        //            set = _locationConfigurate.KeysPrice;

        //        newPoints = FactoryPath(ref lastPoints, set);

        //        if (lastPoint.PointEntity.NeighborsID.Count > 1)
        //            _countFork++;

        //        newPoints.ForEach(point =>
        //        {
        //            levelPoints.Add(point);
        //            point.PointEntity.NeighborsID = new();
        //            point.PointEntity.Level = level;
        //        });
        //    });

        //    return levelPoints;
        //}

        //private List<InteractivePoint> FactoryPath(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet)
        //{
        //    if ((_countFork <= _locationConfigurate.MinimumFork))
        //    {
        //        return _factoryLevel.CreateRandomPoint(ref lastLevelPoint, pointsSet);
        //    }
        //    else if (_countFork < _locationConfigurate.MaximumFork)
        //    {
        //        return _factoryLevel.CreateOnePath(ref lastLevelPoint, pointsSet);
        //    }

        //    return _factoryLevel.CreateTwoPath(ref lastLevelPoint, pointsSet);
        //}

        //public class FactoryLevel
        //{
        //    private Dictionary<int, IPointPatternCreatable> _patternSet = new();
        //    public FactoryLevel()
        //    {
        //        _patternSet.Add(0, new OnePath());
        //        _patternSet.Add(1, new TwoPath());
        //    }

        //    public List<InteractivePoint> CreateOnePath(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet) =>
        //             _patternSet[0].Create(ref lastLevelPoint, pointsSet);

        //    public List<InteractivePoint> CreateTwoPath(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet) =>
        //             _patternSet[1].Create(ref lastLevelPoint, pointsSet);

        //    public List<InteractivePoint> CreateRandomPoint(ref List<InteractivePoint> lastLevelPoint, List<string> pointsSet)
        //    {
        //        int numberPattern = UnityEngine.Random.Range(0, _patternSet.Count);
        //        return _patternSet[numberPattern].Create(ref lastLevelPoint, pointsSet);
        //    }
        //}
    }
}