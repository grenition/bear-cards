using UnityEngine;

namespace Assets.Scripts.Map
{
    public static class ScneneLoaderStatic
    {
        private static SceneLoader _sceneLoader;

        public static void LoadSceneAsync(string key)
        {
            Initialize();
            _sceneLoader.LoadSceneAsync(key);
        }

        private static void Initialize()
        {
            if (_sceneLoader == null)
                _sceneLoader = new();
        }
    }
}
