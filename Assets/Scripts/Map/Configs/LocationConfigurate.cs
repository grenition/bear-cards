using Project;
using System;
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

        //[Header("Fork Setting")]
        //[field: SerializeField] public int MinimumFork { get; private set; }
        //[field: SerializeField] public int MaximumFork { get; private set; }
        //[Header("Point key in location")]
        //[field: SerializeField] public List<string> KeysEnemy { get; private set; }
        //[field: SerializeField] public List<string> KeysPrice { get; private set; }
        //[Tooltip("first point is enemy?")]
        //[field: SerializeField] public bool FirsEnemyPoint { get; private set; }
        [field: SerializeField] public string[] BattleKeys { get; private set; }
        //[field: SerializeField] public string[] EnemyEasyKeys { get; private set; }
        //[field: SerializeField] public string[] EnemyEpicKeys { get; private set; }
        //[field: SerializeField] public string[] EnemyLegendKeys { get; private set; }
        [field: SerializeField] public Sprite BackGround { get; private set; }
        [field: SerializeField] public string MainBossKey { get; private set; }
        [field: SerializeField] public string AdditionalBossKey { get; private set; }

        public string GetBattleKey() =>
            BattleKeys[Random.Range(0, BattleKeys.Length)];

        //public string GetEnemyKey(string keyEnemy)
        //{
        //    return keyEnemy switch
        //    {
        //        "EnemyEasy" => EnemyEasyKeys[Random.Range(0, EnemyEasyKeys.Length)],
        //        "EnemyMeadle" => EnemyEpicKeys[Random.Range(0, EnemyEpicKeys.Length)],
        //        "EnemyLegend" => EnemyLegendKeys[Random.Range(0, EnemyLegendKeys.Length)],
        //        _ => EnemyEasyKeys[Random.Range(0, EnemyEasyKeys.Length)],
        //    };
        //}
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