using Project.Gameplay.Battle.Model.CardPlayers;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [CreateAssetMenu(menuName = "Card Effects/Give Poison")]
    public class GivePoisonEffect : CardEffect
    {
        public override void ApplyEffect(CardModel card, int value)
        {
            card.Effects.Add(EffectTypes.Poison);
        }
        public override void ApplyEffect(CardPlayerModel player, int value)
        {
        }
    }
}
