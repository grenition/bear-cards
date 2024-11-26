using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class PointFactory
    {
        public static PointFactory Instance { get; private set; }
        private Dictionary<string, ViewPoint> _viewPointsMap = new();

        private int _uniqId;
        public PointFactory()
        {
            Instance = this;

            _viewPointsMap.Add("Start", Resources.Load<ViewPoint>("Map/Prefabs/StartPoint"));
            _viewPointsMap.Add("Enemy", Resources.Load<ViewPoint>("Map/Prefabs/EnemyPoint"));
            _viewPointsMap.Add("Card", Resources.Load<ViewPoint>("Map/Prefabs/CardPoint"));
            _viewPointsMap.Add("Hill", Resources.Load<ViewPoint>("Map/Prefabs/HillPoint"));
            _viewPointsMap.Add("Boss", Resources.Load<ViewPoint>("Map/Prefabs/BossPoint"));
        }

        public InteractivePoint CreatePoint(string key)
        {
            InteractivePoint newPoint = key switch
            {
                "Start" => new StartPoint(),
                "Enemy" => new EnemyPoint(),
                "Card" => new CardPoint(),
                "Hill" => new HillPoint(),
                "Boss" => new BossPoint(),
                _ => throw new NotImplementedException(),
            };

            newPoint.PointEntity.ID = _uniqId;
            _uniqId++;
            return newPoint;
        }

        public ViewPoint CreateViewPoint(string key)
        {
            return GameObject.Instantiate(_viewPointsMap[key]);
        }
    }
}