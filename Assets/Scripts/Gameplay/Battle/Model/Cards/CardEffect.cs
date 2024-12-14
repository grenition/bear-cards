using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    public abstract class CardEffect : ScriptableObject
    {
        public abstract void ApplyEffect(CardModel card);
    }
}
