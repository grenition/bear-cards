using Assets.Scripts.Map;
using GreonAssets.Extensions;
using Project.Audio;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private string _sceneName = "MapScene";
        [SerializeField] private AudioClip _musicClip;

        private void Awake()
        {
            _startGameButton.Bind(() =>
            {
                ScneneLoaderStatic.LoadSceneAsync(_sceneName);
            }).AddTo(this);
        }

        private void Start()
        {
            GameAudio.MusicSource.PlayOneShot(_musicClip);
        }
    }
}
