﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MapCompositionRoot : MonoBehaviour
    {
        public static MapCompositionRoot Instance { get; private set; }
        [field: SerializeField] public MapController MapController { get; private set; }
        [field: SerializeField] public MapCamera MapCamera { get; private set; }
        public MapPlayer MapPlayer { get; private set; }
        public LocationProgress Progress { get; private set; }

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
            Progress = new LocationProgress();
            Instance = this;

            var progres = Progress.LoadData();
            _curentLocationNumber = progres.KeyLocation;
            _activeLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[_curentLocationNumber]}");
            _pointOfInterestGenerator = new PointOfInterestGenerator(_startPoint, _endPoint, _activeLocation);
            _enivrimentGenerator = new EnivrimentGenerator(_activeLocation.LocationLevel);

            if (progres.LocationLevel == 0)
                _locationPoints = _pointOfInterestGenerator.Generate();
            else
                _locationPoints = _pointOfInterestGenerator.Generate(progres.Points.ToList());

            Progress.SaveDate(_locationPoints, _activeLocation.LocationLevel, _activeLocation.LocationKey);
            _enivrimentGenerator.Generate(_locationPoints);
            MapPlayer = Instantiate(_playerPrefab, _locationPoints[0].ViewPoint.transform.position, Quaternion.identity);
            MapController.Create(_locationPoints, _activeLocation);

            MapCamera.MoveCameraToPlayer();
        }

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
            //Scene reload
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