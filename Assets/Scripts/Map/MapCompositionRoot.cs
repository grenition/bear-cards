﻿using System;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MapCompositionRoot : MonoBehaviour, IDisposable
    {
        public static MapCompositionRoot MapRoot { get; private set; }
        public static MapController MapController { get; private set; }

        [SerializeField] private InterestingPointConfig _startPoint;
        [SerializeField] private InterestingPointConfig _endPoint;

        private void Awake()
        {
            MapRoot = this;
            var points = new PointOfInterestGenerator("LocationFirst", _startPoint,_endPoint).Generate();
            var generator = new EnivrimentGenerator(points);
            MapController = new();
            MapController.Create(points);
            generator.Generate();
        }

        public void Dispose()
        {
            MapRoot = null;
        }

    }
}
