using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map
{
    public class EnemyPoint : InteractivePoint
    {
        public EnemyPoint()
        {
            PointEntity.Key = "Enemy";
        }

        public override void OnBeginInteract()
        {
            SceneManager.LoadScene(1);
        }

        public override void OnEndInteract()
        {
        }
    }
}