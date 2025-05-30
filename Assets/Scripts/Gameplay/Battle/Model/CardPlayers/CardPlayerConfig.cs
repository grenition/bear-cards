using System;
using System.Collections.Generic;
using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.CardPlayers
{
    [CreateAssetMenu(menuName = "Gameplay/CardPlayerConfig", fileName = "CardPlayer")]
    public class CardPlayerConfig : ScriptableObject
    {
        [Serializable]
        public class Turn
        {
            [field: SerializeField, Range(0f, 1f)] public float PlaceToSlotChance { get; private set; } = 0.5f;
            [field: SerializeField] public List<CardConfig> Cards { get; private set; }
        }
        
        [field: Header("Visual")]
        [field: SerializeField] public string VisualName { get; private set; }
        [field: SerializeField, TextArea] public string VisualDescription { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: Header("Game")]
        [field: SerializeField] public int Health { get; private set; } = 10;
        [field: SerializeField] public int HandSize { get; private set; } = 8;
        [field: SerializeField] public int DeckSize { get; private set; } = 20;
        [field: SerializeField] public int SpellsSize { get; private set; } = 5;
        [field: SerializeField] public int StartLevelElectrons { get; private set; } = 0;
        [field: SerializeField] public int StartHandElectrons { get; private set; } = 0;

        [field: Header("EnemyBehaviour")]
        [field: SerializeField] public List<Turn> Turns { get; private set; }
        
        [field: Header("Optional - predefined cards")]
        [field: SerializeField] public List<CardConfig> Deck { get; private set; }
    }
}
