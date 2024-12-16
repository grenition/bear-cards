using System;
using Project.Gameplay.Battle.Craft;
using TMPro;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIReceipt : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _formulaText;

        public void Init(CardCraftConfig cardConfig)
        {
            if(cardConfig.Output == null) return;
            _titleText.text = $"{cardConfig.Output.VisualName}";
            _descriptionText.text = cardConfig.Output.VisualDescription.Replace("{dmg}", Math.Abs(cardConfig.Output.BaseDamage).ToString());    
            _formulaText.text = cardConfig.Formula;    
        }
    }
}
