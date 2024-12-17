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
        [field: SerializeField] public string[] EnemyFight { get; private set; }
        [field: SerializeField] public Sprite BackGround { get; private set; }
        [field: SerializeField] public string MainBossKey { get; private set; }
        [field: SerializeField] public string AdditionalBossKey { get; private set; }

        [field: SerializeField] public string ComandToBossFight { get; private set; }

        public string GetFightKey()=>
            EnemyFight[Random.Range(0, EnemyFight.Length)];

        public string BossFight()
        {
            var condition = DialoguesStatic.CreateCondition(ComandToBossFight);

            if(condition.GetResult())
                return AdditionalBossKey;

            return MainBossKey;
        }
    }
}