using System;
using GreonAssets.Extensions;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Battle.Cards
{
    [Serializable]
    public class CardModel
    {
        public CardConfig Config => StaticData.Cards.Get(key);
        
        public string key;
    }
}
