using System.Collections.Generic;
using System.Linq;
using Gameplay.Battle.Cards;
using GreonAssets.Extensions;
using Infrastructure;

namespace Gameplay.Battle.CardPlayers
{
    public class CardPlayerModel
    {
        public CardPlayerConfig Config => StaticData.CardPlayers.Get(_key);
        public CardOwner OwnershipType => _battleModel.IsCardPlayerEnemy(this) ? CardOwner.enemy : CardOwner.player;
        
        protected string _key;
        protected List<CardModel> _hand;
        protected List<CardModel> _deck;
        protected BattleModel _battleModel;

        public CardPlayerModel(string key, BattleModel battleModel)
        {
            _key = key;
            _battleModel = battleModel;
            _hand = Config.Hand.Select(x => new CardModel(x.name, battleModel)).ToList();
            _deck = Config.Deck.Select(x => new CardModel(x.name, battleModel)).ToList();
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

            return false;
        }
    }
}
