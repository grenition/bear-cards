using System;
using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project.Gameplay.Battle
{
    public class BattleController : MonoBehaviour
    {
        public static BattleModel Model {
            get {
                Init();
                return _model;
            }
        }
        public static BattleBehaviour Behaviour {
            get {
                Init();
                return _behaviour;
            }
        }

        private static bool _initialized = false;
        private static BattleModel _model;
        private static BattleBehaviour _behaviour;
        
        private static void Init()
        {
            if(_initialized) return;
            
            _model = new BattleModel("demo_battle");
            _behaviour = new BattleBehaviour(_model);
            _initialized = true;
        }
        private void Awake()
        {
            Init();
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

        public static bool IsPlayerTurn() => Behaviour.GetCurrentState() == BattleState.playerTurn;
    }
}
