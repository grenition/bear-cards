using Project;
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
            _viewPointsMap.Add("EnemyEasy", Resources.Load<ViewPoint>("Map/Prefabs/EnemyEasyPoint"));
            _viewPointsMap.Add("EnemyMeadle", Resources.Load<ViewPoint>("Map/Prefabs/EnemyEpicPoint"));
            _viewPointsMap.Add("EnemyLegend", Resources.Load<ViewPoint>("Map/Prefabs/EnemyLegendPoint"));
            _viewPointsMap.Add("CardEasy", Resources.Load<ViewPoint>("Map/Prefabs/CardPointEasy"));
            _viewPointsMap.Add("CardMeadle", Resources.Load<ViewPoint>("Map/Prefabs/CraftPointMeadle"));
            _viewPointsMap.Add("CardLegend", Resources.Load<ViewPoint>("Map/Prefabs/CraftPointLegend"));
            _viewPointsMap.Add("CraftMeadle", Resources.Load<ViewPoint>("Map/Prefabs/CraftPointMeadle"));
            _viewPointsMap.Add("HillEasy", Resources.Load<ViewPoint>("Map/Prefabs/HillPointEasy"));
            _viewPointsMap.Add("HillLegend", Resources.Load<ViewPoint>("Map/Prefabs/HillPointLegend"));
            _viewPointsMap.Add("ReceptMeadle", Resources.Load<ViewPoint>("Map/Prefabs/ReceptMeadlePoint"));
            _viewPointsMap.Add("ReceptEpic", Resources.Load<ViewPoint>("Map/Prefabs/ReceptEpicPoint"));
            _viewPointsMap.Add("Boss", Resources.Load<ViewPoint>("Map/Prefabs/BossPoint"));
        }

        public InteractivePoint CreatePoint(string key)
        {
            InteractivePoint newPoint = key switch
            {
                "Start" => new StartPoint(),
                "EnemyEasy" => new EnemyEasyPoint(),
                "EnemyMeadle" => new EnemyEpicPoint(),
                "EnemyLegend" => new EnemyPointLegend(),
                "CardEasy" => new CardPoint(),
                "CardMeadle" => new CardMeadlePoint(),
                "CardLegend" => new CardLegendPoint(),
                "HillEasy" => new HillPoint(),
                "HillLegend" => new HillLegendPoint(),
                "CraftMeadle" => new CraftPoint(),
                "ReceptMeadle" => new ReceptMeadlePoint(),
                "ReceptEpic" => new ReceptEpicPoint(),
                "Boss" => new BossPoint(),
                _ => throw new NotImplementedException($"{key} not found"),
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