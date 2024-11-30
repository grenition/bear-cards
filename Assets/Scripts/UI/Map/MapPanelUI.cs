using Assets.Scripts.Map;
using GreonAssets.Extensions;
using Project.Audio;
using R3;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public abstract class MapPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _descriptionField;
        [SerializeField] private Button _button;
        [SerializeField] private UIPanelAnimator _animatorPanel;
        private Action _onInteractComplitedAction;

        private void Awake()
        {
            _button.Bind(() =>
            {
                InteractComplited();
                _onInteractComplitedAction?.Invoke();
            }).AddTo(this);
        }
        public void Confirm(Action action)
        {
            _onInteractComplitedAction = action;
        }

        private void HidePanel() => 
            gameObject.SetActive(false);

        public abstract void InteractComplited();


    }
}
