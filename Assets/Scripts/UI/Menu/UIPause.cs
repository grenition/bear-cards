using System;
using Assets.Scripts.Map;
using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class UIPause : MonoBehaviour
    {
        [SerializeField] private GameObject _mainContainer;
        [SerializeField] private GameObject _settingsContainer;
        [SerializeField] private Button _backToGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backToMenuButton;

        private void Start()
        {
            _backToGameButton.Bind(() =>
            {
                _mainContainer.CloseWithChildrensAnimation();
            }).AddTo(this);

            _backToMenuButton.Bind(() =>
            {
                ScneneLoaderStatic.LoadSceneAsync("MainMenu");
            }).AddTo(this);

            _settingsButton.Bind(() =>
            {
                _settingsContainer.SetActive(true);
            }).AddTo(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _mainContainer.SetActiveWithChildrensAnimation(!_mainContainer.activeSelf);
            }
        }
    }
}
