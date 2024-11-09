using System;
using System.Collections.Generic;
using Gameplay.Battle.Cards;
using GreonAssets.Extensions;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Battle.CardPlayers
{
    [Serializable]
    public class CardPlayerModel
    {
        public CardPlayerConfig Config => StaticData.CardPlayers.Get(key);
        
        public string key;
        public List<CardModel> hand = new();
        public List<CardModel> deck = new();
    }
}
