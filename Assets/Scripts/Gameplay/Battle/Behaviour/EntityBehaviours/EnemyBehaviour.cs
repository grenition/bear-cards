using System.Linq;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using Project.Gameplay.Battle.Model.CardPlayers;
using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project.Gameplay.Battle.Behaviour.EntityBehaviours
{
    public class EnemyBehaviour : BaseEntityBehaviour
    {
        public CardPlayerModel PlayerModel => BattleBehaviour.Model.Enemy;
        
        public EnemyBehaviour(BattleBehaviour battleBehaviour) : base(battleBehaviour) { }

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
            
            for (int i = 0; i < PlayerModel.Config.HandSize; i++)
            {
                var card = BattleStaticData.Cards
                    .Where(x => x.Value.CardType != CardType.Spell)
                    .ToDictionary(x => x.Key, x => x.Value)
                    .GetRandomValue();
                
                if (!card) break;
                
                BattleBehaviour.Model.AddCardToDeck(CardOwner.enemy, card.name);
            }

            for (int i = 0; i < PlayerModel.Config.HandSize; i++)
            {
                if (Random.Range(0f, 1f) > PlayerModel.Config.PlaceToSlotChance) continue;

                var card = PlayerModel.Deck
                    .Where(x => x.Card != null)
                    .ToList()
                    .GetRandomElement();
                
                if (card == null) continue;

                BattleBehaviour.Model.TryTransferCard(card.Position, new CardPosition(CardContainer.hand, CardOwner.enemy, i));
                await UniTask.WaitForSeconds(0.1f);
            }
            
            BattleBehaviour.NextTurn();

        }
    }
}
