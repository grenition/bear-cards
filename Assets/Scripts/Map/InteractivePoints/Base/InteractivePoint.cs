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
        public int Level;

        public bool PointActive;
        public bool PointComplited;
        public bool PointPass;
        public bool PointLock;
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

        public string Key { get; protected set; }

        public ViewPoint ViewPoint { get; protected set; }

        public void Initialize(ViewPoint viewPoint)
        {
            ViewPoint = viewPoint;

            ViewPoint.OnClickAction += () =>
            {
                if (PointEntity.PointActive)
                    MapCompositionRoot.Instance.MapController.MoveTo(ViewPoint);
            };

            ViewPoint.OnPlayerInteract += () =>
            {
                Complited();
                MapCompositionRoot.Instance.MapController.PlayerInteractWithPoint(this);
            };
        }

        public void Complited()
        {
            PointEntity.PointActive = false;
            PointEntity.PointActive = false;
            PointEntity.PointComplited = true;
            Debug.Log($"{Key}:I complited");
        }

        public void Pass()
        {
            PointEntity.PointActive = false;
            PointEntity.PointPass = true;
            Debug.Log($"{Key}:I pass");
        }

        public void Active()
        {
            PointEntity.PointActive = true;
            Debug.Log($"{Key}:I active");
        }

        public void Lock()
        {
            PointEntity.PointLock = true;
            PointEntity.PointActive = false;
            Debug.Log($"{Key}:I lock");
        }

        public void Dispose()
        {
            ViewPoint.OnClickAction -= () =>
            {
                if (PointEntity.PointActive)
                    MapCompositionRoot.Instance.MapController.MoveTo(ViewPoint);
            };
            ViewPoint.OnPlayerInteract -= () =>
            {
                Complited();
                MapCompositionRoot.Instance.MapController.PlayerInteractWithPoint(this);
            };
        }
    }
}
