using Project;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Codice.Client.BaseCommands.ProgressStatus;

namespace Assets.Scripts.Map
{
    public class MapCompositionRoot : MonoBehaviour
    {
        public static MapCompositionRoot Instance { get; private set; }
        [field: SerializeField] public MapController MapController { get; private set; }
        [field: SerializeField] public MapCamera MapCamera { get; private set; }
        [field: SerializeField] public MapUI MapUI { get; private set; }
        [field: SerializeField] public int HitPoint{ get; private set; }
        public MapPlayer MapPlayer { get; private set; }

        private List<string> _deck = new List<string>() { "card_phosphorus" };

        [SerializeField] private ViewPoint _startPoint;
        [SerializeField] private ViewPoint _endPoint;
        [SerializeField] private MapPlayer _playerPrefab;
        [SerializeField] private GameObject _cardGiverUIEasy;
        [SerializeField] private GameObject _cardGiverUIMeadle;
        [SerializeField] private GameObject _cardGiverUIStrong;
        [SerializeField] private GameObject _craftGiverUI;
        [SerializeField] private SpriteRenderer _backGround;
        [SerializeField] private UIProgress _progressUI;

        private List<InteractivePoint> _locationPoints;
        private LocationConfigurate _activeLocation;
        private string[] _locationKey = { "LocationFirst", "LocationSecond", "LocationThreed" };
        private PointOfInterestGenerator _pointOfInterestGenerator;
        private EnivrimentGenerator _enivrimentGenerator;
        private int _curentLocationNumber;
        private HillUI _hillPanel;

        private void Awake()
        {
            Instance = this;

            var progres = MapStaticData.LoadData();
            HitPoint = MapStaticData.LoadPlayerData();

            MapUI.Initialize();
            _hillPanel = MapUI.GetUIByKey("hill") as HillUI;
            _hillPanel.OnModificateHP += SavePlayerStat;

            _deck = progres.Deck.ToList();
            _curentLocationNumber = progres.KeyLocation;
            _activeLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[_curentLocationNumber]}");
            _pointOfInterestGenerator = new PointOfInterestGenerator(_startPoint, _endPoint, _activeLocation);
            _enivrimentGenerator = new EnivrimentGenerator(_activeLocation.LocationLevel);

            if (progres.LocationLevel == 0)
                _locationPoints = _pointOfInterestGenerator.Generate();
            else
                _locationPoints = _pointOfInterestGenerator.Generate(progres.Points.ToList());

            MapStaticData.SaveData(_locationPoints.Select(point => point.PointEntity).ToArray(), _activeLocation.LocationLevel, _activeLocation.LocationKey, _deck);
            _enivrimentGenerator.Generate(_locationPoints);
            MapPlayer = Instantiate(_playerPrefab, _locationPoints[0].ViewPoint.transform.position, Quaternion.identity);
            MapController.Create(_locationPoints, _activeLocation);

            MapController.OnPointBeginInteract +=() => _progressUI.Hide();
            MapController.OnMapProgressUpdate += ProgressInit;
            MapCamera.MoveCameraToPlayer();

            ProgressInit();
            _backGround.sprite = _activeLocation.BackGround;
        }

        public void ShowCardGiver(string power)
        {
            switch (power)
            {
                case "easy":
                    _cardGiverUIEasy.SetActive(true);
                    break;
                case "meadle":
                    _cardGiverUIMeadle.SetActive(true);
                    break;
                case "strong":
                    _cardGiverUIStrong.SetActive(true);
                    break;
            }
        }

        public void ShowCraftGiver() => _craftGiverUI.SetActive(true);
        
        public int GetNextLocationKey()
        {
            if(_curentLocationNumber++ >= _locationKey.Length)
            {
                //Game win
                _activeLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[_curentLocationNumber]}");
                return _activeLocation.LocationKey;
            }

            _activeLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[_curentLocationNumber++]}");
            return _activeLocation.LocationKey; 
        }

        public void ReloadMap()
        {
            SceneManager.LoadScene(2);
        }

        public void SavePlayerStat(int modificator)
        {
            HitPoint += modificator;
            _progressUI.UpdateHitPoint(HitPoint);
            MapStaticData.SavePlayerData(HitPoint);
        }

        private void ProgressInit()
        {
            _progressUI.gameObject.SetActive(true);

            var progres = MapStaticData.LoadData();
            HitPoint = MapStaticData.LoadPlayerData();

            _progressUI.UpdateLocation(progres.KeyLocation);
            _progressUI.UpdateHitPoint(HitPoint);
            _progressUI.UpdateCardElement(progres.Deck.Where(card => card.StartsWith("card")).Count());
            _progressUI.UpdateCardMajesty(progres.Deck.Where(card => card.StartsWith("spell")).Count());
        }

        private void OnDisable()
        {
            _hillPanel.OnModificateHP -= SavePlayerStat;
            MapController.OnMapProgressUpdate -= ProgressInit;
            MapController.OnPointBeginInteract -= () => _progressUI.Hide();

            Instance = null;
            MapController = null;

            _locationPoints.ForEach(point =>
            {
                point.Dispose();
            });
        }
    }
}
