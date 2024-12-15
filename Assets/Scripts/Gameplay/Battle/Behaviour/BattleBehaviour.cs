using System;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Common;
using Project.Gameplay.Common.Datas;

namespace Project.Gameplay.Battle.Behaviour
{
    public enum BattleState
    {
        awaiting,
        playerTurn,
        enemyTurn
    }

    public abstract class BattleBehaviour : IDisposable
    {
        public event Action<BattleState> OnStateChanged;
        public event Action<CardOwner> OnTurnStarted;

        public BattleConfig Config => Model.Config;
        public BattleModel Model { get; protected set; }
        public CardOwner TurnOwner { get; protected set; }
        public bool BehaviourActive { get; protected set; }

        public abstract void Start();
        public abstract void NextTurn();
        public abstract void Stop();
        public abstract void Dispose();
        public abstract BattleState GetCurrentState();

        protected void CallOnStateChanged(BattleState obj) => OnStateChanged.SafeInvoke(obj);
        protected void CallOnTurnStarted(CardOwner obj) => OnTurnStarted?.Invoke(obj);

    }
}
