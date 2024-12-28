using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    [Serializable]
    public class PointEntity
    {
        public int ID;
        public List<int> NeighborsID;
        public bool IsEnemyPoint;
    
        public string[] EnemyKeys;
        //public int Level;
        //public string Key;

        [HideInInspector] public int NumberLevel;
        [HideInInspector] public string Key;
        [HideInInspector] public bool PointActive;
        [HideInInspector] public bool PointComplited;
        [HideInInspector] public bool PointPass;
        [HideInInspector] public bool PointLock;
    }

    public abstract class InteractivePoint : IDisposable
    {
        public InteractivePoint()
        {
            PointEntity = new();
        }
        public abstract void OnBeginInteract();
        public abstract void OnEndInteract();
        public PointEntity PointEntity;

        public ViewPoint ViewPoint { get; protected set; }

        public void Initialize(ViewPoint viewPoint)
        {
            ViewPoint = viewPoint;

            ViewPoint.OnClickAction += OnClick;
            ViewPoint.OnPlayerInteract += OnInteract;
        }

        public void Complited()
        {
            PointEntity.PointActive = false;
            PointEntity.PointComplited = true;
        }

        public void Pass()
        {
            PointEntity.PointActive = false;
            PointEntity.PointPass = true;
            ViewPoint.Pass();
        }

        public void Active()
        {
            PointEntity.PointActive = true;
            PointEntity.PointLock = false;
            ViewPoint.Active();
        }

        public void Lock()
        {
            PointEntity.PointLock = true;
            PointEntity.PointActive = false;
            ViewPoint.Lock();
        }

        private void OnClick()
        {
            if (PointEntity.PointActive)
                MapCompositionRoot.Instance.MapController.MoveTo(ViewPoint);
        }

        private void OnInteract()
        {
            Complited();
            MapCompositionRoot.Instance.MapController.PlayerInteractWithPoint(this);
        }

        public void Dispose()
        {
            ViewPoint.OnClickAction -= OnClick;
            ViewPoint.OnPlayerInteract -= OnInteract;
        }
    }
}
