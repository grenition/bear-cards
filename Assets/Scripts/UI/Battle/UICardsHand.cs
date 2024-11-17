using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Cards;
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

        public async void AddCards(IEnumerable<CardModel> cards)
        {
            foreach (var card in cards.ToList())
            {
                AddCard(card);
                await UniTask.WaitForSeconds(cardsSpawnDelay);
            }
        }
        
        public async void AddCard(CardModel cardModel)
        {
            var newSlot = Instantiate(cardSlotPrefab, cardSlotsRoot);
            var newCard = Instantiate(cardPrefab, cardsSpawnPoint.position, cardsSpawnPoint.rotation, UICardsHandler.instance.transform);
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
            newCard.uiCardVisual.GetComponent<UICard>().Init(cardModel);
        }
    }
}
