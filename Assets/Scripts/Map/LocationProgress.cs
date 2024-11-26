using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.Map
{
    public class LocationProgress
    {
        public List<List<InteractivePoint>> LoadData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "locationProgress.json");
            //if (File.Exists(path))
            //{
            //    string json = File.ReadAllText(path);
            //    var data = JsonUtility.FromJson<LocationData>(json);

            //    List<List<InteractivePoint>> locationPointCollection = new(data.LocationLevel);

            //    locationPointCollection.ForEach(level =>
            //    {
            //        for (int i = 0; i < length; i++)
            //        {

            //        }
            //        level.Add()
            //    })

            //    return data;
            //}

            return null;
        }

        public void SaveDate(List<List<InteractivePoint>> currentLevelPoint)
        {

            List<PointEntity> points = new List<PointEntity>();

            currentLevelPoint.ForEach(level =>
            {
                level.ForEach(point =>
                {
                    points.Add(point.PointEntity);
                });
            });

            LocationData locationData = new()
            {
                LocationProgress = 0,
                LocationLevel = 5,
                Points = points.ToArray()
            };

            string json = JsonUtility.ToJson(locationData);
            File.WriteAllText(Application.streamingAssetsPath + "/locationProgress.json", json);
            Debug.Log(Application.streamingAssetsPath + "/locationProgress.json");
        }

        public void DeleteData()
        {

        }

        [Serializable]
        public class LocationData
        {
            public int LocationProgress;
            public int LocationLevel;

            public PointEntity[] Points;
            //public int[] ID;
            //public int[] LevelPoints;
            //public int[] Keys;
            //public Vector2Int[] Neighbor;
        }
    }
}