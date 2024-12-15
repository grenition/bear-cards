using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Give BlockFlying")]
    public class GiveBlockFlyingEffect : CardEffect
    {
        public override void ApplyEffect(CardModel card, int value)
        {
            card.Effects.Add(EffectTypes.BlockFlying);
        }
        public override void ApplyEffect(CardPlayerModel player, int value)
        {
        }
    }
}
