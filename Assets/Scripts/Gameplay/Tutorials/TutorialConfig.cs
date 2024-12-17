using UnityEngine;

namespace Project.Gameplay.Tutorials
{
    [CreateAssetMenu(menuName = "Gameplay/TutorialConfig", fileName = "Tutorial")]
    public class TutorialConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject TutorialPrefab { get; private set; }
    }
}
