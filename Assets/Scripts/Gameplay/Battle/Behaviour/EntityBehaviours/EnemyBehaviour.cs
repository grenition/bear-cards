using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.CardPlayers;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class EnemyBehaviour : BaseEntityBehaviour
    {
        public BattleModel BattleBattleModel { get; protected set; }
        public CardPlayerModel PlayerModel => BattleBattleModel.Player;
        
        public EnemyBehaviour(BattleModel battleModel)
        {
            BattleBattleModel = battleModel;
        }

    }
}
