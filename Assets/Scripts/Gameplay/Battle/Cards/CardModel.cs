using GreonAssets.Extensions;
using Infrastructure;

namespace Gameplay.Battle.Cards
{
    public class CardModel
    {
        public CardConfig Config => StaticData.Cards.Get(_key);
        public CardPosition Position => _battleModel.TryGetCardPosition(this, out var pos) ? pos : default;
        
        protected string _key;
        protected BattleModel _battleModel;
        protected int _damage;
        protected int _health;
        
        public CardModel(string key, BattleModel battleModel)
        {
            _key = key;
            _battleModel = battleModel;
            _damage = Config.BaseDamage;
            _health = Config.BaseHealth;
        }
    }
}
