using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class MapPlayer : MonoBehaviour
    {
        [SerializeField, Range(0,2)] private float _speed;
        [SerializeField, Range(0, 1)] private float _stopDistance;
        private ViewPoint _viewPoint;

        public void MoveTo(ViewPoint point)
        {
            _viewPoint = point;
        }

        private void Update()
        {
            if (_viewPoint == null) return;

            transform.Translate((_viewPoint.transform.position - transform.position) * Time.deltaTime * _speed);

            if (Vector2.Distance(transform.position, _viewPoint.transform.position) < _stopDistance)
            {
                _viewPoint.PlayerInteract();
                _viewPoint = null;
                Debug.Log($"{gameObject.name}: i complited move!");
            }
        }
    }
}