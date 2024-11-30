using UnityEngine;

namespace Project.UI.Common
{
    public class UICardsHandler : MonoBehaviour
    {
        public static UICardsHandler instance;

        private void Awake()
        {
            instance = this;
        }
    }
}
