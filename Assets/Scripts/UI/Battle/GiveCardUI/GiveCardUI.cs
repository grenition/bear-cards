using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common.Datas;
using Project.UI.Battle;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using GreonAssets.UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class GiveCardUI : MonoBehaviour
    {
        [SerializeField] protected UICard[] CardVisual;
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Button _button;

        [Tooltip("Card was be added to panel last generation")]
        [SerializeField] private CardConfig[] _cardCollection;

        [Header("Count generation card")]
        [SerializeField] private CardField[] _cardFields;

        private CardConfig[] _cardInGiver;
        private CardModel[] _cardModels;

        private int _countCardAdded;

        private void Start()
        {
            _buttons.ToList().ForEach(butt =>
            {
                butt.Bind(() =>
                {
                    if (CheckGiverComplited() == 4)
                        return;

                    butt.interactable = !butt.interactable;
                    CheckGiverComplited();
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
            _button.interactable = false;

            var data = DialoguesStatic.LoadData();
            data.CountCardGiveComming++;
            DialoguesStatic.SaveDataAndExecuteDialogue(data);

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].interactable = true;
            }

            Load();
            SetViewCard();
        }

        private int CheckGiverComplited()
        {
            var interactiveCount = _buttons.ToList().Where(button => !button.interactable).Count();

            if (interactiveCount == 4)
                _button.interactable = true;
            else 
                _button.interactable = false;

            return interactiveCount;
        }

        protected virtual void Load()
        {
            _cardInGiver = new CardConfig[5];

            int index = 0;
            _cardFields.ForEach(field =>
            {
                List<CardConfig> actualCardCollection = _cardCollection.ToList().Where(config => config.Rarity == field.Rarity).ToList();
                for (int i = 0; i < field.Count; i++)
                {
                    _cardInGiver[index] = actualCardCollection[UnityEngine.Random.Range(0, actualCardCollection.Count())];
                    index++;
                }
            });
        }

        protected virtual void SetViewCard()
        {
            _cardModels = new CardModel[5];
            for (int i = 0; i < CardVisual.Length; i++)
            {
                _cardModels[i] = new CardModel(_cardInGiver[i].name);
                CardVisual[i].Init(_cardModels[i]);
            }
        }

        private void Complited()
        {
            List<string> cards = new List<string>();
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i].interactable == false)
                    cards.Add(CardVisual[i].Model.Key);
            }

            MapStaticData.AddToDeckAndSave(cards);
            gameObject.CloseWithChildrensAnimation();
        }

        [Serializable]
        private class CardField
        {
            public CardRarity Rarity;
            public int Count;
        }
    }
}
