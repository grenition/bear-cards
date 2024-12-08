using System;
using Project.Gameplay.Battle.Model.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICard : MonoBehaviour
    {
        public CardModel Model { get; protected set; }
        
        [SerializeField] private TMP_Text _electroText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _shortName;
        [SerializeField] private TMP_Text _fullName;
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _descriptionText;
        
        public void Init(CardModel cardModel)
        {
            if(cardModel == null) return;
            
            if(Model != null) Model.OnHealthChange -= OnHealthChange;
            Model = cardModel;
            Model.OnHealthChange += OnHealthChange;

            Visualize();
        }
        private void OnDestroy()
        {
            if(Model != null)
                Model.OnHealthChange -= OnHealthChange;
        }

        private void OnHealthChange(int obj) => Visualize();
        private void Visualize()
        {
            if(_electroText) _electroText.text = Model.Config.ElectroFormula;
            if(_iconImage) _iconImage.sprite = Model.Config.VisualIcon;
            if(_shortName) _shortName.text = Model.Config.VisualShortName;
            if(_fullName) _fullName.text = Model.Config.VisualName;
            if(_damageText) _damageText.text = Model.AttackDamage.ToString();
            if(_healthText) _healthText.text = Model.Health.ToString();            
            if(_descriptionText) _descriptionText.text = Model.Config.VisualDescription.Replace("{dmg}", Math.Abs(Model.AttackDamage).ToString());            
        }
    }
}
