using System;
using System.Collections.Generic;
using System.Linq;
using Project.Gameplay.Common;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Gameplay.Common.Datas;
using UnityEngine;

namespace Project.Gameplay.Battle.Model.Cards
{
    public class CardModel : IDisposable
    {
        public event Action<int> OnHealthChange;
        public event Action<int> OnAttackDamageChange;
        public event Action OnDeath;
        public event Action<CardPosition, CardPosition> OnTransfered;
        public event Action<CardPosition> OnAttack; 
        
        public string Key { get; protected set; }
        public CardConfig Config => BattleStaticData.Cards.Get(Key);
        public CardPosition Position => AttachedSlot.Position;
        public bool IsAlive => Health > 0;
        public CardType Type => Config.CardType;
        public int Cost => Config.Cost;
        public int Level => Config.Level;
        
        public CardSlotModel AttachedSlot { get; internal set; }
        public BattleModel BattleModel { get; protected set; }
        public Dictionary<CardEffect, int> SpellEffects { get; protected set; }
        public List<EffectTypes> Effects { get; protected set; }
        public int AttackDamage { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }

        public CardModel(string key, BattleModel battleModel)
        {
            Key = key;
            BattleModel = battleModel;
            AttackDamage = Config.BaseDamage;
            Health = MaxHealth = Config.BaseHealth;
            SpellEffects = new Dictionary<CardEffect, int>(Config.SpellEffects);
            Effects = Config.Effects.ToList();
            BattleModel.RegisterCard(this);
        }

        internal void ModifyHealth(int modifyValue)
        {
            if(modifyValue == 0) return;
            
            Health += modifyValue;
            Health = Math.Max(0, Health);
            Health = Math.Min(Health, MaxHealth);
            
            OnHealthChange.SafeInvoke(modifyValue);

            if (Health <= 0)
            {
                OnDeath.SafeInvoke();
                BattleModel.Player.ModifyLevelElectrons(Cost);                 
                    
                BattleModel.TryTransferCard(Position, CardPosition.Garbage());
            }
        }
        
        internal void ModifyMaxHealth(int modifyValue)
        {
            if(modifyValue == 0) return;
            
            Health = MaxHealth += modifyValue;
            Health = MaxHealth = Math.Max(0, MaxHealth);
            
            OnHealthChange.SafeInvoke(modifyValue);

            if (MaxHealth <= 0)
            {
                OnDeath.SafeInvoke();
                if (Position.IsPlayerField()) BattleModel.Player.ModifyLevelElectrons(Cost);                 
                    
                BattleModel.TryTransferCard(Position, CardPosition.Garbage());
            }
        }
        
        internal void ModifyAttackDamage(int modifyValue)
        {
            if(modifyValue == 0) return;
            
            AttackDamage += modifyValue;
            AttackDamage = Math.Max(0, Health);

            OnAttackDamageChange.SafeInvoke(modifyValue);
        }
        
        public void AddEffects(Dictionary<CardEffect, int> effects)
        {
            foreach (var effect in effects)
            {
                effect.Key.ApplyEffect(this, effect.Value);
            }
        }
        
        public void AddEffects(List<EffectTypes> effects)
        {
            foreach (var effect in effects)
            {
                Effects.Add(effect);
            }
        }
        public void AddEffects(EffectTypes effect)
        {
            Effects.Add(effect);
        }

        public bool HasEffect(EffectTypes effect)
        {
            return Effects.Contains(effect);
        }
        
        public void Dispose()
        {
            BattleModel.UnregisterCard(this);
        }

        internal void CallOnTransfered(CardPosition fromPosition, CardPosition toPosition) => OnTransfered.SafeInvoke(fromPosition, toPosition);
        internal void CallOnAttack(CardPosition toPosition) => OnAttack.SafeInvoke(toPosition);
        public bool IsPlayerHaveEnoughElectronsForPickUp() => (Cost <= BattleModel.Player.HandElectrons && Level <= BattleModel.Player.Level) || !Position.IsPlayerHand();
        public bool IsAvailableToPickUpByPlayer() => (AttachedSlot?.IsAvailableForPickUp(CardOwner.player) ?? false) && IsPlayerHaveEnoughElectronsForPickUp();
    }
}
