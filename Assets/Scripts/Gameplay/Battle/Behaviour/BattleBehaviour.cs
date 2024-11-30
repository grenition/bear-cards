using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Behaviour.EntityBehaviours;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;

namespace Project.Gameplay.Battle.Behaviour
{
    public enum BattleState
    {
        awaiting,
        playerTurn,
        enemyTurn
    }
    
    public class BattleBehaviour : IDisposable
    {
        public event Action<BattleState> OnStateChanged;
        public event Action<CardOwner> OnTurnStarted;
        
        public BattleConfig Config => Model.Config;
        public BattleModel Model { get; protected set; }
        public CardOwner TurnOwner { get; protected set; }
        public bool BehaviourActive { get; protected set; }
        
        protected BaseEntityBehaviour CurrentBehaviour => TurnOwner == CardOwner.player ? _playerBehaviour : _enemyBehaviour;
        protected PlayerBehaviour _playerBehaviour;
        protected EnemyBehaviour _enemyBehaviour;
        protected bool _nextTurnLocked = false;
        
        public BattleBehaviour(BattleModel model)
        {
            Model = model;
            TurnOwner = Config.FirstTurnOwner;

            _playerBehaviour = new(this);
            _enemyBehaviour = new(this);
        }

        public void Start()
        {
            if(BehaviourActive) return;
            
            BehaviourActive = true;
            CurrentBehaviour.StartTurn();
            
            OnTurnStarted.SafeInvoke(TurnOwner);
            OnStateChanged.SafeInvoke(GetCurrentState());
        }
        
        public async void NextTurn()
        {
            if(_nextTurnLocked || Model.BattleEnded) return;
            if (!BehaviourActive)
            {
                Start();
                return;
            }

            CurrentBehaviour.StopTurn();
            _nextTurnLocked = true;
            OnStateChanged.SafeInvoke(GetCurrentState());

            await DoFight();
            
            if(Model.BattleEnded) return;
            
            _nextTurnLocked = false;
            TurnOwner = TurnOwner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
            CurrentBehaviour.StartTurn();
            
            OnTurnStarted.SafeInvoke(TurnOwner);
            OnStateChanged.SafeInvoke(GetCurrentState());
        }
        public void Stop()
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

        public void Dispose()
        {
            _playerBehaviour.Dispose();
            _enemyBehaviour.Dispose();
        }

        public BattleState GetCurrentState()
        {
            if (_nextTurnLocked) return BattleState.awaiting;
            return TurnOwner == CardOwner.player ? BattleState.playerTurn : BattleState.enemyTurn;
        }
    }
}
