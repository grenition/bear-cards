using System;
using System.Collections.Generic;
using System.Linq;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Infrastructure;

namespace Project.Gameplay.Battle.Model
{
    public class BattleModel : IDisposable
    {
        public event Action<CardModel, CardPosition, CardPosition> OnCardTransfered;
        public event Action<CardModel, CardModel> OnCardAttack;
        public event Action<CardOwner> OnBattleEnded;
        
        public string Key { get; protected set; }
        public BattleConfig Config => BattleStaticData.Battles.Get(Key);
        public CardPlayerModel Player { get; protected set;}
        public CardPlayerModel Enemy { get; protected set; }
        public List<CardSlotModel> PlayerField { get; protected set; } = new();
        public List<CardSlotModel> EnemyField { get; protected set; } = new();
        public Dictionary<CardPosition, CardSlotModel> Slots { get; protected set; } = new();
        public List<CardModel> Cards { get; protected set; } = new();
        public bool BattleEnded { get; protected set; }
        public CardOwner BattleWinner { get; protected set; }

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
                .Where(x => x.Value.Card != null)
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

        public void AddCardToDeck(CardOwner owner, string cardKey)
        {
            if(BattleEnded) return;

            var playerModel = owner == CardOwner.player ? Player : Enemy;
            var freeDeckSlot = playerModel.GetFirstFreeSlotInDeck();
            if(freeDeckSlot == null) return;

            var card = new CardModel(cardKey, this);
            freeDeckSlot.PlaceCard(card);
        }
        
        public bool TryTransferCard(CardPosition from, CardPosition to)
        {
            if(BattleEnded) return false;
            var slot = GetSlotAtPosition(from);
            if (slot == null) return false;

            CardModel card = null;
            if (to.container == CardContainer.garbage)
            {
                card = slot.TakeCard();
                OnCardTransfered.SafeInvoke(card, from, to);
                card.CallOnTransfered(from, to);
                card.Dispose();
                return true;
            }
            
            var newSlot = GetSlotAtPosition(to);
            var owner = slot.Position.owner;
            
            if (newSlot == null) return false;
            if (!slot.IsAvailableForPickUp(owner) || !newSlot.IsAvailableForDrop(owner)) return false;
            
            card = slot.TakeCard();
            newSlot.PlaceCard(card);

            OnCardTransfered.SafeInvoke(card, from, to);
            card.CallOnTransfered(from, to);
            
            return true;
        }

        public void AttackForward(CardPosition attackerPosition)
        {
            if(BattleEnded) return;
            if(attackerPosition.container != CardContainer.field) return;
            
            var card = GetCardAtPosition(attackerPosition);
            var enemyType = attackerPosition.owner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
            var enemyPosition = new CardPosition(attackerPosition.container, enemyType, attackerPosition.index);
            var forwardCard = GetCardAtPosition(enemyPosition);
            var enemyPlayer = enemyType == CardOwner.player ? Player : Enemy;
            
            if(card == null) return;
            card.CallOnAttack(enemyPosition);
            
            if (forwardCard == null)
            {
                enemyPlayer.Damage(card.AttackDamage);
                return;
            }
            forwardCard.Damage(card.AttackDamage);
        }

        public void EndBattle(CardOwner winner)
        {
            if(BattleEnded) return;
            
            BattleEnded = true;
            BattleWinner = winner;
            OnBattleEnded.SafeInvoke(winner);
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