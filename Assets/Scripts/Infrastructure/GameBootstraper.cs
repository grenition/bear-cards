using UnityEngine;

namespace Project.Infrastructure
{
    public static class GameBootstrapper
    {
        private static int maxMobileFramerate = 60;

        [RuntimeInitializeOnLoadMethod]
        static void OnGameStart()
        {
            if (Application.isMobilePlatform)
            {
                Application.targetFrameRate = maxMobileFramerate;
            }
        }
    }
}
