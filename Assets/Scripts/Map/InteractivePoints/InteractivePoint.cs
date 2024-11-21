using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public abstract class InteractivePoint
    {
        public abstract void OnBeginInteract();
        public abstract void OnEndInteract();
        public Sprite View;
        public List<InteractivePoint> ConnectPoints;
        public int Level;

        public bool PointActive { get; private set; }

        public string Key { get; protected set; }

        public ViewPoint ViewPoint { get; protected set; }

        public void Initialize(ViewPoint viewPoint)
        {
            ViewPoint = viewPoint;
            ViewPoint.SetSprite(View);
        }

        public void Complited()
        {
            PointActive = false;
            Debug.Log("{name}:I complited");
        }

        public void Pass()
        {
            PointActive = false;
            Debug.Log("{name}:I pass");
        }

        public void Active()
        {
            PointActive = true;
            Debug.Log("{name}:I active");
        }

        public void Lock()
        {
            PointActive = false;
            Debug.Log("{name}:I lock");
        }
    }
}
