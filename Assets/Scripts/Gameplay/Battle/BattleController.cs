using UnityEngine;

namespace Project.Gameplay.Battle
{
    public class BattleController : MonoBehaviour
    {
        public static BattleModel Model { get; private set; }
        private void Awake()
        {
            Model = new BattleModel("enemy1");
        }
    }
}
