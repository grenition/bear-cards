using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Battle.Behaviour.EntityBehaviours;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Common.Datas;

namespace Project.Gameplay.Battle.Behaviour
{
    public class StandartBattleBehaviour : BattleBehaviour
    {
        protected BaseEntityBehaviour CurrentBehaviour => TurnOwner == CardOwner.player ? _playerBehaviour : _enemyBehaviour;
        protected PlayerBehaviour _playerBehaviour;
        protected EnemyBehaviour _enemyBehaviour;
        protected bool _nextTurnLocked = false;
        
        public StandartBattleBehaviour(BattleModel model)
        {
            Model = model;
            TurnOwner = Config.FirstTurnOwner;

            _playerBehaviour = new(this);
            _enemyBehaviour = new(this);
        }

        public override void Start()
        {
            if(BehaviourActive) return;
            
            BehaviourActive = true;
            CurrentBehaviour.StartTurn();
            
            CallOnTurnStarted(TurnOwner);
            CallOnStateChanged(GetCurrentState());
        }
        
        public async override void NextTurn()
        {
            if(_nextTurnLocked || Model.BattleEnded) return;
            if (!BehaviourActive)
            {
                Start();
                return;
            }

            CurrentBehaviour.StopTurn();
            _nextTurnLocked = true;
            CallOnStateChanged(GetCurrentState());

            await DoFight();
            
            if(Model.BattleEnded) return;
            
            _nextTurnLocked = false;
            TurnOwner = TurnOwner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
            CurrentBehaviour.StartTurn();
            
            CallOnTurnStarted(TurnOwner);
            CallOnStateChanged(GetCurrentState());
        }
        public override void Stop()
        {
            if(!BehaviourActive) return;
            
            CurrentBehaviour.StopTurn();
            BehaviourActive = false;
        }

        public async Task DoFight()
        {
            await UniTask.WaitForSeconds(1f);
            
            var cards = Model.GetCardsAtPosition(TurnOwner, CardContainer.field).ToList();

            foreach (var card in cards)
            {
                Model.AttackForward(card.Position);
                await UniTask.WaitForSeconds(0.2f);

            }
            if(cards.Count == 0) return;

            await UniTask.WaitForSeconds(1.2f);
        }

        public override void Dispose()
        {
            _playerBehaviour.Dispose();
            _enemyBehaviour.Dispose();
        }

        public override BattleState GetCurrentState()
        {
            if (_nextTurnLocked) return BattleState.awaiting;
            return TurnOwner == CardOwner.player ? BattleState.playerTurn : BattleState.enemyTurn;
        }
    }
}
