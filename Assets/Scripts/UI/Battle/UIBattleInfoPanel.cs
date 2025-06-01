using GreonAssets.Extensions;
using Project.Audio;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Behaviour;
using Project.UI.Common.Extensions;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UIBattleInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _turnOwnerText;
        [SerializeField] private LocalizedString LocalizedWait;
        [SerializeField] private LocalizedString LocalizedPlayerTurn;
        [SerializeField] private LocalizedString LocalizedEnemyTurn;

        [Space]
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
                BattleState.awaiting => LocalizedWait.GetLocalizedString(),
                BattleState.playerTurn => LocalizedPlayerTurn.GetLocalizedString(),
                BattleState.enemyTurn => LocalizedEnemyTurn.GetLocalizedString()
            };
            
            _nextTurnButton.SetActiveWithAnimation(state == BattleState.playerTurn);
            
            GameAudio.MusicSource.PlayOneShot(state == BattleState.awaiting ? _turnEnded : _turnStarted);
        }
    }
}
