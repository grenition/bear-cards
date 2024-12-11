using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using R3;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class CraftCardUI : MonoBehaviour
    {
        [SerializeField] private string[] _keys;

        [SerializeField] private Button[] _buttons;
        [SerializeField] private TMP_Text[] _name;
        [SerializeField] private Image[] _image;

        [SerializeField] private Button _button;

        private List<string> _newCards;
        private int _countCardAdded;

        private void Start()
        {
            _buttons.ToList().ForEach(butt =>
            {
                butt.Bind(() =>
                {
                    butt.interactable = false;
                    _countCardAdded++;
                    Complited();
                }).AddTo(this);
            });

            _button.Bind(() =>
            {
                _countCardAdded = 5;
                Complited();
            }).AddTo(this);
        }

        private void OnEnable()
        {
            _countCardAdded = 0;
            _newCards = new List<string>();
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].interactable = true;
                var randomCard = _keys[Random.Range(0, _keys.Length)];
                var card = BattleStaticData.Cards.Get(randomCard);

                _newCards.Add(randomCard);
                _name[i].text = card.VisualName;
                _image[i].sprite = card.VisualIcon;
            }
        }

        private void Complited()
        {
            if (_countCardAdded >= 4)
            {
                List<string> cards = new List<string>();
                for (int i = 0; i < _buttons.Length; i++)
                {
                    if (_buttons[i].interactable == false)
                    {
                        cards.Add(_newCards[i]);
                    }
                }
                MapStaticData.SaveDeck(cards);


                gameObject.SetActive(false);
            }
        }
    }
}
