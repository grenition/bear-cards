using System.Collections.Generic;
using Project.Gameplay.Battle.Cards;
using UnityEngine;

namespace Project.Gameplay.Battle.CardPlayers
{
    [CreateAssetMenu(menuName = "Gameplay/CardPlayerConfig", fileName = "CardPlayer")]
    public class CardPlayerConfig : ScriptableObject
    {
        [field: Header("Visual")]
        [field: SerializeField] public string VisualName { get; private set; }
        [field: SerializeField, TextArea] public string VisualDescription { get; private set; }
        
        [field: Header("Game")]
        [field: SerializeField] public List<CardConfig> Hand { get; private set; }
        [field: SerializeField] public int HandSize { get; private set; } = 8;
        [field: SerializeField] public List<CardConfig> Deck { get; private set; }
        [field: SerializeField] public int DeckSize { get; private set; } = 20;
        [field: SerializeField] public List<CardConfig> Spells { get; private set; }
        [field: SerializeField] public int SpellsSize { get; private set; } = 5;
    }
}
