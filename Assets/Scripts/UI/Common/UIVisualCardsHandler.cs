using UnityEngine;

namespace Project.UI.Common
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
