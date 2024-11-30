using System;
using DG.Tweening;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICardPlayerInfo : MonoBehaviour
    {
        public CardPlayerModel Model { get; protected set; }
        
        [SerializeField] private CardOwner _owner;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private float _damageAnimationInTime = 0.05f;
        [SerializeField] private float _damageAnimationOutTime = 0.3f;
        [SerializeField] private float _damageAnimationShakeTime = 0.25f;
        [SerializeField] private float _shakeAmplitude = 5;
        [SerializeField] private Color _damageColor = Color.red;
        [SerializeField] private Color _healthColor = Color.green;

        private bool _initialized = false;

        private void Start()
        {
            Init(_owner == CardOwner.player ? BattleController.Model.Player : BattleController.Model.Enemy);
        }
        
        public void Init(CardPlayerModel model)
        {
            if(_initialized) return;
            
            Model = model;
            Visualize();

            Model.OnHealthChanged += OnHealthChanged;

            _initialized = true;
        }
        private void OnDestroy()
        {
            if (_initialized)
            {
                Model.OnHealthChanged -= OnHealthChanged;
            }
        }

        public void Visualize()
        {
            _iconImage.sprite = Model.Config.Icon;
            _healthText.text = $"{Model.Health}";
        }

        public async void OnHealthChanged(int modValue)
        {
            _healthText.text = $"{Model.Health}";

            var changeColor = modValue > 0 ? _healthColor : _damageColor;
            transform.DOShakePosition(_damageAnimationShakeTime, _shakeAmplitude).SetEase(Ease.OutBack);
            
            _healthText
                .DOColor(changeColor, _damageAnimationInTime)
                .SetEase(Ease.OutQuad);
            
            await _iconImage
                .DOColor(changeColor, _damageAnimationInTime)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion();
            
            _healthText
                .DOColor(Color.white, _damageAnimationOutTime)
                .SetEase(Ease.Linear);
            
            await _iconImage
                .DOColor(Color.white, _damageAnimationOutTime)
                .SetEase(Ease.Linear)
                .AsyncWaitForCompletion();
        }
    }
}
