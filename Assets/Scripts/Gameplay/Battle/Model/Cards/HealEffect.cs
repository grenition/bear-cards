using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Heal")]
    public class HealEffect : CardEffect
    {
        public int HealValue;
        public override void ApplyEffect(CardModel card)
        {
            card.ModifyHealth(Mathf.Abs(HealValue));
        }
    }
}
