using System.Collections.Generic;
using UnityEngine;

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

        private LocationProgress _progress;

        private void Awake()
        {
            _progress = new LocationProgress();
            Instance = this;
            _activeLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[0]}");
            _locationPoints = new PointOfInterestGenerator(_startPoint, _endPoint, _activeLocation).Generate();

            var generator = new EnivrimentGenerator(_locationPoints,_activeLocation.LocationLevel);
            generator.Generate();

            MapPlayer = Instantiate(_playerPrefab, _locationPoints[0].ViewPoint.transform.position, Quaternion.identity);
            MapController.Create(_locationPoints, _activeLocation);

            _progress.SaveDate(_locationPoints);
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
