using System;
using System.Linq;
using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Common.Datas;
using Project.Infrastructure;
using UnityEngine;

namespace Project.Gameplay.Battle.Behaviour
{
    public class CraftBattleBehaviour : BattleBehaviour
    {
        public bool CraftSuccessed { get; private set; }
        protected bool _nextTurnLocked = false;

        public CraftBattleBehaviour(BattleModel model)
        {
            Model = model;
            TurnOwner = CardOwner.player;
        }
        
        public async override void Start()
        {
            if(BehaviourActive) return;
            
            BehaviourActive = true;
            
            CallOnTurnStarted(TurnOwner);
            CallOnStateChanged(GetCurrentState());
            Model.Player.AddTurnElectrons();
            
            await UniTask.NextFrame();

            var playerConfig = BattleStaticData.CardPlayers.Get(Constants.Player);
            var deck = MapStaticData.LoadData();

            string[] types = new string[0];
            
            foreach(string card in deck.Deck)
            {
                types = ExcludingExpandMassive(types, card);
            }

            string[] sorted = new string[deck.Deck.Length];
            Array.Copy(deck.Deck, sorted, sorted.Length);
            for (int i = 0; i < sorted.Length; i++)
            {
                int minimal = i;
                for(int ii = i + 1; ii < sorted.Length; ii++)
                {
                    if(Array.IndexOf(types, sorted[minimal]) > Array.IndexOf(types, sorted[ii]))
                    {
                        minimal = ii;
                    }
                }

                string temp = sorted[minimal];
                sorted[minimal] = sorted[i];
                sorted[i] = temp;
            }

            sorted.ForEach(card =>
            {
                Model.AddCardToDeck(CardOwner.player, card);
            });

            for (int i = 0; i < Model.Player.Config.HandSize; i++)
            {
                if(!Model.Player.TransferCardFromDeckToHand())
                {
                    break;
                }
                else
                {
                    await UniTask.WaitForSeconds(0.1f);
                }
            }

            GameObject.FindObjectOfType<CardUseLocker>().UnLock();
        }
        public override async void NextTurn()
        {
            _nextTurnLocked = true;
            
            CallOnStateChanged(GetCurrentState());
            
            foreach (var craft in BattleStaticData.Crafts.Values)
            {
                bool mismatch = false;

                var targetMetalCards = craft.Metals.Where(x => x != null).Select(x => x.name).ToList();
                var targetNonMetalCards = craft.NonMetals.Where(x => x != null).Select(x => x.name).ToList();

                for (int i = 0; i < 5; i++)
                {
                    var cardAtField = Model.PlayerField.GetAt(i)?.Card?.Key;
                    targetMetalCards.TryRemove(cardAtField);
                }
                for (int i = 5; i < 10; i++)
                {
                    var cardAtField = Model.PlayerField.GetAt(i)?.Card?.Key;
                    targetNonMetalCards.TryRemove(cardAtField);
                }

                mismatch = targetMetalCards.Count != 0 || targetNonMetalCards.Count != 0;
                if (!mismatch)
                {
                    var locationData = MapStaticData.LoadData();
                    var playerCards = locationData.Deck.ToList();
                    
                    foreach (var cardSlot in Model.PlayerField)
                    {
                        if(cardSlot.Card == null) continue;
                        playerCards.TryRemove(cardSlot.Card.Key);
                        cardSlot.Card.ModifyHealth(-cardSlot.Card.Health);
                    }

                    Model.AddCardToDeck(CardOwner.enemy, craft.Output.name);
                    Model.Enemy.TransferCardFromDeckToHand(false);

                    playerCards.Add(craft.Output.name);

                    var data = DialoguesStatic.LoadData();
                    if (!data.Recepts.Contains(craft.name))
                    {
                        DialoguesStatic.SaveRecept(new string[] { craft.name });
                    }

                    MapStaticData.SetDeckAndSave(playerCards);

                    await UniTask.WaitForSeconds(0.5f);

                    CraftSuccessed = true;
                    Model.EndBattle(CardOwner.player);
                    
                    break;
                }
            }
            
            await UniTask.WaitForSeconds(0.5f);

            CraftSuccessed = false;
            Model.EndBattle(CardOwner.player);
        }
        public override void Stop()
        {
        }
        public override void Dispose()
        {
        }
        public override BattleState GetCurrentState() => _nextTurnLocked ? BattleState.awaiting : BattleState.playerTurn;

        private T[] ExcludingExpandMassive<T>(T[] origin, T value)
        {
            foreach (T t in origin)
            {
                if (t.Equals(value))
                {
                    return origin;
                }
            }

            T[] newMassive = new T[origin.Length + 1];

            Array.Copy(origin, newMassive, origin.Length);

            newMassive[newMassive.Length - 1] = value;

            return newMassive;
        }
    }
}
