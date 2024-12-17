using Assets.Scripts.Map;
using Project.Gameplay.Common;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Battle.Model.CardSlots;
using Project.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Project.Gameplay.Common.Datas;

namespace Project.Gameplay.Battle.Model
{
    public class BattleModel : IDisposable
    {
        public event Action<CardModel, CardPosition, CardPosition> OnCardTransfered;
        public event Action<CardModel, CardPosition, EffectTypes> OnAttackedWithEffect;
        public event Action<CardModel, CardModel> OnCardAttack;
        public event Action<CardOwner> OnBattleEnded;

        public string Key { get; protected set; }
        public BattleConfig Config => BattleStaticData.Battles.Get(Key);
        public CardPlayerModel Player { get; protected set; }
        public CardPlayerModel Enemy { get; protected set; }
        public List<CardSlotModel> PlayerField { get; protected set; } = new();
        public List<CardSlotModel> EnemyField { get; protected set; } = new();
        public Dictionary<CardPosition, CardSlotModel> Slots { get; protected set; } = new();
        public List<CardModel> Cards { get; protected set; } = new();
        public bool BattleEnded { get; protected set; }
        public CardOwner BattleWinner { get; protected set; }

        public BattleModel(string battleKey, string enemyKey)
        {
            Key = battleKey != null? battleKey: "demo_battle";
            Player = new CardPlayerModel(battleKey == Constants.CraftBattle ? Constants.CraftPlayer : Constants.Player, CardOwner.player, this);
            Enemy = new CardPlayerModel(enemyKey, CardOwner.enemy, this);

            var playerFieldPermission = Config.AllowPlayerCardReposition ? CardSlotPermissions.PlayerHand() : CardSlotPermissions.PlayerField();
            for (int i = 0; i < Config.FieldSize; i++)
            {
                PlayerField.Add(new CardSlotModel(this, new CardPosition(CardContainer.field, CardOwner.player, i), playerFieldPermission));
                EnemyField.Add(new CardSlotModel(this, new CardPosition(CardContainer.field, CardOwner.enemy, i), CardSlotPermissions.EnemyField()));
            }
        }

        #region Registry

        internal void RegisterCardSlot(CardSlotModel slot)
        {
            if (slot == null) return;
            if (Slots.ContainsKey(slot.Position)) return;
            Slots.Add(slot.Position, slot);
        }
        internal void UnregisterCardSlot(CardSlotModel slot)
        {
            if (slot == null) return;
            if (!Slots.ContainsKey(slot.Position)) return;
            Slots.Remove(slot.Position);
        }
        internal void RegisterCard(CardModel card)
        {
            if (Cards.Contains(card)) return;
            Cards.Add(card);
        }
        internal void UnregisterCard(CardModel card)
        {
            if (!Cards.Contains(card)) return;
            Cards.Remove(card);
        }

        #endregion

        #region DataGetters

        public CardModel GetCardAtPosition(CardPosition position)
        {
            var slot = Slots.Get(position);
            if (slot == null) return null;
            return slot.Card;
        }
        public IEnumerable<CardModel> GetCardsAtPosition(CardOwner owner, CardContainer container)
        {
            return Slots
                .Where(x => x.Key.container == container && x.Key.owner == owner)
                .Where(x => x.Value.Card != null)
                .Select(x => x.Value.Card);
        }
        public CardSlotModel GetSlotAtPosition(CardPosition position)
        {
            return Slots.Get(position);
        }
        public List<CardSlotModel> GetSlotsAtPosition(CardOwner owner, CardContainer container)
        {
            return Slots
                .Where(x => x.Key.container == container && x.Key.owner == owner)
                .Select(x => x.Value)
                .ToList();
        }
        public bool IsPlaceAvailableForPlayerCard(CardPosition position)
        {
            var slot = Slots.Get(position);
            return slot != null && slot.IsAvailableForDrop(CardOwner.player);
        }
        public List<CardSlotModel> GetSlotsForSpell(CardPosition position, SpellPlacing spellPlacing)
        {
            var slots = new List<CardSlotModel>();
            if (position.container != CardContainer.field) return slots;

            switch (spellPlacing)
            {
                case SpellPlacing.PlayerCard:
                    if (position.owner == CardOwner.player)
                        slots.Add(GetSlotAtPosition(position));
                    break;
                case SpellPlacing.EnemyCard:
                    if (position.owner == CardOwner.enemy)
                        slots.Add(GetSlotAtPosition(position));
                    break;
                case SpellPlacing.AnyCard:
                    slots.Add(GetSlotAtPosition(position));
                    break;
                case SpellPlacing.PlayerField:
                    if (position.owner == CardOwner.player)
                        slots.AddRange(GetSlotsAtPosition(position.owner, position.container));
                    break;
                case SpellPlacing.EnemyField:
                    if (position.owner == CardOwner.enemy)
                        slots.AddRange(GetSlotsAtPosition(position.owner, position.container));
                    break;
                case SpellPlacing.AnyFields:
                    slots.AddRange(GetSlotsAtPosition(position.owner, position.container));
                    break;
                case SpellPlacing.AllField:
                    slots.AddRange(GetSlotsAtPosition(CardOwner.player, position.container));
                    slots.AddRange(GetSlotsAtPosition(CardOwner.enemy, position.container));
                    break;
                
            }
            return slots;
        }
        public bool IsSlotAvailableForSpell(CardSlotModel slot, CardModel card)
        {
            if (card.Type != CardType.Spell) return false;

            var slots = GetSlotsForSpell(slot.Position, card.Config.SpellPlacing);
            return slots.Contains(slot);
        }
        public int GetElectronLevel(int electrons)
        {
            for (int i = 0; i < Config.LevelElectrons.Length; i++)
            {
                if (electrons < Config.LevelElectrons[i])
                    return i;
            }
            return Config.LevelElectrons.Length;
        }

        #endregion

        #region Interactions

        public void AddCardToDeck(CardOwner owner, string cardKey)
        {
            if (BattleEnded) return;

            var playerModel = owner == CardOwner.player ? Player : Enemy;
            var freeDeckSlot = playerModel.GetFirstFreeSlotInDeck();
            if (freeDeckSlot == null) return;

            var card = new CardModel(cardKey, this);
            freeDeckSlot.PlaceCard(card);
        }

        public bool TryTransferCard(CardPosition from, CardPosition to)
        {
            if (BattleEnded) return false;
            var slot = GetSlotAtPosition(from);
            if (slot == null) return false;

            CardModel card = null;
            if (to.container == CardContainer.garbage)
            {
                card = slot.TakeCard();
                OnCardTransfered.SafeInvoke(card, from, to);
                card.CallOnTransfered(from, to);
                card.Dispose();
                return true;
            }

            var newSlot = GetSlotAtPosition(to);
            var owner = slot.Position.owner;

            if (!slot.IsAvailableForPickUp(owner)) return false;
            if (newSlot == null) return false;

            if (IsSlotAvailableForSpell(newSlot, slot.Card))
            {
                AttackSpell(slot.Card, to);
                return true;
            }

            if (from.IsPlayerHand() && !slot.Card.IsPlayerHaveEnoughElectronsForPickUp())
                return false;

            if (!newSlot.IsAvailableForDrop(owner)) return false;

            card = slot.TakeCard();
            newSlot.PlaceCard(card);

            if (from.IsPlayerHand()) Player.ModifeHandElectrons(-card.Cost);

            OnCardTransfered.SafeInvoke(card, from, to);
            card.CallOnTransfered(from, to);

            return true;
        }

        public void AttackSpell(CardModel spell, CardPosition position)
        {
            var slots = GetSlotsForSpell(position, spell.Config.SpellPlacing);

            if (spell.Config.SpellPlayersPlacing == SpellPlayersPlacing.PlayerAndCards
                || spell.Config.SpellPlayersPlacing == SpellPlayersPlacing.EnemyAndCards
                || spell.Config.SpellPlayersPlacing == SpellPlayersPlacing.OnlyCards)
            {
                slots.Where(x => x.Card != null).ForEach(x => x.Card.AddEffects(spell.SpellEffects));
            }

            if (spell.Config.SpellPlayersPlacing == SpellPlayersPlacing.PlayerOnly
                || spell.Config.SpellPlayersPlacing == SpellPlayersPlacing.PlayerAndCards)
            {
                Player.AddEffects(spell.SpellEffects);
            }
            else if (spell.Config.SpellPlayersPlacing == SpellPlayersPlacing.EnemyOnly
                || spell.Config.SpellPlayersPlacing == SpellPlayersPlacing.EnemyAndCards)
            {
                Enemy.AddEffects(spell.SpellEffects);
            }
            
            spell.ModifyHealth(-spell.Health);
        }
        public void AttackForward(CardPosition attackerPosition)
        {
            if (BattleEnded) return;
            if (attackerPosition.container != CardContainer.field) return;

            var card = GetCardAtPosition(attackerPosition);
            var enemyType = attackerPosition.owner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
            var enemyPosition = new CardPosition(attackerPosition.container, enemyType, attackerPosition.index);
            var forwardCard = GetCardAtPosition(enemyPosition);
            var enemyPlayer = enemyType == CardOwner.player ? Player : Enemy;

            if (card == null || card.AttackDamage == 0) return;
            
            if (card.HasEffect(EffectTypes.Explosion))
            {
                if(forwardCard != null)
                    OnAttackedWithEffect.SafeInvoke(forwardCard, forwardCard.Position, EffectTypes.Explosion);
                AttackLeft(attackerPosition);
                AttackRight(attackerPosition);
            }
            card.CallOnAttack(enemyPosition);

            if (forwardCard == null)
            {
                if (card.HasEffect(EffectTypes.Flying))
                    OnAttackedWithEffect.SafeInvoke(card, card.Position, EffectTypes.Flying);
                enemyPlayer.ModifyHealth(-card.AttackDamage);
                return;
            }
            
            if (card.HasEffect(EffectTypes.Flying) && !forwardCard.HasEffect(EffectTypes.BlockFlying))
            {
                OnAttackedWithEffect.SafeInvoke(card, card.Position, EffectTypes.Flying);
                enemyPlayer.ModifyHealth(-card.AttackDamage);
                return;
            }

            if (!card.HasEffect(EffectTypes.Poison) || forwardCard.HasEffect(EffectTypes.PoisonResistance))
            {
                if (card.HasEffect(EffectTypes.Poison))
                    OnAttackedWithEffect.SafeInvoke(forwardCard, forwardCard.Position, EffectTypes.Poison);
                forwardCard.ModifyHealth(-card.AttackDamage);
            }
            else
            {
                forwardCard.ModifyHealth(-forwardCard.Health);
                OnAttackedWithEffect.SafeInvoke(forwardCard, forwardCard.Position, EffectTypes.Poison);
            }
        }
        
        public void AttackLeft(CardPosition attackerPosition)
        {
            if (BattleEnded) return;
            if (attackerPosition.container != CardContainer.field) return;

            var card = GetCardAtPosition(attackerPosition);
            var enemyType = attackerPosition.owner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
            var enemyPlayer = enemyType == CardOwner.player ? Player : Enemy;
            var enemyPosition = new CardPosition(attackerPosition.container, enemyType, attackerPosition.index - 1); //!!!
            var leftCard = GetCardAtPosition(enemyPosition);

            if (card == null || card.AttackDamage == 0) return;
            card.CallOnAttack(enemyPosition);
            
            if (attackerPosition.index - 1 < 0) //!!!
            {
                enemyPlayer.ModifyHealth(-card.AttackDamage);
                return;
            }

            if (leftCard == null || (card.HasEffect(EffectTypes.Flying) && !leftCard.HasEffect(EffectTypes.BlockFlying)))
            {
                enemyPlayer.ModifyHealth(-card.AttackDamage);
                return;
            }
            
            if(!card.HasEffect(EffectTypes.Poison) || leftCard.HasEffect(EffectTypes.PoisonResistance))
                leftCard.ModifyHealth(-card.AttackDamage);
            else
                leftCard.ModifyHealth(-leftCard.Health);
        }
        
        public void AttackRight(CardPosition attackerPosition)
        {
            if (BattleEnded) return;
            if (attackerPosition.container != CardContainer.field) return;

            var card = GetCardAtPosition(attackerPosition);
            var enemyType = attackerPosition.owner == CardOwner.player ? CardOwner.enemy : CardOwner.player;
            var enemyPlayer = enemyType == CardOwner.player ? Player : Enemy;
            var enemyPosition = new CardPosition(attackerPosition.container, enemyType, attackerPosition.index + 1);
            var leftCard = GetCardAtPosition(enemyPosition);

            if (card == null || card.AttackDamage == 0) return;
            card.CallOnAttack(enemyPosition);
            
            if (attackerPosition.index + 1 >= Config.FieldSize) 
            {
                enemyPlayer.ModifyHealth(-card.AttackDamage);
                return;
            }

            if (leftCard == null || (card.HasEffect(EffectTypes.Flying) && !leftCard.HasEffect(EffectTypes.BlockFlying)))
            {
                enemyPlayer.ModifyHealth(-card.AttackDamage);
                return;
            }
            
            if(!card.HasEffect(EffectTypes.Poison) || leftCard.HasEffect(EffectTypes.PoisonResistance))
                leftCard.ModifyHealth(-card.AttackDamage);
            else
                leftCard.ModifyHealth(-leftCard.Health);
        }
            
        public void EndBattle(CardOwner winner)
        {
            if (BattleEnded) return;

            BattleEnded = true;
            BattleWinner = winner;
            OnBattleEnded.SafeInvoke(winner);

            if (winner == CardOwner.player)
                MapStaticData.LevelComplited(Player.Deck.Where(x => x.Card != null).Select(x => x.Card.Key));
            else
                MapStaticData.GameFail();
        }

        #endregion

        public void Dispose()
        {
            Player.Dispose();
            Enemy.Dispose();
            PlayerField.ForEach(x => x.Dispose());
            EnemyField.ForEach(x => x.Dispose());
            Cards.ToList().ForEach(x => x.Dispose());
        }
    }
}
