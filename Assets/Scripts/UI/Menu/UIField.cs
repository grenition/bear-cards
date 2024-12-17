using DG.Tweening;
using UnityEngine;

namespace Project.UI.Menu
{
    public class UIField : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [SerializeField] private float _duration = 0.5f;
        
        public void Show()
        {
            _canvasGroup.blocksRaycasts = true;
            
            var y = _rectTransform.anchoredPosition.y;
            var parentHeight = _rectTransform.parent.GetComponent<RectTransform>().rect.height;
            
            var diff = parentHeight - y;
            var duration = (1 - (diff / parentHeight)) * _duration;
            
            _rectTransform.DOAnchorPosY(0f, duration).SetEase(Ease.OutCubic);
        }
        
        public void Hide()
        {
            var y = _rectTransform.anchoredPosition.y;
            var parentHeight = _rectTransform.parent.GetComponent<RectTransform>().rect.height;
            
            var diff = parentHeight - y;
            var duration = diff / parentHeight * _duration;
            
            _rectTransform.DOAnchorPosY(-parentHeight, duration).SetEase(Ease.InCubic);
            _canvasGroup.blocksRaycasts = false;
        }
    }
}