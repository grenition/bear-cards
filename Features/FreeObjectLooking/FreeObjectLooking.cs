using UnityEngine;

namespace GreonAssets.Features.FreeObjectLooking
{
    public class FreeObjectLooking : FreeObjectLookingBase
    {
        [SerializeField] private float _translateTouchThreshold = 1f;
        [SerializeField] private float _touchSensivity = 0.01f;

#if UNITY_ANDROID || UNITY_IOS
        protected override bool rotateTrigger => Input.touchCount == 1;
        protected override Vector2 rotateDelta
        {
            get
            {
                if (Input.touchCount == 1)
                    return Input.touches[0].deltaPosition * _touchSensivity;
                else
                    return Vector2.zero;
            }
        }

        protected override bool scaleTrigger => Input.touchCount == 2;
        protected override float scaleDelta
        {
            get
            {
                if (Input.touchCount == 2)
                {
                    Touch touch0 = Input.touches[0];
                    Touch touch1 = Input.touches[1];

                    Vector2 prevTouch0Pos = touch0.position - touch0.deltaPosition;
                    Vector2 prevTouch1Pos = touch1.position - touch1.deltaPosition;

                    float prevTouchDeltaMag = (prevTouch0Pos - prevTouch1Pos).magnitude;
                    float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                    float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

                    return deltaMagnitudeDiff * _touchSensivity;
                }
                else
                {
                    return 0f;
                }
            }
        }

        protected override bool translateTrigger => Input.touchCount == 2 && scaleDelta < _translateTouchThreshold;
        protected override Vector2 translateDelta
        {
            get
            {
                if (Input.touchCount == 2)
                {
                    Vector2 delta0 = Input.touches[0].deltaPosition;
                    Vector2 delta1 = Input.touches[1].deltaPosition;
                    return ((delta0 + delta1) / 2f) * _touchSensivity;
                }
                else
                {
                    return Vector2.zero;
                }
            }
        }

        protected override bool focusTrigger => Input.touchCount == 3 && Input.touches[0].phase == TouchPhase.Began;
#else

        protected override bool rotateTrigger => Input.GetKey(KeyCode.Mouse0);
        protected override Vector2 rotateDelta => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        protected override bool scaleTrigger => Input.GetAxis("Mouse ScrollWheel") != 0f;
        protected override float scaleDelta => Input.GetAxis("Mouse ScrollWheel");
        protected override bool translateTrigger => Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse0);
        protected override Vector2 translateDelta => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        protected override bool focusTrigger => Input.GetKeyDown(KeyCode.F);

#endif
    }
}
