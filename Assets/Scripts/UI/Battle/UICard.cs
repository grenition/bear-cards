using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common.Datas;
using System;
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
        [SerializeField] private UIEffectsImages _effects;
        [SerializeField] private UIEffectsAnimationController _effectsAnimation;

        public void Init(CardModel cardModel)
        {
            if (cardModel == null) return;

            if (Model != null)
            {
                Model.OnHealthChange -= OnHealthChange;
                Model.OnAttackDamageChange -= OnAttackDamageChange;
                Model.OnEffectsChange -= OnEffectsChange;
                BattleController.Model.OnAttackedWithEffect -= OnAttackedWithEffect;
            }
            Model = cardModel;
            Model.OnHealthChange += OnHealthChange;
            Model.OnAttackDamageChange += OnAttackDamageChange;
            Model.OnEffectsChange += OnEffectsChange;
            BattleController.Model.OnAttackedWithEffect += OnAttackedWithEffect;

            Visualize();
        }

        private void OnDestroy()
        {
            if (Model != null)
            {
                Model.OnHealthChange -= OnHealthChange;
                Model.OnAttackDamageChange -= OnAttackDamageChange;
                Model.OnEffectsChange -= OnEffectsChange;
                BattleController.Model.OnAttackedWithEffect -= OnAttackedWithEffect;
            }
        }

        private void OnHealthChange(int obj) => Visualize();
        private void OnAttackDamageChange(int obj) => Visualize();
        private void OnEffectsChange() => Visualize();

        private void OnAttackedWithEffect(CardModel card, CardPosition cardPosition, EffectTypes effect)
        {
            if (Model == card && Model.Position == cardPosition) _effectsAnimation.PlayEffectAnimation(effect);
        }

        private void Visualize()
        {
            if (_electroText) _electroText.text = Model.Config.ElectroFormula;
            if (_iconImage) _iconImage.sprite = Model.Config.VisualIcon;
            if (_shortName) _shortName.text = Model.Config.VisualShortName;
            if (_fullName) _fullName.text = Model.Config.VisualName;
            if (_damageText) _damageText.text = Model.AttackDamage.ToString();
            if (_healthText) _healthText.text = Model.Health.ToString();
            if (_descriptionText) _descriptionText.text = Model.Config.VisualDescription.Replace("{dmg}", Math.Abs(Model.AttackDamage).ToString());
            if (_effects) _effects.Effects = Model.Effects;
        }
    }
}
