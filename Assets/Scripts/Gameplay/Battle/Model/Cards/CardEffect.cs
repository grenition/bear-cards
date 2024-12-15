using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    public abstract class CardEffect : ScriptableObject
    {
        public abstract void ApplyEffect(CardModel card, int value);
        public abstract void ApplyEffect(CardPlayerModel player, int value);
    }
    
}
