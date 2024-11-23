using System;
using System.Collections.Generic;
using System.Linq;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;

namespace Project.Gameplay.Battle.Model.CardPlayers
{
    public class CardPlayerModel : IDisposable
    {
        public event Action<int> OnDamage;
        public event Action OnDeath;

        public bool IsAlive => Health > 0;
        public string Key { get; protected set; }
        public CardPlayerConfig Config => BattleStaticData.CardPlayers.Get(Key);
        public BattleModel BattleModel { get; protected set; }
        public CardOwner OwnershipType { get; protected set; }
        public List<CardSlotModel> Hand { get; protected set; } = new();
        public List<CardSlotModel> Deck { get; protected set; } = new();
        public List<CardSlotModel> Spells { get; protected set; } = new();
        public int Health { get; protected set; }

        public CardPlayerModel(string key, CardOwner ownerhipType, BattleModel battleModel)
        {
            Key = key;
            BattleModel = battleModel;
            OwnershipType = ownerhipType;
            Health = Config.Health;

            for (int i = 0; i < Config.HandSize; i++)
            {
                var cardConfig = Config.Hand.GetAt(i);
                var card = cardConfig ? new CardModel(cardConfig.name, BattleModel) : null;
                Hand.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.hand, OwnershipType, i), CardSlotPermissions.Hand(OwnershipType), card));
            }
            for (int i = 0; i < Config.DeckSize; i++)
            {
                var cardConfig = Config.Deck.GetAt(i);
                var card = cardConfig ? new CardModel(cardConfig.name, BattleModel) : null;
                Deck.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.deck, OwnershipType, i), CardSlotPermissions.Deck(OwnershipType), card));
            }
            for (int i = 0; i < Config.SpellsSize; i++)
            {
                var cardConfig = Config.Spells.GetAt(i);
                var card = cardConfig ? new CardModel(cardConfig.name, BattleModel) : null;
                Spells.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.spells, OwnershipType, i), CardSlotPermissions.Hand(OwnershipType), card));
            }
        }
        public void Dispose()
        {
            Hand.ForEach(x => x.Dispose());
            Deck.ForEach(x => x.Dispose());
            Spells.ForEach(x => x.Dispose());
        }

        internal void Damage(int damage)
        {
            if (damage <= 0) return;

            Health -= damage;
            Health = Math.Max(0, Health);

            OnDamage.SafeInvoke(damage);

            if (Health <= 0)
            {
                OnDeath.SafeInvoke();
                BattleModel.EndBattle(OwnershipType == CardOwner.player ? CardOwner.enemy : CardOwner.player);
            }
        }

        public CardModel GetFirstCardInHand() => Hand.FirstOrDefault(x => x.Card != null)?.Card;
        public CardModel GetFirstCardInDeck() => Deck.FirstOrDefault(x => x.Card != null)?.Card;
        public CardModel GetFirstCardInSpells() => Spells.FirstOrDefault(x => x.Card != null)?.Card;
        public CardSlotModel GetFirstFreeSlotInHand() => Hand.FirstOrDefault(x => x.Card == null);
        public CardSlotModel GetFirstFreeSlotInDeck() => Deck.FirstOrDefault(x => x.Card == null);
        public CardSlotModel GetFirstFreeSlotInSpells() => Spells.FirstOrDefault(x => x.Card == null);
        public bool TransferCardFromDeckToHand()
        {
            var card = GetFirstCardInDeck();
            var targetSlot = GetFirstFreeSlotInHand();
            if (card == null || card.Type == CardType.Spell || targetSlot == null) return false;

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
    }
}
