﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class ViewPoint : MonoBehaviour
    {
        public event Action OnClickAction;
        public event Action OnPlayerInteract;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private PointOfInterestPath _pathPrefabs;
        [SerializeField] private Animator _animator;


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

        public void Active() => _spriteRenderer.color = _colorActive;
        public void Lock() => _spriteRenderer.color = _colorInactive;
        public void Pass() => _spriteRenderer.color = _colorPass;

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
