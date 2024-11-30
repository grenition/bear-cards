using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map
{
    public class SceneLoader
    {
        private ScreenLoader _loadScreen;
        private const float _deley = 0.7f;

        public SceneLoader()
        {
            _loadScreen = Resources.Load<ScreenLoader>("Map/ScreenLoader");
        }

        public void Initialize()
        {
        }

        public async void LoadSceneAsync(string key)
        {
            var screenLoader = GameObject.Instantiate<ScreenLoader>(_loadScreen);

            await UniTask.WaitUntil(() => screenLoader.ScreenShow == true);
            await UniTask.WaitForSeconds(_deley);
            await SceneManager.LoadSceneAsync(key, LoadSceneMode.Single);
            screenLoader.HideScreen();
        }
    }
}
