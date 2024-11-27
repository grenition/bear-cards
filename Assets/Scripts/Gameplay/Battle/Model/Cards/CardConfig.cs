using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    public enum CardType
    {
        Metal,
        NonMetal,
        Spell
    }
    public enum CardRarity
    {
        Standart,
        Rare,
        VeryRare,
        Legendary
    }
    public enum SpellPlacing
    {
        PlayerCard,
        EnemyCard,
        AnyCard,
        PlayerField,
        EnemyField,
        AllFields
    }
    public enum SpellPlayersPlacing
    {
        OnlyCards,
        PlayerAndCards,
        EnemyAndCards,
        PlayerOnly,
        EnemyOnly
    }
    
    [CreateAssetMenu(menuName = "Gameplay/CardConfig", fileName = "Card")]
    public class CardConfig : ScriptableObject
    {
        [field: Header("Visual")]
        [field: SerializeField] public string VisualShortName { get; private set; }
        [field: SerializeField] public string VisualName { get; private set; }
        [field: SerializeField, TextArea] public string VisualDescription { get; private set; }
        [field: SerializeField] public string ElectroFormula { get; private set; }
        [field: SerializeField] public Sprite VisualIcon { get; private set; }
        
        [field: Header("Parameters")]
        [field: SerializeField] public CardType CardType { get; private set; }
        [field: SerializeField] public CardRarity Rarity { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int BaseHealth { get; private set; }
        
        [field: Header("Attack")]
        [field: SerializeField] public int BaseDamage { get; private set; }
        [field: SerializeField] public SpellPlacing SpellPlacing { get; private set; }
        [field: SerializeField] public SpellPlayersPlacing SpellPlayersPlacing { get; private set; }
    }
}
