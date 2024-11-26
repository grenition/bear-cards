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
                KeyLocation = 0,
                LocationLevel = 0,
            };
        }

        public void SaveDate(List<InteractivePoint> currentLevelPoint, int levelLocation, int keyNumber)
        {
            List<PointEntity> pointsData = new();

            currentLevelPoint.ForEach(point =>
            {
                pointsData.Add(point.PointEntity);
            });

            LocationData locationData = new()
            {
                KeyLocation = keyNumber,
                LocationLevel = levelLocation,
                Points = pointsData.ToArray()
            };

            string json = JsonUtility.ToJson(locationData);
            File.WriteAllText(Application.streamingAssetsPath + "/locationProgress.json", json);
        }

        public void SaveData(PointEntity[] currentLevelPoint, int levelLocation, int keyNumber)
        {
            LocationData locationData = new()
            {
                KeyLocation = keyNumber,
                LocationLevel = levelLocation,
                Points = currentLevelPoint
            };

            string json = JsonUtility.ToJson(locationData);
            File.WriteAllText(Application.streamingAssetsPath + "/locationProgress.json", json);
        }

        public void DeleteData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "locationProgress.json");

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Save was deleted");
            }
        }

        [Serializable]
        public class LocationData
        {
            public int KeyLocation;
            public int LocationLevel;

            public PointEntity[] Points;
        }
    }
}