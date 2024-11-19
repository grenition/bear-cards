using System;
using GreonAssets.Extensions;
using Project.Gameplay.Data;

namespace Project.Gameplay.Battle.Cards
{
    public class CardModel : IDisposable
    {
        public string Key { get; protected set; }
        public CardConfig Config => StaticData.Cards.Get(Key);
        public CardPosition Position { get; internal set; }
        public BattleModel BattleModel { get; protected set; }
        public int Damage { get; protected set; }
        public int Health { get; protected set; }

        public CardModel(string key, BattleModel battleModel)
        {
            Key = key;
            BattleModel = battleModel;
            Damage = Config.BaseDamage;
            Health = Config.BaseHealth;

            BattleModel.RegisterCard(this);
        }
        
        public void Dispose()
        {
            BattleModel.UnregisterCard(this);
        }
    }
}
