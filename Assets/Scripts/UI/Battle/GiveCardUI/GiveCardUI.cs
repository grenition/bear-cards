using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common.Datas;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class GiveCardUI : MonoBehaviour
    {
        [SerializeField] protected UICardMap[] CardVisual;
        [SerializeField] private Button _button;

        [Tooltip("Card was be added to panel last generation")]
        [SerializeField] private CardConfig[] _cardCollection;

        [Header("Count generation card")]
        [SerializeField] private CardField[] _cardFields;

        private CardConfig[] _cardInGiver;
        private CardModel[] _cardModels;

        private const int _countCartToLockPanel = 4;
        private int _countCardAdded;

        private void Start()
        {
            foreach (var item in CardVisual)
            {
                item.Button.Bind(() =>
                {
                    item.Value = !item.Value;
                    if (!CheckActiveItem())
                    {
                        item.Value = !item.Value;
                        return;
                    }

                    _button.interactable = !CheckActiveItem(_countCartToLockPanel -1) ? true : false;
                    item.PlayAnimationView();
                    UpdateViewState();
                }).AddTo(this);
            }

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

            foreach (var item in CardVisual)
            {
                item.ResetToggle();
            }

            Load();
            SetViewCard();
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
            foreach (var item in CardVisual)
            {
                if (item.Value)
                    cards.Add(item.Model.Key);
            }

            MapStaticData.AddToDeckAndSave(cards);
            gameObject.CloseWithChildrensAnimation();
        }

        private void UpdateViewState()
        {
            foreach (var item in CardVisual)
            {
                item.UpdateToggleState();
            }
        }

        private bool CheckActiveItem(int targetCount = _countCartToLockPanel)
        {
            return CardVisual.Where(x => x.Value == true).Count() <= targetCount;
        }

        [Serializable]
        private class CardField
        {
            public CardRarity Rarity;
            public int Count;
        }
    }
}
