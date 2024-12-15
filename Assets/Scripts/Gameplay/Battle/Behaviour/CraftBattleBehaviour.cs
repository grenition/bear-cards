using System.Linq;
using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Common.Datas;

namespace Project.Gameplay.Battle.Behaviour
{
    public class CraftBattleBehaviour : BattleBehaviour
    {
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
            
            var deck = MapStaticData.LoadData();
            
            foreach (var deckCard in Model.Player.Config.Deck)
            {
                Model.AddCardToDeck(CardOwner.player, deckCard.name);
            }

            deck.Deck.ForEach(card =>
            {
                Model.AddCardToDeck(CardOwner.player, card);
            });

            for (int i = 0; i < Model.Player.Config.HandSize; i++)
            {
                Model.Player.TransferCardFromDeckToHand();
                await UniTask.WaitForSeconds(0.1f);
            }
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
                    foreach (var cardSlot in Model.PlayerField)
                    {
                        if(cardSlot.Card == null) continue;
                        cardSlot.Card.ModifyHealth(-cardSlot.Card.Health);
                    }

                    Model.AddCardToDeck(CardOwner.enemy, craft.Output.name);
                    Model.Enemy.TransferCardFromDeckToHand();

                    await UniTask.WaitForSeconds(0.5f);
                    Model.EndBattle(CardOwner.player);
                    
                    break;
                }
            }
            
            await UniTask.WaitForSeconds(0.5f);
            Model.EndBattle(CardOwner.enemy);
        }
        public override void Stop()
        {
        }
        public override void Dispose()
        {
        }
        public override BattleState GetCurrentState() => _nextTurnLocked ? BattleState.awaiting : BattleState.playerTurn;
    }
}