using UnityEngine;

namespace Project.UI.Battle
{
    public class UICardSlot : MonoBehaviour
    {
        private UICardMovement currentCard;
        
        public bool TryPlaceCard(UICardMovement card)
        {
            if(card == null || currentCard != null) return false;
            
            card.transform.position = transform.position;
            currentCard = card;
            return true;
        }
        public bool TryRemoveCard(UICardMovement card)
        {
            if(card == null || card != currentCard) return false;

            currentCard = null;
            return true;
        }
    }
}
