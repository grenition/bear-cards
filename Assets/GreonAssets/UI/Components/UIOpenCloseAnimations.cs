using DG.Tweening;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GreonAssets.UI.ComponentAnimations
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIOpenCloseAnimations : MonoBehaviour
    {
        public enum AnimationType
        {
            ScaleInOut,
            SlideFromRight,
            SlideFromLeft,
            SlideFromTop,
            SlideFromBottom,
            FadeOnly,
            ScaleAndFade
        }

        [Header("General Settings")]
        [SerializeField] private AnimationType openCloseAnimation = AnimationType.ScaleInOut;
        [SerializeField] private float openDuration = 0.15f;
        [SerializeField] private float closeDuration = 0.15f;
        [SerializeField] private float openDelay = 0f;
        [SerializeField] private float closeDelay = 0f;
        [SerializeField] private Ease openEase = Ease.OutBack;
        [SerializeField] private Ease closeEase = Ease.InBack;

        [Header("Scale Settings (For ScaleInOut, ScaleAndFade)")]
        [SerializeField] private float openScale = 1f;
        [SerializeField] private float closeScale = 0.5f;

        [Header("Slide Settings (For SlideFromDirections)")]
        [SerializeField] private float slideOffsetX = 1920f;
        [SerializeField] private float slideOffsetY = 1080f;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Vector3 initialPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            initialPosition = rectTransform.anchoredPosition3D;
        }

        private void OnEnable()
        {
            Open();
        }

        private void Start()
        {
            SetClosedStateInstantly();
        }

        private void SetClosedStateInstantly()
        {
            canvasGroup.alpha = 0f;
            switch (openCloseAnimation)
            {
                case AnimationType.ScaleInOut:
                case AnimationType.ScaleAndFade:
                    rectTransform.localScale = Vector3.one * closeScale;
                    rectTransform.anchoredPosition3D = initialPosition;
                    break;
                case AnimationType.SlideFromRight:
                    rectTransform.anchoredPosition3D = initialPosition + new Vector3(slideOffsetX, 0f, 0f);
                    rectTransform.localScale = Vector3.one;
                    break;
                case AnimationType.SlideFromLeft:
                    rectTransform.anchoredPosition3D = initialPosition - new Vector3(slideOffsetX, 0f, 0f);
                    rectTransform.localScale = Vector3.one;
                    break;
                case AnimationType.SlideFromTop:
                    rectTransform.anchoredPosition3D = initialPosition + new Vector3(0f, slideOffsetY, 0f);
                    rectTransform.localScale = Vector3.one;
                    break;
                case AnimationType.SlideFromBottom:
                    rectTransform.anchoredPosition3D = initialPosition - new Vector3(0f, slideOffsetY, 0f);
                    rectTransform.localScale = Vector3.one;
                    break;
                case AnimationType.FadeOnly:
                    rectTransform.anchoredPosition3D = initialPosition;
                    rectTransform.localScale = Vector3.one;
                    break;
                default:
                    rectTransform.anchoredPosition3D = initialPosition;
                    rectTransform.localScale = Vector3.one;
                    break;
            }
        }

        public void Open()
        {
            _ = OpenAsync();
        }

        public async Task OpenAsync()
        {
            gameObject.SetActive(true);
            SetClosedStateInstantly();

            rectTransform.DOKill();
            canvasGroup.DOKill();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (openDelay > 0f)
                await Task.Delay((int)(openDelay * 1000));

            Sequence openSequence = DOTween.Sequence();

            switch (openCloseAnimation)
            {
                case AnimationType.ScaleInOut:
                    openSequence.Join(rectTransform.DOScale(openScale, openDuration).SetEase(openEase));
                    openSequence.Join(canvasGroup.DOFade(1f, openDuration).SetEase(Ease.OutQuad));
                    break;

                case AnimationType.SlideFromRight:
                    openSequence.Join(rectTransform.DOAnchorPos3D(initialPosition, openDuration).SetEase(openEase));
                    openSequence.Join(canvasGroup.DOFade(1f, openDuration).SetEase(Ease.OutQuad));
                    break;

                case AnimationType.SlideFromLeft:
                    openSequence.Join(rectTransform.DOAnchorPos3D(initialPosition, openDuration).SetEase(openEase));
                    openSequence.Join(canvasGroup.DOFade(1f, openDuration).SetEase(Ease.OutQuad));
                    break;

                case AnimationType.SlideFromTop:
                    openSequence.Join(rectTransform.DOAnchorPos3D(initialPosition, openDuration).SetEase(openEase));
                    openSequence.Join(canvasGroup.DOFade(1f, openDuration).SetEase(Ease.OutQuad));
                    break;

                case AnimationType.SlideFromBottom:
                    openSequence.Join(rectTransform.DOAnchorPos3D(initialPosition, openDuration).SetEase(openEase));
                    openSequence.Join(canvasGroup.DOFade(1f, openDuration).SetEase(Ease.OutQuad));
                    break;

                case AnimationType.FadeOnly:
                    openSequence.Join(canvasGroup.DOFade(1f, openDuration).SetEase(Ease.OutQuad));
                    break;

                case AnimationType.ScaleAndFade:
                    openSequence.Join(rectTransform.DOScale(openScale, openDuration).SetEase(openEase));
                    openSequence.Join(canvasGroup.DOFade(1f, openDuration).SetEase(Ease.OutQuad));
                    break;
            }

            await openSequence.AsyncWaitForCompletion();

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void Close(bool disableAfterComplete = true)
        {
            _ = CloseAsync(disableAfterComplete);
        }

        public async Task CloseAsync(bool disableAfterComplete = true)
        {
            if(!gameObject.activeInHierarchy) return;
            
            rectTransform.DOKill();
            canvasGroup.DOKill();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (closeDelay > 0f)
                await Task.Delay((int)(closeDelay * 1000));

            Sequence closeSequence = DOTween.Sequence();

            switch (openCloseAnimation)
            {
                case AnimationType.ScaleInOut:
                    closeSequence.Join(rectTransform.DOScale(closeScale, closeDuration).SetEase(closeEase));
                    closeSequence.Join(canvasGroup.DOFade(0f, closeDuration).SetEase(Ease.InQuad));
                    break;

                case AnimationType.SlideFromRight:
                    closeSequence.Join(rectTransform.DOAnchorPos3D(initialPosition + new Vector3(slideOffsetX, 0f, 0f), closeDuration).SetEase(closeEase));
                    closeSequence.Join(canvasGroup.DOFade(0f, closeDuration).SetEase(Ease.InQuad));
                    break;

                case AnimationType.SlideFromLeft:
                    closeSequence.Join(rectTransform.DOAnchorPos3D(initialPosition - new Vector3(slideOffsetX, 0f, 0f), closeDuration).SetEase(closeEase));
                    closeSequence.Join(canvasGroup.DOFade(0f, closeDuration).SetEase(Ease.InQuad));
                    break;

                case AnimationType.SlideFromTop:
                    closeSequence.Join(rectTransform.DOAnchorPos3D(initialPosition + new Vector3(0f, slideOffsetY, 0f), closeDuration).SetEase(closeEase));
                    closeSequence.Join(canvasGroup.DOFade(0f, closeDuration).SetEase(Ease.InQuad));
                    break;

                case AnimationType.SlideFromBottom:
                    closeSequence.Join(rectTransform.DOAnchorPos3D(initialPosition - new Vector3(0f, slideOffsetY, 0f), closeDuration).SetEase(closeEase));
                    closeSequence.Join(canvasGroup.DOFade(0f, closeDuration).SetEase(Ease.InQuad));
                    break;

                case AnimationType.FadeOnly:
                    closeSequence.Join(canvasGroup.DOFade(0f, closeDuration).SetEase(Ease.InQuad));
                    break;

                case AnimationType.ScaleAndFade:
                    closeSequence.Join(rectTransform.DOScale(closeScale, closeDuration).SetEase(closeEase));
                    closeSequence.Join(canvasGroup.DOFade(0f, closeDuration).SetEase(Ease.InQuad));
                    break;
            }

            await closeSequence.AsyncWaitForCompletion();
            
            if(disableAfterComplete)
                HideObject();
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }
    }
}