using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using Project.Audio;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private AudioClip _music;
        public event Action OnPointBeginInteract;
        public event Action OnMapProgressUpdate;

        private MapPlayer _mapPlayer;
        private List<InteractivePoint> _pointCollections;
        private InteractivePoint _currentInteractPoint;
        private LocationConfigurate _locationConfigurate;
        private bool _interact;

        private void Start()
        {
            GameAudio.MusicSource.clip = _music;
            GameAudio.MusicSource.Play();
        }
        public void Create(List<InteractivePoint> pointCollection, LocationConfigurate locationConfigurate)
        {
            _pointCollections = pointCollection;
            _locationConfigurate = locationConfigurate;

            var activePoint = GetLasPointComplited();

            if (activePoint == null)
            {
                throw new System.Exception("Last Level Complited is not detected!");
            }

            _mapPlayer = MapCompositionRoot.Instance.MapPlayer;
            _mapPlayer.transform.position = activePoint.ViewPoint.transform.position;
            _currentInteractPoint = activePoint;

            if (activePoint.PointEntity.Level == _locationConfigurate.LocationLevel - 1)
            {
                _pointCollections.Last().Complited();
                LocationComplited();
                return;
            }

            UpdatePoints();
        }

        //private void Update()
        //{
        //    if (Input.GetMouseButtonDown(0) && !MapCompositionRoot.Instance.MapPlayer.PlayerIsMove )
        //    {
        //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        //        if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out ViewPoint viewpoint))
        //        {
        //            viewpoint.ClickOnPoint();
        //        }
        //    }
        //}

        public void UpdatePoints()
        {
            var complitedPoint = GetLasPointComplited();

            if (complitedPoint == null)
                throw new System.Exception("Last Level Complited is not detected!");

            _pointCollections.ForEach(point => point.Lock());
            complitedPoint.Pass();
            var id = complitedPoint.PointEntity.ID;

            for (int i = complitedPoint.PointEntity.Level-1; i >= 0 ; i--)
            {
                var level = _pointCollections.Where(point => point.PointEntity.Level == i).ToList();
                var idNeighbor = level.Find(point => point.PointEntity.NeighborsID.Contains(id)).PointEntity.ID;
                var pointNeigbor = _pointCollections.Find(point => point.PointEntity.ID ==  idNeighbor);
                pointNeigbor.Pass();
                id = pointNeigbor.PointEntity.ID;
            }

            for (int i = 0; i < complitedPoint.PointEntity.NeighborsID.Count(); i++)
            {
                var neighbor = FindPointByID(complitedPoint.PointEntity.NeighborsID[i]);
                neighbor.Active();
            }

            if (complitedPoint.PointEntity.Level == _locationConfigurate.LocationLevel - 2)
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

            if (_currentInteractPoint.PointEntity.Level != _locationConfigurate.LocationLevel - 1)
                MapStaticData.BattlePointStart(interactivePoint.PointEntity.ID, _locationConfigurate.GetBattleKey(),
                    _locationConfigurate.GetEnemyKey(_currentInteractPoint.PointEntity.Key));
            else
                MapStaticData.BattlePointStart(interactivePoint.PointEntity.ID, _locationConfigurate.GetBattleKey(), _locationConfigurate.BossFight());

            MapCompositionRoot.Instance.MapCamera.MoveCameraToPlayer();
            OnPointBeginInteract?.Invoke();
        }

        public void ComplitePoint()
        {
            if (_currentInteractPoint == null)
                return;

            if (_currentInteractPoint.PointEntity.Level == _locationConfigurate.LocationLevel - 1)
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

            OnMapProgressUpdate?.Invoke();
        }

        public void LocationComplited()
        {
            var keyLocation = MapCompositionRoot.Instance.GetNextLocationKey();

            if(keyLocation == -1)
            {
                WinGame();
                return;
            }

            MapStaticData.SaveData(_pointCollections.Select(point => point.PointEntity).ToArray(), 0, keyLocation);
            MapCompositionRoot.Instance.ReloadMap();
        }

        private void WinGame()
        {
            MapStaticData.GameWin();
        }

        private InteractivePoint FindPointByID(int id)
        {
            InteractivePoint findPoint = _pointCollections.Find(point => point.PointEntity.ID == id);

            if (findPoint != null)
                return findPoint;

            throw new System.Exception($"Point with id:{id} is not found!");
        }

        private InteractivePoint GetLasPointComplited()
        {
            List<InteractivePoint> activePointCollection = _pointCollections.Where(point => point.PointEntity.PointComplited).ToList();
            return activePointCollection.Last();
        }
    }
}