using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project.Gameplay.Battle.Craft
{
    [CreateAssetMenu(menuName = "Gameplay/CardCraftConfig", fileName = "CardCraftConfig")]
    public class CardCraftConfig : ScriptableObject
    {
        [field: SerializeField] public CardConfig[] Metals { get; private set; } = new CardConfig[5];
        [field: SerializeField] public CardConfig[] NonMetals { get; private set; } = new CardConfig[5];

        [field: SerializeField] public CardConfig Output { get; private set; }
        [field: SerializeField] public string Formula { get; private set; }
    }
}
