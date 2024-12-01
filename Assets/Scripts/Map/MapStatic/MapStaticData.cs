using Project;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Map
{
    public static class MapStaticData
    {
        public static string KeyBattle { get; private set; }

        private static LocationProgress _locationProgress;
        private static PlayerStatsData _playerData;

        private static int _idBattlePoint;

        public static LocationProgress.LocationData LoadData()
        {
            Initialize();
            return _locationProgress.LoadData();
        }

        public static void SaveData(PointEntity[] points, int locationLevel, int keyLocation, List<string> deck)
        {
            Initialize();
            _locationProgress.SaveData(points, locationLevel, keyLocation, deck);
        }

        public static void SaveData(PointEntity[] points, int locationLevel, int keyLocation)
        {
            Initialize();
            _locationProgress.SaveData(points, locationLevel, keyLocation);
        }

        public static void SaveDeck(List<string> deck)
        {
            Initialize();
            _locationProgress.SaveDeck(deck);
        }

        public static void GameFail()
        {
            Initialize();
            _locationProgress.DeleteData();
            _playerData.DeletedStat();
        }

        public static void LevelComplited()
        {
            Initialize();

            var locationData = LoadData();
            locationData.Points.ToList().ForEach(point =>
            {
                point.PointLock = true;
                point.PointComplited = false;
            });

            locationData.Points.ToList().Find(point => point.ID == _idBattlePoint).PointComplited = true;
            _locationProgress.SaveData(locationData.Points, locationData.LocationLevel, locationData.KeyLocation);
        }

        public static void BattlePointStart(int ID, string keyBattle)
        {
            _idBattlePoint = ID;
            KeyBattle = keyBattle;
        }


        public static int LoadPlayerData()
        {
            Initialize();
            return _playerData.LoadStat();
        }

        public static void SavePlayerData(int hitPoint)
        {
            Initialize();
            _playerData.SaveStat(hitPoint);
        }

        public static void DeletedPlayerData()
        {
            Initialize();
            _playerData.DeletedStat();
        }

        private static void Initialize()
        {
            _locationProgress ??= new LocationProgress();
            _playerData ??= new PlayerStatsData();
        }
    }
}