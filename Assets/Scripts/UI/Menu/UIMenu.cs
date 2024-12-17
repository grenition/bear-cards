using System.Collections.Generic;
using Assets.Scripts.Map;
using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
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

        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _mainPanel;

        private List<UIField> _fields;
        
        private void Awake()
        {
            _startGameButton.Bind(() =>
            {
                ScneneLoaderStatic.LoadSceneAsync(_sceneName);
                
            }).AddTo(this);

            _settingsButton.Bind(() =>
            {
                _settingsPanel.SetActiveWithChildrensAnimation(true);
                _mainPanel.SetActiveWithChildrensAnimation(false);
            }).AddTo(this);
        }
        
        public void BackToMain()
        {
            _settingsPanel.SetActiveWithChildrensAnimation(false);
            _mainPanel.SetActiveWithChildrensAnimation(true);
        }

        private void Start()
        {
            GameAudio.MusicSource.PlayOneShot(_musicClip);
        }
    }
}
