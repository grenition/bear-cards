using UnityEngine;
namespace Assets.Scripts.Map
{
    public class PointOfInterestPath : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public void CreatePath(ViewPoint point)
        {
            _lineRenderer.SetPosition(0, transform.position);
            Vector3 meadlePoint = point.transform.position;

            if ((transform.position.x - point.transform.position.x) != 0)
            {
                float meadlePointX = point.transform.position.x;
                meadlePoint = new Vector3(transform.position.x, point.transform.position.y, meadlePoint.z);
            }

            _lineRenderer.SetPosition(1, meadlePoint);
            _lineRenderer.SetPosition(2, point.transform.position);
        }
    }
}