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
                List<InteractivePoint> pointsInLevel = points.Where(point => point.PointEntity.NumberLevel == numberLevel).ToList();

                float halfDistance = (pointsInLevel.Count() - 1) * _config.DistanceBeetwenPointByX / 2.0f;


                for (int numberPointInLevel = 0; numberPointInLevel < pointsInLevel.Count(); numberPointInLevel++)
                {
                    var position = new Vector2(
        (numberPointInLevel * _config.DistanceBeetwenPointByX) - halfDistance,
        numberLevel * _config.DistanceBeetwenPointByY
    );

                    var viewObject = PointFactory.Instance.CreateViewPoint(pointsInLevel[numberPointInLevel].PointEntity.Key);
                    viewObject.transform.position = position;
                    pointsInLevel[numberPointInLevel].Initialize(viewObject);
                }
            }
        }

        private void PathGenerated(List<InteractivePoint> points)
        {
            var pointCollection = points.Where(point => point.PointEntity.NeighborsID != null &&
            point.PointEntity.NeighborsID.Count != 0 &&
            point.PointEntity.Key != "Boss");

            foreach (var point in pointCollection)
            {
                point.PointEntity.NeighborsID.ForEach(idNeighbor =>
                {
                    point.ViewPoint.CreatePathTo(FindPointByID(points, idNeighbor).ViewPoint);
                });
            };

            var bossPoint = points.Find(point => point.PointEntity.Key == "Boss");
            pointCollection = points.Where(point => point.PointEntity.NumberLevel == _levelLocation - 2);
            foreach (var point in pointCollection)
            {
                point.ViewPoint.CreatePathTo(bossPoint.ViewPoint);
            };
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