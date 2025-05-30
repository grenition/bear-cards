using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System;

namespace Project
{
    public class UIProgress : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progress;
        [SerializeField] private Button _bockButton;

        [SerializeField] private LocalizedString _locationName;
        [SerializeField] private LocalizedString _hitPointName;
        [SerializeField] private LocalizedString _cardElementName;
        [SerializeField] private LocalizedString _cardMajestyName;
        
        private int _locationNumber;
        private int _hitPoint;
        private int _cardElementCount;
        private int _cardMajestyCount;

        private void Start()
        {
            LocalizationSettings.SelectedLocaleChanged += LocalizationChanged;
            _bockButton.onClick.Bind(() =>
            {
                Debug.Log("Card collection is apper");
            }).AddTo(this);
        }

        public void Hide() => gameObject.CloseWithChildrensAnimation();

        public void UpdateLocation(int locationNumber)
        {
            _locationNumber = locationNumber;
            UpdateInfo();
        }

        public void UpdateHitPoint(int hitPoint)
        {
            _hitPoint = hitPoint;
            UpdateInfo();
        }

        public void UpdateCardElement(int cardElementCount)
        {
            _cardElementCount = cardElementCount;
            UpdateInfo();
        }

        public void UpdateCardMajesty(int cardMajestyCount)
        {
            _cardMajestyCount = cardMajestyCount;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            _progress.text = $"{_locationName.GetLocalizedString()} " + $"{_locationNumber + 1}" + "\n"
                + $"{_hitPointName.GetLocalizedString()} " + $"{_hitPoint}" + "\n"
                + $"{_cardElementName.GetLocalizedString()} " + $"{_cardElementCount}" + "\n"
                + $"{_cardMajestyName.GetLocalizedString()} " + $"{_cardMajestyCount}" + "\n";
        }

        private void LocalizationChanged(Locale locale)
        {
            UpdateInfo();
        }
    }
}
