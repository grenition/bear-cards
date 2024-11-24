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
        private bool _interact;

        public void Create(List<List<InteractivePoint>> pointCollection)
        {
            _pointCollections = pointCollection;

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
            {
                ComplitedInteract();
            }
        }

        public void UpdatePoints()
        {
            _pointCollections.ForEach(level =>
            {
                level.Where(point => point.PointPass).ForEach(point =>
                {
                    point.ConnectPoints.Where(
                        connect => !connect.PointPass && !connect.PointLock).ForEach(
                        connect => connect.Active());
                });
            });
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

            _pointCollections[_currentInteractPoint.Level].Where(
                point => point != _currentInteractPoint).ForEach(
                point => point.Lock());

            _currentInteractPoint.Pass();
            _interact = false;
            UpdatePoints();
        }
    }
}