using System;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Behaviour.EntityBehaviours;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.Cards;

namespace Project.Gameplay.Battle.Behaviour
{
    public class BattleBehaviour : IDisposable
    {
        public event Action<CardOwner> OnTurnStarted;
        
        public BattleConfig Config => Model.Config;
        public BattleModel Model { get; protected set; }
        public CardOwner TurnOwner { get; protected set; }
        public bool BehaviourActive { get; protected set; }

        protected BaseEntityBehaviour CurrentBehaviour => TurnOwner == CardOwner.player ? _playerBehaviour : _enemyBehaviour;
        protected PlayerBehaviour _playerBehaviour;
        protected EnemyBehaviour _enemyBehaviour;
        
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
        }
        
        public void NextTurn()
        {
            if (!BehaviourActive)
            {
                Start();
                return;
            }

            CurrentBehaviour.StopTurn();
            TurnOwner = TurnOwner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
            CurrentBehaviour.StartTurn();
            OnTurnStarted.SafeInvoke(TurnOwner);
        }
        public void Stop()
        {
            if(!BehaviourActive) return;
            
            CurrentBehaviour.StopTurn();
            BehaviourActive = false;
        }
        
        public void Dispose()
        {
            _playerBehaviour.Dispose();
            _enemyBehaviour.Dispose();
        }
    }
}
