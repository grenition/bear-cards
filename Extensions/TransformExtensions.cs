using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GreonAssets.Extensions
{
    public static class TransformExtensions
    {
        public static Pose GetLocalPose(this Transform transform) =>
            new(transform.localPosition, transform.localRotation);

        public static Pose GetPose(this Transform transform) => new(transform.position, transform.rotation);

        public static void DestroyAllChildrens(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
                Object.Destroy(transform.GetChild(i).gameObject);
        }

        public static void DestroyChildrens(this Transform transform, Func<GameObject, bool> predicate)
        {
            if (predicate == null) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;

                if (predicate(child))
                    Object.Destroy(child);
            }
        }

        public static List<Transform> GetAllChildren(this Transform transform)
        {
            var list = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
                list.Add(transform.GetChild(i));

            return list;
        }

        public static int GetActiveSiblingIndex(this Transform transform)
        {
            if (transform == null) return -1;

            var parent = transform.parent;
            if (parent == null)
            {
                return transform.gameObject.activeSelf ? 0 : -1;
            }

            int activeIndex = 0;
            foreach (Transform sibling in parent)
            {
                if (sibling.gameObject.activeSelf)
                {
                    if (sibling == transform)
                        return activeIndex;

                    activeIndex++;
                }
            }

            return -1;
        }
        
        public static int GetActiveChildCount(this Transform transform)
        {
            if (transform == null) return 0;

            int activeCount = 0;

            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    activeCount++;
                }
            }

            return activeCount;
        }
    }
}
