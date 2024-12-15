using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GreonAssets.Extensions;
using Project.Audio;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;
using Project.Gameplay.Common.Datas;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UICraftEnd : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _panel;
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private RectTransform _cardPlaceholder;
        [SerializeField] private RectTransform _cardTargetTransforms;
        [SerializeField] private UICardSlot _targetCardSlot;
        [SerializeField] private float _moveTime = 4f;
        [SerializeField] private Ease _moveEase = Ease.InOutSine;
        [SerializeField] private Color _battleWinColor = Color.green;
        [SerializeField] private Color _battleLooseColor = Color.red;
        [SerializeField] private Button _exitButton;
        [SerializeField] private float _holdTime = 0.5f;
        [SerializeField] private float _fadeTime = 0.5f;
        [SerializeField] private AudioClip _winClip;
        [SerializeField] private AudioClip _loseClip;
        [SerializeField] private float _threshold = 0.5f;
            
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
            _resultText.text = winner == CardOwner.player ? "Получена карта" : "Крафт не удался";
            _resultText.color = winner == CardOwner.player ? _battleWinColor : _battleLooseColor;

            _cardPlaceholder.gameObject.SetActive(winner == CardOwner.player);

            if (winner == CardOwner.player)
            {
                await UniTask.WaitForSeconds(_threshold);
                _targetCardSlot.transform.DOMove(_cardTargetTransforms.position, _moveTime).SetEase(_moveEase);
                _targetCardSlot.transform.DOScale(_cardTargetTransforms.localScale, _moveTime).SetEase(_moveEase);
                var card = UIBattle.Instance.Cards.Get(_targetCardSlot.Model.Card);
                card.uiCardVisual.SetCanvasOverride(true);
            }
            
            _panel.gameObject.SetActive(true);
            _panel.alpha = 0f;

            await UniTask.WaitForSeconds(_holdTime);
            _panel.DOFade(1f, _fadeTime).SetEase(Ease.OutBack);
            
            GameAudio.MusicSource.PlayOneShot(winner == CardOwner.player ? _winClip : _loseClip);
        }
    }
}
