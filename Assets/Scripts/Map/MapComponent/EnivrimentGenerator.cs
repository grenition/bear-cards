using GreonAssets.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class EnivrimentGenerator
    {
        private EnivrimentConfig _config;
        private int _levelLocation;

        public EnivrimentGenerator(int levelLocation)
        {
            _levelLocation = levelLocation;
            _config = Resources.Load<EnivrimentConfig>("Map/EnuvrimentConfig");
        }

        public void Generate(List<InteractivePoint> points)
        {
            InteractivePointGenerated(points);
            PathGenerated(points);
        }

        private void InteractivePointGenerated(List<InteractivePoint> points)
        {
            for (int numberLevel = 0; numberLevel < _levelLocation; numberLevel++)
            {
                List<InteractivePoint> pointsInLevel = points.Where(point => point.PointEntity.Level == numberLevel).ToList();

                for (int numberPointInLevel = 0; numberPointInLevel < pointsInLevel.Count(); numberPointInLevel++)
                {
                    var position = new Vector2((float)(pointsInLevel.Count() * numberPointInLevel) / pointsInLevel.Count()
                        - pointsInLevel.Count() / 2.0f,
                        (float)numberLevel * _config.DistanceBeetwenPointByY);

                    var viewObject = PointFactory.Instance.CreateViewPoint(pointsInLevel[numberPointInLevel].PointEntity.Key);
                    viewObject.transform.position = position;
                    pointsInLevel[numberPointInLevel].Initialize(viewObject);
                }
            }
        }

        private void PathGenerated(List<InteractivePoint> points)
        {            


            points.Where(point => point.PointEntity.NeighborsID != null &&
            point.PointEntity.NeighborsID.Count != 0 &&
            point.PointEntity.Key != "Boss").ForEach
                (point =>
                {
                    point.PointEntity.NeighborsID.ForEach(idNeighbor =>
                    {
                        point.ViewPoint.CreatePathTo(FindPointByID(points, idNeighbor).ViewPoint);
                    });
                });

            var bossPoint = points.Find(point => point.PointEntity.Key == "Boss");
            points.Where(point => point.PointEntity.Level == _levelLocation - 2).ForEach(point =>
            {
                point.ViewPoint.CreatePathTo(bossPoint.ViewPoint);
            });
        }


        private InteractivePoint FindPointByID(List<InteractivePoint> points, int id)
        {
            InteractivePoint findPoint = points.Find(point => point.PointEntity.ID == id);

            if (findPoint != null)
                return findPoint;

            throw new System.Exception($"Point with id:{id} is not found!");
        }
    }
}