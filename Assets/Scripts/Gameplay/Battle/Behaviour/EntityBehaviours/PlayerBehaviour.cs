using Cysharp.Threading.Tasks;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.CardPlayers;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class PlayerBehaviour : BaseEntityBehaviour
    {
        public BattleModel BattleModel { get; protected set; }
        public CardPlayerModel PlayerModel => BattleModel.Player;
        
        public PlayerBehaviour(BattleModel battleModel)
        {
            BattleModel = battleModel;
        }

        protected override async void OnFirstTurnStart()
        {
            for (int i = 0; i < PlayerModel.Config.CardsAtFirstTurn; i++)
            {
                PlayerModel.TransferCardFromDeckToHand();
                await UniTask.WaitForSeconds(0.1f);
            }
        }
        protected override void OnTurnStart()
        {
            if(TurnIndex == 0) return;
            
            PlayerModel.TransferCardFromDeckToHand();
        }
    }
}
