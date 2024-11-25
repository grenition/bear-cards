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

        private List<List<InteractivePoint>> _intersections;
        private LocationConfigurate _activeLocation;
        private string[] _locationKey = { "LocationFirst", "LocationSecond", "LocationThreed" };

        //private LocationProgress _progress;

        private void Awake()
        {
            //_progress = new LocationProgress();
            Instance = this;
            _activeLocation = Resources.Load<LocationConfigurate>($"Map/{_locationKey[0]}");
            _intersections = new PointOfInterestGenerator(_startPoint, _endPoint, _activeLocation).Generate();

            var generator = new EnivrimentGenerator(_intersections);
            generator.Generate();

            MapPlayer = Instantiate(_playerPrefab, _intersections[0][0].ViewPoint.transform.position, Quaternion.identity);
            MapController.Create(_intersections, _activeLocation);

            //_progress.LoadData();
        }

        private void OnDisable()
        {
            Instance = null;
            MapController = null;

            _intersections.ForEach(inter =>
            {
                inter.ForEach(point => { point.Dispose(); });
            });
        }
    }
}
