using System;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Audio;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class UISettings : MonoBehaviour
    {
        [SerializeField] private UIMenu _menu;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _resetGameButton;

        [SerializeField] private TMP_Dropdown _resolutionDropdown;

        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            _backButton.Bind(_menu.BackToMain)
                .AddTo(this);

            _resolutionDropdown.options = Screen.resolutions.Select(x => new TMP_Dropdown.OptionData(x.ToString())).ToList();
            _resolutionDropdown.value = Screen.resolutions.ToList().FindIndex(x => x.Equals(Screen.currentResolution));
            _resolutionDropdown.onValueChanged.Bind(x =>
            {
                var resolution = Screen.resolutions[x];
                Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);

            }).AddTo(this);

            _musicSlider.value = GameAudio.MusicSource.volume * 100;
            _musicSlider.onValueChanged.Bind(x => GameAudio.SetMusicVolume(x / 100))
                .AddTo(this);

            _sfxSlider.value = GameAudio.SFXSource.volume * 100;
            _sfxSlider.onValueChanged.Bind(x => GameAudio.SetSFXVolume(x / 100))
                .AddTo(this);

            _resetButton.Bind(ResetSettings).AddTo(this);
            _resetGameButton.Bind(ResetGame).AddTo(this);
        }

        private void ResetSettings()
        {
            var resolution = Screen.resolutions.Last();
            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);
            _resolutionDropdown.value = Screen.resolutions.Length - 1;

            GameAudio.SetMusicVolume(1f);
            _musicSlider.value = 100;

            GameAudio.SetSFXVolume(1f);
            _sfxSlider.value = 100;
        }

        private void ResetGame()
        {
            string[] jsonFiles = Directory.GetFiles(Application.persistentDataPath);
            jsonFiles.ForEach(x => File.Delete(x));
        }
    }
}
