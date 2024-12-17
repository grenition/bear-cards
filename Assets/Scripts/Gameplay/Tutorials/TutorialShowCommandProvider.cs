using UnityEngine;

namespace Project.Gameplay.Tutorials
{
    public class TutorialShowCommandProvider : MonoBehaviour
    {
        [SerializeField] private string _tutorialKey;

        public void ShowTutorial() => TutorialService.ShowTutorial(_tutorialKey, true);
    }
}
