using Project;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class EnemyPoint : InteractivePoint
    {
        public EnemyPoint()
        {
            PointEntity.Key = "Enemy";
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