using UnityEngine;

namespace GreonAssets.UI.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class UIParallaxBackground : MonoBehaviour
    {
        [field: SerializeField] public Transform TargetTransform { get; set; }
        
        [SerializeField] private Vector2 parallaxFactor = new Vector2(0.5f, 0.5f);
        [SerializeField] private float smoothing = 5f;

        private RectTransform rectTransform;
        private Vector2 initialPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            initialPosition = rectTransform.anchoredPosition;
        }

        private void LateUpdate()
        {
            if (TargetTransform == null)
                return;

            Vector3 targetScreenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, TargetTransform.position);

            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            Vector2 normalizedPosition = new Vector2(
                (targetScreenPosition.x / screenSize.x) - 0.5f,
                (targetScreenPosition.y / screenSize.y) - 0.5f
            );

            Vector2 targetOffset = new Vector2(
                normalizedPosition.x * parallaxFactor.x * screenSize.x,
                normalizedPosition.y * parallaxFactor.y * screenSize.y
            );

            Vector2 desiredPosition = initialPosition + targetOffset;
            Vector2 smoothedPosition = Vector2.Lerp(rectTransform.anchoredPosition, desiredPosition, smoothing * Time.deltaTime);

            rectTransform.anchoredPosition = smoothedPosition;
        }
        
        public void SetTarget(Transform newTarget)
        {
            TargetTransform = newTarget;
        }
        
        public void SetParallaxFactor(float x, float y)
        {
            parallaxFactor = new Vector2(x, y);
        }
        
        public void SetSmoothing(float newSmoothing)
        {
            smoothing = newSmoothing;
        }
    }
}
