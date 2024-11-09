using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Battle.CardPlayers;
using Gameplay.Battle.Cards;
using UnityEngine;

namespace Infrastructure
{
    public static class StaticData
    {
        public static Dictionary<string, CardConfig> Cards => _cards.Value;
        public static Dictionary<string, CardPlayerConfig> CardPlayers => _cardPlayers.Value;
        
        private static Lazy<Dictionary<string, CardConfig>> _cards;
        private static Lazy<Dictionary<string, CardPlayerConfig>> _cardPlayers;
        
        static StaticData()
        {
            _cards = new(() => Resources.LoadAll<CardConfig>("Gameplay/Cards").ToDictionary(x => x.name, x => x));
            _cardPlayers = new(() => Resources.LoadAll<CardPlayerConfig>("Gameplay/CardPlayers").ToDictionary(x => x.name, x => x));
        }
    }
}
