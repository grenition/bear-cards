using GreonAssets.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class MapController : MonoBehaviour
    {
        private MapPlayer _mapPlayer;
        private List<List<InteractivePoint>> _pointCollections;
        private InteractivePoint _currentInteractPoint;
        private LocationConfigurate _locationConfigurate;
        private bool _interact;

        public void Create(List<List<InteractivePoint>> pointCollection, LocationConfigurate locationConfigurate)
        {
            _pointCollections = pointCollection;
            _locationConfigurate = locationConfigurate;

            pointCollection[0][0].Complited();
            UpdatePoints();
            _mapPlayer = MapCompositionRoot.Instance.MapPlayer;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_interact)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out ViewPoint viewpoint))
                {
                    viewpoint.ClickOnPoint();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
                ComplitedInteract();
        }

        public void UpdatePoints()
        {
            _pointCollections.ForEach(level =>
            {
                level.Where(point => point.PointEntity.PointPass).ForEach(point =>
                {
                    for (int i = 0; i < point.PointEntity.NeighborsID.Count(); i++)
                    {
                        var neighbor = FindPointByID(point.PointEntity.NeighborsID[i]);

                        if(!neighbor.PointEntity.PointPass && !neighbor.PointEntity.PointLock)
                            neighbor.Active();
                    }
                });
            });

            if (_currentInteractPoint != null && _currentInteractPoint.PointEntity.Level == _locationConfigurate.LocationLevel - 2)
                _pointCollections.Last().Last().Active();
        }

        public void MoveTo(ViewPoint viewPoint)
        {
            _mapPlayer.MoveTo(viewPoint);
            _interact = true;
            Debug.Log($"PlayerMove to {viewPoint.transform.position}");
        }

        public void PlayerInteractWithPoint(InteractivePoint interactivePoint)
        {
            _currentInteractPoint = interactivePoint;
            _currentInteractPoint.OnBeginInteract();
            MapCompositionRoot.Instance.MapCamera.MoveCameraToPlayer();
        }

        public void ComplitedInteract()
        {
            if (_currentInteractPoint == null)
                return;

            _currentInteractPoint.OnEndInteract();

            if(_currentInteractPoint.PointEntity.Level == _locationConfigurate.LocationLevel)
            {
                _pointCollections.Last().Last().Complited();
                LocationComplited();
                return;
            }

            _pointCollections[_currentInteractPoint.PointEntity.Level].Where(
                point => point != _currentInteractPoint).ForEach(
                point => point.Lock());

            _currentInteractPoint.Pass();
            _interact = false;
            UpdatePoints();
            _currentInteractPoint = null;
        }

        public void LocationComplited()
        {

        }

        private InteractivePoint FindPointByID(int id)
        {
            InteractivePoint findPoint = null;
            _pointCollections.ForEach(level =>
            {
                level.ForEach(point =>
                {
                    if (point.PointEntity.ID == id)
                        findPoint = point;
                });
            });

            if (findPoint != null)
                return findPoint;

            throw new System.Exception($"Point with id:{id} is not found!");
        }
    }
}