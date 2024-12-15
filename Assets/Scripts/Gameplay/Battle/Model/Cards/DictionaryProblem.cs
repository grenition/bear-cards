using System.Collections.Generic;
using Project.Gameplay.Common;
using Project.Gameplay.Common.Datas;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{

    public class DictionaryProblem : MonoBehaviour
    {
        [field: SerializeField]
        private List<CardEffectEntry> cardEffects = new List<CardEffectEntry>();
        
        public Dictionary<CardEffect, int> SpellEffects { get; private set; } = new Dictionary<CardEffect, int>();
        
        private void SyncDictionaryWithList()
        {
            SpellEffects.Clear();
            foreach (var entry in cardEffects)
            {
                if (entry.Effect != null)
                {
                    SpellEffects[entry.Effect] = entry.Value;
                }
            }
        }
        
        private void OnValidate()
        {
            SyncDictionaryWithList();
        }
    }

}
