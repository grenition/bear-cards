using UnityEngine;
using UnityEngine.Localization;

namespace Project
{
    [CreateAssetMenu(fileName = "HillDescriptionConfig", menuName = "Map/HillEasy")]
    public class HillDescriptionConfig : PointDescriptionConfig
    {
        [field: SerializeField] public int HPModificator { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public LocalizedString LocalizedName { get; private set; }
        [field: SerializeField] public LocalizedString LocalizedDescription { get; private set; }
    }
}
