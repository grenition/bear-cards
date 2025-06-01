using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "LocationConfigurate", menuName = "Configs/Map/LocationConfigurate", order = 0)]
    public class LocationConfigurate : ScriptableObject
    {
        [field: SerializeField] public int LocationKey { get; private set; }
        [Tooltip("Count level for location")]
        public int LocationLevel => Levels.Length;

        [field: SerializeField] public Level[] Levels { get; private set; }
        [field: SerializeField] public string[] BattleKeys { get; private set; }
        [field: SerializeField] public Sprite BackGround { get; private set; }
        [field: SerializeField] public string MainBossKey { get; private set; }
        [field: SerializeField] public string TutorialKey { get; private set; }
        [field: SerializeField] public string AdditionalBossKey { get; private set; }

        public string GetBattleKey() =>
            BattleKeys[Random.Range(0, BattleKeys.Length)];

        public string BossFight()
        {
            if (LocationKey == 0)
            {
                var data = DialoguesStatic.LoadData();
                if (data.CountLocationOneUpdate == 1)
                    return AdditionalBossKey;
            }


            return MainBossKey;
        }

        [ContextMenu("GenerateIDPoint")]
        public void GenerateIDPoint()
        {
            GenerateNumber();
            GenerateID();
            GenerateNaighborID();
        }

        private void GenerateID()
        {
            int id = 0;
            foreach (var level in Levels)
            {
                foreach (var point in level.Points)
                {
                    point.ID = id;
                    id++;
                }
            }
        }

        private void GenerateNumber()
        {
            int number = 0;
            foreach (var level in Levels)
            {

                level.Number = number;
                number++;
            }
        }

        private void GenerateNaighborID()
        {
            int numberGenerableLevel = 0;

            while (numberGenerableLevel + 1 < Levels.Length)
            {
                var currentLevel = Levels[numberGenerableLevel];
                var nextLevel = Levels[numberGenerableLevel + 1];

                List<int> usedId = new();

                foreach (var point in currentLevel.Points)
                {
                    var unusedNeighborsID = nextLevel.Points
                        .Where(x => !usedId.Contains(x.ID))
                        .Select(x => x.ID);

                    point.NeighborsID = CalculatePointNaigbor(unusedNeighborsID, nextLevel.Points.Length).ToList();

                    if (currentLevel.Points.Length < nextLevel.Points.Length)
                        usedId.AddRange(point.NeighborsID);
                }

                Levels[numberGenerableLevel] = currentLevel;
                numberGenerableLevel++;
            }
        }

        private IEnumerable<int> CalculatePointNaigbor(IEnumerable<int> idNextPoints, int countPointInCurrentLevel)
        {
            for (int i = 0; i < idNextPoints.Count() / countPointInCurrentLevel; i++)
            {
                yield return idNextPoints.ElementAt(i);
            }
        }
    }

    [Serializable]
    public class Level
    {
        public string Name;
        public int Number;
        public PointEntity[] Points;
        public string[] PointKeys;
    }
}