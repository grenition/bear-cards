using System;
using UnityEngine;

namespace Project.Infrastructure
{
    public class DestroyIfMobilePlatform : MonoBehaviour
    {
        private void Awake()
        {
            if (Application.isMobilePlatform)
                Destroy(gameObject);
        }
    }
}
