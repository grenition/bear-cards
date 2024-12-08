using UnityEngine;
using UnityEngine.InputSystem;

namespace GreonAssets.Features.FreeObjectLooking
{
    public class FreeObjectLooking : FreeObjectLookingBase
    {
        [SerializeField] private InputActionReference _lookAction;
        
        protected override bool rotateTrigger { get; }
        protected override Vector2 rotateDelta { get; }
        protected override bool scaleTrigger { get; }
        protected override float scaleDelta { get; }
        protected override bool translateTrigger { get; }
        protected override Vector2 translateDelta { get; }
    }
}
