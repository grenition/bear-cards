using System;
using System.Collections.Generic;
using System.IO;
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

            return new LocationData()
            {
                LocationProgress = 0,
                LocationLevel = 0,
            };
        }

        public void SaveDate(List<InteractivePoint> currentLevelPoint, int levelLocation, int numberLocation)
        {
            List<PointEntity> pointsData = new();

            currentLevelPoint.ForEach(point =>
            {
                pointsData.Add(point.PointEntity);
            });

            LocationData locationData = new()
            {
                LocationProgress = numberLocation,
                LocationLevel = levelLocation,
                Points = pointsData.ToArray()
            };

            string json = JsonUtility.ToJson(locationData);
            File.WriteAllText(Application.streamingAssetsPath + "/locationProgress.json", json);
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
        }
    }
}