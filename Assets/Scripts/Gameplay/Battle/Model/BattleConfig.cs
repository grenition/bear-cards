using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project.Gameplay.Battle.Model
{
    [CreateAssetMenu(menuName = "Gameplay/BattleConfig", fileName = "BattleConfig")]
    public class BattleConfig : ScriptableObject
    {
        [field: Header("Game")]
        [field: SerializeField] public CardPlayerConfig Enemy { get; private set; }
        [field: SerializeField] public int FieldSize { get; private set; } = 4;
        [field: SerializeField] public CardOwner FirstTurnOwner { get; private set; } = CardOwner.player;
    }
}
