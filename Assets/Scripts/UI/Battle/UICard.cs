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
            Model = cardModel;

            _costText.text = cardModel.Config.Cost.ToString();
            _levelText.text = cardModel.Config.Level.ToString();
            _iconImage.sprite = cardModel.Config.VisualIcon;
            _shortName.text = cardModel.Config.VisualShortName;
            _fullName.text = cardModel.Config.VisualName;
            _damageText.text = cardModel.Damage.ToString();
            _healthText.text = cardModel.Health.ToString();
        }
    }
}
