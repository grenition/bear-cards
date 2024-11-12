using System;
using Gameplay.Battle.CardPlayers;
using Gameplay.Battle.Cards;
using Infrastructure;

namespace Gameplay.Battle
{
    public class BattleModel
    {
        public CardPlayerModel player;
        public CardPlayerModel enemy;
        
        public CardModel[] playerField = new CardModel[4];
        public CardModel[] enemyField = new CardModel[4];
        
        public BattleModel(string enemyKey)
        {
            this.player = new CardPlayerModel(Constants.Player, this);
            this.enemy = new CardPlayerModel(enemyKey, this);
        }

        public bool TryGetCardPosition(CardModel model, out CardPosition cardPosition)
        {
            cardPosition = default;
            if (model == null) return false;

            for (int i = 0; i < playerField.Length; i++)
            {
                if (playerField[i] == model)
                {
                    cardPosition = new CardPosition(CardContainer.field, CardOwner.player, i);
                    return true;
                }
            }
            for (int i = 0; i < enemyField.Length; i++)
            {
                if (enemyField[i] == model)
                {
                    cardPosition = new CardPosition(CardContainer.field, CardOwner.enemy, i);
                    return true;
                }
            }

            if (player.TryGetCardPosition(model, out cardPosition))
                return true;
            if (enemy.TryGetCardPosition(model, out cardPosition))
                return true;

            return false;
        }
        public bool IsCardPlayerEnemy(CardPlayerModel cardPlayerModel) => enemy == cardPlayerModel;

    }
}
