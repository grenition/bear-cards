using Cysharp.Threading.Tasks;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.CardPlayers;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class PlayerBehaviour : BaseEntityBehaviour
    {
        public CardPlayerModel PlayerModel => BattleBehaviour.Model.Player;
        
        protected override async void OnFirstTurnStart()
        {
            for (int i = 0; i < PlayerModel.Config.CardsAtFirstTurn; i++)
            {
                if(PlayerModel.TransferCardFromDeckToHand());
                    await UniTask.WaitForSeconds(0.1f);
            }

            await UniTask.WaitForSeconds(0.1f);
            
            for (int i = 0; i < PlayerModel.Config.SpellsSize; i++)
            {
                if(PlayerModel.TransferCardFromDeckToSpells());
                    await UniTask.WaitForSeconds(0.1f);
            }
        }
        protected override async void OnTurnStart()
        {
            if(TurnIndex == 0) return;
            
            PlayerModel.TransferCardFromDeckToHand();
            
            await UniTask.WaitForSeconds(0.1f);
            for (int i = 0; i < PlayerModel.Config.SpellsSize; i++)
            {
                if(PlayerModel.TransferCardFromDeckToSpells());
                await UniTask.WaitForSeconds(0.1f);
            }
        }
        
        public PlayerBehaviour(BattleBehaviour battleModel) : base(battleModel) { }
    }
}
