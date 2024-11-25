using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "LocationConfigurate", menuName = "Configs/Map/LocationConfigurate", order = 0)]
    public class LocationConfigurate : ScriptableObject
    {
        [Tooltip("Count level for location")]
        [field: SerializeField] public int LocationLevel { get; private set; }

        [field: SerializeField] public int MinimumFork { get; private set; }
        [field: SerializeField] public int MaximumFork { get; private set; }

        [field: SerializeField] public List<string> KeysEnemy { get; private set; }
        [field: SerializeField] public List<string> KeysPrice { get; private set; }
        [field: SerializeField] public bool FirsEnemyPoint {  get; private set; }
    }
}