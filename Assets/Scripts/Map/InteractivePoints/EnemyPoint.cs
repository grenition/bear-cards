namespace Assets.Scripts.Map
{
    public class EnemyPoint : InteractivePoint
    {
        public EnemyPoint()
        {
            Key = "Enemy";
        }
        public override void OnBeginInteract()
        {
            throw new System.NotImplementedException();
        }

        public override void OnEndInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}