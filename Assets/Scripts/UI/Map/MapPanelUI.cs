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
        [SerializeField] private Button _button;
        [SerializeField] private UIPanelAnimator _animatorPanel;
        protected Action OnInteractComplitedAction;

        private void Awake()
        {
            _button.Bind(() =>
            {
                InteractComplited();
                OnInteractComplitedAction?.Invoke();
                _animatorPanel.Hide();
            }).AddTo(this);
        }

        public abstract void InteractComplited();

        private void HidePanel() => 
            gameObject.SetActive(false);
    }
}
