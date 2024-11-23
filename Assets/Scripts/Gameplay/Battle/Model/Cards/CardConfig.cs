using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    public enum CardType
    {
        Card,
        Spell
    }
    
    [CreateAssetMenu(menuName = "Gameplay/CardConfig", fileName = "Card")]
    public class CardConfig : ScriptableObject
    {
        [field: Header("Visual")]
        [field: SerializeField] public string VisualShortName { get; private set; }
        [field: SerializeField] public string VisualName { get; private set; }
        [field: SerializeField, TextArea] public string VisualDescription { get; private set; }
        [field: SerializeField] public Sprite VisualIcon { get; private set; }
        
        [field: Header("Parameters")]
        [field: SerializeField] public CardType CardType { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int BaseDamage { get; private set; }
        [field: SerializeField] public int BaseHealth { get; private set; }
    }
}
