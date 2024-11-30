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
            ScneneLoaderStatic.LoadSceneAsync("BattleScene");
        }

        public override void OnEndInteract()
        {
        }
    }
}