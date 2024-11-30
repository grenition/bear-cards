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
        [SerializeField] private MapPanelUI _giveCard;

        private Dictionary<string, MapPanelUI> _panelMap = new();

        private void Start()
        {
            _panelMap.Add("fight", _fightUI);
            _panelMap.Add("hill", _hillUI);
            _panelMap.Add("giveCard", _giveCard);

            UIActiveAll(false);
        }

        public void ActiveUIByKey(string key, Action action)
        {
            _panelMap[key].gameObject.SetActive(true);
            _panelMap[key].Confirm(action);
        }

        private void UIActiveAll(bool activeUI)
        {
            _panelMap.Values.ForEach(value => value.gameObject.SetActive(activeUI));
        }
    }
}
