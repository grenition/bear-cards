using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Common;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common.Datas;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class EnemyBehaviour : BaseEntityBehaviour
    {
        public CardPlayerModel PlayerModel => BattleBehaviour.Model.Enemy;
        
        public EnemyBehaviour(BattleBehaviour battleBehaviour) : base(battleBehaviour) { }

        protected override void OnFirstTurnStart()
        {
            
        }

        protected override async void OnTurnStart()
        {
            await UniTask.WaitForSeconds(1f);
            
            for (int i = 0; i < PlayerModel.Config.HandSize; i++)
            {
                var card = PlayerModel.Hand.GetAt(i);
                if (card == null) continue;

                BattleBehaviour.Model.TryTransferCard(card.Position, new CardPosition(CardContainer.field, CardOwner.enemy, i));
                await UniTask.WaitForSeconds(0.1f);
            }
            
            await UniTask.WaitForSeconds(1f);

            TransferCardsToDeck();

            var turn = PlayerModel.Config.Turns[Mathf.Clamp(TurnIndex, 0, PlayerModel.Config.Turns.Count - 1)];
            if (turn != null)
            {
                for (int i = 0; i < PlayerModel.Config.HandSize; i++)
                {
                    if (Random.Range(0f, 1f) > turn.PlaceToSlotChance) continue;

                    var card = PlayerModel.Deck
                        .Where(x => x.Card != null)
                        .ToList()
                        .GetRandomElement();

                    if (card == null) continue;

                    BattleBehaviour.Model.TryTransferCard(card.Position, new CardPosition(CardContainer.hand, CardOwner.enemy, i));
                    await UniTask.WaitForSeconds(0.1f);
                }
            }

            DeleteCardsFromDeck();
            
            BattleBehaviour.NextTurn();

        }

        private void TransferCardsToDeck()
        {
            var turn = PlayerModel.Config.Turns[Math.Clamp(TurnIndex, 0, PlayerModel.Config.Turns.Count - 1)];
            if(turn == null) return;
            var cardPool = turn.Cards.ToList();
            
            for (int i = 0; i < PlayerModel.Config.HandSize; i++)
            {
                var card = cardPool.GetAt(i);
                if (!card) continue;
                
                BattleBehaviour.Model.AddCardToDeck(CardOwner.enemy, card.name);
            }
        }
        private void DeleteCardsFromDeck()
        {
            PlayerModel.Deck
                .Where(x => x.Card != null)
                .Select(x => x.TakeCard())
                .ForEach(x => x.Dispose());
        }
    }
}
