using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "CoastConfig", menuName = "Configs/Map/CoastConfig", order = 1)]
    public class PointCoastConfig : ScriptableObject
    {
        public List<Points> PointsCoastConfigurate;

        [Serializable]
        public struct Points
        {
            [Tooltip("Key of point")]
            public string Name;
            public string Description;
            [Tooltip("Points was added for create this point")]
            public int PriseForLocation;
        }
    }
}