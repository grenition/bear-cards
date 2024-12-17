using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
using R3;
using System;
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
