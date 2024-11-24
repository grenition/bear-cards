using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Assets.Scripts.Map
{
    public class MapCompositionRoot : MonoBehaviour
    {
        public static MapCompositionRoot Instance { get; private set; }

        [field: SerializeField] public MapController MapController { get; private set; }
        [field: SerializeField] public MapCamera MapCamera { get; private set; }
        public MapPlayer MapPlayer { get; private set; }

        [SerializeField] private InterestingPointConfig _startPoint;
        [SerializeField] private InterestingPointConfig _endPoint;
        [SerializeField] private MapPlayer _playerPrefab;

        private List<List<InteractivePoint>> _intersections;

        private void Awake()
        {
            Instance = this;
            _intersections = new PointOfInterestGenerator("LocationFirst", _startPoint,_endPoint).Generate();
            var generator = new EnivrimentGenerator(_intersections);
            generator.Generate();
            MapPlayer = Instantiate(_playerPrefab, _intersections[0][0].ViewPoint.transform.position, Quaternion.identity);
            MapController.Create(_intersections);
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
