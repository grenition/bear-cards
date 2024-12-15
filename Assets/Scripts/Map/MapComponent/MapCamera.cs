using UnityEngine;
namespace Assets.Scripts.Map
{
    public class MapCamera : MonoBehaviour
    {
        [SerializeField, Range(0, 2)] private float _speed;
        [SerializeField, Range(0, 2)] private float _stopDistance;
        [SerializeField] private Vector3 _cameraDifference;

        private bool _cameraChangePosition;
        private MapPlayer _player;

        private void Start()
        {
            _player = MapCompositionRoot.Instance.MapPlayer;
        }

        private void Update()
        {
            if (_cameraChangePosition)
                transform.Translate(_speed * Time.deltaTime * ((_player.transform.position + _cameraDifference) - transform.position));

            if (Vector2.Distance(transform.position, (_player.transform.position + _cameraDifference)) < _stopDistance)
            {
                _cameraChangePosition = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            }
        }

        public void MoveCameraToPlayer() =>
            _cameraChangePosition = true;
    }
}