using System;
using GreonAssets.Extensions;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public abstract class BaseEntityBehaviour : IDisposable
    {
        public bool TurnEnded { get; private set; } = false;
        public int TurnIndex { get; private set; } = -1;
        
        public void StartTurn()
        {
            TurnEnded = false;
            TurnIndex++;
            if (TurnIndex == 0)
                OnFirstTurnStart();
            OnTurnStart();
        }
        public void StopTurn()
        {
            OnTurnStop();
            TurnEnded = true;
        }

        protected virtual void OnTurnStart() { }
        protected virtual void OnFirstTurnStart() { }
        protected virtual void OnTurnStop() { }

        public virtual void Dispose() { }
    }
}
