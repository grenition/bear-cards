using GreonAssets.Extensions;
using Project.Audio;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Behaviour;
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
        [SerializeField] private AudioClip _turnStarted;
        [SerializeField] private AudioClip _turnEnded;

        private void Start()
        {
            _nextTurnButton.Bind(() =>
            {
                BattleController.Behaviour.NextTurn();
            }).AddTo(this);
        }
        private void OnEnable()
        {
            BattleController.Behaviour.OnStateChanged += Visualize;
        }
        private void OnDisable()
        {
            BattleController.Behaviour.OnStateChanged -= Visualize;
        }

        private void Visualize(BattleState state)
        {
            _turnOwnerText.text = state switch
            {
                BattleState.awaiting => "Ожидание",
                BattleState.playerTurn => "Ваш ход",
                BattleState.enemyTurn => "Ход противника"
            };
            
            _nextTurnButton.SetActiveWithAnimation(state == BattleState.playerTurn);
            
            GameAudio.MusicSource.PlayOneShot(state == BattleState.awaiting ? _turnEnded : _turnStarted);
        }
    }
}
