using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "HillDescriptionConfig", menuName = "Map/HillEasy")]
    public class HillDescriptionConfig : PointDescriptionConfig
    {
        [field: SerializeField] public int HPModificator { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}
