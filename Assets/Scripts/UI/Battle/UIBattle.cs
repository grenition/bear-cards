using System.Collections.Generic;
using GreonAssets.Extensions;
using Project.Audio;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Gameplay.Common.Datas;
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

        [Header("Base movement prefab")]
        [SerializeField] private UICardMovement _cardMovementPrefab;
        
        [Header("Visual prefabs")]
        [SerializeField] private UICardVisual _cardPrefab;
        [SerializeField] private UICardVisual _spellCardPrefab;

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
                card.Init(cardModel, cardModel.Type == CardType.Spell ? _spellCardPrefab : _cardPrefab);
                card.transform.position = deck.transform.position;
            }

            GameAudio.MusicSource.PlayOneShot(_cardTransferedClip, 0.3f);
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
