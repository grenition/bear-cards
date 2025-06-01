using DG.Tweening;
using Project.Gameplay.Battle.Model.Cards;
using Project.UI.Battle;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class UICardMap : UIToggleButton
    {
        public CardModel Model { get; protected set; }

        [SerializeField] private TMP_Text _electroText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _shortName;
        [SerializeField] private TMP_Text _fullName;
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private UIEffectsImages _effects;
        [SerializeField] private UIEffectsAnimationController _effectsAnimation;

        public void Init(CardModel cardModel)
        {
            if (cardModel == null)
                return;

            Model = cardModel;
            Visualize();

            Value = false;
            ResetToggle();
        }

        private void Visualize()
        {
            if (_electroText) _electroText.text = Model.Config.ElectroFormula;
            if (_iconImage) _iconImage.sprite = Model.Config.VisualIcon;
            if (_shortName) _shortName.text = Model.Config.VisualShortName;
            if (_fullName) _fullName.text = Model.Config.LocalizedName.GetLocalizedString();
            if (_damageText) _damageText.text = Model.AttackDamage.ToString();
            if (_healthText) _healthText.text = Model.Health.ToString();
            if (_descriptionText) _descriptionText.text = Model.Config.LocalizedDescribtion.GetLocalizedString().Replace("{dmg}", Math.Abs(Model.AttackDamage).ToString());
            if (_effects) _effects.Effects = Model.Effects;
        }
    }

    public class UIToggleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _activeState;
        [SerializeField] private RectTransform _nonActiveState;

        public bool Value { get; set; }
        public Button Button => _button;

        private float _baseScale;

        private void Start()
        {
            _baseScale = transform.localScale.x;
        }

        public void ResetToggle()
        {
            Value = false;
            UpdateToggleState();
        }

        public void UpdateToggleState()
        {
            _activeState.gameObject.SetActive(Value);
            _nonActiveState.gameObject.SetActive(!Value);
        }

        public void PlayAnimationView()
        {
            transform.DOScale(_baseScale * .75f, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                transform.DOScale(_baseScale, 0.1f).SetEase(Ease.OutBounce);
            });
        }
    }
}
