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
        
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _shortName;
        [SerializeField] private TMP_Text _fullName;
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private TMP_Text _healthText;
        
        public void Init(CardModel cardModel)
        {
            if(cardModel == null) return;
            
            if(Model != null) Model.OnDamage -= OnDamage;
            Model = cardModel;
            Model.OnDamage += OnDamage;

            Visualize();
        }
        private void OnDestroy()
        {
            if(Model != null)
                Model.OnDamage -= OnDamage;
        }

        private void OnDamage(int obj) => Visualize();
        private void Visualize()
        {
            _costText.text = Model.Config.Cost.ToString();
            _levelText.text = Model.Config.Level.ToString();
            _iconImage.sprite = Model.Config.VisualIcon;
            _shortName.text = Model.Config.VisualShortName;
            _fullName.text = Model.Config.VisualName;
            _damageText.text = Model.AttackDamage.ToString();
            _healthText.text = Model.Health.ToString();            
        }
    }
}
