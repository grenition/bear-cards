using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GreonAssets.Extensions;
using Project.Audio;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UIBattleEnd : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _panel;
        [SerializeField] private TMP_Text _battleResultText;
        [SerializeField] private Color _battleWinColor = Color.green;
        [SerializeField] private Color _battleLooseColor = Color.red;
        [SerializeField] private Button _exitButton;
        [SerializeField] private float _holdTime = 0.5f;
        [SerializeField] private float _fadeTime = 0.5f;
        [SerializeField] private AudioClip _winClip;
        [SerializeField] private AudioClip _loseClip;
            
        private void Start()
        {
            _panel.gameObject.SetActive(false);
            
            BattleController.Model.OnBattleEnded += OnBattleEnd;

            _exitButton.Bind(() =>
            {
                ScneneLoaderStatic.LoadSceneAsync("MapScene");
            }).AddTo(this);
        }
        private void OnDestroy()
        {
            BattleController.Model.OnBattleEnded -= OnBattleEnd;
            DOTween.KillAll();
        }
        private async void OnBattleEnd(CardOwner winner)
        {
            _battleResultText.text = winner == CardOwner.player ? "Победа" : "Проигрыш";
            _battleResultText.color = winner == CardOwner.player ? _battleWinColor : _battleLooseColor;
            
            _panel.gameObject.SetActive(true);
            _panel.alpha = 0f;

            await UniTask.WaitForSeconds(_holdTime);
            _panel.DOFade(1f, _fadeTime).SetEase(Ease.OutBack);
            
            GameAudio.MusicSource.PlayOneShot(winner == CardOwner.player ? _winClip : _loseClip);
        }
    }
}
