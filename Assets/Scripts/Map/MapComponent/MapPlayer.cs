using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MapPlayer : MonoBehaviour
    {
        public bool PlayerIsMove { get; private set; }

        [SerializeField] private float _moveTime = 1f;
        [SerializeField] private Ease _moveEase = Ease.OutQuad;
        
        private ViewPoint _viewPoint;

        public void MoveTo(ViewPoint point)
        {
            _viewPoint = point;
            
            if(_viewPoint == null) return;
            transform.DOKill();
            PlayerIsMove  = true;

            transform
                .DOMove(_viewPoint.transform.position, _moveTime)
                .SetEase(_moveEase)
                .OnComplete(() =>
                {
                    PlayerIsMove = false;
                    _viewPoint.PlayerInteract();
                    _viewPoint = null;
                    Debug.Log($"{gameObject.name}: i complited move!");
                });

        }
    }
}