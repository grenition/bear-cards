using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GreonAssets.UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Common
{
    public class UIDynamicSelector : MonoBehaviour
    {
        public static UIDynamicSelector Instance { get; protected set; }

        [SerializeField] private RectOffset _margin;
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _movementDuration = 0.0f;
        [SerializeField] private Ease _animationEase = Ease.OutBack;
        [SerializeField] private Image _border;
        [SerializeField] private float _pulsePeriod = 0.5f;
        [SerializeField] private Color _pulseColor = Color.green;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private List<RectTransform> _transforms = new();
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Sequence _sequence;

        private void Awake()
        {
            Instance = this;

            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup ??= GetComponent<CanvasGroup>();
            SetSelection(_transforms);

            _border.DOColor(_pulseColor, _pulsePeriod).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void SetSelection(IEnumerable<RectTransform> transforms)
        {
            _transforms = transforms.ToList();

            if (_transforms.Count == 0)
            {
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.Join(_canvasGroup.DOFade(0f, _animationDuration).SetEase(_animationEase)
                    .OnComplete(() => _canvas.StretchRectTransformFromWorldPoints(_rectTransform, Vector2.zero, Vector2.zero, _margin)));
                return;
            }

            _transforms.GetBounds(out var bottomLeft, out var topRight);

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Join(_canvas.DOStretchRectTransformFromWorldPoints(_rectTransform, bottomLeft, topRight, _margin, _movementDuration, _animationEase));
            _sequence.Join(_canvasGroup.DOFade(1f, _animationDuration).SetEase(_animationEase));
        }
        
        public void GetBounds(out Vector2 bottomLeft, out Vector2 topRight) => _transforms.GetBounds(out bottomLeft, out topRight);
    }
}
