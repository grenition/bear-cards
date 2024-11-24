using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "LocationConfigurate", menuName = "Configs/Map/LocationConfigurate", order = 0)]
    public class LocationConfigurate : ScriptableObject
    {
        [Tooltip("Count level for location")]
        public int LocationLevel;

        public int MinimumFork;
        public int MaximumFork;
      
        public int StartResourcesMinimum;
        public int StartResourcesMaximum;
        
        public List<string> KeysEnemy;
        public List<string> KeysPrice;
        //public List<InterestingPointConfig> PricePoint;
        //public List<InterestingPointConfig> EnemyPoint;
    }
}