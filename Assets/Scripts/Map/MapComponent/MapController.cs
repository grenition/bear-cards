using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class MapController : MonoBehaviour
    {
        private MapPlayer _mapPlayer;
        private List<InteractivePoint> _pointCollections;
        private InteractivePoint _currentInteractPoint;
        private LocationConfigurate _locationConfigurate;
        private bool _interact;

        public void Create(List<InteractivePoint> pointCollection, LocationConfigurate locationConfigurate)
        {
            _pointCollections = pointCollection;
            _locationConfigurate = locationConfigurate;

            InteractivePoint activePoint = _pointCollections.Find(point => point.PointEntity.PointComplited);

            if (activePoint == null)
                throw new System.Exception("Last Level Complited is not detected!");

            _mapPlayer = MapCompositionRoot.Instance.MapPlayer;
            _mapPlayer.transform.position = activePoint.ViewPoint.transform.position;
            _currentInteractPoint = activePoint;

            if(activePoint.PointEntity.Level == _locationConfigurate.LocationLevel - 1)
            {
                _pointCollections.Last().Complited();
                LocationComplited();
                return;
            }

            UpdatePoints();
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
        }

        public void UpdatePoints()
        {
            var point = _pointCollections.Find(point => point.PointEntity.PointComplited);

            if (point == null)
                throw new System.Exception("Last Level Complited is not detected!");

            for (int i = 0; i < point.PointEntity.NeighborsID.Count(); i++)
            {
                var neighbor = FindPointByID(point.PointEntity.NeighborsID[i]);
                neighbor.Active();
            }

            if (point.PointEntity.Level == _locationConfigurate.LocationLevel - 2)
                _pointCollections.Last().Active();
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

            if(_currentInteractPoint.PointEntity.Level != _locationConfigurate.LocationLevel - 1)
                MapStaticData.BattlePointStart(interactivePoint.PointEntity.ID, _locationConfigurate.GetFightKey());
            else
                MapStaticData.BattlePointStart(interactivePoint.PointEntity.ID, _locationConfigurate.BossFight);

            MapCompositionRoot.Instance.MapCamera.MoveCameraToPlayer();
        }

        public void ComplitePoint()
        {
            if (_currentInteractPoint == null)
                return;

            if (_currentInteractPoint.PointEntity.Level == _locationConfigurate.LocationLevel-1)
            {
                _pointCollections.Last().Complited();
                LocationComplited();
                return;
            }

            _pointCollections.ForEach(point =>
            {
                point.Lock();
                if (point.PointEntity.PointComplited)
                {
                    point.Pass();
                }
            });
            _currentInteractPoint.OnEndInteract();
            _currentInteractPoint.Complited();
            _currentInteractPoint = null;

            _interact = false;
            UpdatePoints();
            MapStaticData.SaveData(_pointCollections.Select(point => point.PointEntity).ToArray(), _locationConfigurate.LocationLevel, _locationConfigurate.LocationKey);
        }

        public void LocationComplited()
        {
            var keyLocation = MapCompositionRoot.Instance.GetNextLocationKey();
            MapStaticData.SaveData(_pointCollections.Select(point => point.PointEntity).ToArray(),0, keyLocation);
            MapCompositionRoot.Instance.ReloadMap();
        }

        private InteractivePoint FindPointByID(int id)
        {
            InteractivePoint findPoint = _pointCollections.Find(point => point.PointEntity.ID == id);

            if (findPoint != null)
                return findPoint;

            throw new System.Exception($"Point with id:{id} is not found!");
        }
    }
}