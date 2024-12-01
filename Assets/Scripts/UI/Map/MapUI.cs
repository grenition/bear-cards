using GreonAssets.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class MapUI : MonoBehaviour
    {
        [SerializeField] private MapPanelUI _fightUI;
        [SerializeField] private MapPanelUI _hillUI;

        private Dictionary<string, MapPanelUI> _panelMap = new();

        private void Start()
        {
            _panelMap.Add("fight", _fightUI);
            _panelMap.Add("hill", _hillUI);

            UIActiveAll(false);
        }

        public MapPanelUI ActiveUIByKey(string key)
        {
            _panelMap[key].gameObject.SetActive(true);
            return _panelMap[key];
        }

        private void UIActiveAll(bool activeUI)
        {
            _panelMap.Values.ForEach(value => value.gameObject.SetActive(activeUI));
        }
    }
}
