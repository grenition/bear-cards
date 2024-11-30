using System.Collections.Generic;
using Project.Gameplay.Common;
using GreonAssets.Extensions;
using Project.Audio;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.UI.Common;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIBattle : MonoBehaviour
    {
        public static UIBattle Instance;
        public IReadOnlyDictionary<CardSlotModel, UICardSlot> Slots => _slots;
        public IReadOnlyDictionary<CardModel, UICardMovement> Cards => _cards;

        [Header("Audio")]
        [SerializeField] private AudioClip _cardTransferedClip;

        [Header("Bsse movement prefab")]
        [SerializeField] private UICardMovement _cardMovementPrefab;
        [Header("Card visuals")]
        [SerializeField] private UICardVisual _hydrogenCardPrefab;
        [SerializeField] private UICardVisual _spellCardPrefab;
        [SerializeField] private UICardVisual _metalStandartCardPrefab;
        [SerializeField] private UICardVisual _metalRareCardPrefab;
        [SerializeField] private UICardVisual _metalVeryRareCardPrefab;
        [SerializeField] private UICardVisual _metalLegendaryCardPrefab;        
        [SerializeField] private UICardVisual _nonMetalStandartCardPrefab;
        [SerializeField] private UICardVisual _nonMetalRareCardPrefab;
        [SerializeField] private UICardVisual _nonMetalVeryRareCardPrefab;
        [SerializeField] private UICardVisual _nonMetalLegendaryCardPrefab;

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
            GameAudio.MusicSource.clip = BattleController.Model.Config.Music;
            GameAudio.MusicSource.Play();
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
                card = Instantiate(_cardMovementPrefab, UICardsHandler.instance.transform);
                card.Init(cardModel, GetCardPrefab(cardModel));
                card.transform.position = deck.transform.position;
            }

            GameAudio.MusicSource.PlayOneShot(_cardTransferedClip, 0.3f);
        }

        private UICardVisual GetCardPrefab(CardModel model)
        {
            if (model.Key == "card_hydrogen")
                return _hydrogenCardPrefab;

            switch (model.Type)
            {
                case CardType.Spell: return _spellCardPrefab;
                case CardType.Metal:
                    switch (model.Config.Rarity)
                    {
                        case CardRarity.Standart: return _metalStandartCardPrefab;
                        case CardRarity.Rare: return _metalRareCardPrefab;
                        case CardRarity.VeryRare: return _metalVeryRareCardPrefab;
                        case CardRarity.Legendary: return _metalLegendaryCardPrefab;
                    }
                    break;
                case CardType.NonMetal:
                    switch (model.Config.Rarity)
                    {
                        case CardRarity.Standart: return _nonMetalStandartCardPrefab;
                        case CardRarity.Rare: return _nonMetalRareCardPrefab;
                        case CardRarity.VeryRare: return _nonMetalVeryRareCardPrefab;
                        case CardRarity.Legendary: return _nonMetalLegendaryCardPrefab;
                    }
                    break;
            }
            
            return _hydrogenCardPrefab;
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
