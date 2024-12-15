using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Change Damage")]
    public class ChangeDamageEffect : CardEffect
    {
        public override void ApplyEffect(CardModel card, int value)
        {
            card.ModifyAttackDamage(value);
        }
        public override void ApplyEffect(CardPlayerModel player, int value)
        {
        }
    }
}
