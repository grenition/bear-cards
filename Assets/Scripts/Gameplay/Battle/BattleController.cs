using UnityEngine;

namespace Project.Gameplay.Battle
{
    public class BattleController : MonoBehaviour
    {
        public static BattleModel Model { get; private set; }
        private void Awake()
        {
            Model = new BattleModel("demo_battle");
        }
        private void OnDestroy()
        {
            Model.Dispose();
        }
    }
}
