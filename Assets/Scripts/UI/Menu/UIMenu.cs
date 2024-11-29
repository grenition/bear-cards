using System;
using GreonAssets.Extensions;
using Project.Audio;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private string _sceneName = "BattleScene";
        [SerializeField] private AudioClip _musicClip;

        private void Awake()
        {
            _startGameButton.Bind(() =>
            {
                SceneManager.LoadScene(_sceneName);
            }).AddTo(this);
        }

        private void Start()
        {
            GameAudio.MusicSource.clip = _musicClip;
            GameAudio.MusicSource.Play();
        }
    }
}
