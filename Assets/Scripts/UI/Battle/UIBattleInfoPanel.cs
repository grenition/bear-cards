using System;
using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.UI.Common.Extensions;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UIBattleInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _turnOwnerText;
        [SerializeField] private Button _nextTurnButton;

        private void Start()
        {
            _nextTurnButton.Bind(() =>
            {
                BattleController.Behaviour.NextTurn();
            }).AddTo(this);
        }
        private void OnEnable()
        {
            BattleController.Behaviour.OnTurnStarted += Visualize;
        }
        private void OnDisable()
        {
            BattleController.Behaviour.OnTurnStarted -= Visualize;
        }

        private void Visualize(CardOwner owner)
        {
            _turnOwnerText.text = owner == CardOwner.player ? "Ваш ход" : "Ход противника";
            _nextTurnButton.SetActiveWithAnimation(owner == CardOwner.player);
        }
    }
}
