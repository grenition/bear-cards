using System;
using System.Collections.Generic;
using System.Linq;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.CardPlayers;
using Project.Gameplay.Battle.Cards;
using Project.Gameplay.Battle.CardSlots;
using Project.Gameplay.Data;
using Project.Infrastructure;

namespace Project.Gameplay.Battle
{
    public class BattleModel : IDisposable
    {
        public event Action<CardModel, CardPosition, CardPosition> OnCardTransfered;
        
        public string Key { get; protected set; }
        public BattleConfig Config => StaticData.Battles.Get(Key);
        public CardPlayerModel Player { get; protected set;}
        public CardPlayerModel Enemy { get; protected set; }
        public List<CardSlotModel> PlayerField { get; protected set; } = new();
        public List<CardSlotModel> EnemyField { get; protected set; } = new();
        public Dictionary<CardPosition, CardSlotModel> Slots { get; protected set; } = new();
        public List<CardModel> Cards { get; protected set; } = new();

        public BattleModel(string battleKey)
        {
            Key = battleKey;
            Player = new CardPlayerModel(Constants.Player, CardOwner.player, this);
            Enemy = new CardPlayerModel(Config.Enemy.name, CardOwner.enemy, this);

            for (int i = 0; i < Config.FieldSize; i++)
            {
                PlayerField.Add(new CardSlotModel(this, new CardPosition(CardContainer.field, CardOwner.player, i), CardSlotPermissions.PlayerField()));
                EnemyField.Add(new CardSlotModel(this, new CardPosition(CardContainer.field, CardOwner.enemy, i), CardSlotPermissions.EnemyField()));
            }
        }

        #region Registry
        internal void RegisterCardSlot(CardSlotModel slot)
        {
            if(slot == null) return;
            if(Slots.ContainsKey(slot.Position)) return;
            Slots.Add(slot.Position, slot);
        }
        internal void UnregisterCardSlot(CardSlotModel slot)
        {
            if(slot == null) return;
            if(!Slots.ContainsKey(slot.Position)) return;
            Slots.Remove(slot.Position);
        }
        internal void RegisterCard(CardModel card)
        {
            if(Cards.Contains(card)) return;
            Cards.Add(card);
        }
        internal void UnregisterCard(CardModel card)
        {
            if(!Cards.Contains(card)) return;
            Cards.Remove(card);
        }
        #endregion
        
        #region DataGetters
        public CardModel GetCardAtPosition(CardPosition position)
        {
            var slot = Slots.Get(position);
            if (slot == null) return null;
            return slot.Card;
        }
        public IEnumerable<CardModel> GetCardsAtPosition(CardOwner owner, CardContainer container)
        {
            return Slots
                .Where(x => x.Key.container == container && x.Key.owner == owner)
                .Select(x => x.Value.Card);
        }
        public CardSlotModel GetSlotAtPosition(CardPosition position)
        {
            return Slots.Get(position);
        }
        public IEnumerable<CardSlotModel> GetSlotsAtPosition(CardOwner owner, CardContainer container)
        {
            return Slots
                .Where(x => x.Key.container == container && x.Key.owner == owner)
                .Select(x => x.Value);
        }
        public bool IsPlaceAvailableForPlayerCard(CardPosition position)
        {
            var slot = Slots.Get(position);
            return slot != null && slot.IsAvailableForDrop(CardOwner.player);
        }
        #endregion

        #region Interactions

        public bool TryTransferCard(CardPosition from, CardPosition to)
        {
            var slot = GetSlotAtPosition(from);
            var newSlot = GetSlotAtPosition(to);
            var owner = slot.Position.owner;
            
            if (slot == null || newSlot == null) return false;
            if (!slot.IsAvailableForPickUp(owner) || !newSlot.IsAvailableForDrop(owner)) return false;
            
            var card = slot.TakeCard();
            newSlot.PlaceCard(card);

            OnCardTransfered.SafeInvoke(card, from, to);
            
            return true;
        }

        #endregion

        public void Dispose()
        {
            Player.Dispose();
            Enemy.Dispose();
            PlayerField.ForEach(x => x.Dispose());
            EnemyField.ForEach(x => x.Dispose());
            Cards.ToList().ForEach(x => x.Dispose());
        }
    }
}
