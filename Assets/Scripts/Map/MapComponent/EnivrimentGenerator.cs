using GreonAssets.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class EnivrimentGenerator
    {
        private List<InteractivePoint> _points = new();
        private EnivrimentConfig _config;
        private int _levelLocation;
        public EnivrimentGenerator(List<InteractivePoint> points, int levelLocation)
        {
            _points = points;
            _levelLocation = levelLocation;
            _config = Resources.Load<EnivrimentConfig>("Map/EnuvrimentConfig");
        }

        public void Generate()
        {
            InteractivePointGenerated();
            PathGenerated();
        }

        private void InteractivePointGenerated()
        {
            for (int numberLevel = 0; numberLevel < _levelLocation; numberLevel++)
            {
                List<InteractivePoint> pointsInLevel = _points.Where(point => point.PointEntity.Level == numberLevel).ToList();

                for (int numberPointInLevel = 0; numberPointInLevel < pointsInLevel.Count(); numberPointInLevel++)
                {
                    var position = new Vector2((float)(pointsInLevel.Count() * numberPointInLevel) /pointsInLevel.Count()
                        - pointsInLevel.Count() / 2.0f,
                        (float)numberLevel * _config.DistanceBeetwenPointByY);

                    var viewObject = PointFactory.Instance.CreateViewPoint(pointsInLevel[numberPointInLevel].PointEntity.Key);
                    viewObject.transform.position = position;
                    pointsInLevel[numberPointInLevel].Initialize(viewObject);
                }
            }
        }

        private void PathGenerated()
        {
            _points.Where(point => point.PointEntity.NeighborsID != null && point.PointEntity.NeighborsID.Count != 0).ForEach
                (point =>
                {
                    point.PointEntity.NeighborsID.ForEach(idNeighbor =>
                    {
                        point.ViewPoint.CreatePathTo(FindPointByID(idNeighbor).ViewPoint);
                    });
                });

            var bossPoint = _points.Find(point => point.PointEntity.Key == "Boss");
            _points.Where(point => point.PointEntity.Level == _levelLocation - 2).ForEach(point =>
            {
                point.ViewPoint.CreatePathTo(bossPoint.ViewPoint);
            });
        }


        private InteractivePoint FindPointByID(int id)
        {
            InteractivePoint findPoint = _points.Find(point => point.PointEntity.ID == id);

            if (findPoint != null)
                return findPoint;

            throw new System.Exception($"Point with id:{id} is not found!");
        }
    }
}