using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private UIField _mainField;
        [SerializeField] private UIField _settingsField;

        private List<UIField> _fields;
        
        private void Awake()
        {
            _fields = new List<UIField>
            {
                _mainField,
                _settingsField
            };
            
            ShowField(_mainField);

            _startGameButton.Bind(() =>
            {
                ScneneLoaderStatic.LoadSceneAsync(_sceneName);
                
            }).AddTo(this);

            _settingsButton.Bind(() =>
            {
                ShowField(_settingsField);
                
            }).AddTo(this);
        }
        
        public void BackToMain() => ShowField(_mainField);
        
        private void ShowField(UIField field)
        {
            _fields.Except(new []{field}).ForEach(x => x.Hide());
            field.Show();
        }

        private void Start()
        {
            GameAudio.MusicSource.PlayOneShot(_musicClip);
        }
    }
}
