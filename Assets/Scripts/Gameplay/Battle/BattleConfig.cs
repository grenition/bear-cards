using Project.Gameplay.Battle.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle
{
    [CreateAssetMenu(menuName = "Gameplay/BattleConfig", fileName = "BattleConfig")]
    public class BattleConfig : ScriptableObject
    {
        [field: Header("Game")]
        [field: SerializeField] public CardPlayerConfig Enemy { get; private set; }
        [field: SerializeField] public int FieldSize { get; private set; } = 4;
    }
}
