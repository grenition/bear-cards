using DG.Tweening;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Gameplay.Common;
using Project.Gameplay.Common.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICardSlot : MonoBehaviour
    {
        public bool IsAvailable => BattleController.Model.IsPlaceAvailableForPlayerCard(CardPosition);
        public CardSlotModel Model { get; protected set; }
        public bool PlayerCanPickUpCard => Model.IsAvailableForPickUp(CardOwner.player);
        public bool PlayerCanDropCard => Model.IsAvailableForDrop(CardOwner.player);
        public CardPosition CardPosition => Model.Position;
        
        [Header("Visual")]
        [field: SerializeField] public bool AllowCardCurvePositioning { get; protected set; }
        [SerializeField] private float colorTransition = 0.2f;
        [SerializeField] private Image background;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private Ease highlightEase;
        [SerializeField] private bool highlightable = false;
        
        private Color baseColor;
        private bool highlighted;

        public void Init(CardSlotModel slotModel)
        {
            Model = slotModel;
        }
        private void Start()
        {
            baseColor = background.color;
            UIBattle.Instance.RegisterSlot(this);
        }
        private void OnDestroy()
        {
            UIBattle.Instance.UnregisterSlot(this);
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
