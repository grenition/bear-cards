using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;

namespace Project.UI.Battle
{
    public class UIPlayerHand : UICardsHand
    {
        private void OnEnable()
        {
            BattleController.Model.OnCardTransfered += OnCardTranfered;
        }
        private void OnDisable()
        {
            BattleController.Model.OnCardTransfered -= OnCardTranfered;
        }
        
        private void OnCardTranfered(CardModel card, CardPosition fromPosition, CardPosition toPosition)
        {
            if(toPosition.owner != CardOwner.player && toPosition.container != CardContainer.hand) return;

            AddSynamicSlot(card.AttachedSlot);
        }
    }
}
