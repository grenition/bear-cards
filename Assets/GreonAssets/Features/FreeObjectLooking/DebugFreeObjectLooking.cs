using UnityEngine;

namespace GreonAssets.Features.FreeObjectLooking
{
    public class DebugFreeObjectLooking : FreeObjectLookingBase
    {
        protected override bool rotateTrigger => Input.GetKey(KeyCode.Mouse0);
        protected override Vector2 rotateDelta => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        protected override bool scaleTrigger => Input.GetAxis("Mouse ScrollWheel") != 0f;
        protected override float scaleDelta => Input.GetAxis("Mouse ScrollWheel");
        protected override bool translateTrigger => Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse0);
        protected override Vector2 translateDelta => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        protected override bool focusTrigger => Input.GetKeyDown(KeyCode.F);
    }
}
