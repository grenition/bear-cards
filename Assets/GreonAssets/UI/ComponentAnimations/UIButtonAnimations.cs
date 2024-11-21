using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GreonAssets.UI.ComponentAnimations
{
    [RequireComponent(typeof(Button))]
    public class UIButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Animation Preferences")]
        [SerializeField] private float hoverScale = 1.12f;
        [SerializeField] private float clickScale = 1f;
        [SerializeField] private float animationDuration = 0.15f;

        [Header("Outline Preferences")]
        [SerializeField] private bool enableOutline = true;
        [SerializeField] private Color hoverOutlineColor = Color.yellow;
        [SerializeField] private float hoverOutlineWidth = 2f;
        [SerializeField] private float outlineAnimationDuration = 0.15f;

        private Vector3 originalScale;
        private RectTransform rectTransform;
        private Outline outline;
        private Color originalOutlineColor;
        private float originalOutlineWidth;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalScale = Vector3.one;

            outline = GetComponent<Outline>() ?? gameObject.AddComponent<Outline>();
            if (outline != null)
            {
                originalOutlineColor = outline.effectColor;
                originalOutlineWidth = outline.effectDistance.x;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AnimateScale(hoverScale);
            if (enableOutline)
                AnimateOutline(hoverOutlineColor, hoverOutlineWidth);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AnimateScale(originalScale.x);
            if (enableOutline)
                AnimateOutline(originalOutlineColor, originalOutlineWidth);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            AnimateScale(clickScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            AnimateScale(hoverScale);
        }

        private void AnimateScale(float targetScale)
        {
            rectTransform.DOScale(Vector3.one * targetScale, animationDuration).SetEase(Ease.OutBack);
        }

        private void AnimateOutline(Color targetColor, float targetWidth)
        {
            if (outline != null)
            {
                DOTween.To(() => outline.effectColor, x => outline.effectColor = x, targetColor, outlineAnimationDuration);
                DOTween.To(() => outline.effectDistance.x,
                           x => outline.effectDistance = new Vector2(x, x),
                           targetWidth,
                           outlineAnimationDuration);
            }
        }
    }
}