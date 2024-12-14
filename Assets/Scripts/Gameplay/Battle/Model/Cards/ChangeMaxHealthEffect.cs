using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Change Max Health")]
    public class ChangeMaxHealthEffect : CardEffect
    {
        public int MaxHealthChange;
        public override void ApplyEffect(CardModel card)
        {
            card.ModifyMaxHealth(MaxHealthChange);
        }
    }
}
