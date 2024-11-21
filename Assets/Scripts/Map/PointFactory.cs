using System;
using System.Collections.Generic;

namespace Assets.Scripts.Map
{
    public class PointFactory
    {
        public static PointFactory Instance { get; private set; }
        private Dictionary<string, InteractivePoint> _pointsMap;
        public PointFactory()
        {
            Instance = this;
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
    }
}