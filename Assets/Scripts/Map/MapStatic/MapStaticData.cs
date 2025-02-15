using Cysharp.Threading.Tasks;
using Project;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map
{
    public static class MapStaticData
    {
        public static string KeyBattle { get; private set; }
        public static string KeyEnemy { get; private set; }


        private static LocationProgress _locationProgress;
        private static PlayerStatsData _playerData;

        private static int _idBattlePoint;

        public static void SetStartDeck(string[] startDeck)
        {
            Initialize();
            _locationProgress.SetStartDeck(startDeck);
        }

        public static LocationProgress.LocationData LoadData(IEnumerable<string> startDeck = null)
        {
            Initialize();
            return _locationProgress.LoadData(startDeck);
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

        public static void SetDeckAndSave(List<string> deck)
        {
            Initialize();
            _locationProgress.SaveDeck(deck, false);
        }
        public static void AddToDeckAndSave(List<string> deck)
        {
            Initialize();
            _locationProgress.SaveDeck(deck, true);
        }

        public async static void GameWin()
        {
            Initialize();

            var data = DialoguesStatic.LoadData();
            data.GameWinCount++;
            DialoguesStatic.SaveDataAndExecuteDialogue(data);

            await UniTask.WaitUntil(() => !DialoguesStatic.DialogueStatus());

            _locationProgress.DeleteData();
            _playerData.DeletedStat();

            SceneManager.LoadScene(0);
        }

        public static void GameFail()
        {
            Initialize();

            _locationProgress.DeleteData();
            _playerData.DeletedStat();
        }

        public static void LevelComplited(IEnumerable<string> deck)
        {
            Initialize();

            var locationData = LoadData(deck);
            locationData.Points.ToList().ForEach(point =>
            {
                point.PointLock = true;
            });

            locationData.Points.ToList().Find(point => point.ID == _idBattlePoint).PointComplited = true;
            _locationProgress.SaveData(locationData.Points, locationData.LocationLevel, locationData.KeyLocation);
        }

        public static void BattlePointStart(int ID, string keyBattle, string keyEnemy)
        {
            _idBattlePoint = ID;
            KeyBattle = keyBattle;
            KeyEnemy = keyEnemy;
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