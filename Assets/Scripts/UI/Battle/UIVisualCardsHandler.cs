using UnityEngine;

namespace Project.UI.Battle
{
    public class UIVisualCardsHandler : MonoBehaviour
    {

        public static UIVisualCardsHandler instance;

        private void Awake()
        {
            instance = this;
        }
    }
}
