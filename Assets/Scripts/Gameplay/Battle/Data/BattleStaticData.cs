using System;
using System.Collections.Generic;
using System.Linq;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project.Gameplay.Battle.Data
{
    public static class BattleStaticData
    {
        public static Dictionary<string, CardConfig> Cards => _cards.Value;
        public static Dictionary<string, CardPlayerConfig> CardPlayers => _cardPlayers.Value;
        public static Dictionary<string, BattleConfig> Battles => _battles.Value;
        
        private static Lazy<Dictionary<string, CardConfig>> _cards;
        private static Lazy<Dictionary<string, CardPlayerConfig>> _cardPlayers;
        private static Lazy<Dictionary<string, BattleConfig>> _battles;
        
        static BattleStaticData()
        {
            _cards = new(() => Resources.LoadAll<CardConfig>("Gameplay/Cards").ToDictionary(x => x.name, x => x));
            _cardPlayers = new(() => Resources.LoadAll<CardPlayerConfig>("Gameplay/CardPlayers").ToDictionary(x => x.name, x => x));
            _battles = new(() => Resources.LoadAll<BattleConfig>("Gameplay/Battles").ToDictionary(x => x.name, x => x));
        }
    }
}
