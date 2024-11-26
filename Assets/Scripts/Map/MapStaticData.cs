using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Map
{
    public static class MapStaticData
    {
        private static LocationProgress _locationProgress;
        private static int _idBattlePoint;

        public static LocationProgress.LocationData LoadData()
        {
            Initialize();
            return _locationProgress.LoadData();
        }

        public static void SaveData(List<InteractivePoint> points,int locationLevel, int keyLocation)
        {
            Initialize();
            _locationProgress.SaveDate(points, locationLevel, keyLocation);
        }



        public static void GameFail()
        {
            Initialize();
            _locationProgress.DeleteData();
        }

        public static void LevelComplited()
        {
            Initialize();

            var locationData = LoadData();
            locationData.Points.ToList().ForEach(point =>
            {
                point.PointLock = true;
            });

            locationData.Points.ToList().Find(point => point.ID == _idBattlePoint).PointComplited = true;
            _locationProgress.SaveData(locationData.Points, locationData.LocationLevel, locationData.KeyLocation);
        }

        public static void BattlePointStart(int ID)
        {
            _idBattlePoint = ID;
        }

        private static void Initialize()
        {
            if( _locationProgress == null )
                _locationProgress = new LocationProgress();
        }
    }
}