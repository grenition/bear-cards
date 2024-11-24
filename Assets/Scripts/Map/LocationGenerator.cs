using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LocationGenerator
    {
        //private List<List<Point>> _interactablePoints = new();
        //private List<InterestingPointConfig> _interestingPointConfigs = new();

        //private int _ressources;
        //private int _levels;

        //public LocationGenerator()
        //{
        //    var locationConfig = Resources.Load<LocationConfigurate>("Map/LocationFirst");
        //    _interestingPointConfigs = Resources.LoadAll<InterestingPointConfig>("Map/PointsConfig/").ToList();
        //    _ressources = 1; // Set from config
        //    _levels = locationConfig.LocationLevel;
        //}

        //public List<List<Point>> GeneratePoint()
        //{
        //    _interactablePoints.Add(new List<Point>() { new(_ressources, new StartPoint()) });
        //    for (int i = 1; i < _levels - 1; i++)
        //    {
        //        var points = CreatePoints(_interactablePoints[i - 1], i);
        //        _interactablePoints.Add(points);
        //    }
        //    _interactablePoints.Add(new List<Point>() { new(0, new BossPoint()) });
        //    return _interactablePoints;


        //}

        //private List<Point> CreatePoints(List<Point> points, int iteration)
        //{
        //    List<Point> levelPoints = new List<Point>();
        //    points.ForEach(lastPoint =>
        //    {
        //        List<InterestingPointConfig> set = new List<InterestingPointConfig>();
        //        var resources = lastPoint.ResoursePoint;
        //        _interestingPointConfigs.ForEach(point =>
        //        {
        //            if (point.PointCoast <= resources && point.PointCoast >= 0)
        //                set.Add(point);
        //        });

        //        if (set.Count == 0)
        //        {
        //            _interestingPointConfigs.ForEach(point =>
        //            {
        //                if (point.PointCoast <= resources && point.PointCoast < 0)
        //                    set.Add(point);
        //            });
        //        }

        //        var addedPoints = new InteractivePointFactory().Create(set);
        //        addedPoints.ForEach(point =>
        //        {
        //            point.Level = iteration;
        //            point.LastPoint = lastPoint.InteractivePoint;
        //            point.ResoursePoint = resources - point.ResoursePoint;
        //            levelPoints.Add(point);
        //        });


        //    });
        //    return levelPoints;
        //}

        //public class Point
        //{
        //    public Point(int coast, InteractivePoint interactivePoint)
        //    {
        //        ResoursePoint = coast;
        //        InteractivePoint = interactivePoint;
        //    }

        //    public int Level = -2;
        //    public int ResoursePoint;
        //    public InteractivePoint InteractivePoint;
        //    public InteractivePoint LastPoint;
        //}

        //private class InteractivePointFactory
        //{
        //    public InteractivePointFactory() { }

        //    public List<Point> Create(List<InterestingPointConfig> set)
        //    {
        //        if (set.Count() > 1)
        //        {
        //            return CreateTwoPath(set);
        //        }

        //        return new List<Point>()
        //        {
        //            new Point(set[0].PointCoast, set[0].CreateInteractivePoint())
        //        };
        //    }

        //    private List<Point> CreateTwoPath(List<InterestingPointConfig> set)
        //    {
        //        var firstPoint = set[Random.Range(0, set.Count())];
        //        set.Remove(firstPoint);
        //        var secondPoint = set[Random.Range(0, set.Count())];

        //        var TwoPathPoints = new List<Point>() {
        //            new Point(firstPoint.PointCoast, firstPoint.CreateInteractivePoint()),
        //            new Point(firstPoint.PointCoast, firstPoint.CreateInteractivePoint()),
        //        };

        //        return TwoPathPoints;
        //    }
        //}
    }
}
