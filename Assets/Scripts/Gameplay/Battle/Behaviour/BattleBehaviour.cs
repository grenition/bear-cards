using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Behaviour.EntityBehaviours;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.Cards;
using R3;

namespace Project.Gameplay.Battle.Behaviour
{
    public class BattleBehaviour : IDisposable
    {
        public Action<CardOwner> OnTurnChanged;
        
        public BattleConfig Config => Model.Config;
        public BattleModel Model { get; protected set; }
        public CardOwner TurnOwner { get; protected set; }
        public bool BehaviourActive => _battleLoopCts != null;

        protected BaseEntityBehaviour CurrentBehaviour => TurnOwner == CardOwner.player ? _playerBehaviour : _enemyBehaviour;
        protected PlayerBehaviour _playerBehaviour;
        protected EnemyBehaviour _enemyBehaviour;
        protected CancellationDisposable _battleLoopCts;
        
        public BattleBehaviour(BattleModel model)
        {
            Model = model;
            TurnOwner = Config.FirstTurnOwner;

            _playerBehaviour = new(Model);
            _enemyBehaviour = new(Model);
        }

        public void Start()
        {
            _battleLoopCts?.Dispose();
            _battleLoopCts = new();
            
            BattleLoop(_battleLoopCts.Token);
        }

        public async void BattleLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    CurrentBehaviour.StartTurn();
                    await UniTask.WaitUntil(() => CurrentBehaviour.TurnEnded, cancellationToken: cancellationToken);
                }
                catch
                {
                    CurrentBehaviour.StopTurn();
                }
                TurnOwner = TurnOwner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
                OnTurnChanged.SafeInvoke(TurnOwner);
            }
        }
        
        public void Stop()
        {
            _battleLoopCts?.Dispose();
            _battleLoopCts = null;
        }
        public void NextTurn()
        {
            Stop();
            Start();
        }
        
        public void Dispose()
        {
            Stop();
            _playerBehaviour.Dispose();
            _enemyBehaviour.Dispose();
        }
    }
}
