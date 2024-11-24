using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GreonAssets.UI.Extensions
{
    public static class RectTransformExtensions
    {
        public static void GetBounds(this IEnumerable<RectTransform> rectTransforms, out Vector2 bottomLeft, out Vector2 topRight)
        {
            var rTransforms = rectTransforms.ToList();
            if (rTransforms.Count == 0)
            {
                bottomLeft = Vector2.zero;
                topRight = Vector2.zero;
                return;
            }

            bottomLeft = new Vector2(float.MaxValue, float.MaxValue);
            topRight = new Vector2(float.MinValue, float.MinValue);

            foreach (var rectTransform in rTransforms)
            {
                if (rectTransform == null) continue;

                Vector3[] worldCorners = new Vector3[4];
                rectTransform.GetWorldCorners(worldCorners);

                foreach (var corner in worldCorners)
                {
                    if (corner.x < bottomLeft.x) bottomLeft.x = corner.x;
                    if (corner.y < bottomLeft.y) bottomLeft.y = corner.y;

                    if (corner.x > topRight.x) topRight.x = corner.x;
                    if (corner.y > topRight.y) topRight.y = corner.y;
                }
            }
        }
    }
}
