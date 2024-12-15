using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class UIProgress : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progress;
        [SerializeField] private Button _bockButton;

        [SerializeField] private string _locationName;
        [SerializeField] private string _hitPointName;
        [SerializeField] private string _cardElementName;
        [SerializeField] private string _cardMajestyName;

        [SerializeField] private Animator _animator;

        private int _locationNumber;
        private int _hitPoint;
        private int _cardElementCount;
        private int _cardMajestyCount;

        private void Start()
        {
            _bockButton.onClick.Bind(() =>
            {
                Debug.Log("Card collection is apper");
            }).AddTo(this);
        }

        public void Hide() => _animator.SetTrigger("hide");

        private void OnHide() => gameObject.SetActive(false);


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
            _progress.text = $"{_locationName} " + $"{_locationNumber + 1}" + "\n"
                + $"{_hitPointName} " + $"{_hitPoint}" + "\n"
                + $"{_cardElementName} " + $"{_cardElementCount}" + "\n"
                + $"{_cardMajestyName} " + $"{_cardMajestyCount}" + "\n";
        }
    }
}
