using UnityEngine;
using UnityEngine.Localization;

namespace Project
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Configs/Map/Dialogues/Actor")]
    public class ActorConfig : ScriptableObject
    {
        [field: SerializeField] public LocalizedString LocalizedName { get; private set; }
        [field: SerializeField] public string Name { get;private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

    }
}
