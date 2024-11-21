using UnityEngine;

namespace Assets.Scripts.Map
{
    public abstract class InteractivePoint
    {
        public abstract void OnBeginInteract();
        public abstract void OnEndInteract();
        public Sprite View;

        public string Key {  get; protected set; }

        public ViewPoint ViewPoint { get; protected set; }

        public void Initialize(ViewPoint viewPoint)
        {
            ViewPoint = viewPoint;
            ViewPoint.SetSprite(View);
        }

        public void Complited()
        {
            Debug.Log( "{name}:I complited");
        }

        public void Pass()
        {
            Debug.Log("{name}:I pass");
        }

        public void Active()
        {
            Debug.Log("{name}:I active");
        }

        public void Lock()
        {
            Debug.Log("{name}:I lock");
        }
    }
}
