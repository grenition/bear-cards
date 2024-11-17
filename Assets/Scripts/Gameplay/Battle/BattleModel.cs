using Project.Gameplay.Battle.CardPlayers;
using Project.Gameplay.Battle.Cards;
using Project.Infrastructure;

namespace Project.Gameplay.Battle
{
    public class BattleModel
    {
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
        public bool IsCardPlayerEnemy(CardPlayerModel cardPlayerModel) => Enemy == cardPlayerModel;

    }
}
