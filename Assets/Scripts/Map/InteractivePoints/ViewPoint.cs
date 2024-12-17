using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map
{
    public class ViewPoint : MonoBehaviour
    {
        public event Action OnClickAction;
        public event Action OnPlayerInteract;

        [SerializeField] private PointOfInterestPath _pathPrefabs;
        [SerializeField] private Image _image;
        [SerializeField] private CanvasGroup _canvasGroup;


        [Header("PointView")]
        [SerializeField] private Color _colorActive;
        [SerializeField] private Color _colorInactive;
        [SerializeField] private Color _colorPass;


        private List<PointOfInterestPath> _paths = new();
        public void CreatePathTo(ViewPoint viewPoint)
        {
            var path = Instantiate(_pathPrefabs, transform.localPosition, Quaternion.identity);
            path.CreatePath(viewPoint);
            _paths.Add(path);
        }

        public void Active()
        {
            _image.color = _colorActive;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        public void Lock()
        {
            _image.color = _colorInactive;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        public void Pass()
        {
            _image.color = _colorPass;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void ClickOnPoint() =>
            OnClickAction?.Invoke();

        public void PlayerInteract() =>
            OnPlayerInteract?.Invoke();

        private void OnApper()
        {
            _paths.ForEach(path => path.gameObject.SetActive(true));
        }
    }
}
