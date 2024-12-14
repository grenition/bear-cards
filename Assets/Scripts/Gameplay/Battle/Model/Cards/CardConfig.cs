using System.Collections.Generic;
using Project.Gameplay.Common;
using Project.Gameplay.Common.Datas;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{

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
        
        [field: Header("Special")]
        [field: SerializeField] public List<EffectTypes> Effects { get; protected set; }
        
    }
}
