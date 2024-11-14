using UnityEngine;

namespace Project.UI.Battle
{
    [CreateAssetMenu(fileName = "CurveParameters", menuName = "Hand Curve Parameters")]
    public class UICurveParameters : ScriptableObject
    {
        public AnimationCurve positioning;
        public float positioningInfluence = .1f;
        public AnimationCurve rotation;
        public float rotationInfluence = 10f;
    }
}
