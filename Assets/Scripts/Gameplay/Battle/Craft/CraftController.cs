using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model;
using Project.Infrastructure;

namespace Project.Gameplay.Battle.Craft
{
    public class CraftController : BattleController
    {
        protected override void Init()
        {
            if(_initialized) return;
            
            _model = new BattleModel(Constants.CraftBattle, Constants.CraftPlayer);
            _behaviour = new CraftBattleBehaviour(_model);
            _initialized = true;
        }
        protected override void Update()
        {
            
        }
    }
}
