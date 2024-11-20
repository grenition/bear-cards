using System;
using GreonAssets.Extensions;
using R3;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public abstract class BaseEntityBehaviour : IDisposable
    {
        public bool TurnActive { get; private set; } = false;
        public int TurnIndex { get; private set; } = -1;
        public BattleBehaviour BattleBehaviour { get; protected set; }
        
        public void StartTurn()
        {
            TurnActive = true;
            TurnIndex++;
            if (TurnIndex == 0)
                OnFirstTurnStart();
            OnTurnStart();
        }
        public void StopTurn()
        {
            OnTurnStop();
            TurnActive = false;
        }

        protected virtual void OnTurnStart() { }
        protected virtual void OnFirstTurnStart() { }
        protected virtual void OnTurnStop() { }

        public virtual void Dispose() { }

        public BaseEntityBehaviour(BattleBehaviour battleBehaviour)
        {
            BattleBehaviour = battleBehaviour;
        }
    }
}
