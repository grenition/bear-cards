using System;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;
using Project.Gameplay.Common.Datas;

namespace Project.Gameplay.Battle.Model.CardSlots
{
    public struct CardSlotPermissions
    {
        public bool playerCanDropCard;
        public bool playerCanPickUpCard;
        public bool enemyCanDropCard;
        public bool enemyCanPickUpCard;

        public static CardSlotPermissions PlayerHand() => new CardSlotPermissions()
        {
            playerCanDropCard = true,
            playerCanPickUpCard = true,
            enemyCanDropCard = false,
            enemyCanPickUpCard = false,
        };
        public static CardSlotPermissions PlayerField() => new CardSlotPermissions()
        {
            playerCanDropCard = true,
            playerCanPickUpCard = false,
            enemyCanDropCard = false,
            enemyCanPickUpCard = false,
        };
        public static CardSlotPermissions PlayerDeck() => new CardSlotPermissions()
        {
            playerCanDropCard = false,
            playerCanPickUpCard = true,
            enemyCanDropCard = false,
            enemyCanPickUpCard = false,
        };
        public static CardSlotPermissions EnemyHand() => new CardSlotPermissions()
        {
            playerCanDropCard = false,
            playerCanPickUpCard = false,
            enemyCanDropCard = true,
            enemyCanPickUpCard = true,
        };
        public static CardSlotPermissions EnemyField() => new CardSlotPermissions()
        {
            playerCanDropCard = false,
            playerCanPickUpCard = false,
            enemyCanDropCard = true,
            enemyCanPickUpCard = false,
        };
        public static CardSlotPermissions EnemyDeck() => new CardSlotPermissions()
        {
            playerCanDropCard = false,
            playerCanPickUpCard = false,
            enemyCanDropCard = false,
            enemyCanPickUpCard = true,
        };
        public static CardSlotPermissions Hand(CardOwner owner) => owner == CardOwner.player ? PlayerHand() : EnemyHand();
        public static CardSlotPermissions Field(CardOwner owner) => owner == CardOwner.player ? PlayerField() : EnemyField();
        public static CardSlotPermissions Deck(CardOwner owner)  => owner == CardOwner.player ? PlayerDeck() : EnemyDeck();
        
        public bool CanPickUp(CardOwner owner) => owner == CardOwner.enemy && enemyCanPickUpCard || owner == CardOwner.player && playerCanPickUpCard;
        public bool CanDropCard(CardOwner owner) => owner == CardOwner.enemy && enemyCanDropCard || owner == CardOwner.player && playerCanDropCard;
    }
    
    public class CardSlotModel : IDisposable
    {
        public CardPosition Position { get; protected set; }
        public CardSlotPermissions Permissions { get; protected set; }
        public CardModel Card { get; protected set; }
        public BattleModel BattleModel { get; protected set; }

        public CardSlotModel(BattleModel battleModel, CardPosition position, CardSlotPermissions permissions, CardModel startCard = null)
        {
            BattleModel = battleModel;
            Position = position;
            Permissions = permissions;
            Card = startCard;

            if (Card != null)
                Card.AttachedSlot = this;
            
            BattleModel.RegisterCardSlot(this);
        }

        public bool IsAvailableForPickUp(CardOwner owner) => Permissions.CanPickUp(owner) && Card != null;
        public bool IsAvailableForDrop(CardOwner owner) => Permissions.CanDropCard(owner) && Card == null;
        
        internal CardModel TakeCard()
        {
            var savedPtr = Card;
            Card.AttachedSlot = null;
            Card = null;
            return savedPtr;
        }

        internal void PlaceCard(CardModel card)
        {
            Card = card;
            Card.AttachedSlot = this;
        }
        
        public void Dispose()
        {
            BattleModel.UnregisterCardSlot(this);
        }
    }
}
