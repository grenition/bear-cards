using System;
using Gameplay.Battle.CardPlayers;

namespace Gameplay.Battle
{
    [Serializable]
    public class BattleModel
    {
        public CardPlayerModel player;
        public CardPlayerModel enemy;
        
        public BattleModel(CardPlayerModel player, CardPlayerModel enemy)
        {
            
        }
    }
}
