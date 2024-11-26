using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.Map
{
    public class LocationProgress
    {
        public List<InteractivePoint> LoadData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "locationProgress.json");

            return null;
        }

        public void SaveDate(List<InteractivePoint> currentLevelPoint)
        {

            List<PointEntity> pointsData = new List<PointEntity>();

            currentLevelPoint.ForEach(point =>
            {
                pointsData.Add(point.PointEntity);
            });

            LocationData locationData = new()
            {
                LocationProgress = 0,
                LocationLevel = 5,
                Points = pointsData.ToArray()
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
        }
    }
}