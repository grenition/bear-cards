using UnityEngine;
namespace Assets.Scripts.Map
{
    public abstract class InterestingPointConfig : ScriptableObject
    {
        [field: SerializeField] public int PointCoast { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite View { get; private set; }

        public abstract InteractivePoint CreateInteractivePoint();
    }
}