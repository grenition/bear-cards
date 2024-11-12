using System.Collections.Generic;
using Gameplay.Battle.Cards;
using UnityEngine;

namespace Gameplay.Battle.CardPlayers
{
    [CreateAssetMenu(menuName = "Gameplay/CardPlayerConfig", fileName = "CardPlayer")]
    public class CardPlayerConfig : ScriptableObject
    {
        [field: Header("Visual")]
        [field: SerializeField] public string VisualName { get; private set; }
        [field: SerializeField, TextArea] public string VisualDescription { get; private set; }
        
        [field: Header("Game")]
        [field: SerializeField] public List<CardConfig> Hand { get; private set; }
        [field: SerializeField] public List<CardConfig> Deck { get; private set; }
    }
}
