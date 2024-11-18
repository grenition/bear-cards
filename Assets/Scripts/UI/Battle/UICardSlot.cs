using System;
using DG.Tweening;
using GreonAssets.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICardSlot : MonoBehaviour
    {
        public bool IsAvailable => PlayerCanDropCard && !CurrentCard;
        
        [field: Header("Game preferences")]
        [field: SerializeField] public bool AllowCardCurvePositioning { get; protected set; }
        [field: SerializeField] public bool PlayerCanPickUpCard { get; protected set; }
        [field: SerializeField] public bool PlayerCanDropCard { get; protected set; }

        [Header("Visual")]
        [SerializeField] private float colorTransition = 0.2f;
        [SerializeField] private Image background;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private Ease highlightEase;
        [SerializeField] private bool highlightable = false;
        
        public UICardMovement CurrentCard { get; protected set; }
        
        public Action<UICardMovement> OnCardAdded; 
        public Action<UICardMovement> OnCardRemoved;

        private Color baseColor;
        private bool highlighted;

        private void Start()
        {
            baseColor = background.color;
        }

        public bool TryPlaceCard(UICardMovement card)
        {
            if(card == null || CurrentCard != null) return false;
            
            card.transform.position = transform.position;
            CurrentCard = card;

            card.Interactable = PlayerCanPickUpCard;

            OnCardAdded.SafeInvoke(CurrentCard);
            SetHighlight(false);
            return true;
        }
        public bool TryRemoveCard(UICardMovement card)
        {
            if(card == null || card != CurrentCard) return false;

            CurrentCard = null;
            OnCardRemoved.SafeInvoke(card);
            return true;
        }

        public void SetHighlight(bool activeSelf)
        {
            if(!highlightable) return;
            if(activeSelf == highlighted) return;
            highlighted = activeSelf;

            background
                .DOColor(highlighted ? highlightedColor : baseColor, colorTransition)
                .SetEase(highlightEase);
        }
    }
}
