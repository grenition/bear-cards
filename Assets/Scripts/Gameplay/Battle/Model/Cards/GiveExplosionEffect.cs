using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Give Explosion")]
    public class GiveExplosionEffect : CardEffect
    {
        public override void ApplyEffect(CardModel card, int value)
        {
            card.Effects.Add(EffectTypes.Explosion);
        }
        public override void ApplyEffect(CardPlayerModel player, int value)
        {
        }
    }
}
