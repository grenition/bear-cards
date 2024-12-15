using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Change Max Health")]
    public class ChangeMaxHealthEffect : CardEffect
    {
        public override void ApplyEffect(CardModel card, int value)
        {
            card.ModifyMaxHealth(value);
        }
        public override void ApplyEffect(CardPlayerModel player, int value)
        {
            player.ModifyHealth(value);
        }
    }
}
