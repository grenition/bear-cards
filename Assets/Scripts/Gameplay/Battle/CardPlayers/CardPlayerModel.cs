using System;
using System.Collections.Generic;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Cards;
using Project.Gameplay.Battle.CardSlots;
using Project.Gameplay.Data;

namespace Project.Gameplay.Battle.CardPlayers
{
    public class CardPlayerModel : IDisposable
    {
        public string Key { get; protected set; }
        public CardPlayerConfig Config => StaticData.CardPlayers.Get(Key);
        public BattleModel BattleModel { get; protected set; }
        public CardOwner OwnershipType { get; protected set; }
        public List<CardSlotModel> Hand { get; protected set; } = new();
        public List<CardSlotModel> Deck { get; protected set; } = new();
        public List<CardSlotModel> Spells { get; protected set; } = new();

        public CardPlayerModel(string key, CardOwner ownerhipType, BattleModel battleModel)
        {
            Key = key;
            BattleModel = battleModel;
            OwnershipType = ownerhipType;

            for (int i = 0; i < Config.HandSize; i++)
            {
                var cardConfig = Config.Hand.GetAt(i);
                var card = cardConfig ? new CardModel(cardConfig.name, BattleModel) : null;
                Hand.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.hand, OwnershipType, i), CardSlotPermissions.Hand(OwnershipType), card));
            }
            for (int i = 0; i < Config.DeckSize; i++)
            {
                var cardConfig = Config.Deck.GetAt(i);
                var card = cardConfig ? new CardModel(cardConfig.name, BattleModel) : null;
                Deck.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.deck, OwnershipType, i), CardSlotPermissions.Deck(OwnershipType), card));
            }
            for (int i = 0; i < Config.SpellsSize; i++)
            {
                var cardConfig = Config.Spells.GetAt(i);
                var card = cardConfig ? new CardModel(cardConfig.name, BattleModel) : null;
                Spells.Add(new CardSlotModel(BattleModel, new CardPosition(CardContainer.spells, OwnershipType, i), CardSlotPermissions.Hand(OwnershipType), card));
            }
        }
        public void Dispose()
        {
            Hand.ForEach(x => x.Dispose());
            Deck.ForEach(x => x.Dispose());
            Spells.ForEach(x => x.Dispose());
        }
    }
}
