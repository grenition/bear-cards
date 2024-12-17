using Project;

namespace Assets.Scripts.Map
{
    public class BossPoint : InteractivePoint
    {
        public BossPoint()
        {
            PointEntity.Key = "Boss";
        }

        public override void OnBeginInteract()
        {
            var fightPanel = (FightUI)MapCompositionRoot.Instance.MapUI.ActiveUIByKey("fight");

            var data = DialoguesStatic.LoadData();
            switch (MapCompositionRoot.Instance.ActiveLocation.LocationKey)
            {
                case 0:
                    data.CountBossOneUpdate++;
                    break;
                case 1:
                    data.CountBossTwoUpdate++;
                    break;
                case 2:
                    data.CountBossThreeUpdate++;
                    break;
            }
            DialoguesStatic.SaveDataAndExecuteDialogue(data);
        }

        public override void OnEndInteract()
        {
        }
    }
}