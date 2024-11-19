using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Model.CardSlots;
using R3;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UICardsHand : MonoBehaviour
    {
        [SerializeField] private UICardMovement cardPrefab;
        [SerializeField] private UICardSlot cardSlotPrefab;
        [SerializeField] private Transform cardSlotsRoot;
        [SerializeField] private Transform cardsSpawnPoint;
        [SerializeField] private float cardsSpawnDelay = 0.1f;

        private List<UICardSlot> slots = new();

        public async void AddDynamicSlots(IEnumerable<CardSlotModel> newSlots)
        {
            foreach (var slot in newSlots.ToList())
            {
                AddSynamicSlot(slot);
                await UniTask.WaitForSeconds(cardsSpawnDelay);
            }
        }
        
        public async void AddSynamicSlot(CardSlotModel slotModel)
        {
            if(slotModel == null || slotModel.Card == null) return;
            
            var newSlot = Instantiate(cardSlotPrefab, cardSlotsRoot);
            var newCard = Instantiate(cardPrefab, cardsSpawnPoint.position, cardsSpawnPoint.rotation, UICardsHandler.instance.transform);
            newSlot.Init(slotModel);
            slots.Add(newSlot);
            
            void OnCardRemoved(UICardMovement obj)
            {
                Destroy(newSlot.gameObject);
                slots.TryRemove(newSlot);
            }
            
            newSlot.OnCardRemoved += OnCardRemoved;
            Disposable.Create(() =>
            {
                newSlot.OnCardRemoved -= OnCardRemoved;
            }).AddTo(this).AddTo(newSlot);

            await UniTask.NextFrame();
            newCard.TryPlaceCard(newSlot);
            newCard.uiCardVisual.GetComponent<UICard>().Init(slotModel.Card);
        }
    }
}
