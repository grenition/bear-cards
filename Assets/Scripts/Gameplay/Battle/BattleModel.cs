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
        public event Action<CardModel, CardPosition> OnCardPlaced;
        public event Action<CardModel, CardPosition> OnCardPickedUp;
        
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
        
        // public CardModel PickUpCard(CardPosition position)
        // {
        //     CardModel PickUpFromField(CardModel[] field, int index)
        //     {
        //         if (index >= field.Length) return null;
        //         var savedPtr = field[index];
        //         field[index] = null;
        //         return savedPtr;
        //     }
        //
        //     CardModel PickUp()
        //     {
        //         switch (position.owner)
        //         {
        //             case CardOwner.player:
        //                 switch (position.container)
        //                 {
        //                     case CardContainer.field:
        //                         return PickUpFromField(PlayerField, position.index);
        //                     default:
        //                         return Player.PickUpCard(position);
        //                 }
        //             case CardOwner.enemy:
        //                 switch (position.container)
        //                 {
        //                     case CardContainer.field:
        //                         return PickUpFromField(EnemyField, position.index);
        //                     default:
        //                         return Enemy.PickUpCard(position);
        //                 }
        //         }
        //
        //         return null;
        //     }
        //
        //     var result = PickUp();
        //     if (result != null)
        //         OnCardPickedUp.SafeInvoke(result, position);
        //     
        //     return result;
        // }
        // public bool TryPlaceCard(CardModel model, CardPosition position)
        // {
        //     bool TryPlaceToField(CardModel[] field, int index)
        //     {
        //         if (index >= field.Length) return false;
        //         if (field[index] != null) return false;
        //         field[index] = model;
        //         return true;
        //     }
        //
        //     bool TryPlace()
        //     {
        //
        //         PickUpCard(model.Position);
        //
        //         switch (position.owner)
        //         {
        //             case CardOwner.player:
        //                 switch (position.container)
        //                 {
        //                     case CardContainer.field:
        //                         return TryPlaceToField(PlayerField, position.index);
        //                     default:
        //                         return Player.TryPlaceCard(model, position);
        //                 }
        //             case CardOwner.enemy:
        //                 switch (position.container)
        //                 {
        //                     case CardContainer.field:
        //                         return TryPlaceToField(PlayerField, position.index);
        //                     default:
        //                         return Enemy.TryPlaceCard(model, position);
        //                 }
        //         }
        //
        //         return false;
        //     }
        //
        //     var result = TryPlace();
        //     if (result)
        //         OnCardPlaced.SafeInvoke(model, position);
        //     
        //     return result;
        // }

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
