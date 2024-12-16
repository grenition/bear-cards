using Project;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class EnemyEasyPoint : InteractivePoint
    {
        public EnemyEasyPoint()
        {
            PointEntity.Key = "EnemyEasy";
        }

        public override void OnBeginInteract()
        {
            var data = DialoguesStatic.LoadData();
            data.CountEnemyComming++;
            DialoguesStatic.SaveData(data);

            var fightPanel = (FightUI)MapCompositionRoot.Instance.MapUI.ActiveUIByKey("fight");
        }

        public override void OnEndInteract()
        {
        }
    }
}