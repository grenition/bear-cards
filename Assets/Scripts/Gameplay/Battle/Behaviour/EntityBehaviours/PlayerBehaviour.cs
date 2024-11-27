using Cysharp.Threading.Tasks;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class PlayerBehaviour : BaseEntityBehaviour
    {
        public CardPlayerModel PlayerModel => BattleBehaviour.Model.Player;
        
        protected override async void OnFirstTurnStart()
        {
            await UniTask.NextFrame();
            foreach (var preCard in BattleBehaviour.Config.PreDeckCards)
            {
                BattleBehaviour.Model.AddCardToDeck(CardOwner.player, preCard.name);
            }
            foreach (var deckCard in BattleBehaviour.Model.Player.Config.Deck)
            {
                BattleBehaviour.Model.AddCardToDeck(CardOwner.player, deckCard.name);
            }
            
            for (int i = 0; i < BattleBehaviour.Config.CardsAtFirstTurn; i++)
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
            PlayerModel.AddTurnElectrons();
            
            if(TurnIndex == 0) return;

            if (PlayerModel.GetFirstCardInDeck() == null)
            {
                foreach (var postCard in BattleBehaviour.Config.PostDeckCards)
                {
                    BattleBehaviour.Model.AddCardToDeck(CardOwner.player, postCard.name);
                }
            }
            
            for (int i = 0; i < BattleBehaviour.Config.CardsAtAnotherTurns; i++)
            {
                PlayerModel.TransferCardFromDeckToHand();
            }

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
