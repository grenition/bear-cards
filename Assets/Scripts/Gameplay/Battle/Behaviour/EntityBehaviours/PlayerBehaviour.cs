using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class PlayerBehaviour : BaseEntityBehaviour
    {
        public CardPlayerModel PlayerModel => BattleBehaviour.Model.Player;

        private List<CardConfig> _shouldGivedCards = new();

        protected override async void OnFirstTurnStart()
        {
            await UniTask.NextFrame();
            foreach (var preCard in BattleBehaviour.Config.PreDeckCards)
            {
                BattleBehaviour.Model.AddCardToDeck(CardOwner.player, preCard.name);
            }

            var deck = MapStaticData.LoadData();

            _shouldGivedCards = new();
            deck.Deck.ForEach(card =>
            {
                _shouldGivedCards.Add(BattleStaticData.Cards.Get(card));
            });

            for(int lvl = 0; lvl < 7; lvl++)
            {
                CardConfig[] cards = new CardConfig[0];
                foreach(CardConfig card in _shouldGivedCards.ToList())
                {
                    if(card.Level == lvl)
                    {
                        cards = ExpandMassive(cards, card, UnityEngine.Random.Range(0, cards.Length));
                    }
                }

                foreach (var deckCard in cards)
                {
                    if (deckCard == null) continue;
                    if (!BattleBehaviour.Config.GiveCardsByActualLevel || deckCard.Level <= PlayerModel.Level)
                    {
                        BattleBehaviour.Model.AddCardToDeck(CardOwner.player, deckCard.name);
                        _shouldGivedCards.TryRemove(deckCard);
                    }
                }
            }

            for (int i = 0; i < BattleBehaviour.Config.CardsAtFirstTurn; i++)
            {
                if (PlayerModel.TransferCardFromDeckToHand()) ;
                await UniTask.WaitForSeconds(0.1f);
            }

            await UniTask.WaitForSeconds(0.1f);

            for (int i = 0; i < PlayerModel.Config.SpellsSize; i++)
            {
                if (PlayerModel.TransferCardFromDeckToSpells()) ;
                await UniTask.WaitForSeconds(0.1f);
            }
        }
        protected override async void OnTurnStart()
        {
            PlayerModel.AddTurnElectrons();

            if (TurnIndex == 0) return;

            foreach (var deckCard in _shouldGivedCards.ToList())
            {
                if (deckCard == null) continue;

                if (!BattleBehaviour.Config.GiveCardsByActualLevel || deckCard.Level <= PlayerModel.Level)
                {
                    BattleBehaviour.Model.AddCardToDeck(CardOwner.player, deckCard.name);
                        _shouldGivedCards.TryRemove(deckCard);
                }
            }

            if (PlayerModel.GetFirstCardInDeck() == null || PlayerModel.IsAllCardInDeckHigherThanPlayerLevel())
            {
                foreach (var postCard in BattleBehaviour.Config.PostDeckCards)
                {
                    BattleBehaviour.Model.AddCardToDeck(CardOwner.player, postCard.name);
                }
            }

            if (BattleBehaviour.Config.GiveCardsByActualLevel)
            {
                for (int i = 0; i < BattleBehaviour.Config.CardsAtAnotherTurns; i++)
                {
                    PlayerModel.TransferCardFromDeckToHandByPlayerLevel();
                }
            }
            else
            {
                for (int i = 0; i < BattleBehaviour.Config.CardsAtAnotherTurns; i++)
                {
                    PlayerModel.TransferCardFromDeckToHand();
                }
            }

            await UniTask.WaitForSeconds(0.1f);
            for (int i = 0; i < PlayerModel.Config.SpellsSize; i++)
            {
                if (PlayerModel.TransferCardFromDeckToSpells()) ;
                await UniTask.WaitForSeconds(0.1f);
            }
        }

        public PlayerBehaviour(BattleBehaviour battleModel) : base(battleModel) { }

        private T[] ExpandMassive<T>(T[] origin, T value, int index)
        {
            if (index > origin.Length)
            {
                index = origin.Length;
            }
            else if (index < 0)
            {
                index = 0;
            }

            T[] newMassive = new T[origin.Length + 1];

            int lastIndex = 0;
            for (int i = 0; i < newMassive.Length; i++)
            {
                if (i == index)
                {
                    newMassive[i] = value;
                }
                else
                {
                    newMassive[i] = origin[lastIndex];

                    lastIndex++;
                }
            }

            return newMassive;
        }
    }

     
    }
