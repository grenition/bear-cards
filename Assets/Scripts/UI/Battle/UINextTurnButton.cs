using System;
using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Behaviour;
using Project.UI.Common.Extensions;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    [RequireComponent(typeof(Button))]
    public class UINextTurnButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.Bind(() =>
            {
                BattleController.Behaviour.NextTurn();
            }).AddTo(this);
            
            BattleController.Behaviour.OnStateChanged += OnStateChanged;
        }
        private void OnDestroy()
        {
            BattleController.Behaviour.OnStateChanged -= OnStateChanged;
        }
        private void OnStateChanged(BattleState state)
        {
            if (state == BattleState.awaiting)
                _button.CloseWithAnimation();
            else
                _button.gameObject.SetActive(true);
        }
    }
}
