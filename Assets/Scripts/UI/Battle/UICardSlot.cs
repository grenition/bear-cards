using System;
using GreonAssets.Extensions;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UICardSlot : MonoBehaviour
    {
        public UICardMovement CurrentCard { get; protected set; }
        
        public Action<UICardMovement> OnCardAdded; 
        public Action<UICardMovement> OnCardRemoved; 
        
        public bool TryPlaceCard(UICardMovement card)
        {
            if(card == null || CurrentCard != null) return false;
            
            card.transform.position = transform.position;
            CurrentCard = card;

            OnCardAdded.SafeInvoke(CurrentCard);
            return true;
        }
        public bool TryRemoveCard(UICardMovement card)
        {
            if(card == null || card != CurrentCard) return false;

            CurrentCard = null;
            OnCardRemoved.SafeInvoke(card);
            return true;
        }
    }
}
