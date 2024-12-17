using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using GreonAssets.UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Map
{
    public class MapCompositionRoot : MonoBehaviour
    {
        public static MapCompositionRoot Instance { get; private set; }
        [field: SerializeField] public MapController MapController { get; private set; }
        [field: SerializeField] public DialogueController DialogueController { get; private set; }
        [field: SerializeField] public MapCamera MapCamera { get; private set; }
        [field: SerializeField] public MapUI MapUI { get; private set; }
        [field: SerializeField] public RecipeGettedUI ReceptUI { get; private set; }
        [field: SerializeField] public int HitPoint { get; private set; }
        public MapPlayer MapPlayer { get; private set; }

        private List<string> _deck = new List<string>() { "card_phosphorus" };

        [SerializeField] private ViewPoint _startPoint;
        [SerializeField] private ViewPoint _endPoint;
        [SerializeField] private MapPlayer _playerPrefab;
        [SerializeField] private GameObject _cardGiverUIEasy;
        [SerializeField] private GameObject _cardGiverUIMeadle;
        [SerializeField] private GameObject _cardGiverUIStrong;
        [SerializeField] private GameObject _craftGiverUI;
        [SerializeField] private Image _backGround;
        [SerializeField] private UIParallaxBackground _parallaxBackground;
        [SerializeField] private UIProgress _progressUI;

        private List<InteractivePoint> _locationPoints;
        public LocationConfigurate ActiveLocation { get; private set; }
        private string[] _locationKey = { "LocationFirst", "LocationSecond", "LocationThreed" };
        private PointOfInterestGenerator _pointOfInterestGenerator;
        private EnivrimentGenerator _enivrimentGenerator;
        private int _curentLocationNumber;
        private HillUI _hillPanel;

        public void Initialize()
        {
            Instance = this;

            var progres = MapStaticData.LoadData();
            HitPoint = MapStaticData.LoadPlayerData();

            MapUI.Initialize();
            _hillPanel = MapUI.GetUIByKey("hill") as HillUI;
            _hillPanel.OnModificateHP += SavePlayerStat;

            _deck = progres.Deck.ToList();
            _curentLocationNumber = progres.KeyLocation;
            ActiveLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[_curentLocationNumber]}");
            _pointOfInterestGenerator = new PointOfInterestGenerator(_startPoint, _endPoint, ActiveLocation);
            _enivrimentGenerator = new EnivrimentGenerator(ActiveLocation.LocationLevel);

            if (progres.LocationLevel == 0)
            {
                _locationPoints = _pointOfInterestGenerator.Generate();
                var data = DialoguesStatic.LoadData();
                data.FirstStart = true;
                switch (ActiveLocation.LocationKey)
                {
                    case 0:
                        data.CountLocationOneUpdate++;
                        break;
                    case 1:
                        data.CountLocationTwoUpdate++;
                        break;
                    case 2:
                        data.CountLocationThreeUpdate++;
                        break;
                }
                DialoguesStatic.SaveDataAndExecuteDialogue(data);
            }
            else
                _locationPoints = _pointOfInterestGenerator.Generate(progres.Points.ToList());

            MapStaticData.SaveData(_locationPoints.Select(point => point.PointEntity).ToArray(), ActiveLocation.LocationLevel, ActiveLocation.LocationKey, _deck);
            _enivrimentGenerator.Generate(_locationPoints);
            MapPlayer = Instantiate(_playerPrefab, _locationPoints[0].ViewPoint.transform.position, Quaternion.identity);
            MapController.Create(_locationPoints, ActiveLocation);

            MapController.OnPointBeginInteract += HideProgress;
            MapController.OnMapProgressUpdate += ProgressInit;
            MapCamera.MoveCameraToPlayer();

            ProgressInit();
            _backGround.sprite = ActiveLocation.BackGround;
            _parallaxBackground.TargetTransform = MapPlayer.transform;
        }

        public GameObject ShowCardGiver(string power)
        {
            switch (power)
            {
                case "easy":
                    _cardGiverUIEasy.SetActive(true);
                    return _cardGiverUIEasy;
                case "meadle":
                    _cardGiverUIMeadle.SetActive(true);
                    return _cardGiverUIMeadle;
                case "strong":
                    _cardGiverUIStrong.SetActive(true);
                    return _cardGiverUIStrong;
                default: throw new NotImplementedException();
            }
        }

        public void ShowCraftGiver() => _craftGiverUI.SetActive(true);

        public int GetNextLocationKey()
        {
            if (_curentLocationNumber + 1 >= _locationKey.Length)
                return -1;

            ActiveLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[_curentLocationNumber+ 1]}");
            return ActiveLocation.LocationKey;
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

        private void HideProgress() => _progressUI.Hide();
        private void OnDisable()
        {
            _hillPanel.OnModificateHP -= SavePlayerStat;
            MapController.OnMapProgressUpdate -= ProgressInit;
            MapController.OnPointBeginInteract -= HideProgress;

            Instance = null;
            MapController = null;

            _locationPoints.ForEach(point =>
            {
                point.Dispose();
            });
        }
    }
}
