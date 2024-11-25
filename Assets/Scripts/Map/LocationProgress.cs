using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LocationProgress
    {
        public LocationData LoadData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "locationProgress.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<LocationData>(json);
                return data;
            }

            return null;
        }

        public void SaveDate(List<List<InteractivePoint>> currentLevelPoint)
        {
            List<List<PointData>> pointData = new List<List<PointData>>();
            for (int i = 0; i < currentLevelPoint.Count(); i++)
            {
                pointData.Add(new List<PointData>());
                for (int j = 0; j < currentLevelPoint[i].Count(); j++)
                {
                    for (int z = 0; z < currentLevelPoint[i][j].ConnectPoints.Count(); z++)
                    {

                    }
                    pointData[i].Add(new PointData()
                    {
                        KeyPoints = currentLevelPoint[i][j].Key,
                        LevelPoint = currentLevelPoint[i][j].Level,
                        IdConnectPoints = new int[currentLevelPoint[i][j].ConnectPoints.Count()]
                    });
                }
            }

            //currentLevelPoint.ForEach(level =>
            //{
            //    pointData.Add(new List<PointData>());
            //    level.ForEach(point =>
            //    {

            //    });
            //});

            LocationData locationData = new LocationData()
            {
                LocationProgress = 0,
                // Points = new PointData[currentLevelPoint.Count]
            };
        }

        public void DeleteData()
        {

        }

        [Serializable]
        public class LocationData
        {
            public int LocationProgress;
            public PointData[][] Points;
        }
        [Serializable]
        public class PointData
        {
            public int[] IdConnectPoints;
            public string KeyPoints;
            public int LevelPoint;
        }
    }
}