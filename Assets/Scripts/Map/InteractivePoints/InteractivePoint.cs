using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public abstract class InteractivePoint : IDisposable
    {
        public abstract void OnBeginInteract();
        public abstract void OnEndInteract();
        //public Sprite View;
        public List<InteractivePoint> ConnectPoints;
        public int Level;

        public bool PointActive { get; private set; }
        public bool PointComplited { get; private set; }
        public bool PointPass { get; private set; }
        public bool PointLock { get; private set; }

        public string Key { get; protected set; }

        public ViewPoint ViewPoint { get; protected set; }

        public void Initialize(ViewPoint viewPoint)
        {
            ViewPoint = viewPoint;
            //ViewPoint.SetSprite(View);

            ViewPoint.OnClickAction += () =>
            {
                if (PointActive)
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
            PointActive = false;
            PointActive = false;
            PointComplited = true;
            Debug.Log($"{Key}:I complited");
        }

        public void Pass()
        {
            PointActive = false;
            PointPass = true;
            Debug.Log($"{Key}:I pass");
        }

        public void Active()
        {
            PointActive = true;
            Debug.Log($"{Key}:I active");
        }

        public void Lock()
        {
            PointLock = true;
            PointActive = false;
            Debug.Log($"{Key}:I lock");
        }

        public void Dispose()
        {
            ViewPoint.OnClickAction -= () =>
            {
                if (PointActive)
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
