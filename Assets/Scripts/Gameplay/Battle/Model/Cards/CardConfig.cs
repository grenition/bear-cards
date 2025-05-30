using System.Collections.Generic;
using System.Linq;
using Project.Gameplay.Common;
using Project.Gameplay.Common.Datas;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    [System.Serializable]
    public class CardEffectEntry
    {
        public CardEffect Effect;
        public int Value;       
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
        
        [field: Header("Special")]
        [field: SerializeField] public List<EffectTypes> Effects { get; protected set; }


        [SerializeField] private List<CardEffectEntry> cardEffects;
        private Dictionary<CardEffect, int> _spellDictionary;
        public IReadOnlyDictionary<CardEffect, int> SpellEffects {
            get {
                if (_spellDictionary == null)
                {
                    _spellDictionary = cardEffects.ToDictionary(x => x.Effect, x => x.Value);
                }
                return _spellDictionary;
            }
        }
    }
}
