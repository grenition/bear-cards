using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Heal")]
    public class HealEffect : CardEffect
    {
        public override void ApplyEffect(CardModel card, int value)
        {
            card.ModifyHealth(value);
        }
        public override void ApplyEffect(CardPlayerModel player, int value)
        {
            player.ModifyHealth(value);
        }
    }
}
