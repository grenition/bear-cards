using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class PointFactory
    {
        public static PointFactory Instance { get; private set; }
        private Dictionary<string, ViewPoint> _viewPointsMap = new();
        public PointFactory()
        {
            Instance = this;

            _viewPointsMap.Add("Start", Resources.Load<ViewPoint>("Map/Prefabs/StartPoint"));
            _viewPointsMap.Add("Enemy", Resources.Load<ViewPoint>("Map/Prefabs/EnemyPoint"));
            _viewPointsMap.Add("Card", Resources.Load<ViewPoint>("Map/Prefabs/CardPoint"));
            _viewPointsMap.Add("Hill", Resources.Load<ViewPoint>("Map/Prefabs/HillPoint"));
            _viewPointsMap.Add("Boss", Resources.Load<ViewPoint>("Map/Prefabs/BossPoint"));
        }

        public InteractivePoint CreatePoint(string name)
        {
            switch (name)
            {
                case "Start":
                    return new StartPoint();
                case "Enemy":
                    return new EnemyPoint();
                case "Card":
                    return new CardPoint();
                case "Hill":
                    return new HillPoint();
                case "Boss":
                    return new HillPoint();
                default:
                    throw new NotImplementedException();
            }
        }

        public ViewPoint CreateViewPoint(string key)
        {
            return GameObject.Instantiate(_viewPointsMap[key]);
        }
    }
}