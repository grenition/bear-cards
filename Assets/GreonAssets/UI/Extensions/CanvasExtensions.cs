using DG.Tweening;
using UnityEngine;

namespace GreonAssets.UI.Extensions
{
    public static class CanvasExtensions
    {
        public static void StretchRectTransformFromWorldPoints(this Canvas canvas, RectTransform targetRect, Vector3 worldPosLowerLeft, Vector3 worldPosUpperRight, RectOffset margin = default)
        {
            if (canvas == null) return;

            if (targetRect == null) return;

            RectTransform parentRect = targetRect.parent as RectTransform;
            if (parentRect == null) return;

            Vector2 screenPosLowerLeft = RectTransformUtility.WorldToScreenPoint(canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, worldPosLowerLeft);
            Vector2 screenPosUpperRight = RectTransformUtility.WorldToScreenPoint(canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, worldPosUpperRight);

            Vector2 localPosLowerLeft;
            Vector2 localPosUpperRight;

            bool successLower = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPosLowerLeft,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localPosLowerLeft
            );

            bool successUpper = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPosUpperRight,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localPosUpperRight
            );

            if (!successLower || !successUpper) return;

            if (margin != null)
            {
                localPosLowerLeft += new Vector2(margin.left, margin.bottom);
                localPosUpperRight -= new Vector2(margin.right, margin.top);
            }

            Vector2 anchoredPosition = (localPosLowerLeft + localPosUpperRight) / 2;
            Vector2 sizeDelta = new Vector2(
                Mathf.Abs(localPosUpperRight.x - localPosLowerLeft.x),
                Mathf.Abs(localPosUpperRight.y - localPosLowerLeft.y)
            );

            targetRect.anchorMin = new Vector2(0.5f, 0.5f);
            targetRect.anchorMax = new Vector2(0.5f, 0.5f);
            targetRect.anchoredPosition = anchoredPosition;
            targetRect.sizeDelta = sizeDelta;
        }

        public static Tween DOStretchRectTransformFromWorldPoints(this Canvas canvas, RectTransform targetRect, Vector3 worldPosLowerLeft, Vector3 worldPosUpperRight, RectOffset margin = null, float duration = 1f, Ease ease = Ease.Linear)
        {
            if (canvas == null) return null;

            if (targetRect == null) return null;

            RectTransform parentRect = targetRect.parent as RectTransform;
            if (parentRect == null) return null;

            Vector2 screenPosLowerLeft = RectTransformUtility.WorldToScreenPoint(
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                worldPosLowerLeft
            );
            Vector2 screenPosUpperRight = RectTransformUtility.WorldToScreenPoint(
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                worldPosUpperRight
            );

            Vector2 localPosLowerLeft;
            Vector2 localPosUpperRight;

            bool successLower = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPosLowerLeft,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localPosLowerLeft
            );

            bool successUpper = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPosUpperRight,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localPosUpperRight
            );

            if (!successLower || !successUpper) return null;

            if (margin != null)
            {
                localPosLowerLeft += new Vector2(margin.left, margin.bottom);
                localPosUpperRight -= new Vector2(margin.right, margin.top);
            }

            Vector2 targetAnchoredPosition = (localPosLowerLeft + localPosUpperRight) / 2;
            Vector2 targetSizeDelta = new Vector2(
                Mathf.Abs(localPosUpperRight.x - localPosLowerLeft.x),
                Mathf.Abs(localPosUpperRight.y - localPosLowerLeft.y)
            );

            targetRect.anchorMin = new Vector2(0.5f, 0.5f);
            targetRect.anchorMax = new Vector2(0.5f, 0.5f);

            Sequence stretchSequence = DOTween.Sequence();

            stretchSequence.Join(targetRect.DOAnchorPos(targetAnchoredPosition, duration).SetEase(ease));
            stretchSequence.Join(targetRect.DOSizeDelta(targetSizeDelta, duration).SetEase(ease));

            return stretchSequence;
        }
    }
}
