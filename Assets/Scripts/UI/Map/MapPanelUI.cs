using Assets.Scripts.Map;
using GreonAssets.Extensions;
using Project.Audio;
using R3;
using System;
using GreonAssets.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public abstract class MapPanelUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        protected Action OnInteractComplitedAction;

        private void Awake()
        {
            _button.Bind(() =>
            {
                InteractComplited();
                OnInteractComplitedAction?.Invoke();
                gameObject.CloseWithChildrensAnimation();
            }).AddTo(this);
        }

        public abstract void InteractComplited();
    }
}
