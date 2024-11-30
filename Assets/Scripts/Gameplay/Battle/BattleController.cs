using Assets.Scripts.Map;
using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model;
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
            
            _model = new BattleModel(MapStaticData.KeyBattle);
            _behaviour = new BattleBehaviour(_model);
            _initialized = true;
        }
        private void Awake()
        {
            Init();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Behaviour.GetCurrentState() == BattleState.playerTurn)
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
            _initialized = false;
        }

        public static bool IsPlayerTurn() => Behaviour.GetCurrentState() == BattleState.playerTurn;
    }
}
