using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model;
using UnityEngine;

namespace Project.Gameplay.Battle
{
    public class BattleController : MonoBehaviour
    {
        public static BattleModel Model {
            get {
                Instance ??= FindAnyObjectByType<BattleController>();
                Instance.Init();
                return Instance._model;
            }
        }
        public static BattleBehaviour Behaviour {
            get {
                Instance ??= FindAnyObjectByType<BattleController>();
                Instance.Init();
                return Instance._behaviour;
            }
        }
        
        public static BattleController Instance { get; private set; }

        protected bool _initialized = false;
        protected BattleModel _model;
        protected BattleBehaviour _behaviour;
        
        protected virtual void Init()
        {
            if(_initialized) return;
            
            _model = new BattleModel(MapStaticData.KeyBattle);
            _behaviour = new StandartBattleBehaviour(_model);
            _initialized = true;
        }
        protected virtual void Awake()
        {
            Instance = this;
            Init();
        }
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Behaviour.GetCurrentState() == BattleState.playerTurn)
                Behaviour.NextTurn();
        }
        protected virtual void Start()
        {
            Behaviour.Start();
        }
        protected async virtual void OnDestroy()
        {
            Behaviour.Stop();
            Model.Dispose();
            Behaviour.Dispose();
            _initialized = false;

            await UniTask.NextFrame();
            Instance = null;
        }

        public static bool IsPlayerTurn() => Behaviour.GetCurrentState() == BattleState.playerTurn;
    }
}
