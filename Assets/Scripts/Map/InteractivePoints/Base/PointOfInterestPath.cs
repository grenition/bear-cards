using UnityEngine;
namespace Assets.Scripts.Map
{
    public class PointOfInterestPath : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public void CreatePath(ViewPoint point)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, point.transform.position);
        }
    }
}