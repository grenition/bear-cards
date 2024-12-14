using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Change Damage")]
    public class ChangeDamageEffect : CardEffect
    {
        public int DamageChange;
        public override void ApplyEffect(CardModel card)
        {
            card.ModifyAttackDamage(DamageChange);
        }
    }
}
