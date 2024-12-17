using Project;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "LocationConfigurate", menuName = "Configs/Map/LocationConfigurate", order = 0)]
    public class LocationConfigurate : ScriptableObject
    {
        [field: SerializeField] public int LocationKey { get; private set; }
        [Tooltip("Count level for location")]
        [field: SerializeField] public int LocationLevel { get; private set; }
        [Header("Fork Setting")]
        [field: SerializeField] public int MinimumFork { get; private set; }
        [field: SerializeField] public int MaximumFork { get; private set; }
        [Header("Point key in location")]
        [field: SerializeField] public List<string> KeysEnemy { get; private set; }
        [field: SerializeField] public List<string> KeysPrice { get; private set; }
        [Tooltip("first point is enemy?")]
        [field: SerializeField] public bool FirsEnemyPoint { get; private set; }
        [field: SerializeField] public string[] BattleKeys { get; private set; }
        [field: SerializeField] public string[] EnemyKeys { get; private set; }
        [field: SerializeField] public Sprite BackGround { get; private set; }
        [field: SerializeField] public string MainBossKey { get; private set; }
        [field: SerializeField] public string AdditionalBossKey { get; private set; }

        public string GetBattleKey()=>
            BattleKeys[Random.Range(0, EnemyKeys.Length)];

        public string GetEnemyKey() =>
    EnemyKeys[Random.Range(0, EnemyKeys.Length)];

        public string BossFight()
        {
            if(LocationKey == 0)
            {
                var data = DialoguesStatic.LoadData();
                if(data.CountLocationOneUpdate == 1)
                    return AdditionalBossKey;
            }


            return MainBossKey;
        }
    }
}