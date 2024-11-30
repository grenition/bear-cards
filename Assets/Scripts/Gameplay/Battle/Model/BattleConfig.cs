using System.Collections.Generic;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;
using UnityEngine;

namespace Project.Gameplay.Battle.Model
{
    [CreateAssetMenu(menuName = "Gameplay/BattleConfig", fileName = "BattleConfig")]
    public class BattleConfig : ScriptableObject
    {
        [field: Header("Audio and visual")]
        [field: SerializeField] public AudioClip Music { get; private set; }
        
        [field: Header("Game")]
        [field: SerializeField] public CardPlayerConfig Enemy { get; private set; }
        [field: SerializeField] public int FieldSize { get; private set; } = 4;
        [field: SerializeField] public CardOwner FirstTurnOwner { get; private set; } = CardOwner.player;
        
        [field: Header("Electrons")]
        [field: SerializeField] public int MaxHandElectrons { get; private set; } = 15;
        [field: SerializeField] public int ElectronsAtTurn { get; private set; } = 2;
        [field: SerializeField] public int[] LevelElectrons { get; private set; } = { 0, 2, 10, 18, 36, 54, 72 };
        
        [field: Header("PlayerBehaviour")]
        [field: SerializeField] public int CardsAtFirstTurn { get; private set; } = 4;
        [field: SerializeField] public int CardsAtAnotherTurns { get; private set; } = 1;
        [field: SerializeField] public bool GiveCardsByActualLevel { get; private set; } = true;
        [field: SerializeField] public List<CardConfig> PreDeckCards { get; private set; }
        [field: SerializeField] public List<CardConfig> PostDeckCards { get; private set; }
    }
}
