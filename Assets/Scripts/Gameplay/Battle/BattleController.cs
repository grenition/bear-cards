using System;
using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model;
using UnityEngine;

namespace Project.Gameplay.Battle
{
    public class BattleController : MonoBehaviour
    {
        public static BattleModel Model { get; private set; }
        public static BattleBehaviour Behaviour { get; private set; }
        private void Awake()
        {
            Model = new BattleModel("demo_battle");
            Behaviour = new BattleBehaviour(Model);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Behaviour.NextTurn();
        }
        private void Start()
        {
            Behaviour.Start();
        }
        private void OnDestroy()
        {
            Behaviour.Stop();
            Model.Dispose();
            Behaviour.Dispose();
        }
    }
}
