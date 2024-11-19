using System.Collections.Generic;
using System.Linq;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Cards;
using Project.Gameplay.Data;

namespace Project.Gameplay.Battle.CardPlayers
{
    public class CardPlayerModel
    {
        public CardPlayerConfig Config => StaticData.CardPlayers.Get(_key);
        public CardOwner OwnershipType => _battleModel.IsEnemy(this) ? CardOwner.enemy : CardOwner.player;
        public IReadOnlyList<CardModel> Hand => _hand;
        public IReadOnlyList<CardModel> Deck => _deck;
        public IReadOnlyList<CardModel> Spells => _spells;
        
        protected string _key;
        protected List<CardModel> _hand;
        protected List<CardModel> _deck;
        protected List<CardModel> _spells;
        protected BattleModel _battleModel;

        public CardPlayerModel(string key, BattleModel battleModel)
        {
            _key = key;
            _battleModel = battleModel;
            _hand = Config.Hand.Select(x => new CardModel(x.name, battleModel)).ToList();
            _deck = Config.Deck.Select(x => new CardModel(x.name, battleModel)).ToList();
            _spells = Config.Spells.Select(x => new CardModel(x.name, battleModel)).ToList();
        }

        public bool TryGetCardPosition(CardModel cardModel, out CardPosition cardPosition)
        {
            cardPosition = default;
            if (cardModel == null) return false;

            for (int i = 0; i < _hand.Count; i++)
            {
                if (_hand[i] == cardModel)
                {
                    cardPosition = new CardPosition(CardContainer.hand, OwnershipType, i);
                    return true;
                }
            }
            for (int i = 0; i < _deck.Count; i++)
            {
                if (_deck[i] == cardModel)
                {
                    cardPosition = new CardPosition(CardContainer.deck, OwnershipType, i);
                    return true;
                }
            }
            for (int i = 0; i < _spells.Count; i++)
            {
                if (_spells[i] == cardModel)
                {
                    cardPosition = new CardPosition(CardContainer.spells, OwnershipType, i);
                    return true;
                }
            }

            return false;
        }
        
        public CardModel PickUpCard(CardPosition position)
        {
            if (position.owner != OwnershipType) return null;

            CardModel PickUpFromList(List<CardModel> list, int index)
            {
                if (index >= list.Count) return null;
                var savedPtr = list[index];
                list[index] = null;
                return savedPtr;
            }
            
            switch (position.container)
            {
                case CardContainer.deck:
                    return PickUpFromList(_deck, position.index);
                case CardContainer.hand:
                    return PickUpFromList(_hand, position.index);
                case CardContainer.spells:
                    return PickUpFromList(_spells, position.index);
                default:
                    return null;
            }
        }
        public bool TryPlaceCard(CardModel cardModel, CardPosition position)
        {
            if (cardModel == null) return false;
            if (position.owner != OwnershipType) return false;

            PickUpCard(cardModel.Position);

            bool SetOrAppendWithLimit(List<CardModel> list, int limit)
            {
                if (list.Count - 1 >= limit) return false;
                if (position.index < list.Count)
                {
                    if (list[position.index] != null) return false;
                    list[position.index] = cardModel;
                    return true;
                }
                list.Add(cardModel);
                return true;
            }
            
            switch (position.container)
            {
                case CardContainer.deck:
                    return SetOrAppendWithLimit(_deck, Config.DeckSize);
                case CardContainer.hand:
                    return SetOrAppendWithLimit(_hand, Config.HandSize);
                case CardContainer.spells:
                    return SetOrAppendWithLimit(_spells, Config.SpellsSize);
                default:
                    return false;
            }
        }
    }
}
