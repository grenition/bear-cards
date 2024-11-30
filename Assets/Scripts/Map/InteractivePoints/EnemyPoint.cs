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
            //ScneneLoaderStatic.LoadSceneAsync("BattleScene");
            MapCompositionRoot.Instance.MapUI.ActiveUIByKey("fight", null);
        }

        public override void OnEndInteract()
        {
        }
    }
}