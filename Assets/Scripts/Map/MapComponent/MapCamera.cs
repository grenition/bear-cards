using UnityEngine;
namespace Assets.Scripts.Map
{
    public class MapCamera : MonoBehaviour
    {
        [SerializeField, Range(0, 2)] private float _speed;
        [SerializeField, Range(0, 2)] private float _stopDistance;
        [SerializeField] private Vector3 _cameraDifference;

        public bool CameraChangePosition { get; private set; }
        private MapPlayer _player;

        private void Start()
        {
            _player = MapCompositionRoot.Instance.MapPlayer;
        }

        private void Update()
        {
            if (CameraChangePosition)
                transform.Translate(_speed * Time.deltaTime * ((_player.transform.position + _cameraDifference) - transform.position));

            if (Vector2.Distance(transform.position, (_player.transform.position + _cameraDifference)) < _stopDistance)
            {
                CameraChangePosition = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            }
        }

        public void MoveCameraToPlayer() =>
            CameraChangePosition = true;
    }
}