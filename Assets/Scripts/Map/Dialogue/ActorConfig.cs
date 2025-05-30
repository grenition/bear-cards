using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Configs/Map/Dialogues/Actor")]
    public class ActorConfig : ScriptableObject
    {
        [field: SerializeField] public string Name { get;private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

    }
}
