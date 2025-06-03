using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Craft;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Gameplay.Common.Datas;
using Project.UI.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICraftCardMovement : UICardMovement, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
       [SerializeField]  protected UICardSlot FixedSlot;

        public override void Init(CardModel cardModel, UICardVisual overrideVisual = null)
        {
            base.Init(cardModel, overrideVisual);
            FixedSlot = CardSlot;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!Interactable && !pointerPressed)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            pointerPressed = false;

            slotUnderCursor = GetAvailableCardSlotUnderCursor(eventData);
            slotUnderCursor?.SetHighlight(false);
            UIDynamicSelector.Instance?.SetSelection(new List<RectTransform>());

            if (!slotUnderCursor || !BattleController.Model.TryTransferCard(Model.Position, slotUnderCursor.CardPosition))
            {
                if (BattleController.Model.Config.AllowPlayerCardReposition && Model.Position.container == CardContainer.field)
                {
                    //var handSlot = BattleController.Model.Player.GetFirstFreeSlotInHand();
                    if (FixedSlot == null || !BattleController.Model.TryTransferCard(Model.Position, FixedSlot.Model.Position))
                    {
                        print("соси");
                        transform.DOMove(CardSlot ? CardSlot.transform.position : startPosition, moveTime).SetEase(Ease.OutBack);
                    }
                    else
                    {
                        //CardSlot.transform.SetAsFirstSibling();
                        //transform.SetAsFirstSibling();
                        //uiCardVisual.transform.SetAsFirstSibling();
                    }
                }
                else
                {
                    transform.DOMove(CardSlot ? CardSlot.transform.position : startPosition, moveTime).SetEase(Ease.OutBack);
                }
            }

            PointerUpEvent?.Invoke(this, false);
        }
    }
}
