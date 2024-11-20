using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.CardPlayers;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class EnemyBehaviour : BaseEntityBehaviour
    {
        public CardPlayerModel PlayerModel => BattleBehaviour.Model.Enemy;
        
        public EnemyBehaviour(BattleBehaviour battleBehaviour) : base(battleBehaviour) { }
    }
}
