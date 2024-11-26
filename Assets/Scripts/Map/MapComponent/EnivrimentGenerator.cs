using GreonAssets.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class EnivrimentGenerator
    {
        private List<List<InteractivePoint>> _points = new();
        private EnivrimentConfig _config;
        public EnivrimentGenerator(List<List<InteractivePoint>> points)
        {
            _points = points;
            _config = Resources.Load<EnivrimentConfig>("Map/EnuvrimentConfig");
        }

        public void Generate()
        {
            _points.ForEach(level =>
            {
                for (int i = 0; i < level.Count; i++)
                {

                    var position = new Vector2((float)(level.Count * i) / (float)level.Count - (float)level.Count / 2.0f,
                        level[i].PointEntity.Level * _config.DistanceBeetwenPointByY);

                    var viewObject = PointFactory.Instance.CreateViewPoint(level[i].Key);
                    viewObject.transform.position = position;
                    level[i].Initialize(viewObject);
                }
            });

            _points.ForEach(level =>
            {
                level.Where(point => point.PointEntity.NeighborsID != null && point.PointEntity.NeighborsID.Count != 0).ForEach(pointOfLevel =>
                {
                    pointOfLevel.PointEntity.NeighborsID.ForEach(point =>
                    {
                        pointOfLevel.ViewPoint.CreatePathTo(FindPointByID(point).ViewPoint);
                    });
                });
            });

            _points[_points.Count - 2].ForEach(lastpoint =>
            {
                lastpoint.ViewPoint.CreatePathTo(_points[_points.Count - 1].First().ViewPoint);
            });
        }

        private InteractivePoint FindPointByID(int id)
        {
            InteractivePoint findPoint = null;
            _points.ForEach(level =>
            {
                level.ForEach(point =>
                {
                    if (point.PointEntity.ID == id)
                        findPoint = point;
                });
            });

            if (findPoint != null)
                return findPoint;

            throw new System.Exception($"Point with id:{id} is not found!");
        }
    }
}