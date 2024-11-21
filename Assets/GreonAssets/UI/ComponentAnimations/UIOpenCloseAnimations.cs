using DG.Tweening;
using UnityEngine;

namespace GreonAssets.UI.ComponentAnimations
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIOpenCloseAnimations : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float openScale = 1f;
        [SerializeField] private float closeScale = 0.5f;
        [SerializeField] private float openDuration = 0.15f;
        [SerializeField] private float closeDuration = 0.15f;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            Open();
        }
        private void Start()
        {
            rectTransform.localScale = Vector3.one * closeScale;
            canvasGroup.alpha = 0;
        }

        public void Open()
        {
            gameObject.SetActive(true);
            rectTransform.DOScale(openScale, openDuration).SetEase(Ease.OutBack);
            canvasGroup.DOFade(1, openDuration).SetEase(Ease.OutQuad);
        }

        public void Close()
        {
            rectTransform.DOScale(closeScale, closeDuration).SetEase(Ease.InBack);
            canvasGroup.DOFade(0, closeDuration).SetEase(Ease.InQuad).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
