using System;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MapCompositionRoot : MonoBehaviour, IDisposable
    {
        public static MapCompositionRoot MapRoot { get; private set; }

        [SerializeField] private InterestingPointConfig _startPoint;
        [SerializeField] private InterestingPointConfig _endPoint;

        private void Awake()
        {
            MapRoot = this;
            var points = new PointOfInterestGenerator("LocationFirst", _startPoint,_endPoint).Generate();
            var generator = new EnivrimentGenerator(points);
            generator.Generate();

            points.ForEach(level =>
            {
                level.ForEach(point =>
                {
                    Debug.Log(point.Level + " " + point.InteractivePoint.ToString());
                });
            });


            points.ForEach(level =>
            {
                level.ForEach(point =>
                {
                    Debug.LogError(point.Level + " " + point.InteractivePoint.ViewPoint.transform.position);
                });
            });
        }

        public void Dispose()
        {
            MapRoot = null;
        }

    }
}
