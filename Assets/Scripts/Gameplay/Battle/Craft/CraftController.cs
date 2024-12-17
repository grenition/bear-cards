using Project.Gameplay.Battle.Behaviour;
using Project.Gameplay.Battle.Model;
using Project.Gameplay.Tutorials;
using Project.Infrastructure;

namespace Project.Gameplay.Battle.Craft
{
    public class CraftController : BattleController
    {
        protected override void Init()
        {
            if(_initialized) return;
            
            _model = new BattleModel(Constants.CraftBattle);
            _behaviour = new CraftBattleBehaviour(_model);
            _initialized = true;

            TutorialService.ShowTutorial("craft_tutorial");
        }
        protected override void Update()
        {
            
        }
    }
}
