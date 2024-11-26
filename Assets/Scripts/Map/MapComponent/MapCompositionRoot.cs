﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map
{
    public class MapCompositionRoot : MonoBehaviour
    {
        public static MapCompositionRoot Instance { get; private set; }
        [field: SerializeField] public MapController MapController { get; private set; }
        [field: SerializeField] public MapCamera MapCamera { get; private set; }
        public MapPlayer MapPlayer { get; private set; }

        [SerializeField] private ViewPoint _startPoint;
        [SerializeField] private ViewPoint _endPoint;
        [SerializeField] private MapPlayer _playerPrefab;

        private List<InteractivePoint> _locationPoints;
        private LocationConfigurate _activeLocation;
        private string[] _locationKey = { "LocationFirst", "LocationSecond", "LocationThreed" };
        private PointOfInterestGenerator _pointOfInterestGenerator;
        private EnivrimentGenerator _enivrimentGenerator;
        private int _curentLocationNumber;

        private void Awake()
        {
            Instance = this;

            var progres = MapStaticData.LoadData();
            _curentLocationNumber = progres.KeyLocation;
            _activeLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[_curentLocationNumber]}");
            _pointOfInterestGenerator = new PointOfInterestGenerator(_startPoint, _endPoint, _activeLocation);
            _enivrimentGenerator = new EnivrimentGenerator(_activeLocation.LocationLevel);

            if (progres.LocationLevel == 0)
                _locationPoints = _pointOfInterestGenerator.Generate();
            else
                _locationPoints = _pointOfInterestGenerator.Generate(progres.Points.ToList());

            MapStaticData.SaveData(_locationPoints, _activeLocation.LocationLevel, _activeLocation.LocationKey);
            _enivrimentGenerator.Generate(_locationPoints);
            MapPlayer = Instantiate(_playerPrefab, _locationPoints[0].ViewPoint.transform.position, Quaternion.identity);
            MapController.Create(_locationPoints, _activeLocation);

            MapCamera.MoveCameraToPlayer();
        }

        [ContextMenu("Deleted")]
        public void Deleted() => MapStaticData.GameFail();

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

        private void OnDisable()
        {
            Instance = null;
            MapController = null;

            _locationPoints.ForEach(point =>
            {
                point.Dispose();
            });
        }
    }
}
