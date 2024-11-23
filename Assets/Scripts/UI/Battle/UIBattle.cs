using System.Collections.Generic;
using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIBattle : MonoBehaviour
    {
        public static UIBattle Instance;
        public IReadOnlyDictionary<CardSlotModel, UICardSlot> Slots => _slots;
        public IReadOnlyDictionary<CardModel, UICardMovement> Cards => _cards;

        [SerializeField] private UICardMovement _cardPrefab;

        private Dictionary<CardSlotModel, UICardSlot> _slots = new();
        private Dictionary<CardModel, UICardMovement> _cards = new();
        private UIDeck _enemyDeck;
        private UIDeck _playerDeck;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            BattleController.Model.OnCardTransfered += OnCardTransfered;
        }
        private void OnDestroy()
        {
            BattleController.Model.OnCardTransfered -= OnCardTransfered;
        }
        
        private void OnCardTransfered(CardModel cardModel, CardPosition fromPosition, CardPosition toPosition)
        {
            var card = _cards.Get(cardModel);
            if (card == null)
            {
                var deck = fromPosition.owner == CardOwner.player ? _playerDeck : _enemyDeck;
                card = Instantiate(_cardPrefab, UICardsHandler.instance.transform);
                card.Init(cardModel);
                card.transform.position = deck.transform.position;
            }
        }

        #region Registry
        public void RegisterCard(UICardMovement card) => _cards.Set(card.Model, card);
        public void UnregisterCard(UICardMovement card) => _cards.Delete(card.Model);
        public void RegisterSlot(UICardSlot slot) => _slots.Set(slot.Model, slot);
        public void UnregisterSlot(UICardSlot slot) => _slots.Delete(slot.Model);
        public void RegisterDeck(UIDeck deck)
        {
            if (deck.Owner == CardOwner.player) _playerDeck = deck;
            else _enemyDeck = deck;
        }
        public void UnregisterDeck(UIDeck deck)
        {
            if (deck.Owner == CardOwner.player) _playerDeck = null;
            else _enemyDeck = null;
        }
        #endregion
    }
}
