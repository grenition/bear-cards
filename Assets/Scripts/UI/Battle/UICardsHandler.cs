using UnityEngine;

namespace Project.UI.Battle
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
