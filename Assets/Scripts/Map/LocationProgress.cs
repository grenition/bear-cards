using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LocationProgress
    {
        private string[] _startDeck;

        public void SetStartDeck(string[] startDeck)
        {
            _startDeck = startDeck;
        }

        public LocationData LoadData(IEnumerable<string> startDeck = null)
        {
            string path = Application.persistentDataPath + "/locationProgress.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<LocationData>(json);
                return data;
            }
            else
            {
                var Deck = startDeck != null ? startDeck.ToArray() : _startDeck;
                LocationData locationData = new()
                {
                    KeyLocation = 0,
                    LocationLevel = 0,
                    Points = new PointEntity[0],
                    Deck = Deck
                };

                string json = JsonUtility.ToJson(locationData);
                File.WriteAllText(Application.persistentDataPath + "/locationProgress.json", json);

                return locationData;
            }
        }

        public void SaveData(PointEntity[] currentLevelPoint, int levelLocation, int keyNumber, List<string> deck)
        {
            LocationData locationData = new()
            {
                KeyLocation = keyNumber,
                LocationLevel = levelLocation,
                Points = currentLevelPoint,
                Deck = deck.ToArray()
            };

            string json = JsonUtility.ToJson(locationData);
            File.WriteAllText(Application.persistentDataPath + "/locationProgress.json", json);
        }

        public void SaveData(PointEntity[] currentLevelPoint, int levelLocation, int keyNumber)
        {
            string path = Path.Combine(Application.persistentDataPath, "locationProgress.json");
            string[] deck = new string[] { };

            if (File.Exists(path))
            {
                string loadjson = File.ReadAllText(path);
                var data = JsonUtility.FromJson<LocationData>(loadjson);

                deck = data.Deck;
            }

            LocationData locationData = new()
            {
                KeyLocation = keyNumber,
                LocationLevel = levelLocation,
                Points = currentLevelPoint,
                Deck = deck
            };

            string json = JsonUtility.ToJson(locationData);
            File.WriteAllText(Application.persistentDataPath + "/locationProgress.json", json);
        }

        public void SaveDeck(List<string> deck)
        {
            string path = Path.Combine(Application.persistentDataPath, "locationProgress.json");

            if (File.Exists(path))
            {
                string loadjson = File.ReadAllText(path);
                var data = JsonUtility.FromJson<LocationData>(loadjson);

                var newDeck = data.Deck.ToList();
                newDeck.AddRange(deck);

                LocationData locationData = new()
                {
                    KeyLocation = data.KeyLocation,
                    LocationLevel = data.LocationLevel,
                    Points = data.Points,
                    Deck = newDeck.ToArray()
                };
                string json = JsonUtility.ToJson(locationData);
                File.WriteAllText(Application.persistentDataPath + "/locationProgress.json", json);
            }
        }

        public void DeleteData()
        {
            string path = Path.Combine(Application.persistentDataPath, "locationProgress.json");

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
            public string[] Deck;
        }
    }
}