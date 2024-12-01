using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class MapUI : MonoBehaviour
    {
        [SerializeField] private MapPanelUI _fightUI;
        [SerializeField] private MapPanelUI _hillUI;
        [SerializeField] private Button _exitButton;

        private Dictionary<string, MapPanelUI> _panelMap = new();

        public void Initialize()
        {
            _panelMap.Add("fight", _fightUI);
            _panelMap.Add("hill", _hillUI);

            _exitButton.Bind(() =>
            {
                ScneneLoaderStatic.LoadSceneAsync("MainMenu");
            }).AddTo(this);

            UIActiveAll(false);
        }

        public MapPanelUI ActiveUIByKey(string key)
        {
            _panelMap[key].gameObject.SetActive(true);
            return _panelMap[key];
        }

        public MapPanelUI GetUIByKey(string key)
        {
            return _panelMap[key];
        }

        private void UIActiveAll(bool activeUI)
        {
            _panelMap.Values.ForEach(value => value.gameObject.SetActive(activeUI));
        }
    }
}
