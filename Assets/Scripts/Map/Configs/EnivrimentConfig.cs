using UnityEngine;
namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "EnuvrimentConfig", menuName = "Map/EnuvrimentConfig")]
    public class EnivrimentConfig : ScriptableObject
    {
        [field: SerializeField] public int DistanceBeetwenPointByX { get; private set; }
        [field: SerializeField] public int DistanceBeetwenPointByY { get; private set; }
    }
}