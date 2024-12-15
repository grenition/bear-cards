using System;
using System.IO;
using UnityEngine;

namespace Project
{
    public class PlayerStatsData
    {
        public void SaveStat(int HitPoint)
        {
            PlayerData locationData = new()
            {
                HitPoint = HitPoint,
            };

            string json = JsonUtility.ToJson(locationData);
            File.WriteAllText(Application.persistentDataPath + "/playerStatData.json", json);
        }

        public int LoadStat()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStatData.json");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<PlayerData>(json);
                return data.HitPoint;
            }

            return 20;
        }

        public void DeletedStat()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStatData.json");

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Save was deleted");
            }
        }


        [Serializable]
        public class PlayerData
        {
            public int HitPoint;
        }
    }
}
