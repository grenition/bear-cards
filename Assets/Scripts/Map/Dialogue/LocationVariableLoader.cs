using System.IO;
using UnityEngine;

namespace Project
{
    public class LocationVariableLoader
    {
        private const string _path = "/locationVariable.json";
        public void SaveVariables(LocationVariabelsData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + _path, json);
        }

        public LocationVariabelsData LoadVaribels()
        {
            string path = Application.persistentDataPath + _path;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<LocationVariabelsData>(json);
                return data;
            }

            var newData = new LocationVariabelsData();
            SaveVariables(newData);
            return newData;
        }

        public void Deleted()
        {
            string path = Application.persistentDataPath + _path;

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Varibelse was deleted");
            }
        }
    }
}
