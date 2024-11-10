using System;
using Gameplay.Battle.CardPlayers;
using Gameplay.Battle.Cards;

namespace Gameplay.Battle
{
    [Serializable]
    public class BattleModel
    {
        public CardPlayerModel player;
        public CardPlayerModel enemy;
        
        public CardModel[] playerField = new CardModel[4];
        public CardModel[] enemyField = new CardModel[4];
        
        public BattleModel(CardPlayerModel player, CardPlayerModel enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }
    }
}
