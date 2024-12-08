using System;
using GreonAssets.Extensions;
using UnityEngine;

namespace GreonAssets.Features.FreeObjectLooking
{
    public abstract class FreeObjectLookingBase : MonoBehaviour
    {
        [SerializeField] protected bool _allowRotation = true;
        [SerializeField] protected bool _allowScalling = true;
        [SerializeField] protected bool _allowTranslating = true;
        [SerializeField] protected float _rotationSensitivity = 4f;
        [SerializeField] protected float _scaleSensitivity = 40f;
        [SerializeField] protected float _translateSensivity = 5f;
        [SerializeField, Range(0.001f, 1f)] private float _minScale = 0.5f;
        [SerializeField, Range(1f, 1000f)] private float _maxScale = 2f;
        
        protected abstract bool rotateTrigger { get; }
        protected abstract Vector2 rotateDelta { get; }
        protected abstract bool scaleTrigger { get; }
        protected abstract float scaleDelta { get; }
        protected abstract bool translateTrigger { get; }
        protected abstract Vector2 translateDelta { get; }

        protected Vector3 minScaleVector => _startScale * _minScale; 
        protected Vector3 maxScaleVector => _startScale * _maxScale; 
        
        protected Vector3 _startScale;
        protected Camera _mainCamera;
        protected Vector3 _rotation;
        
        private void OnEnable()
        {
            _mainCamera = Camera.main;
            _rotation = transform.localEulerAngles;
        }
        private void Start()
        {
            _startScale = transform.localScale;
        }

        private void Update()
        {
            if (translateTrigger && _allowTranslating)
                Translate(translateDelta);
            else if (rotateTrigger && _allowRotation)
                Rotate(rotateDelta);
            if (scaleTrigger && _allowScalling)
                Scale(scaleDelta);
        }
        protected virtual void Rotate(Vector2 delta)
        {
            delta *= _rotationSensitivity;
            _rotation += new Vector3(-delta.y, delta.x * Math.Sign(Vector3.Dot(Vector3.up, transform.up)), 0f);
            _rotation = LoopEulers(_rotation);
            
            transform.localEulerAngles = _rotation;
        }
        protected virtual void Scale(float delta)
        {
            var newScale = transform.localScale + Vector3.one * (delta * Time.deltaTime * _scaleSensitivity);
            newScale = newScale.Clamp(minScaleVector, maxScaleVector);
            transform.localScale = newScale;
        }
        protected virtual void Translate(Vector2 delta)
        {
            var direction = _mainCamera.transform.up * delta.y + _mainCamera.transform.right * delta.x;
            transform.Translate(-direction, Space.World);
        }
        
        private Vector3 LoopEulers(Vector3 eulers)
        {
            return new Vector3
            {
                x = LoopMagnitude(eulers.x),
                y = LoopMagnitude(eulers.y),
                z = LoopMagnitude(eulers.z)
            };
        }
        private float LoopMagnitude(float value)
        {
            if (value >= 360f)
                value -= 360f;
            else if (value <= -360f)
                value += 360f;
            return value;
        }
    }
}
