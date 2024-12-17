using Assets.Scripts.Map;

namespace Project
{
    public class EnemyEpicPoint : InteractivePoint
    {
        public EnemyEpicPoint()
        {
            PointEntity.Key = "EnemyMeadle";
        }

        public override void OnBeginInteract()
        {
            var data = DialoguesStatic.LoadData();
            data.CountEnemyComming++;
            DialoguesStatic.SaveDataAndExecuteDialogue(data);

            var fightPanel = (FightUI)MapCompositionRoot.Instance.MapUI.ActiveUIByKey("fight");
        }

        public override void OnEndInteract()
        {
        }
    }
}
