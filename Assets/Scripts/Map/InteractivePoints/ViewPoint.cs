using UnityEngine;

namespace Assets.Scripts.Map
{
    public class ViewPoint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private PointOfInterestPath _pathPrefabs;
        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public void CreatePathTo(ViewPoint viewPoint)
        {
            var path = Instantiate(_pathPrefabs, transform.localPosition, Quaternion.identity);
            path.CreatePath(viewPoint);
        }
    }
}
