using System;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.CardPlayers;
using Project.Gameplay.Battle.Cards;
using Project.Infrastructure;

namespace Project.Gameplay.Battle
{
    public class BattleModel
    {
        public event Action<CardModel, CardPosition> OnCardPlaced;
        public event Action<CardModel, CardPosition> OnCardPickedUp;

        public CardPlayerModel Player;
        public CardPlayerModel Enemy;

        public CardModel[] PlayerField = new CardModel[4];
        public CardModel[] EnemyField = new CardModel[4];

        public BattleModel(string enemyKey)
        {
            this.Player = new CardPlayerModel(Constants.Player, this);
            this.Enemy = new CardPlayerModel(enemyKey, this);
        }

        public bool TryGetCardPosition(CardModel model, out CardPosition cardPosition)
        {
            cardPosition = default;
            if (model == null) return false;

            for (int i = 0; i < PlayerField.Length; i++)
            {
                if (PlayerField[i] == model)
                {
                    cardPosition = new CardPosition(CardContainer.field, CardOwner.player, i);
                    return true;
                }
            }
            for (int i = 0; i < EnemyField.Length; i++)
            {
                if (EnemyField[i] == model)
                {
                    cardPosition = new CardPosition(CardContainer.field, CardOwner.enemy, i);
                    return true;
                }
            }

            if (Player.TryGetCardPosition(model, out cardPosition))
                return true;
            if (Enemy.TryGetCardPosition(model, out cardPosition))
                return true;

            return false;
        }
        public bool IsEnemy(CardPlayerModel cardPlayerModel) => Enemy == cardPlayerModel;
        public CardModel GetCardAtPosition(CardPosition position)
        {
            switch (position.owner)
            {
                case CardOwner.player:
                    switch (position.container)
                    {
                        case CardContainer.deck:
                            return Player.Deck.GetAt(position.index);
                        case CardContainer.hand:
                            return Player.Hand.GetAt(position.index);
                        case CardContainer.spells:
                            return Player.Spells.GetAt(position.index);
                        case CardContainer.field:
                            return PlayerField.GetAt(position.index);
                    }
                    break;
                case CardOwner.enemy:
                    switch (position.container)
                    {
                        case CardContainer.deck:
                            return Enemy.Deck.GetAt(position.index);
                        case CardContainer.hand:
                            return Enemy.Hand.GetAt(position.index);
                        case CardContainer.spells:
                            return Enemy.Spells.GetAt(position.index);
                        case CardContainer.field:
                            return EnemyField.GetAt(position.index);
                    }
                    break;
            }
            return null;
        }
        public bool IsPlaceAvailableForPlayerCard(CardPosition position)
        {
            return position.container == CardContainer.field
                   && position.owner == CardOwner.player
                   && GetCardAtPosition(position) == null;
        }

        public CardModel PickUpCard(CardPosition position)
        {
            CardModel PickUpFromField(CardModel[] field, int index)
            {
                if (index >= field.Length) return null;
                var savedPtr = field[index];
                field[index] = null;
                return savedPtr;
            }

            CardModel PickUp()
            {
                switch (position.owner)
                {
                    case CardOwner.player:
                        switch (position.container)
                        {
                            case CardContainer.field:
                                return PickUpFromField(PlayerField, position.index);
                            default:
                                return Player.PickUpCard(position);
                        }
                    case CardOwner.enemy:
                        switch (position.container)
                        {
                            case CardContainer.field:
                                return PickUpFromField(EnemyField, position.index);
                            default:
                                return Enemy.PickUpCard(position);
                        }
                }

                return null;
            }

            var result = PickUp();
            if (result != null)
                OnCardPickedUp.SafeInvoke(result, position);
            
            return result;
        }

        public bool TryPlaceCard(CardModel model, CardPosition position)
        {
            bool TryPlaceToField(CardModel[] field, int index)
            {
                if (index >= field.Length) return false;
                if (field[index] != null) return false;
                field[index] = model;
                return true;
            }

            bool TryPlace()
            {

                PickUpCard(model.Position);

                switch (position.owner)
                {
                    case CardOwner.player:
                        switch (position.container)
                        {
                            case CardContainer.field:
                                return TryPlaceToField(PlayerField, position.index);
                            default:
                                return Player.TryPlaceCard(model, position);
                        }
                    case CardOwner.enemy:
                        switch (position.container)
                        {
                            case CardContainer.field:
                                return TryPlaceToField(PlayerField, position.index);
                            default:
                                return Enemy.TryPlaceCard(model, position);
                        }
                }

                return false;
            }

            var result = TryPlace();
            if (result)
                OnCardPlaced.SafeInvoke(model, position);
            
            return result;
        }

    }
}
