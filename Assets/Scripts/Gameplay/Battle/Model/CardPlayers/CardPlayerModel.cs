using Assets.Scripts.Map;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Gameplay.Common.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Gameplay.Battle.Model.CardPlayers
{
    public class CardPlayerModel : IDisposable
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDeath;
        public event Action<int> OnLevelElectronsChanged;
        public event Action<int> OnHandElectronsChanged;

        public bool IsAlive => Health > 0;
        public int Level => BattleModel.GetElectronLevel(LevelElectrons);
        public string Key { get; protected set; }
        public CardPlayerConfig Config => BattleStaticData.CardPlayers.Get(Key);
        public BattleModel BattleModel { get; protected set; }
        public CardOwner OwnershipType { get; protected set; }
        public List<CardSlotModel> Hand { get; protected set; } = new();
        public List<CardSlotModel> Deck { get; protected set; } = new();
        public List<CardSlotModel> Spells { get; protected set; } = new();
        public int Health { get; protected set; }
        public int HandElectrons { get; protected set; }
        public int LevelElectrons { get; protected set; }


        public CardPlayerModel(string key, CardOwner ownerhipType, BattleModel battleModel)
        {
            Key = key;
            BattleModel = battleModel;
            OwnershipType = ownerhipType;
            Health = ownerhipType == CardOwner.player ? MapStaticData.LoadPlayerData() : Config.Health;

            LevelElectrons = MapStaticData.LoadData().KeyLocation + 1;
            HandElectrons = Config.StartHandElectrons;

            for (int i = 0; i < Config.HandSize; i++)
            {
                Hand.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.hand, OwnershipType, i), CardSlotPermissions.Hand(OwnershipType)));
            }
            for (int i = 0; i < Config.DeckSize; i++)
            {
                Deck.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.deck, OwnershipType, i), CardSlotPermissions.Deck(OwnershipType)));
            }
            for (int i = 0; i < Config.SpellsSize; i++)
            {
                Spells.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.spells, OwnershipType, i), CardSlotPermissions.Hand(OwnershipType)));
            }
        }
        public void Dispose()
        {
            Hand.ForEach(x => x.Dispose());
            Deck.ForEach(x => x.Dispose());
            Spells.ForEach(x => x.Dispose());
        }

        internal void ModifyHealth(int modifyValue)
        {
            if (modifyValue == 0) return;

            var startValue = Health;
            Health += modifyValue;
            Health = Math.Max(0, Health);

            OnHealthChanged.SafeInvoke(Health - startValue);

            if (Health <= 0)
            {
                OnDeath.SafeInvoke();
                BattleModel.EndBattle(OwnershipType == CardOwner.player ? CardOwner.enemy : CardOwner.player);
            }
        }
        internal void ModifyLevelElectrons(int modifyValue)
        {
            var startValue = LevelElectrons;
            LevelElectrons += modifyValue;
            LevelElectrons = Math.Max(0, LevelElectrons);

            OnLevelElectronsChanged.SafeInvoke(LevelElectrons - startValue);
        }
        internal void ModifeHandElectrons(int modifyValue)
        {
            var startValue = HandElectrons;
            HandElectrons += modifyValue;
            HandElectrons = Math.Max(0, HandElectrons);

            OnHandElectronsChanged.SafeInvoke(HandElectrons - startValue);
        }

        public CardModel GetFirstCardInHand() => Hand.FirstOrDefault(x => x.Card != null)?.Card;
        public CardModel GetFirstCardInDeck() => Deck.FirstOrDefault(x => x.Card != null)?.Card;
        public CardModel GetFirstCardInDeckByPlayerLevel() => Deck
            .Where(x => x.Card != null)
            .Where(x => x.Card.Level <= Level)
            .Select(x => x.Card)
            .ToList()
            .GetRandomElement();
        public CardModel GetFirstCardInSpells() => Spells.FirstOrDefault(x => x.Card != null)?.Card;
        public CardSlotModel GetFirstFreeSlotInHand() => Hand.FirstOrDefault(x => x.Card == null);
        public CardSlotModel GetFirstFreeSlotInDeck() => Deck.FirstOrDefault(x => x.Card == null);
        public CardSlotModel GetFirstFreeSlotInSpells() => Spells.FirstOrDefault(x => x.Card == null);
        public bool IsAllCardInDeckHigherThanPlayerLevel() => Deck.Where(x => x.Card != null).All(x => x.Card.Level > Level);
        public bool TransferCardFromDeckToHand(bool ignoreSpells = true)
        {
            var card = GetFirstCardInDeck();
            var targetSlot = GetFirstFreeSlotInHand();
            if (card == null || (card.Type == CardType.Spell && ignoreSpells) || targetSlot == null) return false;

            BattleModel.TryTransferCard(card.Position, targetSlot.Position);
            return true;
        }
        public bool TransferCardFromDeckToHandByPlayerLevel(bool ignoreSpells = true)
        {
            var card = GetFirstCardInDeckByPlayerLevel();
            var targetSlot = GetFirstFreeSlotInHand();
            if (card == null || (card.Type == CardType.Spell && ignoreSpells) || targetSlot == null) return false;

            BattleModel.TryTransferCard(card.Position, targetSlot.Position);
            return true;
        }
        public bool TransferCardFromDeckToSpells()
        {
            var spell = Deck.FirstOrDefault(x => x.Card != null && x.Card.Type == CardType.Spell)?.Card;
            var targetSlot = GetFirstFreeSlotInSpells();
            if (spell == null || targetSlot == null) return false;

            BattleModel.TryTransferCard(spell.Position, targetSlot.Position);
            return true;
        }

        public void AddEffects(Dictionary<CardEffect, int> effects)
        {
            foreach (var effect in effects)
            {
                effect.Key.ApplyEffect(this, effect.Value);
            }
        }

        public void AddTurnElectrons() => ModifeHandElectrons(Level + 1);
    }
}
