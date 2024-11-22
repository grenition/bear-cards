using System;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.CardSlots;

namespace Project.Gameplay.Battle.Model.Cards
{
    public class CardModel : IDisposable
    {
        public event Action<int> OnDamage;
        public event Action OnDeath;
        public event Action<CardPosition, CardPosition> OnTransfered;
        public event Action<CardPosition> OnAttack; 
        
        public string Key { get; protected set; }
        public CardConfig Config => BattleStaticData.Cards.Get(Key);
        public CardPosition Position => AttachedSlot.Position;
        public bool IsAlive => Health > 0;
        public CardSlotModel AttachedSlot { get; internal set; }
        public BattleModel BattleModel { get; protected set; }
        public int AttackDamage { get; protected set; }
        public int Health { get; protected set; }

        public CardModel(string key, BattleModel battleModel)
        {
            Key = key;
            BattleModel = battleModel;
            AttackDamage = Config.BaseDamage;
            Health = Config.BaseHealth;

            BattleModel.RegisterCard(this);
        }

        internal void Damage(int damage)
        {
            if(damage <= 0) return;
            
            Health -= damage;
            OnDamage.SafeInvoke(damage);

            if (Health <= 0)
            {
                OnDeath.SafeInvoke();
                BattleModel.TryTransferCard(Position, CardPosition.Garbage());
            }
        }
        
        public void Dispose()
        {
            BattleModel.UnregisterCard(this);
        }

        internal void CallOnTransfered(CardPosition fromPosition, CardPosition toPosition) => OnTransfered.SafeInvoke(fromPosition, toPosition);
        internal void CallOnAttack(CardPosition toPosition) => OnAttack.SafeInvoke(toPosition);
        public bool IsAvailableToPickUpByPlayer() => AttachedSlot?.IsAvailableForPickUp(CardOwner.player) ?? false;
    }
}
