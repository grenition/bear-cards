using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class HillUI : MapPanelUI
    {
        [SerializeField] private Image _mainIcon;
        [SerializeField] private TMP_Text _effectText;
        [SerializeField] private TMP_Text _name;
        private int _modificatorHP;

        public  void Apper(Action action,
            Sprite icon,
            string effectText,
            int modificatorHP,
            string name)
        {
            _mainIcon.sprite = icon;
            _effectText.text = effectText;
            _modificatorHP = modificatorHP;
            _name.text = name;

            OnInteractComplitedAction = action;
        }

        public override void InteractComplited()
        {
            //modificate hit point
            Debug.Log("You get hp point");
        }
    }
}
